using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management; // 需要添加对 System.Management 的引用
using GalaSoft.MvvmLight.Messaging;

namespace Setting.Helper
{
    public static class CommandBuilder
    {
     
        public static byte[] BuildHeartCmd(string mac)
        {
         
            return System.Text.Encoding.ASCII.GetBytes($"?HEART#{mac}**");



        }


        public static byte[] BuildGIFSuccessCmd(string mac)
        {
           
            return System.Text.Encoding.ASCII.GetBytes($"!TRANSPARENTDATA#{mac}#00**");



        }

        public static byte[] BuildGIFFailCmd(string mac)
        {
      
            return System.Text.Encoding.ASCII.GetBytes($"!TRANSPARENTDATA#{mac}#01**");



        }
    }
}
