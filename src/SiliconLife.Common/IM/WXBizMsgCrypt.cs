// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace SiliconLife.Common.IM;

/// <summary>
/// 微信系平台（企业微信、公众号、小程序客服）回调消息加解密工具类。
/// 基于腾讯官方 WXBizMsgCrypt C# 实现，使用 AES-256-CBC 加解密与 SHA1 签名校验。
/// </summary>
/// <remarks>
/// 加密明文格式：random(16) + msg_len(4, 大端序) + msg + appid，再以 32 字节块进行 PKCS7 填充。
/// AES 密钥由 EncodingAESKey（43 字符 Base64）末尾补 "=" 后解码得到（32 字节）；IV 取密钥前 16 字节。
/// 签名算法：SHA1(sort(token, timestamp, nonce, encrypt))，输出小写十六进制。
/// </remarks>
public class WXBizMsgCrypt
{
    /// <summary>成功</summary>
    public const int OK = 0;
    /// <summary>签名校验失败</summary>
    public const int ValidateSignature_Error = -40001;
    /// <summary>XML 解析失败</summary>
    public const int ParseXml_Error = -40002;
    /// <summary>签名计算失败</summary>
    public const int CalcSignature_Error = -40003;
    /// <summary>EncodingAESKey 非法</summary>
    public const int IllegalAesKey = -40004;
    /// <summary>AppID 校验失败</summary>
    public const int ValidateAppid_Error = -40005;
    /// <summary>AES 加密失败</summary>
    public const int EncryptAES_Error = -40006;
    /// <summary>AES 解密失败</summary>
    public const int DecryptAES_Error = -40007;
    /// <summary>解密后缓冲区非法</summary>
    public const int IllegalBuffer = -40008;

    /// <summary>微信自定义 PKCS7 填充块大小（32 字节，非 AES 标准的 16 字节）</summary>
    private const int BlockSize = 32;

    private readonly string _token;
    private readonly byte[] _aesKey;
    private readonly string _appid;

    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="token">公众平台/企业微信设置的 Token</param>
    /// <param name="encodingAESKey">公众平台/企业微信设置的 EncodingAESKey（43 字符 Base64）</param>
    /// <param name="appid">公众/企业应用的 AppID 或 CorpID</param>
    /// <exception cref="ArgumentException">EncodingAESKey 长度非法或解码后不为 32 字节</exception>
    public WXBizMsgCrypt(string token, string encodingAESKey, string appid)
    {
        _token = token;
        _appid = appid;

        if (string.IsNullOrEmpty(encodingAESKey) || encodingAESKey.Length != 43)
        {
            throw new ArgumentException("EncodingAESKey 必须为 43 字符", nameof(encodingAESKey));
        }

        // EncodingAESKey 为 43 字符的 Base64，末尾补 "=" 后解码得到 32 字节 AES 密钥
        _aesKey = Convert.FromBase64String(encodingAESKey + "=");
        if (_aesKey.Length != 32)
        {
            throw new ArgumentException("EncodingAESKey 解码后必须为 32 字节", nameof(encodingAESKey));
        }
    }

    /// <summary>
    /// 验证 URL，用于处理微信服务器 URL 验证请求（GET 回调）。
    /// 流程：校验签名 → 解密 echostr → 返回明文 echostr。
    /// </summary>
    /// <param name="sVerifyMsgSig">微信回调签名</param>
    /// <param name="sVerifyTimeStamp">时间戳</param>
    /// <param name="sVerifyNonce">随机数</param>
    /// <param name="sVerifyEchoStr">加密的 echostr</param>
    /// <param name="sEchoStr">输出的明文 echostr</param>
    /// <returns>0 表示成功，其他为错误码</returns>
    public int VerifyURL(string sVerifyMsgSig, string sVerifyTimeStamp, string sVerifyNonce,
        string sVerifyEchoStr, ref string sEchoStr)
    {
        // 1. 校验签名：SHA1(sort(token, timestamp, nonce, echostr)) == msg_signature
        int ret = VerifySignature(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, sVerifyEchoStr);
        if (ret != OK)
        {
            return ret;
        }

        // 2. 解密 echostr 并校验 AppID
        try
        {
            byte[] cipherBytes = Convert.FromBase64String(sVerifyEchoStr);
            (string message, string appid) = Decrypt(cipherBytes);
            if (appid != _appid)
            {
                return ValidateAppid_Error;
            }
            sEchoStr = message;
            return OK;
        }
        catch
        {
            return DecryptAES_Error;
        }
    }

