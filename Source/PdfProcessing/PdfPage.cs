using System;
using System.Diagnostics;
using HouseUtils;
using HouseImaging;


namespace PdfProcessing
{
  public class PdfPage
  {
    public IntPtr Ptr { get; private set; }

    public PdfDocument Document { get; }

    public int Index { get; private set; }

    public Size2D Size { get; set; }


    public PdfPage(PdfDocument doc, Size2D pageSize)
    {
      // Create a new page
      this.Document = doc;
      this.Index = -1;
      this.Size = pageSize;
    }


    public PdfPage(PdfDocument doc, int index)
    {
      this.Document = doc;
      this.Index = index;

      if (Open())
      {
        double width = LibPdfium.GetPageWidth(Ptr);
        double height = LibPdfium.GetPageHeight(Ptr);
        this.Size = new Size2D(width, height);
        Close();
      }
      else
      {
        Debug.Fail("PdfPage creation error");
      }

      Debug.Assert(this.Size != null, "PdfPage size error");
    }


    public bool IsOpen
    {
      get { return Ptr != IntPtr.Zero; }
    }


    public bool Open()
    {
      if (Document.Open())
      {
        if (this.Index < 0)
        {
          // New page
          this.Index = Document.PageCount;
          this.Ptr = LibPdfium.CreatePage(Document.Ptr, this.Size, this.Index + 1);
        }
        else
        {
          this.Ptr = LibPdfium.LoadPage(Document.Ptr, this.Index);
        }
      }

      return IsOpen;
    }


    public void Close()
    {
      if (IsOpen)
      {
        LibPdfium.ClosePage(Ptr);
        Ptr = IntPtr.Zero;
        Document.Close();
      }
    }


    public ImageInfo ExtractSingleImage()
    {
      ImageInfo result = null;

      if (Open())
      {
        result = new ImageInfo(LibPdfium.GetSingleImageFromPdfDocument(Document.Ptr, this.Ptr));
        Close();
      }

      return result;
    }


    public ImageInfo Render(int resolution)
    {
      int pixWidth = (int)(Size.Width * resolution);
      int pixHeight = (int)(Size.Height * resolution);
      return Render(pixWidth, pixHeight);
    }


    public ImageInfo Render(int pixWidth, int pixHeight)
    {
      ImageInfo result = null;

      if (Open())
      {
        result = new ImageInfo(LibPdfium.Render(this.Ptr, pixWidth, pixHeight));
        Close();
      }

      return result;
    }


    public void DrawImage(ImageInfo sourceImage, ImageTransformer transform, Bounds imageBounds)
    {
      if (Open())
      {
        System.Drawing.Image image = sourceImage.SystemImage;
        ImageFormatEnum imageFormat = sourceImage.GetImageFormat();

        switch (imageFormat)
        {
          case ImageFormatEnum.Jpeg:
            {
              LibPdfium.AddJpegToPage(Document.Ptr, this.Ptr, image, imageBounds, transform.Value);
            }
            break;

          case ImageFormatEnum.Bmp:
            {
              LibPdfium.AddBitmapToPage(Document.Ptr, this.Ptr, image, imageBounds, transform.Value);
            }
            break;

          case ImageFormatEnum.Png:
            {
              // Keep PNG because:
              // -It seems PNG and BMP are both stored the same way
              // -Converting to BMP loses the Alpha component of ARGB
              LibPdfium.AddBitmapToPage(Document.Ptr, this.Ptr, image, imageBounds, transform.Value);
            }
            break;

          default:
            {
              // All other types convert to PNG
              System.Drawing.Image storedImage = ImageTools.ConvertImage(image, System.Drawing.Imaging.ImageFormat.Png, 0);
              LibPdfium.AddBitmapToPage(Document.Ptr, this.Ptr, storedImage, imageBounds, transform.Value);
            }
            break;
        }

        Close();
      }
    }
  }
}
