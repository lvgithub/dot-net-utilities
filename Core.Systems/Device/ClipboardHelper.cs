using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ClipboardProxy = System.Windows.Forms.Clipboard;
using System;
using System.Runtime.InteropServices;


namespace Core.Systems
{
    /// <summary>Provides methods for manipulating the Clipboard.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public class ClipboardHepler
    {
        #region Methods

        /// <summary>Clears the Clipboard.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void Clear()
        {
            ClipboardProxy.Clear();
        }

        /// <summary>Indicates whether the Clipboard contains audio data.</summary>
        /// <returns>True if audio data is stored on the Clipboard; otherwise False.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static bool ContainsAudio()
        {
            return ClipboardProxy.ContainsAudio();
        }

        /// <summary>Indicates whether the Clipboard contains data in the specified custom format.</summary>
        /// <returns>True if data in the specified custom format is stored on the Clipboard; otherwise False.</returns>
        /// <param name="format">String. Name of the custom format to be checked. Required. </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static bool ContainsData(string format)
        {
            return ClipboardProxy.ContainsData(format);
        }

        /// <summary>Returns a Boolean indicating whether the Clipboard contains a file drop list.</summary>
        /// <returns>True if a file drop list is stored on the Clipboard; otherwise False.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static bool ContainsFileDropList()
        {
            return ClipboardProxy.ContainsFileDropList();
        }


        /// <summary>Returns a Boolean indicating whether an image is stored on the Clipboard.</summary>
        /// <returns>True if an image is stored on the Clipboard; otherwise False.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static bool ContainsImage()
        {
            return ClipboardProxy.ContainsImage();
        }


        /// <summary>Determines if there is text on the Clipboard.</summary>
        /// <returns>True if the Clipboard contains text; otherwise False.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static bool ContainsText()
        {
            return ClipboardProxy.ContainsText();
        }

        /// <summary>Determines if there is text on the Clipboard.</summary>
        /// <returns>True if the Clipboard contains text; otherwise False.</returns>
        /// <param name="format"><see cref="T:System.Windows.Forms.TextDataFormat"></see>. If specified, identifies what text format to be checked for. Required. </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static bool ContainsText(TextDataFormat format)
        {
            return ClipboardProxy.ContainsText(format);
        }


        /// <summary>Retrieves an audio stream from the Clipboard.</summary>
        /// <returns><see cref="T:System.IO.Stream"></see></returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static Stream GetAudioStream()
        {
            return ClipboardProxy.GetAudioStream();
        }


        /// <summary>Retrieves data in a custom format from the Clipboard.</summary>
        /// <returns>Object.</returns>
        /// <param name="format">String. Name of the data format. Required. </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static object GetData(string format)
        {
            return ClipboardProxy.GetData(format);
        }


        /// <summary>Retrieves data from the Clipboard as an <see cref="T:System.Windows.Forms.IDataObject"></see>.</summary>
        /// <returns><see cref="T:System.Windows.Forms.IDataObject"></see></returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static IDataObject GetDataObject()
        {
            return ClipboardProxy.GetDataObject();
        }


        /// <summary>Retrieves a collection of strings representing file names from the Clipboard.</summary>
        /// <returns><see cref="T:System.Collections.Specialized.StringCollection"></see></returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static StringCollection GetFileDropList()
        {
            return ClipboardProxy.GetFileDropList();
        }


        /// <summary>Retrieves an image from the Clipboard.</summary>
        /// <returns><see cref="T:System.Drawing.Image"></see></returns>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static Image GetImage()
        {
            return ClipboardProxy.GetImage();
        }


        /// <summary>Retrieves text from the Clipboard.</summary>
        /// <returns>String.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static string GetText()
        {
            return ClipboardProxy.GetText();
        }

        /// <summary>Retrieves text from the Clipboard.</summary>
        /// <returns>String.</returns>
        /// <param name="format"><see cref="T:System.Windows.Forms.TextDataFormat"></see>. If specified, identifies what text format should be retrieved. Default is <see cref="F:System.Windows.Forms.TextDataFormat.CommaSeparatedValue"></see>. Required. </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static string GetText(TextDataFormat format)
        {
            return ClipboardProxy.GetText(format);
        }

        /// <summary>Writes audio data to the Clipboard.</summary>
        /// <param name="audioStream"><see cref="T:System.IO.Stream"></see> Audio data to be written to the clipboard. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetAudio(Stream audioStream)
        {
            ClipboardProxy.SetAudio(audioStream);
        }

        /// <summary>Writes audio data to the Clipboard.</summary>
        /// <param name="audioBytes">Byte array. Audio data to be written to the Clipboard. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetAudio(byte[] audioBytes)
        {
            ClipboardProxy.SetAudio(audioBytes);
        }



        /// <summary>Writes data in a custom format to the Clipboard.</summary>
        /// <param name="data">Object. Data object to be written to the Clipboard. Required. </param>
        /// <param name="format">String. Format of data. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetData(string format, object data)
        {
            ClipboardProxy.SetData(format, data);
        }

        /// <summary>Writes a <see cref="T:System.Windows.Forms.DataObject"></see> to the Clipboard.</summary>
        /// <param name="data"><see cref="T:System.Windows.Forms.DataObject"></see>. Data object to be written to the Clipboard. Required. </param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static void SetDataObject(DataObject data)
        {
            ClipboardProxy.SetDataObject(data);
        }


        /// <summary>Writes a collection of strings representing file paths to the Clipboard.</summary>
        /// <param name="filePaths"><see cref="T:System.Collections.Specialized.StringCollection"></see>. List of file names. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetFileDropList(StringCollection filePaths)
        {
            ClipboardProxy.SetFileDropList(filePaths);
        }

        /// <summary>Writes an image to the Clipboard.</summary>
        /// <param name="image"><see cref="T:System.Drawing.Image"></see>. Image to be written. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetImage(Image image)
        {
            ClipboardProxy.SetImage(image);
        }

        /// <summary>Writes text to the Clipboard.</summary>
        /// <param name="text">String. Text to be written. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetText(string text)
        {
            ClipboardProxy.SetText(text);
        }


        /// <summary>Writes text to the Clipboard.</summary>
        /// <param name="format"><see cref="T:System.Windows.Forms.TextDataFormat"></see>. Format to be used when writing text. Default is <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText"></see>. Required. </param>
        /// <param name="text">String. Text to be written. Required. </param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public static void SetText(string text, TextDataFormat format)
        {
            ClipboardProxy.SetText(text, format);
        }

        #endregion

        [ThreadStatic]
        static int SafeSetClipboardDataVersion;

        /// <summary>
        /// 线程安全的设置内容
        /// </summary>
        /// <param name="dataObject"></param>
        public static void SafeSetClipboard(object dataObject)
        {
            // Work around ExternalException bug. (SD2-426)
            // Best reproducable inside Virtual PC.
            int version = unchecked(++SafeSetClipboardDataVersion);
            try
            {
                Clipboard.SetDataObject(dataObject, true);
            }
            catch (ExternalException)
            {
                Timer timer = new Timer();
                timer.Interval = 100;
                timer.Tick += delegate
                {
                    timer.Stop();
                    timer.Dispose();
                    if (SafeSetClipboardDataVersion == version)
                    {
                        try
                        {
                            Clipboard.SetDataObject(dataObject, true, 10, 50);
                        }
                        catch (ExternalException) { }
                    }
                };
                timer.Start();
            }
        }
    }
}
