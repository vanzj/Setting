using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
                var DefaultThemeDir = GetDefaultThemeDir();
                CopyFiles(DefaultThemeDir, GetThemDir());
            }

                var AllFileString = OpenByPath(sortPath);
                var AllFile = JsonConvert.DeserializeObject<List<JsonFileInfo>>(AllFileString);
                return AllFile;


        }
        public static int CopyFiles(string sourceFolder, string destFolder)
                {
            try
                {
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }

                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                                                          {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest,true);
                }
                return 1;
             }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        private  static string GetThemDir()
        {
            var currentDir = Environment.CurrentDirectory;
            var ThemePath = "\\Json\\Theme";
            return  currentDir + ThemePath;
        }
        private static string GetDefaultThemeDir()
        {
            var currentDir = Environment.CurrentDirectory;
            var ThemePath = "\\Json\\DefaultTheme";
            return currentDir + ThemePath;
        }
        public static void Save(string json, JsonFileInfo jsonFileInfo)
        {
           
            var jsonfile = GetThemDir() + "\\" + jsonFileInfo.FileName + Extension;
            if (!string.IsNullOrEmpty( jsonFileInfo.NewFileName))
            {
                SaveFileName(jsonFileInfo);
                jsonFileInfo.FileName = jsonFileInfo.NewFileName;
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
                var temp = AllFile.FirstOrDefault(c => c.FileName == jsonFileInfo.FileName);
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
                    var json = File.ReadAllText(jsonfile);
                    if (!json.Contains("AllPonitList"))
                    {
                        json = $"{{ \"FrameRate\":20,\"AllPonitList\":{json}}}";
                    }
                    return json;
                  
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
        /// <summary>
        /// 异步下载 JSON 文件并保存到指定路径。
        /// </summary>
        /// <param name="url">JSON 文件的 URL。</param>
        /// <param name="filePath">要保存的文件路径。</param>
        /// <returns>返回一个表示异步操作的任务。</returns>
        public static async Task DownloadJsonFileAsync(string url, string filePath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // 发送 GET 请求以获取 JSON 数据
                    HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容并写入文件
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                  fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"请求错误: {e.Message}");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"文件 I/O 错误: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"未知错误: {e.Message}");
                }
            }
        }

    }
}
