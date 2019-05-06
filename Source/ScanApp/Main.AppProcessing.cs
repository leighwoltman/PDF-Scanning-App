using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Documents;
using PdfProcessing;
using HouseImaging;
using HouseUtils.Threading;


namespace ScanApp
{
  class AppProcessing
  {
    private HouseDispatcher fUI;
    private HouseDispatcher fThread;
    private AppSettings fAppSettings;
    private AppModel fAppModel;


    public AppProcessing(AppSettings appSettings, AppModel appModel)
    {
      fUI = new DispatcherUI();
      fThread = new DispatcherThread("Processing");
      fAppSettings = appSettings;
      fAppModel = appModel;

      LibPdfium.Initialize();
    }


    public BitmapImage BlankThumbnail { get; set; } = null;


    public void Terminate()
    {
      fThread.Stop();
      LibPdfium.Close();
    }


    #region Import/Export

    public void ImportFiles(string[] filenames)
    {
      foreach (string filename in filenames)
      {
        ImportFile(filename);
      }
    }


    public void ImportFile(string filename)
    {
      switch (Path.GetExtension(filename).ToLower())
      {
        case ".pdf":
          {
            ImportPdf(filename);
          }
          break;
        default:
          {
            ImportImageFile(filename);
          }
          break;
      }
    }


    public void ImportImageFile(string filename)
    {
      Page page = PageFromImageFile.Create(fAppSettings.GetDefaultPageSize(), filename);
      AddPage(page);
    }


    public void ImportPdf(string filename)
    {
      PdfDocument pdf = new PdfDocument(filename);

      pdf.Open();

      for (int i = 0; i < pdf.PageCount; i++)
      {
        PdfPage pdfPage = pdf.GetPage(i);

        Page page = PageFromPdf.Create(pdfPage, fAppSettings.PdfSettings);

        AddPage(page);
      }

      pdf.Close();
    }


    public void AddPage(Page page)
    {
      ListViewPageItem pageItem = new ListViewPageItem(fAppModel.PageItems.Count, page);
      fAppModel.PageItems.Add(pageItem);
      InvalidatePage(pageItem);
      RefreshDisplay();
    }


    public void ExportToPdf(string filename, List<ListViewPageItem> pageItems, bool append, ExportSettings exportSettings)
    {
      fAppModel.Exporting = true;

      fThread.Post(() =>
      {
        PdfDocument pdfDocument = new PdfDocument();

        pdfDocument.Open();

        if (append && File.Exists(filename))
        {
          PdfDocument sourceDoc = new PdfDocument(filename);

          sourceDoc.Open();

          for (int i = 0; i < sourceDoc.PageCount; i++)
          {
            pdfDocument.CopyNativePage(sourceDoc.GetPage(i));
          }

          sourceDoc.Close();
        }

        for (int index = 0; index < pageItems.Count; index++)
        {
          fUI.Post(() =>
          {
            fAppModel.ExportText = string.Format("Exporting {0} of {1} items", index + 1, pageItems.Count);
          });

          ListViewPageItem item = pageItems[index];
          Page page = item.Page;
          page.ExportToPdf(pdfDocument, exportSettings);
        }

        // Save the document...
        fUI.Post(() =>
        {
          fAppModel.ExportText = "Exporting: Saving file...";
        });

        pdfDocument.Save(filename);
        pdfDocument.Close();

        fUI.Post(() =>
        {
          if (exportSettings.RemovePagesAfterPdfExport)
          {
            foreach (ListViewPageItem item in pageItems)
            {
              fAppModel.PageItems.Remove(item);
            }
          }

          fAppModel.ExportText = "Exporting: Done";
          fAppModel.Exporting = false;
        });

      });
    }


    public void SaveImages(string filename, List<ListViewPageItem> pageItems, ExportSettings exportSettings)
    {
      // This is not really necessary since all this is done in main thread
      fAppModel.Exporting = true;

      string directory = Path.GetDirectoryName(filename);
      string name = Path.GetFileNameWithoutExtension(filename);
      string extension = Path.GetExtension(filename);

      foreach (var item in pageItems)
      {
        Page page = item.Page;

        string path;

        if (pageItems.Count > 1)
        {
          name = page.Name;
        }

        path = Path.Combine(directory, name + extension);

        page.SaveImageToFile(path, exportSettings);
      }

      fAppModel.Exporting = false;
    }

    #endregion


    #region Editing

    public void PagesMirrorHorizontally(List<ListViewPageItem> pageItems)
    {
      foreach (var item in pageItems)
      {
        fThread.Post(() => { item.Page.ImageMirrorHorizontally(); });
        InvalidatePage(item);
      }
      RefreshDisplay();
    }


    public void PagesMirrorVertically(List<ListViewPageItem> pageItems)
    {
      foreach (var item in pageItems)
      {
        fThread.Post(() => { item.Page.ImageMirrorVertically(); });
        InvalidatePage(item);
      }
      RefreshDisplay();
    }


    public void PagesRotateCounterClockwise(List<ListViewPageItem> pageItems)
    {
      foreach (var item in pageItems)
      {
        fThread.Post(() =>
        {
          item.Page.ImageRotateCounterClockwise();
          item.Page.RotateSideways();
        });

        InvalidatePage(item);
      }
      RefreshDisplay();
    }


