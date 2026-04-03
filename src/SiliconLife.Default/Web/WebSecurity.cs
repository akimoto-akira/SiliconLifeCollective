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

namespace SiliconLife.Default.Web;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class WebCodeAttribute : Attribute
{
    public string Description { get; set; } = "";
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class WebHiddenAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class WebRouteAttribute : Attribute
{
    public string Path { get; set; } = "";
    public string Method { get; set; } = "GET";
}

public class WebSecurity
{
    private readonly HashSet<string> _allowedIPs = new();
    private readonly HashSet<string> _deniedIPs = new();
    private readonly object _lock = new();
    private bool _useBlacklist = true;

    public bool IsEnabled { get; set; } = false;

    public void SetBlacklistMode(bool blacklist)
    {
        _useBlacklist = blacklist;
    }

    public void AddAllowedIP(string ip)
    {
        lock (_lock)
        {
            _allowedIPs.Add(ip);
        }
    }

    public void AddDeniedIP(string ip)
    {
        lock (_lock)
        {
            _deniedIPs.Add(ip);
        }
    }

    public void RemoveAllowedIP(string ip)
    {
        lock (_lock)
        {
            _allowedIPs.Remove(ip);
        }
    }

    public void RemoveDeniedIP(string ip)
    {
        lock (_lock)
        {
            _deniedIPs.Remove(ip);
        }
    }

    public void ClearAllowedIPs()
    {
        lock (_lock)
        {
            _allowedIPs.Clear();
        }
    }

    public void ClearDeniedIPs()
    {
        lock (_lock)
        {
            _deniedIPs.Clear();
        }
    }

    public bool IsAllowed(string ip)
    {
        lock (_lock)
        {
            if (_deniedIPs.Contains(ip))
                return false;

            if (_useBlacklist)
                return true;

            if (_allowedIPs.Count == 0)
                return true;

            return _allowedIPs.Contains(ip);
        }
    }

    public IReadOnlyList<string> GetAllowedIPs()
    {
        lock (_lock)
        {
            return _allowedIPs.ToList();
        }
    }

    public IReadOnlyList<string> GetDeniedIPs()
    {
        lock (_lock)
        {
            return _deniedIPs.ToList();
        }
    }

    public void LoadFromConfig(Dictionary<string, object> config)
    {
        if (config.TryGetValue("web_security_enabled", out var enabled))
        {
            if (enabled is bool b)
                IsEnabled = b;
            else if (bool.TryParse(enabled?.ToString(), out var parsed))
                IsEnabled = parsed;
        }

        if (config.TryGetValue("ip_whitelist", out var whitelist))
        {
            ClearAllowedIPs();
            if (whitelist is IEnumerable<object> list)
            {
                foreach (var item in list)
                {
                    AddAllowedIP(item?.ToString() ?? "");
                }
            }
        }

        if (config.TryGetValue("ip_blacklist", out var blacklist))
        {
            ClearDeniedIPs();
            if (blacklist is IEnumerable<object> list)
            {
                foreach (var item in list)
                {
                    AddDeniedIP(item?.ToString() ?? "");
                }
            }
        }
    }
}

public class WebCodeBrowser
{
    private readonly Dictionary<string, Type> _registeredTypes = new();

    public void RegisterType(Type type)
    {
        var attr = type.GetCustomAttributes(typeof(WebCodeAttribute), true).FirstOrDefault() as WebCodeAttribute;
        var key = type.FullName ?? type.Name;
        
        _registeredTypes[key] = type;
    }

    public void RegisterAssemblyTypes(string assemblyName)
    {
        try
        {
            var assembly = System.Reflection.Assembly.Load(assemblyName);
            var types = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(WebCodeAttribute), true).Any());
            
            foreach (var type in types)
            {
                RegisterType(type);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to register assembly types: {ex.Message}");
        }
    }

    public List<WebCodeTypeInfo> GetAllTypes()
    {
        var result = new List<WebCodeTypeInfo>();
        
        foreach (var kvp in _registeredTypes)
        {
            var type = kvp.Value;
            var attr = type.GetCustomAttributes(typeof(WebCodeAttribute), true).FirstOrDefault() as WebCodeAttribute;
            
            result.Add(new WebCodeTypeInfo
            {
                Name = type.Name,
                FullName = type.FullName ?? "",
                Description = attr?.Description ?? "",
                IsClass = type.IsClass,
                IsInterface = type.IsInterface,
                IsEnum = type.IsEnum,
                Namespace = type.Namespace ?? "",
                Properties = type.GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(WebHiddenAttribute), true).Length == 0)
                    .Select(p => new WebCodePropertyInfo
                    {
                        Name = p.Name,
                        Type = p.PropertyType.Name,
                        CanRead = p.CanRead,
                        CanWrite = p.CanWrite
                    }).ToList(),
                Methods = type.GetMethods()
                    .Where(m => !m.IsSpecialName && m.DeclaringType == type)
                    .Select(m => new WebCodeMethodInfo
                    {
                        Name = m.Name,
                        ReturnType = m.ReturnType.Name,
                        Parameters = m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}").ToList()
                    }).ToList()
            });
        }
        
        return result;
    }

    public WebCodeTypeInfo? GetType(string fullName)
    {
        if (_registeredTypes.TryGetValue(fullName, out var type))
        {
            var attr = type.GetCustomAttributes(typeof(WebCodeAttribute), true).FirstOrDefault() as WebCodeAttribute;
            
            return new WebCodeTypeInfo
            {
                Name = type.Name,
                FullName = type.FullName ?? "",
                Description = attr?.Description ?? "",
                IsClass = type.IsClass,
                IsInterface = type.IsInterface,
                IsEnum = type.IsEnum,
                Namespace = type.Namespace ?? "",
                Properties = type.GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(WebHiddenAttribute), true).Length == 0)
                    .Select(p => new WebCodePropertyInfo
                    {
                        Name = p.Name,
                        Type = p.PropertyType.Name,
                        CanRead = p.CanRead,
                        CanWrite = p.CanWrite
                    }).ToList(),
                Methods = type.GetMethods()
                    .Where(m => !m.IsSpecialName && m.DeclaringType == type)
                    .Select(m => new WebCodeMethodInfo
                    {
                        Name = m.Name,
                        ReturnType = m.ReturnType.Name,
                        Parameters = m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}").ToList()
                    }).ToList()
            };
        }
        
        return null;
    }
}

public class WebCodeTypeInfo
{
    public string Name { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsClass { get; set; }
    public bool IsInterface { get; set; }
    public bool IsEnum { get; set; }
    public string Namespace { get; set; } = "";
    public List<WebCodePropertyInfo> Properties { get; set; } = new();
    public List<WebCodeMethodInfo> Methods { get; set; } = new();
}

public class WebCodePropertyInfo
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
}

public class WebCodeMethodInfo
{
    public string Name { get; set; } = "";
    public string ReturnType { get; set; } = "";
    public List<string> Parameters { get; set; } = new();
}
