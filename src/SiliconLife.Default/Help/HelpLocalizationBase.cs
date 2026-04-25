// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

namespace SiliconLife.Default.Help;

/// <summary>
/// Abstract base class for help documentation localization.
/// Defines all help document content as abstract properties.
/// </summary>
public abstract class HelpLocalizationBase
{
    #region Help Documents (Markdown Content)
    
    /// <summary>Getting Started</summary>
    public abstract string GettingStarted { get; }
    
    /// <summary>Being Management</summary>
    public abstract string BeingManagement { get; }
    
    /// <summary>Chat System</summary>
    public abstract string ChatSystem { get; }
    
    /// <summary>Tasks and Timers</summary>
    public abstract string TaskTimer { get; }
    
    /// <summary>Permission Management</summary>
    public abstract string Permission { get; }
    
    /// <summary>Configuration</summary>
    public abstract string Config { get; }
    
    /// <summary>FAQ</summary>
    public abstract string FAQ { get; }
    
    #endregion

    #region Help Document Titles (Display Titles)
    
    /// <summary>Getting Started Title</summary>
    public abstract string GettingStarted_Title { get; }
    
    /// <summary>Being Management Title</summary>
    public abstract string BeingManagement_Title { get; }
    
    /// <summary>Chat System Title</summary>
    public abstract string ChatSystem_Title { get; }
    
    /// <summary>Tasks and Timers Title</summary>
    public abstract string TaskTimer_Title { get; }
    
    /// <summary>Permission Management Title</summary>
    public abstract string Permission_Title { get; }
    
    /// <summary>Configuration Title</summary>
    public abstract string Config_Title { get; }
    
    /// <summary>FAQ Title</summary>
    public abstract string FAQ_Title { get; }
    
    #endregion

    #region Help Document Tags (Search Tags)
    
    /// <summary>Getting Started Tags</summary>
    public abstract string[] GettingStarted_Tags { get; }
    
    /// <summary>Being Management Tags</summary>
    public abstract string[] BeingManagement_Tags { get; }
    
    /// <summary>Chat System Tags</summary>
    public abstract string[] ChatSystem_Tags { get; }
    
    /// <summary>Tasks and Timers Tags</summary>
    public abstract string[] TaskTimer_Tags { get; }
    
    /// <summary>Permission Management Tags</summary>
    public abstract string[] Permission_Tags { get; }
    
    /// <summary>Configuration Tags</summary>
    public abstract string[] Config_Tags { get; }
    
    /// <summary>FAQ Tags</summary>
    public abstract string[] FAQ_Tags { get; }
    
    #endregion
}
