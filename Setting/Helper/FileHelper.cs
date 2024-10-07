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
using Webapi;

namespace Setting.Helper
{
    public class FileHelper
    {


        public static List<JsonFileInfo> GetOrInitThemeList()
        {
            var sortPath = GetThemDir() + "\\" + "sort.json";

            if (!File.Exists(sortPath))
            {


                var AllFile = new List<JsonFileInfo>()
                {    new JsonFileInfo()
                {
                    Name = "开机动画",
                    FileName = "run.json",
                    Default= true,

                },
                    new JsonFileInfo()
                {
                    Name = "CPU状态",
                    FileName = "cpu.json",
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
            File.WriteAllText(jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
            if (!string.IsNullOrEmpty( jsonFileInfo.NewFileName))
            {
               // SaveFileName(jsonFileInfo);
                File.Delete(jsonfile);
                jsonfile =  jsonFileInfo.NewFileName ;
                File.WriteAllText(GetThemDir() + "\\" + jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                // 创建 MemoryStream 并写入字节数组

                var temp = client.AddUsingPOSTAsync(DevId, new FileParameter(new MemoryStream(bytes)), jsonfile, jsonFileInfo.Name, 9).GetAwaiter().GetResult();
                if (jsonFileInfo.Id != null)
                {
                    var  tempdelete = client.DeleteUsingGETAsync(jsonFileInfo.Id);
                }
            }
            else
            {
                if (jsonFileInfo.Id == null)
                {
                    File.WriteAllText(GetThemDir() + "\\" + jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    // 创建 MemoryStream 并写入字节数组

                    var temp = client.AddUsingPOSTAsync(DevId, new FileParameter(new MemoryStream(bytes)), jsonfile, jsonFileInfo.Name, 9).GetAwaiter().GetResult();
            }

            }
    
    

            ;
        }
        public static void SaveThemeName( JsonFileInfo jsonFileInfo, string DevId)
        {
           // 修改名称
        }

        public static void Remove(JsonFileInfo jsonFileInfo, string DevId)
        {

            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            var temp = client.DeleteUsingGETAsync(jsonFileInfo.Id).GetAwaiter().GetResult();
            var jsonPaht = GetThemDir() + "\\" + jsonFileInfo.FileName;
            if (File.Exists(jsonPaht))
            {
                File.Delete(jsonPaht);
            }

        }
        public static void Copy(JsonFileInfo ori,JsonFileInfo taget, string DevId)
        {
            var json = Open(ori.FileName);
            var jsonfile = GetThemDir() + "\\" + taget.FileName;
            File.WriteAllText(jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中


            File.WriteAllText(GetThemDir() + "\\" + jsonfile, json, System.Text.Encoding.UTF8);//将内容写进jon文件中
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            // 创建 MemoryStream 并写入字节数组
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            var temp = client.AddUsingPOSTAsync(DevId, new FileParameter(new MemoryStream(bytes)), jsonfile, taget.FileName, 9).GetAwaiter().GetResult();
        }
        public static string Open(string filename)
        {
            var jsonfile = GetThemDir() + "\\" + filename;
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
