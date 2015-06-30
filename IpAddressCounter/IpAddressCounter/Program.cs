using System;

namespace IPAddressCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the starting IP address: ");
            string firstIP = Console.ReadLine();

            Console.WriteLine("Please enter the second IP address: ");
            string secondIP = Console.ReadLine();

            /** for testing 
            string firstIP = "100.100.254.200";
            string secondIP = "100.101.255.205";
            */

            IpCounter.GetAllIpAddresses(firstIP, secondIP);
        }
    }
}
