using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yad2_Automatic_Jump.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            MyScheduler.IntervalInMinutes(2,0,20, () =>
            {

            });
        }
    }
}