    /// <summary>
    /// 解密回调消息（POST 回调）。
    /// 流程：解析 XML 提取 Encrypt → 校验签名 → 解密 → 校验 AppID。
    /// </summary>
    /// <param name="sMsgSignature">消息签名</param>
    /// <param name="sTimeStamp">时间戳</param>
    /// <param name="sNonce">随机数</param>
    /// <param name="sPostData">POST 回调的 XML 内容</param>
    /// <param name="sMsg">输出的明文消息 XML</param>
    /// <returns>0 表示成功，其他为错误码</returns>
    public int DecryptMsg(string sMsgSignature, string sTimeStamp, string sNonce,
        string sPostData, ref string sMsg)
    {
        // 1. 解析 XML，提取加密消息体 <Encrypt>
        string encrypt;
        try
        {
            XDocument doc = XDocument.Parse(sPostData);
            XElement? encryptElement = doc.Root?.Element("Encrypt");
            if (encryptElement == null || string.IsNullOrEmpty(encryptElement.Value))
            {
                return ParseXml_Error;
            }
            encrypt = encryptElement.Value;
        }
        catch
        {
            return ParseXml_Error;
        }

        // 2. 校验签名：SHA1(sort(token, timestamp, nonce, encrypt)) == msg_signature
        int ret = VerifySignature(sMsgSignature, sTimeStamp, sNonce, encrypt);
        if (ret != OK)
        {
            return ret;
        }

        // 3. 解密消息并校验 AppID
        try
        {
            byte[] cipherBytes = Convert.FromBase64String(encrypt);
            (string message, string appid) = Decrypt(cipherBytes);
            if (appid != _appid)
            {
                return ValidateAppid_Error;
            }
            sMsg = message;
            return OK;
        }
        catch
        {
            return DecryptAES_Error;
        }
    }

    /// <summary>
    /// 加密回复消息。
    /// 流程：AES 加密 → Base64 编码 → 计算 SHA1 签名 → 组装 XML。
    /// </summary>
    /// <param name="sReplyMsg">明文回复消息 XML</param>
    /// <param name="sTimeStamp">时间戳</param>
    /// <param name="sNonce">随机数</param>
    /// <param name="sEncryptMsg">输出的加密消息 XML（含 Encrypt/MsgSignature/TimeStamp/Nonce）</param>
    /// <returns>0 表示成功，其他为错误码</returns>
    public int EncryptMsg(string sReplyMsg, string sTimeStamp, string sNonce, ref string sEncryptMsg)
    {
        // 1. AES-256-CBC 加密并 Base64 编码
        string encrypt;
        try
        {
            byte[] cipherBytes = Encrypt(sReplyMsg);
            encrypt = Convert.ToBase64String(cipherBytes);
        }
        catch
        {
            return EncryptAES_Error;
        }

        // 2. 计算签名：SHA1(sort(token, timestamp, nonce, encrypt))
        string signature;
        try
        {
            signature = CalcSignature(_token, sTimeStamp, sNonce, encrypt);
        }
        catch
        {
            return CalcSignature_Error;
        }

        // 3. 组装加密回复 XML
        sEncryptMsg =
            $"<xml>\n" +
            $"<Encrypt><![CDATA[{encrypt}]]></Encrypt>\n" +
            $"<MsgSignature><![CDATA[{signature}]]></MsgSignature>\n" +
            $"<TimeStamp>{sTimeStamp}</TimeStamp>\n" +
            $"<Nonce><![CDATA[{sNonce}]]></Nonce>\n" +
            $"</xml>";

        return OK;
    }

    /// <summary>
    /// 校验签名：SHA1(sort(token, timestamp, nonce, encrypt)) == sig。
    /// </summary>
    private int VerifySignature(string sig, string timestamp, string nonce, string encrypt)
    {
        try
        {
            string calculated = CalcSignature(_token, timestamp, nonce, encrypt);
            return calculated == sig ? OK : ValidateSignature_Error;
        }
        catch
        {
            return CalcSignature_Error;
        }
    }

