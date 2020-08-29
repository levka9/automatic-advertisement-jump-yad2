using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yad2_Automatic_Jump
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IEnumerable<string> orderIds = new List<string>() 
                {
                    "42149280",
                    "42149235",
                    "42149100"
                };

                using (var yad2 = new Yad2())
                {
                    yad2.Start(orderIds);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //throw;
            }            

            Console.ReadKey();
        }
    }
}
