﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DIS.Business.Library.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DIS.Business.Library.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to There are some fulfillments already been timed out..
        /// </summary>
        internal static string ErrorFulfillmentTimedOut {
            get {
                return ResourceManager.GetString("ErrorFulfillmentTimedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Certificate not found..
        /// </summary>
        internal static string Exception_CertificateNotFound {
            get {
                return ResourceManager.GetString("Exception_CertificateNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File path does not exist..
        /// </summary>
        internal static string Exception_FilePathNotFound {
            get {
                return ResourceManager.GetString("Exception_FilePathNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Certificate error, please try run it as administrator..
        /// </summary>
        internal static string Exception_GetprivateKeyError {
            get {
                return ResourceManager.GetString("Exception_GetprivateKeyError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file content is not correct, please confirm it..
        /// </summary>
        internal static string Exception_ImportFileInvalid {
            get {
                return ResourceManager.GetString("Exception_ImportFileInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid file name or path..
        /// </summary>
        internal static string Exception_InvalidPath {
            get {
                return ResourceManager.GetString("Exception_InvalidPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Permissions error, please run it as administrator..
        /// </summary>
        internal static string Exception_PermissionsMsg {
            get {
                return ResourceManager.GetString("Exception_PermissionsMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data in database have been changed, please re-select keys..
        /// </summary>
        internal static string ExportKeys_DataChangeError {
            get {
                return ResourceManager.GetString("ExportKeys_DataChangeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Keys in file do not match Down Level System configuration, please confirm it..
        /// </summary>
        internal static string ImportKey_SourceFromDLSError {
            get {
                return ResourceManager.GetString("ImportKey_SourceFromDLSError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Keys in file do not match ULS and DLS configuration, please confirm it..
        /// </summary>
        internal static string ImportKey_SourceFromError {
            get {
                return ResourceManager.GetString("ImportKey_SourceFromError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Keys in file do not match Up Level System configuration, please confirm it..
        /// </summary>
        internal static string ImportKey_SourceFromULSError {
            get {
                return ResourceManager.GetString("ImportKey_SourceFromULSError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File does not match your Customer Number..
        /// </summary>
        internal static string KeyProxy_WrongExportTarget {
            get {
                return ResourceManager.GetString("KeyProxy_WrongExportTarget", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File does not match your User and Password!.
        /// </summary>
        internal static string KeyProxy_WrongFileUserPsd {
            get {
                return ResourceManager.GetString("KeyProxy_WrongFileUserPsd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can not remove the last manager account in system..
        /// </summary>
        internal static string UserProxy_CannotRemoveLastAdmin {
            get {
                return ResourceManager.GetString("UserProxy_CannotRemoveLastAdmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your Login ID or Password is incorrect..
        /// </summary>
        internal static string UserProxy_LoginIDOrPasswordIncorrect {
            get {
                return ResourceManager.GetString("UserProxy_LoginIDOrPasswordIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Login ID has already been used..
        /// </summary>
        internal static string UserProxy_UserAlreadyExist {
            get {
                return ResourceManager.GetString("UserProxy_UserAlreadyExist", resourceCulture);
            }
        }
    }
}