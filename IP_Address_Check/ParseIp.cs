using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Address_Check
{
    class ParseIp
    {
        /*
         * This class takes the raw output from GetIp as a parameter and then parses it using regex
         */
        public static List<string> SplitIp(string rawIp)
        {
            /*
             * Here the regex pattern is set, and then used as a delimiter
             * The result is the raw output from GetIp is split line-by-line into elements of string array
             */
            string pattern = @"\r\n";
            string[] ipArray = System.Text.RegularExpressions.Regex.Split(rawIp, pattern);

            /*
             * I decided to convert the array to a list, as it is easier to manipulate elements
             */
            List<string> ipList = ipArray.ToList();

            /*
             * This uses the Parameter defined below the class to remove all elements without any information
             */
            ipList.RemoveAll(EmptyLine);

            return ipList;

        }

        public static bool EmptyLine(string s)
        {
            return s.Equals("\n") || s.Equals("");
        }
    }
}
