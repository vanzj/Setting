using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model
{
    public class JsonFileInfo : ViewModelBase
    {
        public long? Id { get; set; }


        private string name;

        public string Name
        {
            get { return name; }
            set
            {
               
                    name = value;
                RaisePropertyChanged();
            }
        }

        public string BakName
        {
            get;set;
        }
        public bool Default { get; set; }
        public bool IsDynamic { get; set; }
        public string FileName { get; set; }
        [JsonIgnore]
        public string NewFileName { get; set; }
    }
}