    /// <summary>
    /// 计算 SHA1 签名：将 token、timestamp、nonce、encrypt 按字典序排序后拼接，取 SHA1 小写十六进制。
    /// </summary>
    private static string CalcSignature(string token, string timestamp, string nonce, string encrypt)
    {
        string[] arr = { token, timestamp, nonce, encrypt };
        Array.Sort(arr, StringComparer.Ordinal);
        string raw = string.Concat(arr);
        byte[] hash = SHA1.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>
    /// AES-256-CBC 加密。
    /// 明文格式：random(16) + msg_len(4, 大端序) + msg + appid，再 PKCS7 填充。
    /// IV 为密钥前 16 字节。
    /// </summary>
    private byte[] Encrypt(string plainText)
    {
        byte[] msgBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] appidBytes = Encoding.UTF8.GetBytes(_appid);
        byte[] randomBytes = RandomNumberGenerator.GetBytes(16);

        // 组装明文：random(16) + msg_len(4, 大端序) + msg + appid
        byte[] content = new byte[16 + 4 + msgBytes.Length + appidBytes.Length];
        Buffer.BlockCopy(randomBytes, 0, content, 0, 16);
        BinaryPrimitives.WriteInt32BigEndian(content.AsSpan(16, 4), msgBytes.Length);
        Buffer.BlockCopy(msgBytes, 0, content, 20, msgBytes.Length);
        Buffer.BlockCopy(appidBytes, 0, content, 20 + msgBytes.Length, appidBytes.Length);

        // PKCS7 填充到 32 字节倍数（微信自定义块大小）
        byte[] padded = PKCS7Pad(content);

        // AES-256-CBC 加密，IV 为密钥前 16 字节
        using Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = _aesKey;
        aes.IV = _aesKey[..16];
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.None;

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        return encryptor.TransformFinalBlock(padded, 0, padded.Length);
    }

    /// <summary>
    /// AES-256-CBC 解密。
    /// 解密后移除 PKCS7 填充，解析出消息体和 AppID。
    /// IV 为密钥前 16 字节。
    /// </summary>
    private (string message, string appid) Decrypt(byte[] cipherBytes)
    {
        using Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = _aesKey;
        aes.IV = _aesKey[..16];
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.None;

        using ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        // 移除 PKCS7 填充
        byte[] unpadded = PKCS7Unpad(decrypted);

        // 解析：random(16) + msg_len(4, 大端序) + msg + appid
        if (unpadded.Length < 20)
        {
            throw new InvalidOperationException("解密后数据长度不足");
        }

        int msgLength = BinaryPrimitives.ReadInt32BigEndian(unpadded.AsSpan(16, 4));
        if (msgLength < 0 || 20 + msgLength > unpadded.Length)
        {
            throw new InvalidOperationException("消息长度字段非法");
        }

        string message = Encoding.UTF8.GetString(unpadded, 20, msgLength);
        string appid = Encoding.UTF8.GetString(unpadded, 20 + msgLength, unpadded.Length - 20 - msgLength);

        return (message, appid);
    }

    /// <summary>
    /// PKCS7 填充（微信自定义块大小 32 字节）。
    /// </summary>
    private static byte[] PKCS7Pad(byte[] data)
    {
        int padLen = BlockSize - (data.Length % BlockSize);
        byte[] result = new byte[data.Length + padLen];
        Buffer.BlockCopy(data, 0, result, 0, data.Length);
        for (int i = data.Length; i < result.Length; i++)
        {
            result[i] = (byte)padLen;
        }
        return result;
    }

    /// <summary>
    /// 移除 PKCS7 填充（微信自定义块大小 32 字节）。
    /// </summary>
    private static byte[] PKCS7Unpad(byte[] data)
    {
        if (data.Length == 0)
        {
            return data;
        }

        int padLen = data[data.Length - 1];
        if (padLen < 1 || padLen > BlockSize)
        {
            return data;
        }

        return data[..^padLen];
    }
}
