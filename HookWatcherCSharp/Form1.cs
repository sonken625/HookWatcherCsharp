using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace HookWatcherCSharp
{

    public partial class Form1 : Form
    {

        private IntPtr hHook;
        public delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SetHook();
        }

        public int SetHook()
        {
            IntPtr hmodule = WindowsAPI.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);

            
            hHook = WindowsAPI.SetWindowsHookEx((int)WindowsAPI.HookType.WH_KEYBOARD_LL, (HOOKPROC)MyHookProc, hmodule, IntPtr.Zero);
            
            // 以下のようにWH_KEYBOARDなどはこれでは動かない。MyHookProcをdllにする必要がある？
            // hHook = WindowsAPI.SetWindowsHookEx((int)WindowsAPI.HookType.WH_KEYBOARD, (HOOKPROC)MyHookProc, hmodule, IntPtr.Zero);
            if (hHook == null)
            {
                MessageBox.Show("SetWindowsHookEx 失敗", "Error");
                return -1;
            }
            else
            {
                MessageBox.Show("SetWindowsHookEx 成功", "OK");
                return 0;
            }
        }

        public IntPtr MyHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (0 == WindowsAPI.HC_ACTION)
            {
                WindowsAPI.KBDLLHOOKSTRUCT MouseHookStruct = (WindowsAPI.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(WindowsAPI.KBDLLHOOKSTRUCT));
                label1.Text = string.Format("Mouse Position : {0:d}, {1:d}", MouseHookStruct.scanCode, MouseHookStruct.vkCode);
            }

            return WindowsAPI.CallNextHookEx(hHook, nCode, wParam, lParam);

        }



    }
}
