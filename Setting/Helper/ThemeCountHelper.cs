
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
  public class ThemeCountHelper
    {
        private static string Getfilename()
        {

            var currentDir = Environment.CurrentDirectory+ "\\Json\\Init\\";
            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }

            var FileName = "ThemeCount.json";
            return currentDir + FileName;
        }



        public static string GetNextCount()
        {
        var current= GetCurrentCount();
        var next=    CountAddOneAndSave(current);

            return next.ToString().PadLeft(4, '0');
        }
        private static CountModel GetCurrentCount()
        {
            if (File.Exists(Getfilename()))
            {
                using (StreamReader file = File.OpenText(Getfilename()))
                {
                    var json = File.ReadAllText(Getfilename());

                    try
                    {
                        var temp = JsonConvert.DeserializeObject<CountModel>(json);

                        return temp;
                    }
                    catch (Exception ex)
                    {
                        File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(new CountModel()), System.Text.Encoding.UTF8);//将内容写进jon文件中
                        return new CountModel();
                    }
                }
            }
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(new CountModel()), System.Text.Encoding.UTF8);//将内容写进jon文件中
            return new CountModel();
        }


        private static int CountAddOneAndSave(CountModel json)
        {
            json.count++;
            if (json.count > 9999)
            {
                json.count = 0;
            }
            File.WriteAllText(Getfilename(), JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8);//将内容写进jon文件中

            return json.count;
        }
    }

    public class CountModel
    {
        public int count { get; set; } = 0;

    }

}
