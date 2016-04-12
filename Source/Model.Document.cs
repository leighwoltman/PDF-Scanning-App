using System;
using System.Collections.Generic;
using System.Text;


namespace Model
{
  class DocumentPageEventArgs : EventArgs
  {
    public int Index;
  }


  class DocumentPageMoveEventArgs : EventArgs
  {
    public int SourceIndex;
    public int TargetIndex;
  }


  public class Document
  {
    private List<Page> fPages;


    public Document()
    {
      fPages = new List<Page>();
    }


    public Page GetPage(int index)
    {
      return fPages[index];
    }


    public int NumPages
    {
      get { return fPages.Count; }
    }


    public void AddPage(Page newPage)
    {
      int index = fPages.Count;
      fPages.Insert(index, newPage);
      RaisePageAdded(index);
    }


    public void DeletePage(int index)
    {
      Page pageToDelete = fPages[index];
      fPages.RemoveAt(index);
      pageToDelete.CleanUp();
      RaisePageRemoved(index);
    }


    public void RemoveAll()
    {
      int count = fPages.Count;

      for(int i = 0; i < count; i++)
      {
        fPages[0].CleanUp();
        fPages.RemoveAt(0);
      }

      RaisePageRemoved(-1);
    }


    public void RearrangePages2Sided()
    {
      int loc_of_next = 1;
      while(loc_of_next < this.NumPages - 1)
      {
        this.MovePage(this.NumPages - 1, loc_of_next);
        loc_of_next += 2;
      }
    }


    // TODO: Provide a generic orientation function
    public void RotatePageClockwise(int index)
    {
      Page targetPage = fPages[index];
      targetPage.ImageRotateClockwise();
      RaisePageUpdated(index);
    }


    public void RotatePageCounterClockwise(int index)
    {
      Page targetPage = fPages[index];
      targetPage.ImageRotateCounterClockwise();
      RaisePageUpdated(index);
    }


    public void MirrorPageHorizontally(int index)
    {
      Page targetPage = fPages[index];
      targetPage.ImageMirrorHorizontally();
      RaisePageUpdated(index);
    }


    public void MirrorPageVertically(int index)
    {
      Page targetPage = fPages[index];
      targetPage.ImageMirrorVertically();
      RaisePageUpdated(index);
    }


    public void LandscapePage(int index)
    {
      Page targetPage = fPages[index];
      targetPage.RotateSideways();
      RaisePageUpdated(index);
    }


    public void MovePage(int sourceIndex, int targetIndex)
    {
      Page targetPage = fPages[sourceIndex];
      fPages.RemoveAt(sourceIndex);
      fPages.Insert(targetIndex, targetPage);
      RaisePageMoved(sourceIndex, targetIndex);
    }


    public event EventHandler OnPageAdded;


    private void RaisePageAdded(int index)
    {
      if(OnPageAdded != null)
      {
        DocumentPageEventArgs args = new DocumentPageEventArgs();
        args.Index = index;
        OnPageAdded(this, args);
      }
    }


    public event EventHandler OnPageRemoved;


    private void RaisePageRemoved(int index)
    {
      if(OnPageRemoved != null)
      {
        DocumentPageEventArgs args = new DocumentPageEventArgs();
        args.Index = index;
        OnPageRemoved(this, args);
      }
    }


    public event EventHandler OnPageUpdated;


    private void RaisePageUpdated(int index)
    {
      if(OnPageUpdated != null)
      {
        DocumentPageEventArgs args = new DocumentPageEventArgs();
        args.Index = index;
        OnPageUpdated(this, args);
      }
    }


    public event EventHandler OnPageMoved;


    private void RaisePageMoved(int sourceIndex, int targetIndex)
    {
      if(OnPageMoved != null)
      {
        DocumentPageMoveEventArgs args = new DocumentPageMoveEventArgs();
        args.SourceIndex = sourceIndex;
        args.TargetIndex = targetIndex;
        OnPageMoved(this, args);
      }
    }
  }
}
