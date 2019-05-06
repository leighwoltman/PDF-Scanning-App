using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Twain
{
  class Utils
  {
    public static float FloatFromFix32(UInt32 value)
    {
      short whole = (short)(value & 0x0000ffff);
      ushort frac = (ushort)(value >> 16);
      return (float)whole + ((float)frac / 65536.0f);
    }


    public static UInt32 Fix32FromFloat(float value)
    {
      int i = (int)((value * 65536.0f) + 0.5f);
      short Whole = (short)(i >> 16);
      ushort Frac = (ushort)(i & 0x0000ffff);
      return (UInt32)(ushort)Whole + ((uint)Frac << 16);
    }
  }
}