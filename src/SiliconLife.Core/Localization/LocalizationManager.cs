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

namespace SiliconLife.Collective;

/// <summary>
/// Localization manager for managing and retrieving localization instances
/// </summary>
public class LocalizationManager
{
    private static readonly Lazy<LocalizationManager> _instance = new Lazy<LocalizationManager>(() => new LocalizationManager());
    private readonly Dictionary<Language, Func<LocalizationBase>> _localizationFactories;
    private readonly Dictionary<Language, LocalizationBase> _cache;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static LocalizationManager Instance => _instance.Value;

    private LocalizationManager()
    {
        _localizationFactories = new Dictionary<Language, Func<LocalizationBase>>();
        _cache = new Dictionary<Language, LocalizationBase>();
    }

    /// <summary>
    /// Registers a localization factory for the specified language
    /// </summary>
    /// <param name="language">The language to register</param>
    /// <param name="factory">The factory function to create the localization instance</param>
    public void Register(Language language, Func<LocalizationBase> factory)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        _localizationFactories[language] = factory;
        _cache.Remove(language);
    }

    /// <summary>
    /// Registers a localization type for the specified language
    /// </summary>
    /// <typeparam name="T">The localization type that inherits from LocalizationBase</typeparam>
    /// <param name="language">The language to register</param>
    public void Register<T>(Language language) where T : LocalizationBase, new()
    {
        Register(language, () => new T());
    }

    /// <summary>
    /// Gets the localization instance for the specified language
    /// </summary>
    /// <param name="language">The language to get</param>
    /// <returns>The localization instance</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the language is not registered</exception>
    public LocalizationBase GetLocalization(Language language)
    {
        if (_cache.TryGetValue(language, out LocalizationBase? cached))
        {
            return cached;
        }

        if (_localizationFactories.TryGetValue(language, out Func<LocalizationBase>? factory))
        {
            LocalizationBase instance = factory();
            _cache[language] = instance;
            return instance;
        }

        throw new KeyNotFoundException($"No localization registered for language: {language}");
    }

    /// <summary>
    /// Tries to get the localization instance for the specified language
    /// </summary>
    /// <param name="language">The language to get</param>
    /// <param name="localization">The output localization instance</param>
    /// <returns>True if the localization was found, false otherwise</returns>
    public bool TryGetLocalization(Language language, out LocalizationBase? localization)
    {
        try
        {
            localization = GetLocalization(language);
            return true;
        }
        catch
        {
            localization = null;
            return false;
        }
    }

    /// <summary>
    /// Clears the cache of localization instances
    /// </summary>
    public void ClearCache()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Gets all registered languages
    /// </summary>
    /// <returns>A collection of registered languages</returns>
    public IEnumerable<Language> GetRegisteredLanguages()
    {
        return _localizationFactories.Keys;
    }

    /// <summary>
    /// Checks if a language is registered
    /// </summary>
    /// <param name="language">The language to check</param>
    /// <returns>True if the language is registered, false otherwise</returns>
    public bool IsRegistered(Language language)
    {
        return _localizationFactories.ContainsKey(language);
    }
}
