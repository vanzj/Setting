﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class GetInfoSend : SendBase<string>
    {
        public GetInfoSend()
        {
            cmd = "getInfo";
            data = "";
        }
    }
    public class GetInfoRetrun : ReturnBase<GetInfoRetrunData>
    {
        public GetInfoRetrun()
        {
            origincmd = "getInfo";
        }
    }

    public class GetInfoRetrunData
    {
        public string mac { get; set; }
        public string brightness { get; set; }
        public string themeName { get; set; }
    }
}
