using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model
{
    public class JsonFileInfo 
    {


        private string name;

        public string Name
        {
            get { return name; }
            set
            {
               
                    name = value;
            }
        }

        public string FileName { get; set; }
    }
}