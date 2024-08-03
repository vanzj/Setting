using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class FileHelper
    {
      static  string Extension = ".json";


        public static List<JsonFileInfo> GetThemeList()
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {
                var AllFile = GetThemeJsonList();
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
        public static List<JsonFileInfo> GetThemeJsonList()
        {
            if (!Directory.Exists(GetThemDir()))
            {
                Directory.CreateDirectory(GetThemDir());
            }

            DirectoryInfo root = new DirectoryInfo(GetThemDir());
            var tempslit = root.GetFiles();
            return root.GetFiles().Where(c => c.Extension == Extension && c.Name != "sort.json").Select(c =>new JsonFileInfo() { Name= c.Name.Replace(c.Extension, ""), FileName = c.Name.Replace(c.Extension, "") }  ).ToList();

        }
        private  static string GetThemDir()
        {
            var currentDir = Environment.CurrentDirectory;
            var ThemePath = "\\Json\\Theme";
            return  currentDir + ThemePath;
        }

        public static void Save(string json, JsonFileInfo jsonFileInfo)
        {
           

            var jsonfile = GetThemDir() + "\\" + jsonFileInfo.FileName + Extension;
            File.WriteAllText(jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
        }
        public static void SaveName( JsonFileInfo jsonFileInfo)
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {

                var AllFile = GetThemeJsonList();
                AllFile.Add(jsonFileInfo);
                File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
            }
            else
            {
                var AllFileString = OpenByPath(sortPath);
                var AllFile = JsonConvert.DeserializeObject<List<JsonFileInfo>>(AllFileString);
                if (AllFile.All(c => c.FileName != jsonFileInfo.FileName))
                {
                    AllFile.Add(jsonFileInfo);
                    File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
                }
                else
                {
                    AllFile.FirstOrDefault(c => c.FileName == jsonFileInfo.FileName).Name = jsonFileInfo.Name;
                    File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
                }
            }
        }

        public static void Remove(JsonFileInfo jsonFileInfo)
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";
            var jsonPaht = GetThemDir() + "\\" + jsonFileInfo.FileName+".json";
            if (!File.Exists(sortPath))
            {

                var AllFile = GetThemeJsonList();
                File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
            }
            else
            {
                var AllFileString = OpenByPath(sortPath);
                var AllFile = JsonConvert.DeserializeObject<List<JsonFileInfo>>(AllFileString);
                var temp = AllFile.FirstOrDefault(c => c.FileName != jsonFileInfo.FileName);
                if (temp!=null)
                {
                    AllFile.Remove(temp);
                    File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
                }
                
            }
            if (File.Exists(jsonPaht))
            {
                File.Delete(jsonPaht);
            }

        }
        public static void Copy(JsonFileInfo ori,JsonFileInfo taget)
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {
                var AllFile = GetThemeJsonList();
                AllFile.Add(taget);
                File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
            }
            else
            {
                var AllFileString = OpenByPath(sortPath);
                var AllFile = JsonConvert.DeserializeObject<List<JsonFileInfo>>(AllFileString);
                if (AllFile.All(c => c.FileName != taget.FileName))
                {
                    AllFile.Add(taget);
                    File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
                }
                else
                {
                    AllFile.FirstOrDefault(c => c.FileName == taget.FileName).Name = taget.Name;
                    File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
                }
            }

            var json = Open(ori.FileName);
            var jsonfile = GetThemDir() + "\\" + taget.FileName + Extension;
            File.WriteAllText(jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
        }
        public static string Open(string filename)
        {
            var jsonfile = GetThemDir() + "\\" + filename + Extension;
            using (StreamReader file = File.OpenText(jsonfile))
            {
                return File.ReadAllText(jsonfile);
            }
           

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
