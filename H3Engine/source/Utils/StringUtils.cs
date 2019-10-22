using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Utils
{
    public class StringUtils
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            bool isFirst = true;
            foreach (byte b in ba)
            {
                if (!isFirst)
                {
                    hex.Append(" ");
                }
                else
                {
                    isFirst = false;
                }
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

    }
}
