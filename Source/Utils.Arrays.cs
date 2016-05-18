using System;
using System.Collections.Generic;
using System.Text;


namespace Utils
{
  class Arrays
  {
    public static bool ByteArrayCompare(byte[] array1, byte[] array2, int pos1, int pos2, int length)
    {
      bool result = true;

      while(result && (length > 0))
      {
        if(array1[pos1] != array2[pos2])
        {
          result = false;
        }

        pos1++;
        pos2++;
        length--;
      }

      return result;
    }


  }
}
