﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            IpCounter.GetAllIpAddresses(firstIP, secondIP);
        }
    }
}
