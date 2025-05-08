
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
  public class LoginInfoHelper
    {
        private static string Getfilename()
        {

            var currentDir = Environment.CurrentDirectory+ "\\Json\\Init\\";
            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }

            var FileName = "init.json";
            return currentDir + FileName;
        }
        public static LoginModel Open()
        {
            if (File.Exists(Getfilename()))
            {
                try
                {
                    using (StreamReader file = File.OpenText(Getfilename()))
                {
                    var json = File.ReadAllText(Getfilename());

                   
                        var temp  = JsonConvert.DeserializeObject<LoginModel>(json);
                        if (temp.isRSA)
                        {
                            temp.Password = RSADecrypt(temp.Password);
                            temp.isRSA = false;
                        }
                        return temp ;
                    }
                    
                }
                catch (Exception ex)
                {
                    File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(new LoginModel()), System.Text.Encoding.UTF8);//将内容写进jon文件中
                    return new LoginModel();
                }
            }
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(new LoginModel()), System.Text.Encoding.UTF8);//将内容写进jon文件中
            return new LoginModel();
        }

        public static void DisableAutoLogin( )
        {
            var json = Open();
            json.isAutoLogin = false;
            if (!json.isRSA)
            {
                json.isRSA = true;
                json.Password = RSAEncryption(json.Password);
            }
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8);//将内容写进jon文件中
        }

        public static void Save(LoginModel json)
        {
            if (!json.isRSA)
            {
                json.isRSA = true;
                json.Password = RSAEncryption(json.Password);
            }

            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8);//将内容写进jon文件中
        }
        /// <summary> 
        /// RSA加密数据 
        /// </summary> 
        /// <param name="express">要加密数据</param> 
        /// <param name="KeyContainerName">密匙容器的名称</param> 
        /// <returns></returns> 
        public static string RSAEncryption(string express)
        {

            System.Security.Cryptography.CspParameters param = new System.Security.Cryptography.CspParameters();
            param.KeyContainerName = "wpfsetting"; //密匙容器的名称，保持加密解密一致才能解密成功
            using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(param))
            {
                byte[] plaindata = System.Text.Encoding.Default.GetBytes(express);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }
        /// <summary> 
        /// RSA解密数据 
        /// </summary> 
        /// <param name="express">要解密数据</param> 
        /// <param name="KeyContainerName">密匙容器的名称</param> 
        /// <returns></returns> 
        public static string RSADecrypt(string ciphertext)
        {
            System.Security.Cryptography.CspParameters param = new System.Security.Cryptography.CspParameters();
            param.KeyContainerName = "wpfsetting"; //密匙容器的名称，保持加密解密一致才能解密成功
            using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(ciphertext);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return System.Text.Encoding.Default.GetString(decryptdata);
            }
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool isAutoLogin { get; set; }
        public bool isRSA { get; set; }
    }

}
