﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FreeSCADA.Designer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FreeSCADA.Designer.StringResources", typeof(StringResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FreeSCADA 2 files (*.fs2)|*.fs2|All files (*.*)|*.*.
        /// </summary>
        internal static string FileDialogFilter {
            get {
                return ResourceManager.GetString("FileDialogFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Designer.
        /// </summary>
        internal static string MainWindowName {
            get {
                return ResourceManager.GetString("MainWindowName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [{0}] - Designer.
        /// </summary>
        internal static string MainWindowNameEx {
            get {
                return ResourceManager.GetString("MainWindowNameEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {unsaved project file}.
        /// </summary>
        internal static string UnsavedProjectName {
            get {
                return ResourceManager.GetString("UnsavedProjectName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Untitled.
        /// </summary>
        internal static string UntitledSchema {
            get {
                return ResourceManager.GetString("UntitledSchema", resourceCulture);
            }
        }
    }
}
