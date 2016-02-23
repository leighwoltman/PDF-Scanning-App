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


  class Document
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
      pageToDelete.cleanUp();
      RaisePageRemoved(index);
    }


    public void RemoveAll()
    {
      int count = fPages.Count;

      for(int i = 0; i < count; i++)
      {
        fPages[0].cleanUp();
        fPages.RemoveAt(0);
      }

      RaisePageRemoved(-1);
    }


    // TODO: Provide a generic orientation function
    public void RotatePage(int index)
    {
      Page targetPage = fPages[index];
      targetPage.rotate();
      RaisePageUpdated(index);
    }


    public void LandscapePage(int index)
    {
      Page targetPage = fPages[index];
      targetPage.makeLandscape();
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
