using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.TCP
{
    public class ReceiveFilter : BeginEndMarkReceiveFilter<StringPackageInfo>
    {
        byte[] BeginMark;
        byte[] EndMark;
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="beginMark">开始字符串 或 开始字节数组</param>
        /// <param name="endMark">结束字符串 或 结束字节数组</param>
        public ReceiveFilter(byte[] beginMark, byte[] endMark) : base(beginMark, endMark)
        {
            this.BeginMark = beginMark;
            this.EndMark = endMark;
        }

        /// <summary>
        /// 经过过滤器，收到的字符串会到这个函数
        /// </summary>
        /// <param name="bufferStream"></param>
        /// <returns></returns>
        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            //获取接收到的完整数据，包括头和尾
            //string body = bufferStream.ReadString((int)bufferStream.Length, Encoding.Default);
           // 掐头去尾，只返回中间的数据
            //body = body.Substring(BeginMark.Length, body.Length - EndMark.Length - BeginMark.Length);

            //Skip(int count):从数据源跳过指定的字节个数。直接获取过滤后的数据
            string body = bufferStream.Skip(BeginMark.Length).ReadString((int)bufferStream.Length - BeginMark.Length - EndMark.Length, Encoding.Default);
            return new StringPackageInfo("", "!"+body+"**", new string[] { });
        }
    }

}
