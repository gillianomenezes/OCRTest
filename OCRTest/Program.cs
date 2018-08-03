using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OCRTest
{
    class Program
    {        
        static void Main(string[] args)
        {
            string skeys = ConfigurationManager.AppSettings["skeys"];
            Console.Write(skeys);
            Console.ReadLine();
        }
    }
}
