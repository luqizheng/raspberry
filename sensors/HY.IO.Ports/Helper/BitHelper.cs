using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HY.IO.Ports.Helper
{
    public static class BitHelper
    {
        public static string BitToString(byte[] bytes)
        {
            var result
               = from n in bytes select n.ToString("X2");
            return string.Join(" ", result);
        }
    }
}
