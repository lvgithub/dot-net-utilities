using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Security;
using System.Security.Permissions;

namespace Core.Systems
{
    /// <summary>
    /// 声音播放辅助类
    /// </summary>
    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AudioHelper
    {
        private static SoundPlayer _SoundPlayer;

        #region Methods
        private static void InternalStop(SoundPlayer sound)
        {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
            try
            {
                sound.Stop();
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }
        }


        /// <summary>Plays a .wav sound file.</summary>
        /// <param name="location">A String containing the name of the sound file </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlThread" /><IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public static void Play(string location)
        {
            Play(location, AudioPlayMode.Background);
        }


        /// <summary>Plays a .wav sound file.</summary>
        /// <param name="playMode">AudioPlayMode Enumeration mode for playing the sound. By default, AudioPlayMode.Background.</param>
        /// <param name="location">A String containing the name of the sound file </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlThread" /><IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public static void Play(string location, AudioPlayMode playMode)
        {
            ValidateAudioPlayModeEnum(playMode, "playMode");
            string text1 = ValidateFilename(location);
            SoundPlayer player1 = new SoundPlayer(text1);
            Play(player1, playMode);
        }


        /// <summary>Plays a .wav sound file.</summary>
        /// <param name="playMode">AudioPlayMode Enumeration mode for playing the sound. By default, AudioPlayMode.Background.</param>
        /// <param name="stream"><see cref="T:System.IO.Stream"></see> that represents the sound file.</param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlThread" /><IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public static void Play(Stream stream, AudioPlayMode playMode)
        {
            ValidateAudioPlayModeEnum(playMode, "playMode");
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            Play(new SoundPlayer(stream), playMode);
        }


        /// <summary>Plays a .wav sound file.</summary>
        /// <param name="data">Byte array that represents the sound file.</param>
        /// <param name="playMode">AudioPlayMode Enumeration mode for playing the sound. By default, AudioPlayMode.Background.</param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlThread" /><IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public static void Play(byte[] data, AudioPlayMode playMode)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            ValidateAudioPlayModeEnum(playMode, "playMode");
            MemoryStream stream1 = new MemoryStream(data);
            Play(stream1, playMode);
            stream1.Close();
        }


        private static void Play(SoundPlayer sound, AudioPlayMode mode)
        {
            if (_SoundPlayer != null)
            {
                InternalStop(_SoundPlayer);
            }
            _SoundPlayer = sound;
            switch (mode)
            {
                case AudioPlayMode.WaitToComplete:
                    _SoundPlayer.PlaySync();
                    return;

                case AudioPlayMode.Background:
                    _SoundPlayer.Play();
                    return;

                case AudioPlayMode.BackgroundLoop:
                    _SoundPlayer.PlayLooping();
                    return;
            }
        }


        /// <summary>Plays a system sound.</summary>
        /// <param name="systemSound"><see cref="T:System.Media.SystemSound"></see> object representing the system sound to play.</param>
        public static void PlaySystemSound(SystemSound systemSound)
        {
            if (systemSound == null)
            {
                throw new ArgumentNullException("systemSound"); 
            }
            systemSound.Play();
        }


        /// <summary>Stops a sound playing in the background.</summary>
        /// <filterpriority>1</filterpriority>
        public static void Stop()
        {
            SoundPlayer player1 = new SoundPlayer();
            InternalStop(player1);
        }


        private static void ValidateAudioPlayModeEnum(AudioPlayMode value, string paramName)
        {
            if ((value < AudioPlayMode.WaitToComplete) || (value > AudioPlayMode.BackgroundLoop))
            {
                throw new InvalidEnumArgumentException(paramName, (int)value, typeof(AudioPlayMode));
            }
        }


        private static string ValidateFilename(string location)
        {
            if (String.IsNullOrEmpty(location))
            {
                throw new ArgumentNullException("location");

            }
            return location;
        }

 

        #endregion
    }

    /// <summary>Indicates how to play sounds when calling audio methods.</summary>
    public enum AudioPlayMode
    {
        /// <summary>
        /// play the sound, and waits until it completes before calling code continues. 
        /// </summary>
        WaitToComplete,

        /// <summary>
        /// play the sound in the background. The calling code continues to execute. 
        /// </summary>
        Background,

        /// <summary>
        /// play the sound in the background until the Stop Method is called. The calling code continues to execute. 
        /// </summary>
        BackgroundLoop
    }
}
