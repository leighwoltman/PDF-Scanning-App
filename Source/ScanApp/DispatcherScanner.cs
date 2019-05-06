using System;
using System.Threading;
using System.Windows.Interop;
using System.Windows;
using HouseUtils.Threading;


namespace ScanApp
{
  class DispatcherScanner : DispatcherUI
  {
    private Window fDummyWindow;


    public IntPtr GetWindowHandle()
    {
      // This must be called from the WindowThread (Name: "DispatcherScanner")
      return new WindowInteropHelper(fDummyWindow).Handle;
    }


    public DispatcherScanner()
    {
      AutoResetEvent signal = new AutoResetEvent(false);

      Thread windowThread = new Thread(new ThreadStart(() =>
      {
        fDummyWindow = new Window();
        fDummyWindow.WindowStartupLocation = WindowStartupLocation.Manual;
        fDummyWindow.Left = -100;
        fDummyWindow.Top = 0;
        fDummyWindow.Height = 0;
        fDummyWindow.Width = 0;
        fDummyWindow.ShowInTaskbar = false;
        fDummyWindow.WindowStyle = WindowStyle.None;
        fDummyWindow.AllowsTransparency = true;
        fDummyWindow.Show();
        // Framework returns zero handle if the window is not shown
        signal.Set();
        System.Windows.Threading.Dispatcher.Run();
      }));
      windowThread.Name = "DispatcherScanner";
      windowThread.SetApartmentState(ApartmentState.STA);
      windowThread.IsBackground = true;
      windowThread.Start();

      // Wait until the window is created
      signal.WaitOne(10000);

      this.Init(fDummyWindow.Dispatcher);
    }


    public override void Stop()
    {
      this.Post(() =>
      {
        if (fDummyWindow != null)
        {
          fDummyWindow.Close();
          fDummyWindow = null;
        }
      });
    }
  }
}
