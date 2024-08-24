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


        public static List<JsonFileInfo> GetOrInitThemeList()
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {


                var AllFile = new List<JsonFileInfo>()
                {
                    new JsonFileInfo()
                {
                    Name = "CPU状态",
                    FileName = "7ebb348c-eebf-462b-9354-78d1ef3042c8",
                    Default= true,
                    IsDynamic =true
                },
                    new JsonFileInfo()
                {
                    Name = "音乐律动",
                    FileName = "57cd5309-9af2-4b1c-bf9a-18a5b17c71f0",
                    Default= true,
                }

            };


                AllFile.AddRange(GetThemeJsonList());
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
            return root.GetFiles().Where(c => c.Extension == Extension && c.Name != "sort.json"&& c.Name!= "57cd5309-9af2-4b1c-bf9a-18a5b17c71f0.json").Select(c =>new JsonFileInfo() { Name= c.Name.Replace(c.Extension, ""), FileName = c.Name.Replace(c.Extension, "") }  ).ToList();

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
            if (!string.IsNullOrEmpty( jsonFileInfo.NewFileName))
            {
                SaveFileName(jsonFileInfo);
                File.Delete(jsonfile);
                jsonfile = GetThemDir() + "\\" + jsonFileInfo.NewFileName + Extension;
            }
            File.WriteAllText(jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
        }
        public static void SaveThemeName( JsonFileInfo jsonFileInfo)
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {

                GetOrInitThemeList();
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
        public static void SaveFileName(JsonFileInfo jsonFileInfo)
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {

                GetOrInitThemeList();
            }
            else
            {
                var AllFileString = OpenByPath(sortPath);
                var AllFile = JsonConvert.DeserializeObject<List<JsonFileInfo>>(AllFileString);
                if (AllFile.All(c => c.Name != jsonFileInfo.Name))
                {
                    if (!string.IsNullOrEmpty(jsonFileInfo.NewFileName))
                    {
                        jsonFileInfo.FileName = jsonFileInfo.NewFileName;
                    }
                    AllFile.Add(jsonFileInfo);
                    File.WriteAllText(sortPath, JsonConvert.SerializeObject(AllFile), System.Text.Encoding.UTF8);//将内容写进jon文件中
                }
                else
                {
                    AllFile.FirstOrDefault(c => c.Name == jsonFileInfo.Name).FileName = jsonFileInfo.NewFileName;
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

                GetOrInitThemeList();
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
                GetOrInitThemeList();
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
