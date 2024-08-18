using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace Setting.Helper
{
    public class CursorSafeHandle : SafeHandle
    {
        public CursorSafeHandle(IntPtr preexistingHandle, bool ownsHandle)
            : base(IntPtr.Zero, ownsHandle)
        {
            handle = preexistingHandle;
        }
        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }
        protected override bool ReleaseHandle()
        {
            return true;
        }
    }
    public class CursorHelper
    {
        public static Cursor MOVE()
        {
            Bitmap dimage = new Bitmap(Environment.CurrentDirectory + "\\Img\\移动_move-one-cursor.png");

            var handle = dimage.GetHicon();
            var c = new CursorSafeHandle(handle, true);
            return CursorInteropHelper.Create(c);
        }
        public static Cursor MAGIC()
        {
            Bitmap dimage = new Bitmap(Environment.CurrentDirectory + "\\Img\\魔法棒_magic-cursor.png");

            var handle = dimage.GetHicon();
            var c = new CursorSafeHandle(handle, true);
            return CursorInteropHelper.Create(c);
        }
        public static Cursor ERASE()
        {
            Bitmap dimage = new Bitmap(Environment.CurrentDirectory + "\\Img\\擦除_erase-cursor.png");

            var handle = dimage.GetHicon();
            var c = new CursorSafeHandle(handle, true);
            return CursorInteropHelper.Create(c);
        }
    }
}
