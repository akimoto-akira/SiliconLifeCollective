// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Security.Cryptography;
using System.Text;

namespace SiliconLife.Collective;

/// <summary>
/// AES-256 encryption/decryption for silicon being custom code storage.
/// Key is derived from the being's GUID using PBKDF2.
/// Password is the GUID string in uppercase.
/// </summary>
public static class CodeEncryption
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(CodeEncryption));
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int IvSize = 16;

    /// <summary>
    /// Converts a GUID to its uppercase string representation, used as the encryption password.
    /// </summary>
    /// <param name="guid">The silicon being's GUID</param>
    /// <returns>The GUID string in uppercase</returns>
    public static string GuidToPassword(Guid guid)
    {
        return guid.ToString().ToUpperInvariant();
    }

    /// <summary>
    /// Derives a 256-bit AES key from the being's GUID using PBKDF2.
    /// </summary>
    /// <param name="guid">The silicon being's GUID</param>
    /// <param name="salt">Optional salt (generated if null)</param>
    /// <returns>The derived key bytes and the salt used</returns>
    public static (byte[] Key, byte[] Salt) DeriveKey(Guid guid, byte[]? salt = null)
    {
        salt ??= RandomNumberGenerator.GetBytes(SaltSize);

#pragma warning disable SYSLIB0041
        using var pbkdf2 = new Rfc2898DeriveBytes(
            GuidToPassword(guid),
            salt,
            Iterations);
#pragma warning restore SYSLIB0041

        return (pbkdf2.GetBytes(KeySize / 8), salt);
    }

    /// <summary>
    /// Encrypts source code bytes using AES-256-CBC with PBKDF2 key derivation.
    /// Output format: [Salt (16 bytes)] [IV (16 bytes)] [Ciphertext]
    /// </summary>
    /// <param name="plainBytes">The source code bytes to encrypt</param>
    /// <param name="guid">The silicon being's GUID (used to derive the encryption key)</param>
    /// <returns>Encrypted bytes: salt + IV + ciphertext</returns>
    public static byte[] Encrypt(byte[] plainBytes, Guid guid)
    {
        ArgumentNullException.ThrowIfNull(plainBytes);

        _logger.Debug(null, "Encrypting data for GUID {0}, input size={1}", guid, plainBytes.Length);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        (byte[] key, _) = DeriveKey(guid, salt);

        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        byte[] result = new byte[SaltSize + IvSize + cipherBytes.Length];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(aes.IV, 0, result, SaltSize, IvSize);
        Buffer.BlockCopy(cipherBytes, 0, result, SaltSize + IvSize, cipherBytes.Length);

        return result;
    }

    /// <summary>
    /// Encrypts a source code string using AES-256-CBC with PBKDF2 key derivation.
    /// </summary>
    /// <param name="plainText">The source code string to encrypt</param>
    /// <param name="guid">The silicon being's GUID</param>
    /// <returns>Encrypted bytes: salt + IV + ciphertext</returns>
    public static byte[] Encrypt(string plainText, Guid guid)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        return Encrypt(Encoding.UTF8.GetBytes(plainText), guid);
    }

    /// <summary>
    /// Tries to decrypt AES-256-CBC encrypted data.
    /// Input format: [Salt (16 bytes)] [IV (16 bytes)] [Ciphertext]
    /// </summary>
    /// <param name="encryptedBytes">The encrypted bytes (salt + IV + ciphertext)</param>
    /// <param name="guid">The silicon being's GUID</param>
    /// <param name="decryptedBytes">The decrypted source code bytes</param>
    /// <returns>True if decryption succeeded, false if the data is invalid or wrong GUID</returns>
    public static bool TryDecrypt(byte[] encryptedBytes, Guid guid, out byte[]? decryptedBytes)
    {
        decryptedBytes = null;

        if (encryptedBytes == null || encryptedBytes.Length < SaltSize + IvSize + 1)
        {
            _logger.Warn(null, "Decryption failed: data too short ({0} bytes)", encryptedBytes?.Length ?? 0);
            return false;
        }

        try
        {
            byte[] salt = new byte[SaltSize];
            byte[] iv = new byte[IvSize];
            byte[] cipherBytes = new byte[encryptedBytes.Length - SaltSize - IvSize];

            Buffer.BlockCopy(encryptedBytes, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(encryptedBytes, SaltSize, iv, 0, IvSize);
            Buffer.BlockCopy(encryptedBytes, SaltSize + IvSize, cipherBytes, 0, cipherBytes.Length);

            (byte[] key, _) = DeriveKey(guid, salt);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            _logger.Debug(null, "Decryption successful for GUID {0}", guid);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Decryption failed for GUID {0}: {1}", guid, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Tries to decrypt AES-256-CBC encrypted data to a UTF-8 string.
    /// </summary>
    /// <param name="encryptedBytes">The encrypted bytes</param>
    /// <param name="guid">The silicon being's GUID</param>
    /// <param name="decryptedText">The decrypted source code string</param>
    /// <returns>True if decryption succeeded</returns>
    public static bool TryDecryptToString(byte[] encryptedBytes, Guid guid, out string? decryptedText)
    {
        decryptedText = null;

        if (!TryDecrypt(encryptedBytes, guid, out byte[]? bytes) || bytes == null)
        {
            return false;
        }

        try
        {
            decryptedText = Encoding.UTF8.GetString(bytes);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
