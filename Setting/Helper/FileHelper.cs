using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi;

namespace Setting.Helper
{
    public class FileHelper
    {
      static  string Extension = ".json";


        public static List<JsonFileInfo> GetOrInitThemeList()
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {


                var AllFile = new List<JsonFileInfo>()
                {    new JsonFileInfo()
                {
                    Name = "开机动画",
                    FileName = "run",
                    Default= true,

                },
                    new JsonFileInfo()
                {
                    Name = "CPU状态",
                    FileName = "cpu",
                    Default= true,
                    IsDynamic =true
                },
                //      new JsonFileInfo()
                //{
                //    Name = "GPU状态",
                //    FileName = "gpu",
                //    Default= true,
                //    IsDynamic =true
                //},
                //        new JsonFileInfo()
                //{
                //    Name = "网速（待开发）",
                //    FileName = "wifi",
                //    Default= true,
                //    IsDynamic =true
                //},
                       
            };
                File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);
                return AllFile;         
             }
            else
            {
                var AllFileString = OpenByPath(sortPath);
                var AllFile = JsonConvert.DeserializeObject<List<JsonFileInfo>>(AllFileString);
                return AllFile;
            }

        }

        private  static string GetThemDir()
        {
            var currentDir = Environment.CurrentDirectory;
            var ThemePath = "\\Json\\Theme";
            return  currentDir + ThemePath;
        }

        public static void Save(string json, JsonFileInfo jsonFileInfo,string DevId)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            var jsonfile =  jsonFileInfo.FileName ;
            if (!string.IsNullOrEmpty( jsonFileInfo.NewFileName))
            {
               // SaveFileName(jsonFileInfo);
                File.Delete(jsonfile);
                jsonfile =  jsonFileInfo.NewFileName ;
            }
            File.WriteAllText(GetThemDir() + "\\"+ jsonfile+ Extension, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
          var temp =  client.AddUsingPOSTAsync(DevId, jsonfile, 9).GetAwaiter().GetResult();

           var temp2 =  client.MacListUsingGET2Async(DevId, 9).GetAwaiter().GetResult();

            ;
        }
        public static void SaveThemeName( JsonFileInfo jsonFileInfo, string DevId)
        {
           // 修改名称
        }
        public static void SaveFileName(JsonFileInfo jsonFileInfo, string DevId)
        {
         
            
           
            
        }
        public static void Remove(JsonFileInfo jsonFileInfo, string DevId)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            var jsonPaht = GetThemDir() + "\\" + jsonFileInfo.FileName+".json";
            if (File.Exists(jsonPaht))
            {
                File.Delete(jsonPaht);
            }

        }
        public static void Copy(JsonFileInfo ori,JsonFileInfo taget, string DevId)
        {
            var json = Open(ori.FileName);
            var jsonfile = GetThemDir() + "\\" + taget.FileName + Extension;
            File.WriteAllText(jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
        }
        public static string Open(string filename)
        {
            var jsonfile = GetThemDir() + "\\" + filename + Extension;
            if (File.Exists(jsonfile))
            {
                using (StreamReader file = File.OpenText(jsonfile))
                {
                    return File.ReadAllText(jsonfile);
                }
            }
            return "";
        }
        public static string OpenByPath(string jsonfile)
        {
            using (StreamReader file = File.OpenText(jsonfile))
            {
                return File.ReadAllText(jsonfile);
            }
          

        }
    }
}
