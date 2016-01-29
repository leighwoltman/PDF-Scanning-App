using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PDFScanningApp
{
  public class Settings
  {
    public string CurrentScanner;
    public string LastDirectory;
    public double CustomWidth;
    public double CustomHeight;

    public static Settings GetSettings()
    {
      Settings result = new Settings();

      try
      {
        XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
        // To read the file, create a FileStream.
        using (FileStream myFileStream = new FileStream(GetFileName(), FileMode.Open))
        {
          // Call the Deserialize method and cast to the object type.
          result = (Settings)mySerializer.Deserialize(myFileStream);
          myFileStream.Close();
        }

      }
      catch(Exception)
      {
        // on all failures
        result.LastDirectory = "M:\\";
        result.CurrentScanner = "";
        result.CustomHeight = 5;
        result.CustomWidth = 7;
      }

      return result;
    }

    private Settings()
    {

    }

    private static string GetFileName()
    {
      return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PDFSettings.xml");
    }

    public void SetLastDirectory(string directory)
    {
      this.LastDirectory = directory;
    }

    public void SetCurrentScanner(string scanner)
    {
      this.CurrentScanner = scanner;
    }

    public void SetCustomSize(double width, double height)
    {
      this.CustomWidth = width;
      this.CustomHeight = height;
      SaveSettings();
    }

    public void SaveSettings()
    {
      XmlSerializer x = new XmlSerializer(this.GetType());
      using (TextWriter writer = File.CreateText(GetFileName()))
	    {
        x.Serialize(writer, this);
        writer.Close();
      }
    }
  }
}
