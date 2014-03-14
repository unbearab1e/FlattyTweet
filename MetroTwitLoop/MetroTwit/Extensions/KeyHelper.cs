namespace MetroTwit.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Input;

    public static class KeyHelper
    {
        public static char GetCharFromKey(this Key key)
        {
            char ch = ' ';
            int num = KeyInterop.VirtualKeyFromKey(key);
            byte[] lpKeyState = new byte[0x100];
            GetKeyboardState(lpKeyState);
            uint wScanCode = MapVirtualKey((uint)num, MapType.MAPVK_VK_TO_VSC);
            StringBuilder pwszBuff = new StringBuilder(2);
            switch (ToUnicode((uint)num, wScanCode, lpKeyState, pwszBuff, pwszBuff.Capacity, 0))
            {
                case -1:
                    return ch;

                case 0:
                    return ch;

                case 1:
                    return pwszBuff[0];
            }
            return pwszBuff[0];
        }

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

        public enum MapType : uint
        {
            MAPVK_VK_TO_CHAR = 2,
            MAPVK_VK_TO_VSC = 0,
            MAPVK_VSC_TO_VK = 1,
            MAPVK_VSC_TO_VK_EX = 3
        }
    }
}

