using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Imports
{
  class LibUser32
  {
    [DllImport("user32.dll", ExactSpelling = true)]
    public static extern int GetMessagePos();

    [DllImport("user32.dll", ExactSpelling = true)]
    public static extern int GetMessageTime();
  }
}
