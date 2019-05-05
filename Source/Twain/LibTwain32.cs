using System;
using System.Collections;
using System.Runtime.InteropServices;
using NativeLibs;


namespace Twain
{
  class LibTwain32
  {
    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSMparent([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr refptr);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSMident([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwIdentity idds);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSMstatus([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSMcallback([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, ref TwCallback dsmcallback);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSuserif([In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, TwUserInterface guif);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSevent([In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref TwEvent evt);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSstatus([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DScap([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwCapability capa);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSiinf([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwImageInfo imginf);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSilayout([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwImageLayout value);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSixfer([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr hbitmap);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DSpxfer([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwPendingXfers pxfr);

    [DllImport("twain_32.dll", EntryPoint = "#1")]
    public static extern TwRC DScustomData([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwCustomDsData data);
  }


  [Flags]
  public enum TwDG : short
  {
    Control = 0x0001,
    Image = 0x0002,
    Audio = 0x0004
  }


  public enum TwDAT : short
  {
    Null = 0x0000,
    Capability = 0x0001,
    Event = 0x0002,
    Identity = 0x0003,
    Parent = 0x0004,
    PendingXfers = 0x0005,
    SetupMemXfer = 0x0006,
    SetupFileXfer = 0x0007,
    Status = 0x0008,
    UserInterface = 0x0009,
    XferGroup = 0x000a,
    TwunkIdentity = 0x000b,
    CustomDSData = 0x000c,
    DeviceEvent = 0x000d,
    FileSystem = 0x000e,
    PassThru = 0x000f,
    Callback = 0x0010,
    ImageInfo = 0x0101,
    ImageLayout = 0x0102,
    ImageMemXfer = 0x0103,
    ImageNativeXfer = 0x0104,
    ImageFileXfer = 0x0105,
    CieColor = 0x0106,
    GrayResponse = 0x0107,
    RGBResponse = 0x0108,
    JpegCompression = 0x0109,
    Palette8 = 0x010a,
    ExtImageInfo = 0x010b,
    SetupFileXfer2 = 0x0301
  }


  public enum TwMSG : short
  {
    Null = 0x0000,
    Get = 0x0001,
    GetCurrent = 0x0002,
    GetDefault = 0x0003,
    GetFirst = 0x0004,
    GetNext = 0x0005,
    Set = 0x0006,
    Reset = 0x0007,
    QuerySupport = 0x0008,
    XFerReady = 0x0101,
    CloseDSReq = 0x0102,
    CloseDSOK = 0x0103,
    DeviceEvent = 0x0104,
    CheckStatus = 0x0201,
    OpenDSM = 0x0301,
    CloseDSM = 0x0302,
    OpenDS = 0x0401,
    CloseDS = 0x0402,
    UserSelect = 0x0403,
    DisableDS = 0x0501,
    EnableDS = 0x0502,
    EnableDSUIOnly = 0x0503,
    ProcessEvent = 0x0601,
    EndXfer = 0x0701,
    StopFeeder = 0x0702,
    ChangeDirectory = 0x0801,
    CreateDirectory = 0x0802,
    Delete = 0x0803,
    FormatMedia = 0x0804,
    GetClose = 0x0805,
    GetFirstFile = 0x0806,
    GetInfo = 0x0807,
    GetNextFile = 0x0808,
    Rename = 0x0809,
    Copy = 0x080A,
    AutoCaptureDir = 0x080B,
    PassThru = 0x0901,
    RegisterCallback = 0x0902
  }


  public enum TwRC : short
  {
    Success = 0x0000,
    Failure = 0x0001,
    CheckStatus = 0x0002,
    Cancel = 0x0003,
    DSEvent = 0x0004,
    NotDSEvent = 0x0005,
    XferDone = 0x0006,
    EndOfList = 0x0007,
    InfoNotSupported = 0x0008,
    DataNotAvailable = 0x0009
  }


  public enum TwCC : short
  {
    Success = 0x0000,
    Bummer = 0x0001,
    LowMemory = 0x0002,
    NoDS = 0x0003,
    MaxConnections = 0x0004,
    OperationError = 0x0005,
    BadCap = 0x0006,
    BadProtocol = 0x0009,
    BadValue = 0x000a,
    SeqError = 0x000b,
    BadDest = 0x000c,
    CapUnsupported = 0x000d,
    CapBadOperation = 0x000e,
    CapSeqError = 0x000f,
    Denied = 0x0010,
    FileExists = 0x0011,
    FileNotFound = 0x0012,
    NotEmpty = 0x0013,
    PaperJam = 0x0014,
    PaperDoubleFeed = 0x0015,
    FileWriteError = 0x0016,
    CheckDeviceOnline = 0x0017
  }


  public enum TwOn : short
  {
    Array = 0x0003,
    Enum = 0x0004,
    One = 0x0005,
    Range = 0x0006,
    DontCare = -1
  }


  public enum TwType : short
  {
    Int8 = 0x0000,
    Int16 = 0x0001,
    Int32 = 0x0002,
    UInt8 = 0x0003,
    UInt16 = 0x0004,
    UInt32 = 0x0005,
    Bool = 0x0006,
    Fix32 = 0x0007,
    Frame = 0x0008,
    Str32 = 0x0009,
    Str64 = 0x000a,
    Str128 = 0x000b,
    Str255 = 0x000c,
    Str1024 = 0x000d,
    Str512 = 0x000e,
    DontCare16 = -1 // 0xFFFF
  }


  public enum TwCap : short
  {
    // all data sources are REQUIRED to support these capabilities
    XferCount = 0x0001,

    // image data sources are REQUIRED to support these capabilities
    ICompression = 0x0100,
    IPixelType = 0x0101,
    IUnits = 0x0102,
    IXferMech = 0x0103,

    // all data sources MAY support these capabilities
    Author = 0x1000,
    Caption = 0x1001,
    FeederEnabled = 0x1002,
    FeederLoaded = 0x1003,
    Timedate = 0x1004,
    SupportedCapabilities = 0x1005,
    Extendedcaps = 0x1006,
    AutoFeed = 0x1007,
    ClearPage = 0x1008,
    FeedPage = 0x1009,
    RewindPage = 0x100a,
    Indicators = 0x100b,
    SupportedCapsExt = 0x100c,
    PaperDetectable = 0x100d,
    UIControllable = 0x100e,
    DeviceOnline = 0x100f,
    AutoScan = 0x1010,
    ThumbnailsEnabled = 0x1011,
    Duplex = 0x1012,
    DuplexEnabled = 0x1013,
    Enabledsuionly = 0x1014,
    CustomdsData = 0x1015,
    Endorser = 0x1016,
    JobControl = 0x1017,
    Alarms = 0x1018,
    AlarmVolume = 0x1019,
    AutomaticCapture = 0x101a,
    TimeBeforeFirstCapture = 0x101b,
    TimeBetweenCaptures = 0x101c,
    ClearBuffers = 0x101d,
    MaxBatchBuffers = 0x101e,
    DeviceTimeDate = 0x101f,
    PowerSupply = 0x1020,
    CameraPreviewUI = 0x1021,
    DeviceEvent = 0x1022,
    SerialNumber = 0x1024,
    Printer = 0x1026,
    PrinterEnabled = 0x1027,
    PrinterIndex = 0x1028,
    PrinterMode = 0x1029,
    PrinterString = 0x102a,
    PrinterSuffix = 0x102b,
    Language = 0x102c,
    FeederAlignment = 0x102d,
    FeederOrder = 0x102e,
    ReAcquireAllowed = 0x1030,
    BatteryMinutes = 0x1032,
    BatteryPercentage = 0x1033,
    CameraSide = 0x1034,
    Segmented = 0x1035,
    CameraEnabled = 0x1036,
    CameraOrder = 0x1037,
    MicrEnabled = 0x1038,
    FeederPrep = 0x1039,
    Feederpocket = 0x103a,

    // image data sources MAY support these capabilities
    Autobright = 0x1100,
    Brightness = 0x1101,
    Contrast = 0x1103,
    CustHalftone = 0x1104,
    ExposureTime = 0x1105,
    Filter = 0x1106,
    Flashused = 0x1107,
    Gamma = 0x1108,
    Halftones = 0x1109,
    Highlight = 0x110a,
    ImageFileFormat = 0x110c,
    LampState = 0x110d,
    LightSource = 0x110e,
    Orientation = 0x1110,
    PhysicalWidth = 0x1111,
    PhysicalHeight = 0x1112,
    Shadow = 0x1113,
    Frames = 0x1114,
    XNativeResolution = 0x1116,
    YNativeResolution = 0x1117,
    XResolution = 0x1118,
    YResolution = 0x1119,
    MaxFrames = 0x111a,
    Tiles = 0x111b,
    Bitorder = 0x111c,
    Ccittkfactor = 0x111d,
    Lightpath = 0x111e,
    Pixelflavor = 0x111f,
    Planarchunky = 0x1120,
    Rotation = 0x1121,
    SupportedSizes = 0x1122,
    Threshold = 0x1123,
    Xscaling = 0x1124,
    Yscaling = 0x1125,
    Bitordercodes = 0x1126,
    Pixelflavorcodes = 0x1127,
    Jpegpixeltype = 0x1128,
    Timefill = 0x112a,
    BitDepth = 0x112b,
    BitDepthReduction = 0x112c,
    Undefinedimagesize = 0x112d,
    Imagedataset = 0x112e,
    Extimageinfo = 0x112f,
    Minimumheight = 0x1130,
    Minimumwidth = 0x1131,
    Fliprotation = 0x1136,
    Barcodedetectionenabled = 0x1137,
    Supportedbarcodetypes = 0x1138,
    Barcodemaxsearchpriorities = 0x1139,
    Barcodesearchpriorities = 0x113a,
    Barcodesearchmode = 0x113b,
    Barcodemaxretries = 0x113c,
    Barcodetimeout = 0x113d,
    Zoomfactor = 0x113e,
    Patchcodedetectionenabled = 0x113f,
    Supportedpatchcodetypes = 0x1140,
    Patchcodemaxsearchpriorities = 0x1141,
    Patchcodesearchpriorities = 0x1142,
    Patchcodesearchmode = 0x1143,
    Patchcodemaxretries = 0x1144,
    Patchcodetimeout = 0x1145,
    Flashused2 = 0x1146,
    Imagefilter = 0x1147,
    Noisefilter = 0x1148,
    Overscan = 0x1149,
    Automaticborderdetection = 0x1150,
    Automaticdeskew = 0x1151,
    Automaticrotate = 0x1152,
    Jpegquality = 0x1153,
    Feedertype = 0x1154,
    Iccprofile = 0x1155,
    Autosize = 0x1156,
    AutomaticCropUsesFrame = 0x1157,
    AutomaticLengthDetection = 0x1158,
    AutomaticColorEnabled = 0x1159,
    AutomaticColorNonColorPixelType = 0x115a,
    ColorManagementEnabled = 0x115b,
    ImageMerge = 0x115c,
    ImageMergeHeightThreshold = 0x115d,
    SupoortedExtImageInfo = 0x115e,
    Audiofileformat = 0x1201,
    Xfermech = 0x1202
  }


  public enum TwCapPixelType : short
  {
    BW = 0,
    Gray = 1,
    RGB = 2,
    Palette = 3,
    CMY = 4,
    CMYK = 5,
    YUV = 6,
    YUVK = 7,
    CIEXYZ = 8,
    LAB = 9,
    SRGB = 10,
    SCRGB = 11,
    INFRARED = 16
  }


  public enum TwCapPageType : short
  {
    None = 0,
    A4 = 1,
    JISB5 = 2,
    UsLetter = 3,
    UsLegal = 4,
    A5 = 5,
    ISOB4 = 6,
    ISOB6 = 7,
    B = 8,
    UsLedger = 9,
    UsExecutive = 10,
    A3 = 11,
    ISOB3 = 12,
    A6 = 13,
    C4 = 14,
    C5 = 15,
    C6 = 16,
    _4A0 = 17,
    _2A0 = 18,
    A0 = 19,
    A1 = 20,
    A2 = 21,
    A7 = 22,
    A8 = 23,
    A9 = 24,
    A10 = 25,
    ISOB0 = 26,
    ISOB1 = 27,
    ISOB2 = 28,
    ISOB5 = 29,
    ISOB7 = 30,
    ISOB8 = 31,
    ISOB9 = 32,
    ISOB10 = 33,
    JISB0 = 34,
    JISB1 = 35,
    JISB2 = 36,
    JISB3 = 37,
    JISB4 = 38,
    JISB6 = 39,
    JISB7 = 40,
    JISB8 = 41,
    JISB9 = 42,
    JISB10 = 43,
    C0 = 44,
    C1 = 45,
    C2 = 46,
    C3 = 47,
    C7 = 48,
    C8 = 49,
    C9 = 50,
    C10 = 51,
    UsStatement = 52,
    BusinessCard = 53,
    MaxSize = 54,
  }


  public enum TwCapBitDepthReductionType : short
  {
    THRESHOLD = 0,
    HALFTONE,
    CUSTHALFTONE,
    DIFFUSION,
    DYNAMICTHRESHOLD
  }


  public enum TwLanguage : short
  {
    USA = 13
  }


  public enum TwCountry : short
  {
    USA = 1
  }


  class TwProtocol
  {
    public const short Major = 1;
    public const short Minor = 9;
  }


  // ------------------- STRUCTS --------------------------------------------

  [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
  public class TwIdentity
  {
    public IntPtr Id;
    public TwVersion Version;
    public short ProtocolMajor;
    public short ProtocolMinor;
    public int SupportedGroups;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
    public string Manufacturer;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
    public string ProductFamily;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
    public string ProductName;

    public TwIdentity()
    {
      Id = IntPtr.Zero;
    }

    public override string ToString()
    {
      return ProductName;
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
  public struct TwVersion
  {
    public short MajorNum;
    public short MinorNum;
    public TwLanguage Language;
    public TwCountry Country;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
    public string Info;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwUserInterface
  {
    public short ShowUI;        // bool is strictly 32 bit, so use short
    public short ModalUI;
    public IntPtr ParentHand;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwStatus
  {
    public TwCC ConditionCode;
    public short Reserved;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct TwCallback
  {
    public IntPtr CallBackProc;
    public UInt32 RefCon;
    public UInt16 Message;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct TwEvent
  {
    public IntPtr EventPtr;
    public TwMSG Message;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwImageInfo
  {
    public int XResolution;
    public int YResolution;
    public int ImageWidth;
    public int ImageLength;
    public short SamplesPerPixel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public short[] BitsPerSample;
    public short BitsPerPixel;
    public short Planar;
    public short PixelType;
    public short Compression;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwFrame
  {
    private UInt32 fLeft;
    private UInt32 fTop;
    private UInt32 fRight;
    private UInt32 fBottom;

    public float Left
    {
      get { return Utils.FloatFromFix32(fLeft); }
      set { fLeft = Utils.Fix32FromFloat(value); }
    }

    public float Top
    {
      get { return Utils.FloatFromFix32(fTop); }
      set { fTop = Utils.Fix32FromFloat(value); }
    }

    public float Right
    {
      get { return Utils.FloatFromFix32(fRight); }
      set { fRight = Utils.Fix32FromFloat(value); }
    }

    public float Bottom
    {
      get { return Utils.FloatFromFix32(fBottom); }
      set { fBottom = Utils.Fix32FromFloat(value); }
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwImageLayout
  {
    public TwFrame Frame;
    public UInt32 DocumentNumber;
    public UInt32 PageNumber;
    public UInt32 FrameNumber;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwPendingXfers
  {
    public short Count;
    public int EOJ;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwCustomDsData
  {
    public UInt32 InfoLength;
    public IntPtr hData;
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwContainer
  {
    virtual public TwType GetValueType(IntPtr handle) { return TwType.DontCare16; }
    virtual public object GetCurrentValue(IntPtr handle) { return null; }
    virtual public void SetCurrentValue(IntPtr handle, object value) { }
    virtual public int GetNumItems(IntPtr handle) { return 0; }
    virtual public object GetItem(IntPtr handle, int index) { return null; }
    virtual public int GetCurrentItemIndex(IntPtr handle) { return -1; }
    virtual public object GetMinValue(IntPtr handle) { return null; }
    virtual public object GetMaxValue(IntPtr handle) { return null; }
    virtual public object GetStepSize(IntPtr handle) { return null; }

    protected int SizeOf(TwType type)
    {
      int result;

      switch (type)
      {
        case TwType.Bool:
          {
            result = sizeof(bool);
          }
          break;

        case TwType.Fix32:
          {
            result = sizeof(float);
          }
          break;

        case TwType.UInt16:
          {
            result = sizeof(UInt16);
          }
          break;

        default:
        case TwType.DontCare16:
          {
            result = 0;
          }
          break;
      }

      return result;
    }

    protected UInt32 ToRawData(TwType type, object value)
    {
      UInt32 result;

      switch (type)
      {
        case TwType.Bool:
          {
            result = (UInt32)(((bool)value) ? 1 : 0);
          }
          break;

        case TwType.Fix32:
          {
            result = Utils.Fix32FromFloat((float)value);
          }
          break;

        case TwType.UInt16:
          {
            result = (UInt32)(UInt16)value;
          }
          break;

        default:
        case TwType.DontCare16:
          {
            result = 0;
          }
          break;
      }

      return result;
    }

    protected object FromRawData(TwType type, UInt32 rawData)
    {
      // Convert data types that fit within 4 bytes
      // Extract the raw data of the correct size from the 4 byte field
      object result;

      switch (type)
      {
        case TwType.Bool:
          {
            byte data = (byte)(rawData & 0x000000ff);
            result = (bool)(data != 0);
          }
          break;

        case TwType.Fix32:
          {
            result = Utils.FloatFromFix32(rawData);
          }
          break;

        default:
        case TwType.UInt16:
          {
            result = (UInt16)(rawData & 0x0000ffff);
          }
          break;
      }

      return result;
    }
  }


  // Capability container for one value.
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwOneValue : TwContainer
  {
    [MarshalAs(UnmanagedType.U2)]
    public TwType ItemType;
    public UInt32 Item;

    public TwOneValue()
    {
      ItemType = TwType.DontCare16;
      Item = 0;
    }

    public TwOneValue(TwType valueType, object value)
    {
      ItemType = valueType;
      Item = ToRawData(valueType, value);
    }

    override public TwType GetValueType(IntPtr handle)
    {
      return ItemType;
    }

    override public object GetCurrentValue(IntPtr handle)
    {
      return FromRawData(ItemType, Item);
    }
  }


  // Capability container for enumerated value.
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwEnumeration : TwContainer
  {
    [MarshalAs(UnmanagedType.U2)]
    public TwType ItemType;
    public UInt32 NumItems;
    public UInt32 CurrentIndex;
    public UInt32 DefaultIndex;

    override public TwType GetValueType(IntPtr handle)
    {
      return ItemType;
    }

    override public object GetCurrentValue(IntPtr handle)
    {
      return GetItem(handle, GetCurrentItemIndex(handle));
    }

    override public int GetNumItems(IntPtr handle)
    {
      return (int)NumItems;
    }

    override public object GetItem(IntPtr handle, int index)
    {
      int offset = index * SizeOf(ItemType);
      IntPtr pItem = (IntPtr)(handle.ToInt64() + Marshal.SizeOf(this) + offset);
      uint bytes = (uint)Marshal.PtrToStructure(pItem, typeof(uint));
      return FromRawData(ItemType, bytes);
    }

    override public int GetCurrentItemIndex(IntPtr handle)
    {
      return (int)CurrentIndex;
    }
  }


  // Capability container for enumerated value.
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwArray : TwContainer
  {
    [MarshalAs(UnmanagedType.U2)]
    public TwType ItemType;
    public UInt32 NumItems;

    override public TwType GetValueType(IntPtr handle)
    {
      return ItemType;
    }

    override public int GetNumItems(IntPtr handle)
    {
      return (int)NumItems;
    }

    override public object GetItem(IntPtr handle, int index)
    {
      int offset = index * SizeOf(ItemType);
      IntPtr pItem = (IntPtr)(handle.ToInt64() + Marshal.SizeOf(this) + offset);
      uint bytes = (uint)Marshal.PtrToStructure(pItem, typeof(uint));
      return FromRawData(ItemType, bytes);
    }
  }


  // Capability container for range of value.
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwRange : TwContainer
  {
    [MarshalAs(UnmanagedType.U2)]
    public TwType ItemType;
    public UInt32 MinValue;
    public UInt32 MaxValue;
    public UInt32 StepSize;
    public UInt32 DefaultValue;
    public UInt32 CurrentValue;

    override public TwType GetValueType(IntPtr handle)
    {
      return ItemType;
    }

    override public object GetCurrentValue(IntPtr handle)
    {
      return FromRawData(ItemType, CurrentValue);
    }

    override public void SetCurrentValue(IntPtr handle, object value)
    {
      CurrentValue = ToRawData(ItemType, value);

      try
      {
        IntPtr pv = LibKernel32.GlobalLock(handle);
        Marshal.StructureToPtr(this, pv, true);
      }
      finally
      {
        LibKernel32.GlobalUnlock(handle);
      }
    }

    override public object GetMinValue(IntPtr handle)
    {
      return FromRawData(ItemType, MinValue);
    }

    override public object GetMaxValue(IntPtr handle)
    {
      return FromRawData(ItemType, MaxValue);
    }

    override public object GetStepSize(IntPtr handle)
    {
      return FromRawData(ItemType, StepSize);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class TwCapability
  {
    public TwCap CapabilityType;
    public TwOn ContainerType;
    public IntPtr Handle;

    public TwCapability(TwCap cap)
      : this(cap, TwType.DontCare16, null)
    {
    }

    public TwCapability(TwCap cap, TwType valueType, object value)
    {
      CapabilityType = cap;
      ContainerType = TwOn.One;

      value = CastFromCapabilityType(value);

      TwOneValue container = new TwOneValue(valueType, value);

      Handle = LibKernel32.GlobalAlloc(0x42, Marshal.SizeOf(container));

      try
      {
        IntPtr pv = LibKernel32.GlobalLock(Handle);
        Marshal.StructureToPtr(container, pv, true);
      }
      finally
      {
        LibKernel32.GlobalUnlock(Handle);
      }
    }

    ~TwCapability()
    {
      if (Handle != IntPtr.Zero)
      {
        LibKernel32.GlobalFree(Handle);
      }
    }

    private TwContainer GetContainer()
    {
      TwContainer result;
      IntPtr pv = LibKernel32.GlobalLock(Handle);

      try
      {
        if (ContainerType == TwOn.One)
        {
          result = (TwContainer)Marshal.PtrToStructure(pv, typeof(TwOneValue));
        }
        else if (ContainerType == TwOn.Enum)
        {
          result = (TwContainer)Marshal.PtrToStructure(pv, typeof(TwEnumeration));
        }
        else if (ContainerType == TwOn.Array)
        {
          result = (TwContainer)Marshal.PtrToStructure(pv, typeof(TwArray));
        }
        else if (ContainerType == TwOn.Range)
        {
          result = (TwContainer)Marshal.PtrToStructure(pv, typeof(TwRange));
        }
        else
        {
          result = new TwContainer();
        }
      }
      finally
      {
        LibKernel32.GlobalUnlock(Handle);
      }

      return result;
    }

    private object CastToCapabilityType(object item)
    {
      if (item != null)
      {
        switch (CapabilityType)
        {
          case TwCap.SupportedSizes:
            {
              item = (TwCapPageType)(UInt16)item;
            }
            break;

          case TwCap.IPixelType:
            {
              item = (TwCapPixelType)(UInt16)item;
            }
            break;

          case TwCap.SupportedCapabilities:
            {
              item = (TwCap)(UInt16)item;
            }
            break;

          case TwCap.BitDepthReduction:
            {
              item = (TwCapBitDepthReductionType)(UInt16)item;
            }
            break;

          case TwCap.BitDepth:
            {
              item = (UInt16)item;
            }
            break;
        }
      }

      return item;
    }

    private object CastFromCapabilityType(object item)
    {
      if (item != null)
      {
        switch (CapabilityType)
        {
          case TwCap.SupportedSizes:
            {
              item = (UInt16)(TwCapPageType)item;
            }
            break;

          case TwCap.IPixelType:
            {
              item = (UInt16)(TwCapPixelType)item;
            }
            break;

          case TwCap.SupportedCapabilities:
            {
              item = (UInt16)(TwCap)item;
            }
            break;

          case TwCap.BitDepthReduction:
            {
              item = (UInt16)(TwCapBitDepthReductionType)item;
            }
            break;

          case TwCap.BitDepth:
            {
              item = (UInt16)item;
            }
            break;
        }
      }

      return item;
    }

    public TwType GetValueType()
    {
      return GetContainer().GetValueType(Handle);
    }

    public object GetMinValue()
    {
      return GetContainer().GetMinValue(Handle);
    }

    public object GetMaxValue()
    {
      return GetContainer().GetMaxValue(Handle);
    }

    public object GetStepSize()
    {
      return GetContainer().GetStepSize(Handle);
    }

    public object GetCurrentValue()
    {
      return CastToCapabilityType(GetContainer().GetCurrentValue(Handle));
    }

    public void SetCurrentValue(object value)
    {
      GetContainer().SetCurrentValue(Handle, CastFromCapabilityType(value));
    }

    public int GetNumItems()
    {
      return GetContainer().GetNumItems(Handle);
    }

    public object GetItem(int index)
    {
      return CastToCapabilityType(GetContainer().GetItem(Handle, index));
    }

    public int GetCurrentItemIndex()
    {
      return GetContainer().GetCurrentItemIndex(Handle);
    }
  }
}
