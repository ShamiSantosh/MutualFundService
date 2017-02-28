using MutualFund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            GetMFData data = new GetMFData();
            GetMutualFudNamesFromWeb mfNames = new GetMutualFudNamesFromWeb();
            //mfNames.fillMutualFundNamesTable();
            data.UpdateMutualFundDetails();

        }
    }
}
