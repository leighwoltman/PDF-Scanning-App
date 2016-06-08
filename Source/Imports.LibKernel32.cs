using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;


namespace Imports
{
  [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
  [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
  public class MemoryMappedHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public MemoryMappedHandle()
      : base(true)
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
      return LibKernel32.CloseHandle(handle);
    }
  }

  public class MappedViewHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public MappedViewHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
      return LibKernel32.UnmapViewOfFile(handle);
    }
  }

  
  class LibKernel32
  {
    [DllImport("kernel32.dll", ExactSpelling = true)]
    public static extern IntPtr GlobalAlloc(int flags, int size);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    public static extern IntPtr GlobalLock(IntPtr handle);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    public static extern bool GlobalUnlock(IntPtr handle);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    public static extern IntPtr GlobalFree(IntPtr handle);
  
    [DllImport("kernel32.dll", ExactSpelling = true)]
    public static extern int GlobalSize(IntPtr handle);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
    public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern MemoryMappedHandle CreateFileMapping(SafeHandle hFile, IntPtr lpFileMappingAttributes, FileMapProtection flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, [MarshalAs(UnmanagedType.LPTStr)] string lpName);

    [Flags]
    public enum FileMapProtection : uint
    {
      PageReadonly = 0x02,
      PageReadWrite = 0x04,
      PageWriteCopy = 0x08,
      PageExecuteRead = 0x20,
      PageExecuteReadWrite = 0x40,
      SectionCommit = 0x8000000,
      SectionImage = 0x1000000,
      SectionNoCache = 0x10000000,
      SectionReserve = 0x4000000,
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern MappedViewHandle MapViewOfFile(SafeHandle hFileMappingObject, FileMapAccess dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

    [Flags]
    public enum FileMapAccess : uint
    {
      FileMapCopy = 0x0001,
      FileMapWrite = 0x0002,
      FileMapRead = 0x0004,
      FileMapAllAccess = 0x001f,
      FileMapExecute = 0x0020,
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);
  }
}
