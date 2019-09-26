using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace UsefulThings.Func
{
    public class Helpers
    {
        public static StringCollection GetStringCollection(string list)
        {
            StringCollection coll = new StringCollection();

            string[] s = list.Split(new Char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in s)
            {

                char[] charsToTrim = { '\n', '\r' };
                string str_trim = str.Trim(charsToTrim);
                if (str_trim != null && str_trim != "" && str_trim != String.Empty)
                {
                    coll.Add(str_trim);
                }
            }
            return coll;
        }
    }
}
