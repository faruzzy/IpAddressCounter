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
        private static int[] lowerBlocks;
        private static int[] upperBlocks;
        private static int smallestIpGroupIndex = -1;
        private static List<string> IpAddresses;

        public static void GetAllIpAddresses(string firstIP, string secondIP)
        {
            if (!IsValidOperation(firstIP, secondIP)) return;
            Compute();
        }

        /// <summary>
        /// Start descending the Ip Blocks until the lowest
        /// is reached
        /// </summary>
        private static void Compute()
        {
            int index = 3;
            bool bumping = false;
            IpAddresses = new List<string>();

            while (!IsAtMaxValue(lowerBlocks, upperBlocks))
            {
                if (lowerBlocks[index] < max)
                {
                    if (index >= 0 && bumping)
                    {
                        bumping = false;
                        while (lowerBlocks[index] == max)
                        {
                            index--;
                            lowerBlocks[index] = 0;
                        }
                        lowerBlocks[index]++;
                        IpAddresses.Add(string.Join(".", lowerBlocks));
                        index = 3;
                    }
                    else
                    {
                        if (index >= 0)
                        {
                            lowerBlocks[index]++;
                            IpAddresses.Add(string.Join(".", lowerBlocks));
                        }
                    }
                }
                else
                {
                    lowerBlocks[index] = 0;
                    bumping = true;
                    index--;
                }
            }

            using (var sw = new StreamWriter("IP_address_list.txt"))
            {
                foreach (var ipAddress in IpAddresses)
                    sw.WriteLine(ipAddress);
            }
        }

        /// <summary>
        /// Check whether a string matches the format of
        /// an IP address
        /// </summary>
        /// <param name="ip">A string IP address</param>
        /// <returns>True if it matches the format, otherwise returns false</returns>
        private static bool CheckIpFormat(string ip)
        {
            var regex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}");
            if (regex.IsMatch(ip))
            {
                string[] blocks = ip.Split('.');
                foreach (string val in blocks)
                    if (Convert.ToInt16(val) > max) return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verifies that the lower block is the same with the upper block
        /// </summary>
        /// <param name="lower">An array representing the lower bound IP Address</param>
        /// <param name="upper">An array representing the upper bound IP Address</param>
        /// <returns>Return true if the lower block is the same with the uppper block, otherwise returns false</returns>
        private static bool IsAtMaxValue(int[] lower, int[] upper)
        {
            for (int i = 0; i < lower.Length; i++)
                if (lower[i] != upper[i]) return false;
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
            if (!CheckIpFormat(lowerBound) || !CheckIpFormat(upperBound)) return false;

            lowerBlocks = Array.ConvertAll(lowerBound.Split('.'), int.Parse);
            upperBlocks = Array.ConvertAll(upperBound.Split('.'), int.Parse);

            return true;
        }
    }
}
