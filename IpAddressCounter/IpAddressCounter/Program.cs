using System;

namespace IPAddressCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            string firstIP = string.Empty;
            string secondIP = string.Empty;

            if (args.Length == 0 || args.Length == 1)
            {
                if (args.Length == 1)
                {
                    Console.Error.WriteLine("Not enough arguments were passed!");
                }

                Console.Write("Please enter the starting IP address: ");
                firstIP = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Please enter the second IP address: ");
                secondIP = Console.ReadLine();
                Console.WriteLine();
            }

            if (args.Length == 2)
            {
                if (!args[0].Equals(""))
                    firstIP = args[0];

                if (!args[1].Equals(""))
                    secondIP = args[1];
            }

            IpCounter.GetAllIpAddresses(firstIP, secondIP);
        }
    }
}