    public void PagesRotateClockwise(List<ListViewPageItem> pageItems)
    {
      foreach (var item in pageItems)
      {
        fThread.Post(() =>
        {
          item.Page.ImageRotateClockwise();
          item.Page.RotateSideways();
        });

        InvalidatePage(item);
      }
      RefreshDisplay();
    }


    public void PagesRotateSideways(List<ListViewPageItem> pageItems)
    {
      foreach (var item in pageItems)
      {
        fThread.Post(() => { item.Page.RotateSideways(); });
        InvalidatePage(item);
      }
      RefreshDisplay();
    }


    public void PagesDelete(List<ListViewPageItem> pageItems)
    {
      foreach (var item in pageItems)
      {
        fAppModel.PageItems.Remove(item);
      }
    }


    public void AllPagesDelete()
    {
      fAppModel.PageItems.Clear();
    }


    public void AllPagesSelect()
    {
      foreach (ListViewPageItem item in fAppModel.PageItems)
      {
        item.IsSelected = true;
      }
    }


    public void AllPagesShuffle2Sided()
    {
      int loc_of_next = 1;
      while (loc_of_next < fAppModel.PageItems.Count - 1)
      {
        fAppModel.PageItems.Move(fAppModel.PageItems.Count - 1, loc_of_next);
        loc_of_next += 2;
      }
    }


    private void InvalidatePage(ListViewPageItem pageItem)
    {
      if (pageItem != null)
      {
        pageItem.Thumbnail = BlankThumbnail;
        pageItem.Image = null;
        pageItem.ThumbnailIsDirty = true;
        pageItem.ImageIsDirty = true;
      }
    }


    bool fTasked = false;


    public void RefreshDisplay()
    {
      ListViewPageItem pageItem = fAppModel.SelectedPageItem;

      if (pageItem != null)
      {
        if (pageItem.Image == null)
        {
          pageItem.Image = pageItem.Thumbnail;
        }
      }

      if (fTasked == false)
      {
        fTasked = true;
        DoNextTask();
      }
    }


    private void DoNextTask()
    {
      if (RefreshSelected())
      {
        // We queued a task;
      }
      else if (RefreshThumb())
      {
        // We queued a task;
      }
      else
      {
        // Fine
        fTasked = false;
      }
    }


    private bool RefreshSelected()
    {
      bool result = false;

      ListViewPageItem pageItem = fAppModel.SelectedPageItem;

      if ((pageItem != null) && pageItem.ImageIsDirty)
      {
        // Clear the flag first because this way if the page is edited again while we are working on it
        // we will not know that this happened by checking the flag again.
        pageItem.ImageIsDirty = false;

        fThread.Post(() =>
        {
          using (ImageInfo layout = pageItem.Page.GetLayout())
          {
            ImageSource layoutImageSource = layout.GetSystemImageSource();

            layoutImageSource.Freeze();

            fUI.Post(() =>
            {
              if(pageItem.ImageIsDirty == false)
              {
                pageItem.Image = layoutImageSource;
              }
              DoNextTask();
            });
          }
        });

        result = true;
      }

      return result;
    }


    private bool RefreshThumb()
    {
      bool result = false;

      ListViewPageItem pageItem = fAppModel.PageItems.FirstOrDefault(x => x.ThumbnailIsDirty);

      if (pageItem != null)
      {
        // Clear the flag first because this way if the page is edited again while we are working on it
        // we will not know that this happened by checking the flag again.
        pageItem.ThumbnailIsDirty = false; 

        fThread.Post(() =>
        {
          using (ImageInfo thumb = pageItem.Page.CreateThumbnail(100, 100))
          {
            ImageSource thumbImageSource = thumb.GetSystemImageSource();

            // For fixing: Must create DependencySource on same Thread as the DependencyObject.
            thumbImageSource.Freeze();

            fUI.Post(() =>
            {
              if (pageItem.ThumbnailIsDirty == false)
              {
                pageItem.Thumbnail = thumbImageSource;
              }
              DoNextTask();
            });
          }
        });

        result = true;
      }

      return result;
    }


    public delegate void ComparePagesCallback(bool isEqual);

    public void ComparePages(ListViewPageItem pageItem1, ListViewPageItem pageItem2, ComparePagesCallback callback)
    {
      fThread.Post(() =>
      {
        using (ImageInfo image1 = pageItem1.Page.GetSourceImage())
        {
          using (ImageInfo image2 = pageItem2.Page.GetSourceImage())
          {
            bool result = image1.IsEqual(image2);
            fUI.Post(callback, result);
          }
        }
      });
    }


    public delegate void GetPageInfoCallback(List<KeyValuePair<string, string>> properties);

    public void GetPageInfo(ListViewPageItem pageItem, GetPageInfoCallback callback)
    {
      fThread.Post(() =>
      {
        List<KeyValuePair<string, string>> result = pageItem.Page.GetInfoTable();
        fUI.Post(callback, result);
      });
    }

    #endregion 
  }
}
