using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using Imports;
using Utils;
using PdfiumDotNetWrapper;


namespace PdfProcessing
{
  class LibPdfium
  {
    // Interned strings are cached over AppDomains. This means that when we
    // lock on this string, we actually lock over AppDomain's. The Pdfium
    // library is not thread safe, and this way of locking
    // guarantees that we don't access the Pdfium library from different
    // threads, even when there are multiple AppDomain's in play.
    private static readonly string LockString = String.Intern("e362349b-001d-4cb2-bf55-a71606a3e36f");

    public static void SampleCode()
    {
      lock(LockString)
      {
        Pdfium.InitLibrary();
      }
    }


    private static bool fInitialized = false;


    public static void Initialize()
    {
      if(fInitialized == false)
      {
        Pdfium.InitLibrary();
        fInitialized = true;
      }
    }


    public static void Close()
    {
      Pdfium.CloseLibrary();
      fInitialized = false;
    }


    public static IntPtr LoadDocument(string path)
    {
      IntPtr result = IntPtr.Zero;
      byte[] bytes = Encoding.ASCII.GetBytes(path);

      unsafe
      {
        fixed(byte* p = bytes)
        {
          result = Pdfium.LoadDocument((sbyte*)p, null);
        }
      }

      return result;
    }


    public static bool SaveDocument(IntPtr document, string filename)
    {
      return Pdfium.SaveDocument(document, filename);
    }


    public static IntPtr CreateNewDocument()
    {
      return Pdfium.CreateNewDocument();
    }


    public static void CloseDocument(IntPtr document)
    {
      Pdfium.CloseDocument(document);
    }


    public static IntPtr LoadPage(IntPtr document, int pageNumber)
    {
      return Pdfium.LoadPage(document, pageNumber);
    }


    public static IntPtr CreatePage(IntPtr document, SizeInches size)
    {
      int pageIndex = GetPageCount(document);
      double widthInPoints = size.Width * 72;
      double heightInPoints = size.Height * 72;
      return Pdfium.CreatePage(document, pageIndex, widthInPoints, heightInPoints);
    }


    public static void AddBitmapToPage(IntPtr document, IntPtr page, Image image, BoundsInches imageBounds, int transformationIndex)
    {
      Bitmap bitmap = image as Bitmap;

      BitmapData bi = bitmap.LockBits(
          new Rectangle(0, 0, bitmap.Width, bitmap.Height),
          ImageLockMode.ReadOnly,
          bitmap.PixelFormat);

      PixelFormatEnum pf = ConvertPixelFormat_SystemToLibPdfium(bi.PixelFormat);

      TransformationMatrix matrix = GetTransformationMatrix(imageBounds, transformationIndex);

      try
      {
        unsafe
        {
          BitmapInfo info = new BitmapInfo();
          info.Height = bitmap.Height;
          info.Width = bitmap.Width;
          info.PixelFormat = pf;
          info.Data = bi.Scan0;
          info.Stride = bi.Stride;

          Pdfium.AddBitmapToPage(document, page, &info, &matrix);
        }
      }
      finally
      {
        bitmap.UnlockBits(bi);
      }
    }


    public static void AddJpegToPage(IntPtr document, IntPtr page, Image image, BoundsInches imageBounds, int transformationIndex)
    {
      byte[] buffer = Utils.Imaging.ByteArrayFromImage(image);

      // imageBounds are dimensions of transformed image (as displayed)
      TransformationMatrix matrix = GetTransformationMatrix(imageBounds, transformationIndex);

      unsafe
      {
        fixed(byte* p = buffer)
        {
          JpegInfo info = new JpegInfo();
          info.Height = image.Height;
          info.Width = image.Width;
          info.Data = (IntPtr)p;
          info.Size = buffer.Length;

          Pdfium.AddJpegImageToPage(document, page, &info, &matrix);
        }
      }
    }


    private static int[,] fTransformationArray = new int[8, 6] 
    {
      { 1, 0, 0, 1, 0, 0},
      { 0,-1, 1, 0, 0, 1},
      {-1, 0, 0,-1, 1, 1},
      { 0, 1,-1, 0, 1, 0},
      {-1, 0, 0, 1, 1, 0},
      { 0, 1, 1, 0, 0, 0},
      { 1, 0, 0,-1, 0, 1},
      { 0,-1,-1, 0, 1, 1}
    };


    private static TransformationMatrix GetTransformationMatrix(BoundsInches imageBounds, int transformationIndex)
    {
      TransformationMatrix result = new TransformationMatrix();

      result.a = fTransformationArray[transformationIndex, 0];
      result.b = fTransformationArray[transformationIndex, 1];
      result.c = fTransformationArray[transformationIndex, 2];
      result.d = fTransformationArray[transformationIndex, 3];
      result.e = fTransformationArray[transformationIndex, 4];
      result.f = fTransformationArray[transformationIndex, 5];

      result.a *= imageBounds.Width;
      result.b *= imageBounds.Height;
      result.c *= imageBounds.Width;
      result.d *= imageBounds.Height;
      result.e *= imageBounds.Width;
      result.e += imageBounds.X;
      result.f *= imageBounds.Height;
      result.f += imageBounds.Y;

      result.a *= 72;
      result.b *= 72;
      result.c *= 72;
      result.d *= 72;
      result.e *= 72;
      result.f *= 72;

      return result;
    }


