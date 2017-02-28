using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace MutualFundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MutualFundService" in both code and config file together.
    public class MutualFundService : IMutualFundService
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["MFConnectionString"].ConnectionString;
        DataTable table = new DataTable("mutualfund");

        public DataTable GetMutualFundData()
        {
            string queryString = "SELECT * FROM [MutualFund].[dbo].[MutualFundDetails]";
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
            adapter.Fill(table);

            return table;
        }
    }
}
