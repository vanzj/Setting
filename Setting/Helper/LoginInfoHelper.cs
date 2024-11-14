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
                using (StreamReader file = File.OpenText(Getfilename()))
                {
                    var json = File.ReadAllText(Getfilename());

                    try
                    {
                        return JsonConvert.DeserializeObject<LoginModel>(json);
                    }
                    catch (Exception ex)
                    {
                        File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(new LoginModel()), System.Text.Encoding.UTF8);//将内容写进jon文件中
                        return new LoginModel();
                    }
                }
            }
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(new LoginModel()), System.Text.Encoding.UTF8);//将内容写进jon文件中
            return new LoginModel();
        }

        public static void DisableAutoLogin( )
        {
            var open = Open();
            open.isAutoLogin = false;
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(open), System.Text.Encoding.UTF8);//将内容写进jon文件中
        }

        public static void Save(LoginModel json)
        {
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8);//将内容写进jon文件中
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool isAutoLogin { get; set; }
    }
}
