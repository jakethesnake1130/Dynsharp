using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dynsharp
{
    public class ReturnIp
    {
        /*
         * This class is the final stage in the process
         * it takes the list of lines from ParseIp as a parameter and removes the useless lines
         */
        public static List<string> WindowsReturn(IEnumerable<string> ipList)
        {

            List<string> buffer = ipList.ToList();

            /*
             * These regex objects will be used to organize the lines into blocks of information
             */
            Regex rxNewBlock = new(@"^\w");
            Regex rxContBlock = new(@"^\s");

            //All allIpData will eventually be a list of the IP addresses organized
            List<string> allIpData = new List<string>();
            //temp is used as temporary storage when data from ipList is waiting to be permanently stored in allIpData
            List<string> temp = new List<string>();

            /*
             * Each element of ipList passes through the conditionals and is removed from ipList when it is through being handled
             * The loop exits when the countdown is complete and there are no more elements left to process
             */
            while (buffer.Count != 0)
            {
                /*
                 * If a line begins with the regex used in rxNewBlock (a word), then it is a header and begins a new block
                 * Furthermore, if temp has no elements stored that means it is the first header, and so the loop will continue without storing to allIpData (more on that later)
                 */
                if (rxNewBlock.IsMatch(buffer[0]) && temp.Count == 0)
                {
                    temp.Add(buffer[0]);
                    buffer.RemoveAt(0);
                }
                /*
                 * If a header is found and temp *does* have an element stored, then that means it is the second+ header
                 * That means that the previous header and all of its content have been 
                 * The contents of temp are concatonated into a string and then stored in the list, then temp is cleared to prepare for the next block of header/content
                 * The last line of content will also trigger this conditional to ensure that the last block stored in temp will be processed before exiting
                 */
                else if ((rxNewBlock.IsMatch(buffer[0]) && temp.Count > 0) || buffer.Count == 1)
                {
                    string tempString = "";
                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i < temp.Count - 1)
                        {
                            tempString += temp[i] + "\n";
                        }
                        else
                        {
                            tempString += temp[i];
                        }
                    }
                    allIpData.Add(tempString);
                    temp.Clear();
                    temp.Add(buffer[0]);
                    buffer.RemoveAt(0);
                }
                /*
                 * If a line begins with the regex used in rxContBlock (an empty space), then it is a line of content under the header
                 * An element meeting this condition will get stored in temp underneath its respective header, to be passed along later
                 */
                else if (rxContBlock.IsMatch(buffer[0]))
                {
                    temp.Add(buffer[0]);
                    buffer.RemoveAt(0);
                }
            }

            /*
             * This uses the Parameter defined below the class to remove all elements without any IP addresses listed
             * This ensures that any blocks for unused connections or meaningless headers will be omitted from the final output
             */
            allIpData.RemoveAll(LacksIp);

            /*
             * allIpData is returned and represents the final output to be stored in the text file
             */
            return allIpData;
        }

        public static List<string> LinuxReturn(IEnumerable<string> ipList)
        {
            List<string> buffer = ipList.ToList();
            List<string> allIpData = new List<string>();

            buffer.RemoveAll(LacksIp);
            allIpData = buffer;

            return allIpData;

        }

        public static bool LacksIp(string s)
        {
            Regex testIp = new(@"\d+\.\d+\.\d+\.\d+");

            return !testIp.IsMatch(s);
        }
    }
}
