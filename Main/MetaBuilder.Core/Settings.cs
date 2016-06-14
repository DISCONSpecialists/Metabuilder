using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Win32;

namespace MetaBuilder.Core
{
    /// <summary>
    /// Stores non-volatile name/value settings. These persist between sessions
    /// of the application.
    /// </summary>
    /// <remarks>
    /// On Windows, this class uses the registry.
    /// </remarks>
    public sealed class Settings
    {
        private const string hkcuKey = @"SOFTWARE\MetaBuilder";

        public static readonly Settings SystemWide = new Settings(Registry.LocalMachine);
        public static readonly Settings CurrentUser = new Settings(Registry.CurrentUser);

        private const string pointXSuffix = ".X";
        private const string pointYSuffix = ".Y";

        private RegistryKey rootKey;

        private Settings(RegistryKey rootKey)
        {
            this.rootKey = rootKey;
        }

        public RegistryKey CreateSettingsKey(bool writable)
        {
            RegistryKey softwareKey = null;

            try
            {
                softwareKey = this.rootKey.OpenSubKey(hkcuKey, writable);
            }

            catch (Exception)
            {
                softwareKey = null;
            }

            if (softwareKey == null)
            {
                try
                {
                    softwareKey = rootKey.CreateSubKey(hkcuKey);
                }

                catch (Exception)
                {
                    throw;
                }
            }

            return softwareKey;
        }

        /// <summary>
        /// Deletes a settings key.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        public void Delete(string key)
        {
            using (RegistryKey mbKey = CreateSettingsKey(true))
            {
                mbKey.DeleteValue(key, false);
            }
        }

        /// <summary>
        /// Deletes several settings keys.
        /// </summary>
        /// <param name="keys">The keys to delete.</param>
        public void Delete(string[] keys)
        {
            using (RegistryKey mbKey = CreateSettingsKey(true))
            {
                foreach (string key in keys)
                {
                    mbKey.DeleteValue(key, false);
                }
            }
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public object GetObject(string key)
        {
            using (RegistryKey mbKey = CreateSettingsKey(false))
            {
                return mbKey.GetValue(key);
            }
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public object GetObject(string key, object defaultValue)
        {
            using (RegistryKey mbKey = CreateSettingsKey(false))
            {
                return mbKey.GetValue(key, defaultValue);
            }
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetObject(string key, object value)
        {
            using (RegistryKey mbKey = CreateSettingsKey(true))
            {
                mbKey.SetValue(key, value);
            }
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public string GetString(string key)
        {
            return (string)GetObject(key);
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public string GetString(string key, string defaultValue)
        {
            return (string)GetObject(key, defaultValue);
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetString(string key, string value)
        {
            SetObject(key, value);
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public bool GetBoolean(string key)
        {
            return bool.Parse(GetString(key));
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public bool GetBoolean(string key, bool defaultValue)
        {
            return bool.Parse(GetString(key, defaultValue.ToString()));
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetBoolean(string key, bool value)
        {
            SetString(key, value.ToString());
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public Point GetPoint(string key)
        {
            int x = GetInt32(key + pointXSuffix);
            int y = GetInt32(key + pointYSuffix);
            return new Point(x, y);
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public Point GetPoint(string key, Point defaultValue)
        {
            int x = GetInt32(key + pointXSuffix, defaultValue.X);
            int y = GetInt32(key + pointYSuffix, defaultValue.Y);
            return new Point(x, y);
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetPoint(string key, Point value)
        {
            SetInt32(key + pointXSuffix, value.X);
            SetInt32(key + pointYSuffix, value.Y);
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public Int32 GetInt32(string key)
        {
            return Int32.Parse(GetString(key));
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public Int32 GetInt32(string key, Int32 defaultValue)
        {
            return Int32.Parse(GetString(key, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public float GetSingle(string key, float defaultValue)
        {
            return Single.Parse(GetString(key, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public float GetSingle(string key)
        {
            return Single.Parse(GetString(key));
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetInt32(string key, int value)
        {
            SetString(key, value.ToString());
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetSingle(string key, float value)
        {
            SetString(key, value.ToString());
        }

        /// <summary>
        /// Gets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        /// <remarks>This method treats the key value as a stream of base64 encoded bytes that represent a PNG image.</remarks>
        public Image GetImage(string key)
        {
            string imageB64 = GetString(key);
            byte[] pngBytes = Convert.FromBase64String(imageB64);
            MemoryStream ms = new MemoryStream(pngBytes);
            Image image = Image.FromStream(ms);
            ms.Close();
            return image;
        }

        /// <summary>
        /// Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        /// <remarks>This method saves the key value as a stream of base64 encoded bytes that represent a PNG image.</remarks>
        public void SetImage(string key, Image value)
        {
            MemoryStream ms = new MemoryStream();
            value.Save(ms, ImageFormat.Png);
            byte[] buffer = ms.GetBuffer();
            string base64 = Convert.ToBase64String(buffer);
            SetString(key, base64);
            ms.Close();
        }
    }
}