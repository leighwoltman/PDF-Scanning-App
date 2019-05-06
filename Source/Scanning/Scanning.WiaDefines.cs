﻿using System;
using System.Collections.Generic;
using System.Text;


namespace Scanning
{
  partial class WiaDataSourceManager
  {
    enum WiaProperty
    {
      DeviceId = 2,
      Manufacturer = 3,
      Description = 4,
      Type = 5,
      Port = 6,
      Name = 7,
      Server = 8,
      RemoteDevId = 9,
      UIClassId = 10,
      FirmwareVersion = 1026,
      ConnectStatus = 1027,
      DeviceTime = 1028,
      PicturesTaken = 2050,
      PicturesRemaining = 2051,
      ExposureMode = 2052,
      ExposureCompensation = 2053,
      ExposureTime = 2054,
      FNumber = 2055,
      FlashMode = 2056,
      FocusMode = 2057,
      FocusManualDist = 2058,
      ZoomPosition = 2059,
      PanPosition = 2060,
      TiltPostion = 2061,
      TimerMode = 2062,
      TimerValue = 2063,
      PowerMode = 2064,
      BatteryStatus = 2065,
      Dimension = 2070,
      HorizontalBedSize = 3074,
      VerticalBedSize = 3075,
      HorizontalSheetFeedSize = 3076,
      VerticalSheetFeedSize = 3077,
      SheetFeederRegistration = 3078,         // 0 = LEFT_JUSTIFIED, 1 = CENTERED, 2 = RIGHT_JUSTIFIED
      HorizontalBedRegistration = 3079,       // 0 = LEFT_JUSTIFIED, 1 = CENTERED, 2 = RIGHT_JUSTIFIED
      VerticalBedRegistraion = 3080,          // 0 = TOP_JUSTIFIED, 1 = CENTERED, 2 = BOTTOM_JUSTIFIED
      PlatenColor = 3081,
      PadColor = 3082,
      FilterSelect = 3083,
      DitherSelect = 3084,
      DitherPatternData = 3085,
      DocumentHandlingCapabilities = 3086,    // WiaPropertyDocumentHandlingCapabilities
      DocumentHandlingStatus = 3087,          // WiaPropertyDocumentHandlingStatus  
      DocumentHandlingSelect = 3088,          // WiaPropertyDocumentHandlingSelect      
      DocumentHandlingCapacity = 3089,
      HorizontalOpticalResolution = 3090,
      VerticalOpticalResolution = 3091,
      EndorserCharacters = 3092,
      EndorserString = 3093,
      ScanAheadPages = 3094,                  // ALL_PAGES = 0
      MaxScanTime = 3095,
      Pages = 3096,                           // ALL_PAGES = 0
      PageSize = 3097,                        // A4 = 0, LETTER = 1, CUSTOM = 2
      PageWidth = 3098,
      PageHeight = 3099,
      Preview = 3100,                         // FINAL_SCAN = 0, PREVIEW = 1
      TransparencyAdapter = 3101,
      TransparecnyAdapterSelect = 3102,
      ItemName = 4098,
      FullItemName = 4099,
      ItemTimeStamp = 4100,
      ItemFlags = 4101,
      AccessRights = 4102,
      DataType = 4103,
      BitsPerPixel = 4104,
      PreferredFormat = 4105,
      Format = 4106,
      Compression = 4107,                     // 0 = NONE, JPG = 5, PNG = 8
      MediaType = 4108,
      ChannelsPerPixel = 4109,
      BitsPerChannel = 4110,
      Planar = 4111,
      PixelsPerLine = 4112,
      BytesPerLine = 4113,
      NumberOfLines = 4114,
      GammaCurves = 4115,
      ItemSize = 4116,
      ColorProfiles = 4117,
      BufferSize = 4118,
      RegionType = 4119,
      ColorProfileName = 4120,
      ApplicationAppliesColorMapping = 4121,
      StreamCompatibilityId = 4122,
      ThumbData = 5122,
      ThumbWidth = 5123,
      ThumbHeight = 5124,
      AudioAvailable = 5125,
      AudioFormat = 5126,
      AudioData = 5127,
      PicturesPerRow = 5128,
      SequenceNumber = 5129,
      TimeDelay = 5130,
      CurrentIntent = 6146,
      HorizontalResolution = 6147,
      VerticalResolution = 6148,
      HorizontalStartPosition = 6149,
      VerticalStartPosition = 6150,
      HorizontalExtent = 6151,
      VerticalExtent = 6152,
      PhotometricInterpretation = 6153,
      Brightness = 6154,
      Contrast = 6155,
      Orientation = 6156,                     // 0 = PORTRAIT, 1 = LANDSCAPE, 2 = 180°, 3 = 270°
      Rotation = 6157,                        // 0 = PORTRAIT, 1 = LANDSCAPE, 2 = 180°, 3 = 270°
      Mirror = 6158,
      Threshold = 6159,
      Invert = 6160,
      LampWarmUpTime = 6161,
    }


    enum WiaPropertyDocumentHandlingCapabilities
    {
      FEED = 0x01,
      FLAT = 0x02,
      DUP = 0x04,
      DETECT_FLAT = 0x08,
      DETECT_SCAN = 0x10,
      DETECT_FEED = 0x20,
      DETECT_DUP = 0x40,
      DETECT_FEED_AVAIL = 0x80,
      DETECT_DUP_AVAIL = 0x100
    }


    enum WiaPropertyDocumentHandlingStatus
    {
      FEED_READY = 0x01,
      FLAT_READY = 0x02,
      DUP_READY = 0x04,
      FLAT_COVER_UP = 0x08,
      PATH_COVER_UP = 0x10,
      PAPER_JAM = 0x20
    }


    enum WiaPropertyDocumentHandlingSelect
    {
      FEEDER = 0x001,
      FLATBED = 0x002,
      DUPLEX = 0x004,
      FRONT_FIRST = 0x008,
      BACK_FIRST = 0x010,
      FRONT_ONLY = 0x020,
      BACK_ONLY = 0x040,
      NEXT_PAGE = 0x080,
      PREFEED = 0x100,
      AUTO_ADVANCE = 0x200
    }


    enum WiaPropertyCurrentIntent
    { 
      NONE = 0,
      IMAGE_TYPE_COLOR = 1,
      IMAGE_TYPE_GRAYSCALE = 2,
      IMAGE_TYPE_TEXT = 4,
      MINIMIZE_SIZE = 0x00010000,
      MAXIMIZE_QUALITY = 0x00020000,
      BEST_PREVIEW = 0x00040000
    }


    class WiaDefines
    {
      public const string FormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
    }
  }
}