    private static int GetSignOrZero(double value)
    {
      int result;

      if(value < 0)
      {
        result = -1;
      }
      else if(value > 0)
      {
        result = 1;
      }
      else
      {
        result = 0;
      }

      return result;
    }


    private static int GetTransformationIndex(TransformationMatrix transformationMatrix, BoundsInches imageBounds)
    {
      int[] vector = new int[4];

      vector[0] = GetSignOrZero(transformationMatrix.a);
      vector[1] = GetSignOrZero(transformationMatrix.b);
      vector[2] = GetSignOrZero(transformationMatrix.c);
      vector[3] = GetSignOrZero(transformationMatrix.d);

      int result = 0; // Default is to use the original picture

      for(int i = 0; i < 8; i++)
      {
        bool match = true;

        for(int j = 0; j < 4; j++)
        {
          if(fTransformationArray[i, j] != vector[j])
          {
            match = false;
          }
        }

        if(match)
        {
          result = i;
        }
      }

      // TODO: Calculate imageBounds
      imageBounds = null;

      return result;
    }


    public static void ClosePage(IntPtr page)
    {
      Pdfium.ClosePage(page);
    }


    public static int GetPageCount(IntPtr document)
    {
      return Pdfium.GetPageCount(document);
    }


    public static bool CopyPage(IntPtr destDoc, IntPtr sourceDoc, int pageNumber)
    {
      int pageIndex = GetPageCount(destDoc);
      return Pdfium.CopyPage(destDoc, sourceDoc, pageNumber, pageIndex);
    }


    public static double GetPageWidth(IntPtr page)
    {
      // Convert points to inches
      return Pdfium.GetPageWidth(page) / 72;
    }


    public static double GetPageHeight(IntPtr page)
    {
      // Convert points to inches
      return Pdfium.GetPageHeight(page) / 72;
    }


    private static int GetNumObjectsInPage(IntPtr page)
    {
      return Pdfium.GetNumObjectsInPage(page);
    }


    private static IntPtr GetObjectFromPage(IntPtr page, int index)
    {
      return Pdfium.GetObjectFromPage(page, index);
    }


    private static PdfImageInfo GetImageInfoFromObject(IntPtr pageObject)
    {
      PdfImageInfo info = new PdfImageInfo();

      unsafe
      {
        Pdfium.GetImageFromPageObject(pageObject, &info);
      }
      return info;
    }


    public static Bitmap Render(IntPtr page, int pixWidth, int pixHeight)
    {
      Bitmap bitmap = new Bitmap(pixWidth, pixHeight, PixelFormat.Format32bppArgb);

      BitmapData bi = bitmap.LockBits(
          new Rectangle(0, 0, bitmap.Width, bitmap.Height),
          ImageLockMode.ReadWrite,
          bitmap.PixelFormat);

      PixelFormatEnum pf = ConvertPixelFormat_SystemToLibPdfium(bi.PixelFormat);

      try
      {
        unsafe
        {
          BitmapInfo info = new BitmapInfo();
          info.Height = bitmap.Height;
          info.Width = bitmap.Width;
          info.PixelFormat = pf;
          info.Data = bi.Scan0;
          info.Stride = bi.Stride;

          Pdfium.RenderPageToBitmap(page, &info);
        }
      }
      finally
      {
        bitmap.UnlockBits(bi);
      }

      return bitmap;
    }


    public static Image GetSingleImageFromPdfDocument(IntPtr docPtr, IntPtr pagePtr)
    {
      Image result = null;

      try
      {
        int numObjects = LibPdfium.GetNumObjectsInPage(pagePtr);

        // TODO: Temporary code to do the single image only if there is a single object in the page.
        if(numObjects > 1)
        {
          numObjects = 0;
        }

        for(int n = 0; (n < numObjects) && (result == null); n++)
        {
          IntPtr objPtr = LibPdfium.GetObjectFromPage(pagePtr, n);

          PdfImageInfo imageInfo = LibPdfium.GetImageInfoFromObject(objPtr);

          if((imageInfo.DataSize > 0) && (imageInfo.Height > 0) && (imageInfo.Width > 0))
          {
            byte[] byteArray = new byte[imageInfo.DataSize];

            // TODO: transformationIndex = GetTransformationIndex(imageInfo.Matrix, imageBounds);

            unsafe
            {
              IntPtr hData = LibKernel32.GlobalLock((IntPtr)imageInfo.pData);
              Marshal.Copy(hData, byteArray, 0, imageInfo.DataSize);
              LibKernel32.GlobalUnlock((IntPtr)imageInfo.pData);
            }

            if(imageInfo.Filter == DecoderEnum.FlateDecode)
            {
              MemoryStream compressedStream = new MemoryStream(byteArray, 2, byteArray.Length - 6);
              MemoryStream decompressedStream = new MemoryStream();

              DeflateStream ds = new DeflateStream(compressedStream, CompressionMode.Decompress, true);

              ds.CopyTo(decompressedStream);

              byte[] outputBuffer = decompressedStream.ToArray();
              result = Utils.Imaging.ImageFromByteArray(outputBuffer);
            }
            else if(imageInfo.Filter == DecoderEnum.DCTDecode)
            {
              // JPeg
              result = Utils.Imaging.ImageFromByteArray(byteArray);
            }
            else // Image already decoded
            {
              result = SaveStreamContents(imageInfo, byteArray);
            }
          }
        }
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
      }

      return result;
    }


