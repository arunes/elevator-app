﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elevator.Messages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Elevator.Messages.Errors", typeof(Errors).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevator can not be called from floor {0}. Building only has {1} floors..
        /// </summary>
        public static string ERR01 {
            get {
                return ResourceManager.GetString("ERR01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevator can not be called from floor {0}. Minimum floor elevator can go is 1..
        /// </summary>
        public static string ERR02 {
            get {
                return ResourceManager.GetString("ERR02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevator can not be called to go up. Calling floor is the top floor..
        /// </summary>
        public static string ERR03 {
            get {
                return ResourceManager.GetString("ERR03", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevator can not be called to go down. Calling floor is the bottom floor..
        /// </summary>
        public static string ERR04 {
            get {
                return ResourceManager.GetString("ERR04", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevator can not go to floor {0}. Building only has {1} floors..
        /// </summary>
        public static string ERR05 {
            get {
                return ResourceManager.GetString("ERR05", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevator can not go to floor {0}. Minimum floor elevator can go is 1..
        /// </summary>
        public static string ERR06 {
            get {
                return ResourceManager.GetString("ERR06", resourceCulture);
            }
        }
    }
}