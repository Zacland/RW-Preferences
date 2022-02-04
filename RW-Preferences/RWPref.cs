using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RW_Preferences
{
    class RWPref
    {
        /// <summary>
        /// Retourne le chemin et le fichier des préférences de paramétrage (url + login + WebAuthToken ...)
        /// </summary>
        private string PreferencesFilePath
        {
            get
            {
                var userPrefsDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var prefsFilePath = Path.Combine(userPrefsDir, "MonAppli", "Preferences.xml");
                return prefsFilePath;
            }
        }

        /// <summary>
        /// Retourne une Valeur pour une clé donnée venant du fichier xml
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string getPreferencesFieldValue(string fieldName)
        {
            try
            {
                //string xmlText = File.ReadAllText(PreferencesFilePath);

                XDocument xdoc = XDocument.Load(PreferencesFilePath);
                XElement el = xdoc.XPathSelectElement("AppPreferences/" + fieldName);
                if (el != null)
                    return el.Value;
                else
                    return "";
                //var value = (string)xdoc.Descendants(fieldName).FirstOrDefault();
                //return value;

            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Debug.WriteLine("Config file not found : Will be create later !");
                }
                else
                {
                    Debug.WriteLine($"Exception 1 : {e.Message}");
                }
                return "";
            }
        }

        private static XElement GetOrCreateElement(XContainer container, string name)
        {
            var element = container.Element(name);
            if (element == null)
            {
                element = new XElement(name);
                container.Add(element);
            }
            return element;
        }

        private void setPreferencesFieldValue(string fieldName, string fieldValue)
        {
            try
            {
                XDocument doc = new XDocument();
                try
                {
                    Directory.CreateDirectory(Directory.GetParent(PreferencesFilePath).FullName);

                    XDocument testDoc = XDocument.Load(PreferencesFilePath);
                }
                catch
                {
                    doc.Add(new XElement("AppPreferences"));
                    doc.Save(PreferencesFilePath);
                }


                doc = XDocument.Load(PreferencesFilePath);



                XElement rootEl = GetOrCreateElement(doc, "AppPreferences");
                XElement leafEl = GetOrCreateElement(rootEl, fieldName);
                leafEl.Value = fieldValue;

                doc.Save(PreferencesFilePath);
            }
            catch
            {

            }
        }

        //WebAuthToken

        public string WebAuthToken
        {
            get
            {
                return getPreferencesFieldValue("WebAuthToken");
            }
            set
            {
                setPreferencesFieldValue("WebAuthToken", value);
            }
        }

        public string ServerUrl
        {
            get
            {
                string val = getPreferencesFieldValue("ServerUrl");
                if (val != null && !val.EndsWith("/") && val.Length > 0)
                {
                    val = val + "/";
                }

                if (val == null || val.Length == 0)
                {
                    val = "https://mon_adresse_test.com";
                }

                return val;
            }
            set
            {
                string val = value;
                if (val != null && !val.EndsWith("/") && val.Length > 0)
                {
                    val = val + "/";
                }
                setPreferencesFieldValue("ServerUrl", val);
            }
        }

        public string UserLogin
        {
            get
            {
                return getPreferencesFieldValue("UserLogin");
            }
            set
            {
                setPreferencesFieldValue("UserLogin", value);
            }
        }

        public string UserPassword
        {
            get
            {
                string encryptedPass = getPreferencesFieldValue("UserPassword");
                return EncryptionHelper.DecryptString(encryptedPass);
            }
            set
            {
                setPreferencesFieldValue("UserPassword", EncryptionHelper.EncryptString(value));
            }
        }

        public static class EncryptionHelper
        {
            public static string EncryptString(string clearText)
            {
                if (clearText == null)
                    return null;
                string EncryptionKey = "ma_cle_a_moi";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x4d, 0x61, 0x72, 0x6b, 0x76, 0x61, 0x72, 0x74, 0x20, 0x47, 0x69, 0x6c, 0x64, 0x77, 0x69, 0x6e });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            public static string DecryptString(string cipherText)
            {
                if (cipherText == null)
                {
                    return null;
                }

                try
                {
                    string EncryptionKey = "ma_cle_a_moi";
                    cipherText = cipherText.Replace(" ", "+");
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x4d, 0x61, 0x72, 0x6b, 0x76, 0x61, 0x72, 0x74, 0x20, 0x47, 0x69, 0x6c, 0x64, 0x77, 0x69, 0x6e });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            cipherText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                    return cipherText;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
