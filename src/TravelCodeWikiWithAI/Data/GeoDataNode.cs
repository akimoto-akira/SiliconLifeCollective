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

using System.Reflection;
using SiliconLife.Collective;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 轻量级数据节点基类，替代旧项目的 XMLBase。
/// 仅保留父子对象关系和子对象创建工厂方法，去除 XML 序列化、[Browsable] 绑定、FindType() 反射。
/// Lightweight data node base class replacing legacy XMLBase.
/// Only retains parent-child relationships and child creation factory methods,
/// removing XML serialization, [Browsable] binding, and FindType() reflection.
/// </summary>
public abstract class GeoDataNode
{
    /// <summary>
    /// 父对象引用 / Parent object reference
    /// </summary>
    public GeoDataNode? _parent;

    /// <summary>
    /// 基础路径，从父对象获取当前对象的路径 / Base path, obtained from parent object
    /// </summary>
    public virtual string? BasePath
    {
        get
        {
            if (_parent == null)
            {
                return null;
            }
            else
            {
                return _parent.GetObjectPath(this);
            }
        }
    }

    /// <summary>
    /// 获取对象在父对象中的路径 / Get the path of this object in parent
    /// </summary>
    /// <param name="obj">要查找路径的对象 / Object to find path for</param>
    /// <returns>对象路径字符串 / Object path string</returns>
    public virtual string GetObjectPath(object obj)
    {
        return "unPath";
    }

    /// <summary>
    /// 创建子对象 / Create child object
    /// 通过反射分析构造函数参数类型，自动传入正确的父对象引用。
    /// Creates child objects via reflection, analyzing constructor parameter types
    /// to automatically pass the correct parent reference.
    /// </summary>
    /// <param name="t">子对象类型 / Child object type</param>
    /// <param name="args">构造参数 / Constructor arguments</param>
    /// <returns>创建的子对象 / Created child object</returns>
    public virtual object? CreateChild(Type t, params object[] args)
    {
        ConstructorInfo[] cis = t.GetConstructors();
        int needPath = 0;

        foreach (ConstructorInfo a in cis)
        {
            ParameterInfo[] b = a.GetParameters();
            if (b.Length == 1)
            {
                ParameterInfo c = b[0];
                if (c.ParameterType == typeof(GeoDataNode) || c.ParameterType == typeof(GeoDataBase))
                {
                    needPath = 1;
                }
            }
        }

        if (needPath == 1)
        {
            return (GeoDataNode?)ServiceLocator.Instance.ObjectFactory?.CreateInstance(t, args[0])!;
        }
        else
        {
            return (GeoDataNode?)ServiceLocator.Instance.ObjectFactory?.CreateInstance(t)!;
        }
    }

    /// <summary>
    /// 将数据保存到 IStorage / Save data to IStorage
    /// </summary>
    /// <param name="storage">存储接口 / Storage interface</param>
    /// <param name="key">存储键 / Storage key</param>
    public virtual void SaveToStorage(IStorage storage, string key)
    {
        storage.Write(key, this);
    }

    /// <summary>
    /// 从 IStorage 加载数据 / Load data from IStorage
    /// 返回单个对象，若存储中无数据则返回 null
    /// Returns a single object; returns null if no data is found in storage
    /// </summary>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <param name="storage">存储接口 / Storage interface</param>
    /// <param name="key">存储键 / Storage key</param>
    /// <returns>加载的单个对象，或 null / Loaded single object, or null</returns>
    public static T? LoadFromStorage<T>(IStorage storage, string key) where T : class
    {
        return storage.Read<T>(key).FirstOrDefault();
    }
}
