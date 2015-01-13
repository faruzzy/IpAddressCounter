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
        private static string[] lowerBlocks;
        private static string[] upperBlocks;
        private static int? startingPointIndex;
        private static List<string> IpAddresses;
        public static void GetAllIpAddresses(string firstIP, string secondIP)
        {
            // overiding for testing purposes
            firstIP = "99.100.101.200";
            secondIP = "100.101.102.205";

            int[] firstIpArray = Array.ConvertAll(firstIP.Split('.'), int.Parse);
            int[] secondIpArray = Array.ConvertAll(secondIP.Split('.'), int.Parse);

            int length = firstIpArray.Length;
            int smallestIpGroupIndex = -1;

            for (int i = 0; i < length; i++)
            {
                if (firstIpArray[i] < secondIpArray[i])
                {
                    smallestIpGroupIndex = i;
                    break;
                }
            }

            while (firstIpArray[smallestIpGroupIndex] < secondIpArray[smallestIpGroupIndex])
            {
                CalculateIP(smallestIpGroupIndex, firstIpArray, secondIpArray);
            }

        }

        private static void CalculateIP(int index, int[] firstIpArray, int[] secondIpArray)
        {
            int length = firstIpArray.Length;
            IpAddresses = new List<string>();

            for (int i = length - 1; i >= index; i--)
            {
                string ip = BuildIp(i, firstIpArray);
                bool success = false;

                for (int j = firstIpArray[i]; j < max; ++j)
                {
                    IpAddresses.Add(String.Concat(ip, j));

                    if (j == max - 1)
                    {
                        //IpAddresses.Add(String.Concat(ip, "0"));
                        success = true;
                    }

                    if (success)
                    {
                        continue;
                    }
                }
            }
        }

        private static string BuildIp(int index, int[] array)
        {
            int length = index;
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
                sb.Append(array[i] + ".");

            return sb.ToString();
        }

        private static bool IsValidIp(string ip)
        {
            var regex = new Regex(@"\d+");
            string[] blocks = ip.Split('.');
            if (blocks.Length != 4) return false;

            foreach (string val in blocks)
                if (regex.Match(val).Length < 4) return false;
                else if (Convert.ToInt16(val) > max) return false;

            return true;
        }

        private static bool IsValidOperation(string lowerBound, string upperBound)
        {
            if (!IsValidIp(lowerBound) || !IsValidIp(upperBound))
                return false;

            lowerBlocks = lowerBound.Split('.');
            upperBlocks = upperBound.Split('.');

            for (int i = 0; i < lowerBlocks.Length; i++)
            {
                if (Convert.ToByte(lowerBlocks[i]) <= Convert.ToByte(upperBound[i]))
                    continue;
                else
                {
                    startingPointIndex = i;
                    return false;
                }
            }

            return true;
        }
    }
}
