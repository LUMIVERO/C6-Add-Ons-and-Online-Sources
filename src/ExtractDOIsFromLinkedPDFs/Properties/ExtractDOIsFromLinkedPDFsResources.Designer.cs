﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExtractDOIsFromLinkedPDFsResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExtractDOIsFromLinkedPDFsResources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.Properties.ExtractDOIsFromLinkedPD" +
                            "FsResources", typeof(ExtractDOIsFromLinkedPDFsResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
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
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap addon {
            get {
                object obj = ResourceManager.GetObject("addon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die [en] DOIs in PDFs ermitteln ähnelt.
        /// </summary>
        internal static string CommandButtonText {
            get {
                return ResourceManager.GetString("CommandButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die [en] PDF-Attachments werden ermittelt ähnelt.
        /// </summary>
        internal static string GenericDialogFetchAttributsTitle {
            get {
                return ResourceManager.GetString("GenericDialogFetchAttributsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die [en] Dieses Cloud-Projekt enthält Attachments, die auf Ihrem Computer nicht lokal verfügbar sind. Sie müssen zunächst alle Attachments herunterladen, um die DOIs extrahieren zu können. Rufen Sie dazu folgende Funktion auf: Datei &gt; Dieses Projekt &gt; Eigenschaften &gt; Anhänge &gt; Alle Attachments lokal vorhalten. ähnelt.
        /// </summary>
        internal static string UserMessageUnavailableAttachements {
            get {
                return ResourceManager.GetString("UserMessageUnavailableAttachements", resourceCulture);
            }
        }
    }
}
