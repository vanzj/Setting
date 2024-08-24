using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class ReturnBase<T>
    {
        protected string origincmd { get; set; }
        public string cmd { get; set; }
        public string code { get; set; }
        public string errorMsg { get; set; }
        public T data { get; set; }

        public virtual bool IsthisCmdRerun()
        {
            return origincmd==cmd;
        }

        public virtual bool IsSuccess()
        {
            return !(code!="-1");
        }
    }
}
