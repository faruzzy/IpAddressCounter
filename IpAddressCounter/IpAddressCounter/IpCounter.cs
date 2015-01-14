using System;
using System.Collections.Generic;
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

            while (lowerBlocks[smallestIpGroupIndex] < upperBlocks[smallestIpGroupIndex])
            {
                CalculateIP();
            }
        }

        private static void CalculateIP()
        {
            int length = lowerBlocks.Length;
            int index = smallestIpGroupIndex;
            IpAddresses = new List<string>();

            while (lowerBlocks[index] < upperBlocks[index])
            {
                for (int i = length; i >= index; i--)
                {
                    string ip = BuildIp(i, lowerBlocks);

                    // starting from the back
                    for (int j = lowerBlocks[length - 1]; j <= max; ++j)
                    {
                        var parts = Regex.Split(ip, @"\.\.");
                        if (parts.Length != 2)
                        {
                            IpAddresses.Add(String.Concat(ip, j));
                            if (j == max)
                                lowerBlocks = Array.ConvertAll(
                                    IpAddresses.Last().Split(' '), byte.Parse); 
                            if (smallestIpGroupIndex == (length - 1)) return;
                        }
                        else
                            IpAddresses.Add(String.Concat(parts[0], ".", j, ".", parts[1]));
                    }
                }
            }
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
            if (index == array.Length)
                str = String.Format("{0}.{1}.{2}.", array[0], array[1], array[2]);
            else if (index == array.Length - 1)
                str = String.Format("{0}.{1}..{2}", array[0], array[1], array[3]);
            else if (index == array.Length - 2)
                str = String.Format("{0}..{1}.{2}", array[0], array[2], array[3]);
            else if (index == array.Length - 3)
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

            for (int i = 0; i < lowerBlocks.Length; i++)
            {
                byte currentLowerValue = lowerBlocks[i];
                byte currentUpperValue = upperBlocks[i];
                if (currentLowerValue < currentUpperValue)
                {
                    smallestIpGroupIndex = i;
                    break;
                }
                else if (currentLowerValue == currentUpperValue) continue;
            }

            return true;
        }
    }
}
