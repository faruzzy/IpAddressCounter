using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IPAddressCounter
{
    class IpCounter
    {
        private const byte max = byte.MaxValue;
        private static int[] lowerBlocks;
        private static int[] upperBlocks;
        private static List<string> IpAddresses;

        public static void GetAllIpAddresses(string firstIP, string secondIP)
        {
            if (!IsValidOperation(firstIP, secondIP))
            {
                Console.Error.WriteLine("Invalid Format - Please make sure the IP addresses you entered have the valid format");
                Console.ReadLine();
                return;
            }
            Compute(IpAddresses: new List<string>());
        }

        /// <summary>
        /// Start descending the Ip Blocks until the lowest
        /// is reachedm and start going up again until the two IpAddresses match
        /// <param name="bumping">A flag to indicate whether we're bumping the previous block</param>
        /// <param name="index">Starting index</param>
        /// <param name="IpAddresses">A list of all IP addresses between the lower and upper Ip (Inclusive)</param>
        /// </summary>
        private static void Compute(List<string> IpAddresses, int index = 3, bool bumping = false)
        {
            if (!IsAtMaxValue(lowerBlocks, upperBlocks))
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
                        IpAddresses.Add(string.Join(".", lowerBlocks));
                        lowerBlocks[index]++;
                        index = 3;
                    }
                    else
                    {
                        if (index >= 0)
                        {
                            IpAddresses.Add(string.Join(".", lowerBlocks));
                            lowerBlocks[index]++;
                        }
                    }
                }
                else
                {
                    if (lowerBlocks[index] == max)
                    {
                        IpAddresses.Add(string.Join(".", lowerBlocks));
                    }
                    lowerBlocks[index] = 0;
                    bumping = true;
                    index--;
                }
                Compute(IpAddresses, index, bumping);
            }
            else
            {
                IpAddresses.Add(string.Join(".", lowerBlocks));
                using (var sw = new StreamWriter("IP_address_list.txt"))
                {
                    foreach (var ipAddress in IpAddresses)
                        sw.WriteLine(ipAddress);
                }
                return;
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
        /// Check for constraints. 
        /// An IP address block (whent it has a preceeding block) can be greater 
        /// than the corresponding (upper) block only if its preceeded by a block that
        /// is not greater that the block associated with it.
        /// </summary>
        /// <param name="lowerBound">A string representing the lower bound IP Address</param>
        /// <param name="upperBound">A string representing the upper bound Ip Address</param>
        /// <returns>False if the constraint rule is not met, otherwise it returns true</returns>
        private static bool CheckConstraint(string lowerBound, string upperBound)
        {
            var lowerArray = lowerBound.Split('.');
            var upperArray = upperBound.Split('.');

            for (int i = lowerArray.Length - 1; i >= 1; i--)
            {
                if (int.Parse(lowerArray[i]) > int.Parse(upperArray[i]))
                    if (int.Parse(lowerArray[i - 1]) >= int.Parse(upperArray[i - 1]))
                        return false;
            }

            return true;
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
            if (!CheckConstraint(lowerBound, upperBound)) return false;

            lowerBlocks = Array.ConvertAll(lowerBound.Split('.'), int.Parse);
            upperBlocks = Array.ConvertAll(upperBound.Split('.'), int.Parse);

            return true;
        }
    }
}
