﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AoSConsole.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoSConsole.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///
        ///&lt;!--&gt;
        ///Template for a model profile
        ///
        ///Attributes: Name
        ///
        ///Integers:
        ///Move (Inches)
        ///Save
        ///Bravery
        ///Wounds
        ///Unit Size
        ///
        ///&lt;model name = &quot;&quot;&gt;
        ///  &lt;stats&gt;
        ///    &lt;move&gt;&lt;/move&gt;
        ///    &lt;save&gt;&lt;/save&gt;
        ///    &lt;bravery&gt;&lt;/bravery&gt;
        ///    &lt;wounds&gt;&lt;/wounds&gt;
        ///  &lt;/stats&gt;
        ///  &lt;size&gt;&lt;/size&gt;
        ///  &lt;weapons&gt;&lt;/weapons&gt;
        ///  &lt;upgrades&gt;&lt;/upgrades&gt;
        ///  &lt;abilities&gt;&lt;/abilities&gt;
        ///&lt;/model&gt;
        ///&lt;--&gt;
        ///
        ///&lt;seraphon&gt;
        ///    &lt;model name = &quot;Saurus Warriors&quot;&gt;
        ///        &lt;stats&gt;
        ///            &lt;move&gt;5&lt;/move&gt;
        ///            &lt;sav [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string seraphon_models {
            get {
                return ResourceManager.GetString("seraphon_models", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///
        ///&lt;!--&gt;Template for a weapon profile
        ///Attributes: Name
        ///Integers: Range
        ///          Attacks
        ///          To tohit
        ///          To towound
        ///          Rend
        ///          Damage
        ///&lt;weapon name = &quot;&quot;&gt;
        ///    &lt;range&gt;&lt;/range&gt;
        ///    &lt;attacks&gt;&lt;/attacks&gt;
        ///    &lt;tohit&gt;&lt;/tohit&gt;
        ///    &lt;towound&gt;&lt;/towound&gt;
        ///    &lt;rend&gt;&lt;/rend&gt;
        ///    &lt;damage&gt;/&lt;damage&gt;
        ///&lt;/weapon&gt;&lt;--&gt;
        ///
        ///&lt;seraphon&gt;
        ///    &lt;melee&gt;
        ///        &lt;weapon name = &quot;Celestite Club&quot;&gt;
        ///            &lt;range&gt;1&lt;/range&gt;
        ///            &lt;attacks&gt;1&lt;/attacks&gt;
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string seraphon_weapons {
            get {
                return ResourceManager.GetString("seraphon_weapons", resourceCulture);
            }
        }
    }
}
