using Password.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.UI.Popups;

namespace Password.Logic
{
	public static class SettingsManager
	{
		private static string _salt = new EasClientDeviceInformation().Id.ToString(); // сюда надо сделать номер жесткого диска

		public static string HashStringBCrypt(string pass)
		{
			var crypt = new SHA256Managed();
			var hash = new StringBuilder();
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(pass + _salt));
			foreach (byte theByte in crypto)
			{
				hash.Append(theByte.ToString("x2"));
			}
			return hash.ToString();
		}

		public static string EncryptStringAES(string plainText, string sharedSecret)
		{
			if (string.IsNullOrEmpty(plainText))
				throw new ArgumentNullException("plainText");
			if (string.IsNullOrEmpty(sharedSecret))
				throw new ArgumentNullException("sharedSecret");

			string outStr = null;
			RijndaelManaged aesAlg = null;

			try
			{
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Encoding.UTF8.GetBytes(_salt));
				aesAlg = new RijndaelManaged();
				aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
					msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
					}
					outStr = Encoding.UTF8.GetString(msEncrypt.ToArray());
				}
			}
			finally
			{
				if (aesAlg != null)
					aesAlg.Clear();
			}
			return outStr;
		}

		public static string DecryptStringAES(string cipherText, string sharedSecret)
		{
			if (string.IsNullOrEmpty(cipherText))
				throw new ArgumentNullException("cipherText");
			if (string.IsNullOrEmpty(sharedSecret))
				throw new ArgumentNullException("sharedSecret");
			RijndaelManaged aesAlg = null;
			string plaintext = null;
			try
			{
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Encoding.UTF8.GetBytes(_salt)); 
				byte[] bytes = Encoding.UTF8.GetBytes(cipherText);
				using (MemoryStream msDecrypt = new MemoryStream(bytes))
				{
					aesAlg = new RijndaelManaged();
					aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
					aesAlg.IV = ReadByteArray(msDecrypt);
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
							plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
			finally
			{
				if (aesAlg != null)
					aesAlg.Clear();
			}

			return plaintext;
		}

		private static byte[] ReadByteArray(Stream s)
		{
			byte[] rawLength = new byte[sizeof(int)];
			if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
			{
				throw new SystemException("Stream did not contain properly formatted byte array");
			}

			byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
			if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new SystemException("Did not read byte array properly");
			}

			return buffer;
		}

		public static async Task<bool> WinHelloAuthAsync(bool showDialog = false)
        {
			string returnMsg;
			bool check = false;
			try
            {
                var consentResult = await Windows.Security.Credentials.UI.UserConsentVerifier.RequestVerificationAsync("Подтвердите активацию входа по отпечатку пальца.");
                switch (consentResult)
                {
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.Verified:
                        returnMsg = "Отпечаток успешно считан.";
                        check = true;
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceBusy:
                        returnMsg = "Датчик отпечатка пальца занят.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceNotPresent:
                        returnMsg = "Нет датчика отпечатка пальца.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DisabledByPolicy:
                        returnMsg = "Биометрия отключена групповой политикой.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.NotConfiguredForUser:
                        returnMsg = "Не найдено не одного сохраненного отпечатка пальца.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.RetriesExhausted:
                        returnMsg = "Слишком много попыток обращения к датчику отпечатка пальца.";
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.Canceled:
                    default:
                        returnMsg = "Ошибка обращения к датчику отпечатка пальца.";
                        break;
                }
            }
            catch (Exception ex)
            {
                returnMsg = "Ошибка обращения к датчику: " + ex.ToString();
            }

            if (showDialog)
            {
				await new MessageDialog("", returnMsg).ShowAsync();
			}

			return check;
        }

		public static void SaveLoginPassPairs(IEnumerable<LoginPassPair> pairs)
        {
			int id;
            if (!int.TryParse((App.LocalSettings.Values["Passwords"] as string).Split('\n').FirstOrDefault().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out id))
            {
				id = 0;
			}
            foreach (var pair in pairs)
            {
				App.LocalSettings.Values["Passwords"] = (++id).ToString() + " " + EncryptStringAES(pair.Login, GetMasterKey()) + " " + EncryptStringAES(pair.Pass, GetMasterKey()) + "\n";
            }
        }

		public static void SaveLoginPassPair(LoginPassPair pair)
        {
			int id;
            if (!int.TryParse((App.LocalSettings.Values["Passwords"] as string).Split('\n').FirstOrDefault().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out id))
            {
				id = 0;
			}
			App.LocalSettings.Values["Passwords"] = (++id).ToString() + " " + EncryptStringAES(pair.Login, GetMasterKey()) + " " + EncryptStringAES(pair.Pass, GetMasterKey()) + "\n";
        }

		public static int GetNextId()
        {
			int id;
            if (!int.TryParse((App.LocalSettings.Values["Passwords"] as string).Split('\n').FirstOrDefault().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out id))
            {
				id = 0;
			}
			return id;
        }

		public static IEnumerable<LoginPassPair> GetAllLoginPassPairs()
		{
			return (App.LocalSettings.Values["Passwords"] as string).Split('\n').Select(x => new LoginPassPair(int.Parse(x.Split(' ')[0]), 
																								DecryptStringAES(x.Split(' ')[1], GetMasterKey()),
																								DecryptStringAES(x.Split(' ')[2], GetMasterKey())));
		}

		public static void SavePassword(string pass) => App.LocalSettings.Values["Hash"] = HashStringBCrypt(pass);

		public static bool CheckPassword(string pass) => App.LocalSettings.Values["Hash"] as string == HashStringBCrypt(pass);

		public static void SaveMasterKey(string key) => App.LocalSettings.Values["MasterKey"] = EncryptStringAES(key, _salt);

		public static string GetMasterKey() => DecryptStringAES(App.LocalSettings.Values["MasterKey"] as string, _salt);

		public static void SaveWinHelloState(bool? useWindowsHello) => App.LocalSettings.Values["UseWindowsHello"] = useWindowsHello;

		public static bool? GetWinHelloState() => App.LocalSettings.Values["UseWindowsHello"] as bool?;

		public static bool? IsFirstLaunch() => App.LocalSettings.Values["IsFirstLaunch"] as bool?;

		public static void NotFirstLaunch() => App.LocalSettings.Values["IsFirstLaunch"] = false;

		public static void SaveSecret(string secret) => App.LocalSettings.Values["Secret"] = secret;

		public static string GetSecret() => App.LocalSettings.Values["Secret"] as string;

		public static void ResetAll() => App.LocalSettings.Values["IsFirstLaunch"] = true;
	}
}
