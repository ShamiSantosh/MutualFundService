using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data;

namespace MutualFundServiceHost
{
    class Program
    {
        static void Main()
        {
           
            using(ServiceHost host = new ServiceHost(typeof(MutualFundService.MutualFundService)))
            {
                host.Open();
                Console.WriteLine("Host started at: " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
    }
}
