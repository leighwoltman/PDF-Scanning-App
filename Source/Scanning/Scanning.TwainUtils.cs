using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;


namespace Scanning
{
  partial class TwainDataSourceManager
  {
    private class TwainUtils
    {
      //public static float FloatFromFix32(UInt32 value)
      //{
      //  short whole = (short)(value & 0x0000ffff);
      //  ushort frac = (ushort)(value >> 16);
      //  return (float)whole + ((float)frac / 65536.0f);
      //}


      //public static UInt32 Fix32FromFloat(float value)
      //{
      //  int i = (int)((value * 65536.0f) + 0.5f);
      //  short Whole = (short)(i >> 16);
      //  ushort Frac = (ushort)(i & 0x0000ffff);
      //  return (UInt32)(ushort)Whole + ((uint)Frac << 16);
      //}


      public static Image DibToImage(IntPtr dibPtr)
      {
        // Saraff.Twain.dll
        // © SARAFF SOFTWARE 2011.

        MemoryStream _stream = new MemoryStream();
        BinaryWriter _writer = new BinaryWriter(_stream);

        BITMAPINFOHEADER _bmi = (BITMAPINFOHEADER)Marshal.PtrToStructure(dibPtr, typeof(BITMAPINFOHEADER));

        int _extra = 0;
        if(_bmi.biCompression == 0)
        {
          int _bytesPerRow = ((_bmi.biWidth * _bmi.biBitCount) >> 3);
          _extra = Math.Max(_bmi.biHeight * (_bytesPerRow + ((_bytesPerRow & 0x3) != 0 ? 4 - _bytesPerRow & 0x3 : 0)) - _bmi.biSizeImage, 0);
        }

        int _dibSize = _bmi.biSize + _bmi.biSizeImage + _extra + (_bmi.biClrUsed << 2);

        #region BITMAPFILEHEADER

        _writer.Write((ushort)0x4d42);
        _writer.Write(14 + _dibSize);
        _writer.Write(0);
        _writer.Write(14 + _bmi.biSize + (_bmi.biClrUsed << 2));

        #endregion

        #region BITMAPINFO and pixel data

        byte[] _data = new byte[_dibSize];
        Marshal.Copy(dibPtr, _data, 0, _data.Length);
        _writer.Write(_data);

        #endregion

        return Image.FromStream(_stream);
      }


      [StructLayout(LayoutKind.Sequential, Pack = 2)]
      private class BITMAPINFOHEADER
      {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
      }
    }
  }
}