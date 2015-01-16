using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPAddressCounter
{
    class IpCounter
    {
        private const byte max = 255;
        private static byte[] lowerBlocks;
        private static byte[] upperBlocks;
        private static int smallestIpGroupIndex = -1;
        private static List<string> IpAddresses;

        public static void GetAllIpAddresses(string firstIP, string secondIP)
        {
            if (!IsValidOperation(firstIP, secondIP)) return;
            if (DetermineLowerstIpBlock())
                Compute();
            else return;
        }

        private static void Compute()
        {
            DescendIpBlock();
            //AscendIPBlock();
        }

        /// <summary>
        /// Start ascending Ip Blocks once the former
        /// lowest block of the lower IP block is the same 
        /// with its coreesponding block in the upper Ip Block
        /// </summary>
        private static void AscendIPBlock()
        {
            DetermineLowerstIpBlock();
            while(lowerBlocks[3] != upperBlocks[3])
            {
                int idx = smallestIpGroupIndex;
                //for(int j = 0; j < lowerBlocks.Length; j++)
            }
        }

        /// <summary>
        /// Start descending the Ip Blocks until the lowest
        /// is reached
        /// </summary>
        private static void DescendIpBlock()
        {
            int length = lowerBlocks.Length;
            int index = smallestIpGroupIndex;
            //byte[] currentIpBlock = new byte[4];
            string regex = @"\.\.";
            IpAddresses = new List<string>();

            while (lowerBlocks[index] != upperBlocks[index] && lowerBlocks[3] != upperBlocks[3])
            {
                for (int i = length - 1; i >= index; i--)
                {
                    string ip = BuildIp(i, lowerBlocks);
                    for (int j = lowerBlocks[i]; j <= max; j++)
                    {
                        if(!Regex.IsMatch(ip, regex))
                        {
                            IpAddresses.Add(String.Concat(ip, j));
                            //currentIpBlock = Array.ConvertAll(String.Concat(ip, j).Split('.'), byte.Parse);
                            if (j == max)
                                lowerBlocks = 
                                    Array.ConvertAll(IpAddresses.Last().Split('.'), byte.Parse); 

                            if (smallestIpGroupIndex == (length - 1)) return;
                       }
                       else
                       {
                           var parts = Regex.Split(ip, regex);
                           if (i != index)
                               for (int k = j + 1; k <= max; k++)
                               {
                                   IpAddresses.Add(String.Concat(parts[0], ".", j, ".", parts[1]));
                                   i += 2;
                                   break;
                               }
                           else if (i == index)
                           {
                               //byte[] currentIpBlock = Array.ConvertAll(IpAddresses.Last().Split('.'), byte.Parse);
                               for (int k = j + 1; k <= upperBlocks[i]; k++)
                                   if (upperBlocks[i + 1] == 255)
                                       IpAddresses.Add(String.Concat(parts[0], ".", k, ".", "0"));
                                   else
                                       IpAddresses.Add(String.Concat(parts[0], ".", k, ".", parts[1]));

                               upperBlocks = Array.ConvertAll(IpAddresses.Last().Split('.'), byte.Parse);
                               break;
                           }
                       }
                    }
                }
            }

            using(var sw = new StreamWriter("IP address list.txt"))
                foreach (var ipAddress in IpAddresses)
                    sw.WriteLine(ipAddress);
        }

        /// <summary>
        /// Returns an incomplete IP address string partition 
        /// that the main program will complete
        /// </summary>
        /// <param name="index">The smallest Ip Group index</param>
        /// <param name="array">An array of IP address blocks</param>
        /// <returns>A incomplete IP address string partition</returns>
        private static string BuildIp(int index, byte[] array)
        {
            string str = String.Empty;
            if (index == 3)
                str = String.Format("{0}.{1}.{2}.", array[0], array[1], array[2]);
            else if (index == 2)
                str = String.Format("{0}.{1}..{2}", array[0], array[1], array[3]);
            else if (index == 1)
                str = String.Format("{0}..{1}.{2}", array[0], array[2], array[3]);
            else if (index == 0)
                str = String.Format(".{0}.{1}.{2}", array[1], array[2], array[3]);

            return str;
        }

        /// <summary>
        /// Check whether a string matches the format of
        /// an IP address
        /// </summary>
        /// <param name="ip">A string IP address</param>
        /// <returns>True if it matches the format, otherwise returns false</returns>
        private static bool CheckIpFormat(string ip)
        {
            var regex = new Regex(@"\d+");
            string[] blocks = ip.Split('.');
            if (blocks.Length != 4) return false;

            foreach (string val in blocks)
                if (regex.IsMatch(val))
                    if (Convert.ToInt16(val) > max) return false;
                    else continue;
                else
                    return false;

            return true;
        }

        /// <summary>
        /// Check wether the lowerBound provided is
        /// indeed lower than the upperBound provided
        /// </summary>
        /// <param name="lowerBound">A string representing the lower bound IP Address</param>
        /// <param name="upperBound">A string representing the upper bound Ip Address</param>
        /// <returns>Returns true if the Operation is legit, otherwise returns false</returns>
        private static bool IsValidOperation(string lowerBound, string upperBound)
        {
            if (!CheckIpFormat(lowerBound) || !CheckIpFormat(upperBound))
                return false;

            lowerBlocks = Array.ConvertAll(lowerBound.Split('.'), byte.Parse);
            upperBlocks = Array.ConvertAll(upperBound.Split('.'), byte.Parse);

            return true;
        }

        /// <summary>
        /// Determine the lowest IP Block
        /// </summary>
        private static bool DetermineLowerstIpBlock()
        {
            for (int i = 0; i < lowerBlocks.Length; i++)
            {
                byte currentLowerValue = lowerBlocks[i];
                byte currentUpperValue = upperBlocks[i];
                if (currentLowerValue < currentUpperValue)
                {
                    smallestIpGroupIndex = i;
                    return true;
                }
                else if (currentLowerValue == currentUpperValue) continue;
            }

            return false;
        }
    }
}