    static private Image SaveStreamContents(PdfImageInfo imageInfo, byte[] byteArray)
    {
      Image result = null;

      Int32 BitsPerComponent = imageInfo.BitsPerComponent;
      Int32 ImageWidth = imageInfo.Width;
      Int32 ImageHeight = imageInfo.Height;

      if(BitsPerComponent == 8)
      {
        ColorSpaceEnum ColorSpace = imageInfo.ColorSpace;

        if((ColorSpace == ColorSpaceEnum.RGB) || (ColorSpace == ColorSpaceEnum.Gray))
        {
          // create empty bitmap
          Bitmap BM = new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format24bppRgb);

          // create a new contents array with bmp width
          Byte[] PixelBuf = new Byte[((3 * ImageWidth + 3) & ~3) * ImageHeight];

          // copy row by row
          Int32 IPtr = 0;
          Int32 BPtr = 0;

          if(ColorSpace == ColorSpaceEnum.RGB)
          {
            for(Int32 Row = 0; Row < ImageHeight; Row++)
            {
              // copy column by column
              for(Int32 Col = 0; Col < ImageWidth; Col++)
              {
                PixelBuf[BPtr + 2] = byteArray[IPtr++];
                PixelBuf[BPtr + 1] = byteArray[IPtr++];
                PixelBuf[BPtr] = byteArray[IPtr++];
                BPtr += 3;
              }
              BPtr = (BPtr + 3) & ~3;
            }
          }
          else
          {
            for(Int32 Row = 0; Row < ImageHeight; Row++)
            {
              // copy column by column
              for(Int32 Col = 0; Col < ImageWidth; Col++)
              {
                PixelBuf[BPtr + 2] = byteArray[IPtr];
                PixelBuf[BPtr + 1] = byteArray[IPtr];
                PixelBuf[BPtr] = byteArray[IPtr++]; // Increment only after the last one
                BPtr += 3;
              }
              BPtr = (BPtr + 3) & ~3;
            }
          }

          // Lock the bitmap's bits.  
          Rectangle LockRect = new Rectangle(0, 0, ImageWidth, ImageHeight);
          BitmapData BmpData = BM.LockBits(LockRect, ImageLockMode.WriteOnly, BM.PixelFormat);

          // Get the address of the first line.
          IntPtr ImagePtr = BmpData.Scan0;

          // Copy contents into the bitmap
          Marshal.Copy(PixelBuf, 0, ImagePtr, PixelBuf.Length);

          // unlock the bitmap
          BM.UnlockBits(BmpData);

          result = BM;
        }
      }
      else if(BitsPerComponent == 1)
      {
        // create empty bitmap
        Bitmap BM = new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format1bppIndexed);

        // Lock the bitmap's bits.  
        Rectangle LockRect = new Rectangle(0, 0, ImageWidth, ImageHeight);
        BitmapData BmpData = BM.LockBits(LockRect, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

        Byte[] PixelBuf = new Byte[BmpData.Stride * BmpData.Height];

        int instride = (ImageWidth + 7) / 8;
        int outstride = BmpData.Stride;
        int inpos = 0;
        int outpos = 0;
        for(int y = 0; y < BmpData.Height; y++)
        {
          Array.Copy(byteArray, inpos, PixelBuf, outpos, instride);
          inpos += instride;
          outpos += outstride;
        }

        // Get the address of the first line.
        IntPtr ImagePtr = BmpData.Scan0;

        // Copy contents into the bitmap
        Marshal.Copy(PixelBuf, 0, ImagePtr, PixelBuf.Length);

        // unlock the bitmap
        BM.UnlockBits(BmpData);

        result = BM;
      }
      else
      { }

      return result;
    }


    static private PixelFormatEnum ConvertPixelFormat_SystemToLibPdfium(PixelFormat value)
    {
      PixelFormatEnum result;

      switch(value)
      {
        case PixelFormat.Format1bppIndexed: result = PixelFormatEnum.Bits1_BW; break;
        case PixelFormat.Format8bppIndexed: result = PixelFormatEnum.Bits8_Gray; break;
        case PixelFormat.Format24bppRgb: result = PixelFormatEnum.Bits24_Rgb; break;
        case PixelFormat.Format32bppRgb: result = PixelFormatEnum.Bits32_Rgb; break;
        case PixelFormat.Format32bppArgb: result = PixelFormatEnum.Bits32_Argb; break;
        default: result = PixelFormatEnum.Invalid; break;
      }

      return result;
    }
  }
}
