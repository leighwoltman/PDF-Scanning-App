using System;
using System.Collections.Generic;
using System.Text;
using Twain;


namespace Scanning
{
  partial class TwainDataSourceManager
  {
    private class TwainCapability
    {
      private Portal fTwain;
      private TwIdentity fDataSourceId;
      private TwCap fCapType;
      private TwType fValueType;

      public List<object> Items;
      public object MinValue;
      public object MaxValue;
      public object StepSize;

      private object fCurrentValue;

      public object CurrentValue
      {
        get
        {
          return fCurrentValue;
        }
        set
        {
          fCurrentValue = value;
          ApplyValue(fCurrentValue);
        }
      }


      public double ScaledValue // 0 = min, 1 = max
      {
        get
        {
          double val = (double)(float)CurrentValue;
          double max = (double)(float)MaxValue;
          double min = (double)(float)MinValue;
          return (val - min) / (max - min);
        }
        set
        {
          double max = (double)(float)MaxValue;
          double min = (double)(float)MinValue;
          double val = min + (max - min) * value;
          CurrentValue = (float)val;
        }
      }


      public List<string> GetAvailableValuesByName()
      {
        List<string> result = new List<string>();

        foreach(object item in Items)
        {
          result.Add(item.ToString());
        }

        return result;
      }


      public string GetCurrentValueByName()
      {
        string result;

        try
        {
          result = CurrentValue.ToString();
        }
        catch
        {
          result = "";
        }

        return result;
      }


      public void SetCurrentValueByName(string value)
      {
        for(int i = 0; i < Items.Count; i++)
        {
          if(Items[i].ToString() == value)
          {
            CurrentValue = Items[i];
          }
        }
      }


      private bool ApplyValue(object value)
      {
        bool result = false;

        if(fTwain.SetDataSourceCapability(fDataSourceId, fCapType, fValueType, value))
        {
          object final = fTwain.GetDataSourceCapability(fDataSourceId, fCapType);

          if(final.Equals(value))
          {
            result = true;
          }
        }

        return result;
      }


      public TwainCapability(Portal twain, TwIdentity dataSourceId, TwCap capType)
      {
        fTwain = twain;
        fDataSourceId = dataSourceId;
        fCapType = capType;
        fValueType = TwType.DontCare16;

        TwCapability cap = new TwCapability(capType);

        if(fTwain.GetDataSourceAvailableCapabilityValues(dataSourceId, cap))
        {
          fValueType = cap.GetValueType();

          Items = new List<object>();

          int numItems = cap.GetNumItems();

          for(int i = 0; i < numItems; i++)
          {
            Items.Add(cap.GetItem(i));
          }

          MinValue = cap.GetMinValue();
          MaxValue = cap.GetMaxValue();
          StepSize = cap.GetStepSize();
          fCurrentValue = cap.GetCurrentValue();
        }
      }
    }
  }
}