using System.Security.Permissions;
using System.Windows.Forms;
using SendKeysProxy = System.Windows.Forms.SendKeys;

namespace Core.Systems
{
    /// <summary>
    /// Provides properties for accessing the current state of the keyboard, 
    /// such as what keys are currently pressed, and provides a method to send keystrokes to the active window.
    /// </summary>
    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    public class KeyboardHelper
    {
        #region Properties
        /// <summary>Gets a Boolean indicating if the ALT key is down.</summary>
        /// <returns>A Boolean value: True if the ALT key is down; otherwise False.</returns>
        public static bool AltKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Alt) > Keys.None);
            }
        }

        /// <summary>Gets a Boolean indicating if CAPS LOCK is turned on. </summary>
        /// <returns>A Boolean value: True if CAPS LOCK is turned on; otherwise False.</returns>
        /// <filterpriority>1</filterpriority>
        public static bool CapsLock
        {
            get
            {
                return false;
               // return ((UnsafeNativeMethods.GetKeyState(20) & 1) > 0);
            }
        }

        /// <summary>Gets a Boolean indicating if a CTRL key is down.</summary>
        /// <returns>A Boolean value. True if a CTRL key is down; otherwise False.</returns>
        /// <filterpriority>1</filterpriority>
        public static bool CtrlKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Control) > Keys.None);
            }
        }

        /// <summary>Gets a Boolean indicating if the NUM LOCK key is on. </summary>
        /// <returns>A Boolean value. True if NUM LOCK is on; otherwise False.</returns>
        /// <filterpriority>1</filterpriority>
        public static bool NumLock
        {
            get
            {
                return false;

                // return ((UnsafeNativeMethods.GetKeyState(0x90) & 1) > 0);
            }
        }

        /// <summary>Gets a Boolean indicating whether the SCROLL LOCK key is on. </summary>
        /// <returns>A Boolean value. True if SCROLL LOCK is on; otherwise False.</returns>
        /// <filterpriority>1</filterpriority>
        public static bool ScrollLock
        {
            get
            {
                return false;
                ////return ((UnsafeNativeMethods.GetKeyState(0x91) & 1) > 0);
            }
        }

        /// <summary>Gets a Boolean indicating if a SHIFT key is down.</summary>
        /// <returns>A Boolean value. True if a SHIFT key is down; otherwise False.</returns>
        /// <filterpriority>1</filterpriority>
        public static bool ShiftKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Shift) > Keys.None);
            }
        }


        #endregion

        #region Methods
        /// <summary>Sends one or more keystrokes to the active window, as if typed on the keyboard.</summary>
        /// <param name="keys">A String that defines the keys to send.</param>
        public static void SendKeys(string keys)
        {
            SendKeys(keys, false);
        }

        /// <summary>Sends one or more keystrokes to the active window, as if typed on the keyboard.</summary>
        /// <param name="keys">A String that defines the keys to send.</param>
        /// <param name="wait">Optional. A Boolean that specifies whether or not to wait for keystrokes to get processed before the application continues. True by default.</param>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        public static void SendKeys(string keys, bool wait)
        {
            if (wait)
            {
                SendKeysProxy.SendWait(keys);
            }
            else
            {
                SendKeysProxy.Send(keys);
            }
        }

        #endregion
    }
}
