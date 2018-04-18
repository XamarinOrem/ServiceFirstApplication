using ServiceFirstApplication.Models;
using ServiceFirstApplication.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ServiceFirstApplication.Controllers
{
    //public static class Helper
    //{
    //    public static string ToAbsoluteUrl(this string relativeUrl) //Use absolute URL instead of adding phycal path for CSS, JS and Images     
    //    {
    //        if (string.IsNullOrEmpty(relativeUrl)) return relativeUrl;
    //        if (HttpContext.Current == null) return relativeUrl;
    //        if (relativeUrl.StartsWith("/")) relativeUrl = relativeUrl.Insert(0, "~");
    //        if (!relativeUrl.StartsWith("~/")) relativeUrl = relativeUrl.Insert(0, "~/");
    //        var url = HttpContext.Current.Request.Url;
    //        var port = url.Port != 80 ? (":" + url.Port) : String.Empty;
    //        return String.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
    //    }
    //    public static string GeneratePassword(int length) //length of salt    
    //    {
    //        const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
    //        var randNum = new Random();
    //        var chars = new char[length];
    //        var allowedCharCount = allowedChars.Length;
    //        for (var i = 0; i <= length - 1; i++)
    //        {
    //            chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randNum.NextDouble())];
    //        }
    //        return new string(chars);
    //    }
    //    public static string EncodePassword(string pass, string salt) //encrypt password    
    //    {
    //        byte[] bytes = Encoding.Unicode.GetBytes(pass);
    //        byte[] src = Encoding.Unicode.GetBytes(salt);
    //        byte[] dst = new byte[src.Length + bytes.Length];
    //        System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
    //        System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
    //        HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
    //        byte[] inArray = algorithm.ComputeHash(dst);
    //        //return Convert.ToBase64String(inArray);    
    //        return EncodePasswordMd5(Convert.ToBase64String(inArray));
    //    }
    //    public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
    //    {
    //        Byte[] originalBytes;
    //        Byte[] encodedBytes;
    //        MD5 md5;
    //        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
    //        md5 = new MD5CryptoServiceProvider();
    //        originalBytes = ASCIIEncoding.Default.GetBytes(pass);
    //        encodedBytes = md5.ComputeHash(originalBytes);
    //        //Convert encoded bytes back to a 'readable' string    
    //        return BitConverter.ToString(encodedBytes);
    //    }
    //    public static string base64Encode(string sData) // Encode    
    //    {
    //        try
    //        {
    //            byte[] encData_byte = new byte[sData.Length];
    //            encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
    //            string encodedData = Convert.ToBase64String(encData_byte);
    //            return encodedData;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("Error in base64Encode" + ex.Message);
    //        }
    //    }
    //    public static string base64Decode(string sData) //Decode    
    //    {
    //        try
    //        {
    //            var encoder = new System.Text.UTF8Encoding();
    //            System.Text.Decoder utf8Decode = encoder.GetDecoder();
    //            byte[] todecodeByte = Convert.FromBase64String(sData);
    //            int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
    //            char[] decodedChar = new char[charCount];
    //            utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
    //            string result = new String(decodedChar);
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("Error in base64Decode" + ex.Message);
    //        }
    //    }
    //}
    public class HomeController : Controller
    {
        //ServiceFirstEntities db = new ServiceFirstEntities();
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       

        public static string EncryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);

          

            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string DecryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Convert.FromBase64String(text);

            byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);

            // Remove salt
            int saltLength = GetSaltLength();
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];

            

            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                   // AES.Padding = PaddingMode.None;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        public static int GetSaltLength()
        {
            return 8;
        }
    }
}