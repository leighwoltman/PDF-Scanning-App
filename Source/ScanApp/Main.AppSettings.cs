using System.Collections.Generic;
using System.IO;
using HouseUtils;
using Documents;


namespace ScanApp
{
  public class AppSettings
  {
    #region Settings

    public string CurrentScanner { get; set; } = string.Empty;

    public Dictionary<string, ScanSettings> ScanProfiles { get; set; } = new Dictionary<string, ScanSettings>();

    public Dictionary<string, Size2D> PageSizes { get; set; } = new Dictionary<string, Size2D>();

    public string DefaultPageType { get; set; } = string.Empty;

    public PdfSettings PdfSettings { get; set; } = new PdfSettings();

    public ExportSettings ExportSettings { get; set; } = new ExportSettings();

    public string LastDirectoryForSaving { get; set; } = string.Empty;

    public string LastDirectoryForLoading { get; set; } = string.Empty;

    public bool ShowPrintButton { get; set; } = false;

    #endregion


    #region Helper Methods

    public List<string> GetScanProfileNames()
    {
      return new List<string>(ScanProfiles.Keys);
    }


    public void RemoveProfile(string name)
    {
      try
      {
        ScanProfiles.Remove(name);
      }
      catch
      { }
    }


    public void RenameProfile(string oldName, string newName)
    {
      try
      {
        if(newName != oldName)
        {
          ScanSettings value = ScanProfiles[oldName];
          ScanProfiles.Remove(oldName);
          ScanProfiles[newName] = value;
        }
      }
      catch
      { }
    }


    public void UpdateProfile(string name, ScanSettings value)
    {
      try
      {
        ScanProfiles[name] = value;
      }
      catch
      { }
    }


    public ScanSettings GetScanSettings(string profile)
    {
      ScanSettings result;

      try
      {
        result = ScanProfiles[profile].Copy();
      }
      catch
      {
        result = null;
      }

      return result;
    }


    public Size2D GetDefaultPageSize()
    {
      return GetPageSize(DefaultPageType);
    }


    public Size2D GetCustomPageSize()
    {
      return GetPageSize("Custom");
    }


    public void SetCustomPageSize(Size2D value)
    {
      SetPageSize("Custom", value);
    }


    public Size2D GetPageSize(string pageType)
    {
      Size2D result;

      try
      {
        result = PageSizes[pageType];
      }
      catch
      {
        result = new Size2D(8.5, 11); // Letter size
      }

      return result;
    }


    public void SetPageSize(string pageType, Size2D value)
    {
      PageSizes[pageType] = value;
    }
    
    #endregion


    #region Serialization

    private string Serialize()
    {
      return HouseUtils.Json.Serialize2(this);
    }

    private static AppSettings Deserialize(string json)
    {
      return HouseUtils.Json.Deserialize2<AppSettings>(json);
    }

    public void Save()
    {
      File.WriteAllText(GetFileName(), Serialize());
    }

    public static AppSettings Load()
    {
      return Deserialize(File.ReadAllText(GetFileName()));
    }

    public static AppSettings LoadOrNew()
    {
      AppSettings result;

      try
      {
        result = Load();
      }
      catch
      {
        result = new AppSettings();
      }

      if (string.IsNullOrEmpty(result.DefaultPageType))
      {
        result.DefaultPageType = "Letter";
      }

      if (result.PageSizes.ContainsKey("Letter") == false)
      {
        result.PageSizes.Add("Letter", new Size2D(8.5, 11));
      }

      if (result.PageSizes.ContainsKey("Legal") == false)
      {
        result.PageSizes.Add("Legal", new Size2D(8.5, 14));
      }

      if (result.PageSizes.ContainsKey("A4") == false)
      {
        result.PageSizes.Add("A4", new Size2D(8.27, 11.69));
      }

      if (result.PageSizes.ContainsKey("Custom") == false)
      {
        result.PageSizes.Add("Custom", result.GetDefaultPageSize());
      }

      return result;
    }

    private static string GetFileName()
    {
      string result = AppInfo.GetFullPathToUserApplicationData("Settings.json");

      string folder = Path.GetDirectoryName(result);

      if (Directory.Exists(folder) == false)
      {
        Directory.CreateDirectory(folder);
      }

      return result;
    }

    #endregion
  }
}