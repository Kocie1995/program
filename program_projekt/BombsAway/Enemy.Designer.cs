﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BombsAway {
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
    internal class Enemy {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Enemy() {
        }
        
   
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BombsAway.Enemy", typeof(Enemy).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
     
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
   
        internal static System.Drawing.Bitmap Bomb {
            get {
                object obj = ResourceManager.GetObject("Bomb", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
      
        internal static System.Drawing.Bitmap Enemy_left {
            get {
                object obj = ResourceManager.GetObject("Enemy-left", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
       
        internal static System.Drawing.Bitmap Enemy_right {
            get {
                object obj = ResourceManager.GetObject("Enemy-right", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
     
        internal static System.Drawing.Bitmap Rocket_L {
            get {
                object obj = ResourceManager.GetObject("Rocket_L", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
    
        internal static System.Drawing.Bitmap Rocket_R {
            get {
                object obj = ResourceManager.GetObject("Rocket_R", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
