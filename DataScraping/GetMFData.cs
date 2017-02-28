using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Collections.Specialized;

namespace MutualFund
{
    public class GetMFData
    {
        DataTable table = new DataTable("mutualfund");
        string connectionstring = ConfigurationManager.ConnectionStrings["MFConnectionString"].ConnectionString;

        /// <summary>
        /// Updates the MutualFundDetails table in database
        /// </summary>
        public void UpdateMutualFundDetails()
        {
            table.Columns.Add("Mutual Fund Name", typeof(String));
            table.Columns.Add("P/E", typeof(String));
            table.Columns.Add("P/B", typeof(String));
            table.Columns.Add("Mean", typeof(String));
            table.Columns.Add("StdDeviation", typeof(String));
            table.Columns.Add("Sharpe", typeof(String));
            table.Columns.Add("Sortino", typeof(String));
            table.Columns.Add("Beta", typeof(String));
            table.Columns.Add("Alpha", typeof(String));
            table.Columns.Add("R-Squared", typeof(String));
            table.Columns.Add("Expense", typeof(String));
            table.Columns.Add("Fund Manager Value", typeof(String));
            table.Columns.Add("Fund Style", typeof(String));
            table.Columns.Add("Expiry Date", typeof(String));

            scrapeHTML(getMutualFundName());

            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            string insertQuersy = "";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                insertQuersy = "insert into [MutualFund].[dbo].[MutualFundDetails] values('"+ DateTime.Today +"','"
                      + table.Rows[i]["Mutual Fund Name"].ToString().Trim() + "','"
                      + table.Rows[i]["P/E"].ToString().Trim() + "','"
                      + table.Rows[i]["P/B"].ToString().Trim() + "','"
                      + table.Rows[i]["Mean"].ToString().Trim() + "','"
                      + table.Rows[i]["StdDeviation"].ToString().Trim() + "','"
                      + table.Rows[i]["Sharpe"].ToString().Trim() + "','"
                      + table.Rows[i]["Sortino"].ToString().Trim() + "','"
                      + table.Rows[i]["Beta"].ToString().Trim() + "','"
                      + table.Rows[i]["Alpha"].ToString().Trim() + "','"
                      + table.Rows[i]["R-Squared"].ToString().Trim() + "','"
                      + table.Rows[i]["Expense"].ToString().Trim() + "','"
                      + table.Rows[i]["Fund Manager Value"].ToString().Trim() + "','"
                      + table.Rows[i]["Fund Style"].ToString().Trim()+"','"
                      + table.Rows[i]["Expiry Date"]+"')";
                      

                SqlCommand command = new SqlCommand(insertQuersy, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        /// <summary>
        /// Helper method to insert data into MutualFundDetails table
        /// </summary>
        /// <param name="MFName"></param>
        /// <param name="Expense"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="fundStyle"></param>
        /// <param name="pb"></param>
        /// <param name="pe"></param>
        /// <param name="fundmanagerValue"></param>
        /// <param name="rSquared"></param>
        /// <returns></returns>
        public DataTable fillTable(string MFName, string Expense, string p2, string p3, string fundStyle, string pb, string pe, DateTime fundmanagerValue, string rSquared)
        {
            string mean = p2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ",string.Empty);
            string stddev = p2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace(" ", string.Empty);
            string sharpe = p2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[3].Replace(" ", string.Empty);
            string sortino = p2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[4].Replace(" ", string.Empty);
            string beta = p2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[5].Replace(" ", string.Empty);
            string alpha = p2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[6].Replace(" ", string.Empty);

            string categorymean = p3.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", string.Empty);
            string categorystddev = p3.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace(" ", string.Empty);
            string categorysharpe = p3.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[3].Replace(" ", string.Empty);
            string categorysortino = p3.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[4].Replace(" ", string.Empty);
            string categorybeta = p3.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[5].Replace(" ", string.Empty);
            string categoryalpha = p3.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[6].Replace(" ", string.Empty);

            if (fundStyle.Equals("GL", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Growth Large";
            else if (fundStyle.Equals("GM", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Growth Medium";
            else if (fundStyle.Equals("GS", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Growth Small";
            else if (fundStyle.Equals("BL", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Blend Large";
            else if (fundStyle.Equals("BM", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Blend Medium";
            else if (fundStyle.Equals("BS", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Blend Small";
            else if (fundStyle.Equals("VL", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Value Large";
            else if (fundStyle.Equals("VM", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Value Medium";
            else if (fundStyle.Equals("VS", StringComparison.InvariantCultureIgnoreCase))
                fundStyle = "Value Small";


            DataRow row1 = table.NewRow();
            row1["Mutual Fund Name"] = (MFName);
            row1["P/E"] = (pe);
            row1["P/B"] = (pb);
            row1["Mean"] = (mean);
            row1["StdDeviation"] = (stddev);
            row1["Sharpe"] = (sharpe);
            row1["Sortino"] = (sortino);
            row1["Beta"] = (beta);
            row1["Alpha"] = (alpha);
            row1["R-Squared"] = (rSquared);
            row1["Expense"] = (Expense);
            row1["Fund Manager Value"] = (fundmanagerValue.ToString("MMMM yyyy"));
            row1["Fund Style"] = (fundStyle);
            row1["Expiry Date"] = (DateTime.Today.AddDays(5));
            table.Rows.Add(row1);

            DataRow row2 = table.NewRow();
            row2["Mutual Fund Name"] = ("Category");
            row2["P/E"] = (null);
            row2["P/B"] = (null);
            row2["Mean"] = (categorymean);
            row2["StdDeviation"] = (categorystddev);
            row2["Sharpe"] = (categorysharpe);
            row2["Sortino"] = (categorysortino);
            row2["Beta"] = (categorybeta);
            row2["Alpha"] = (categoryalpha);
            row2["R-Squared"] = (null);
            row2["Expense"] = (null);
            row2["Fund Manager Value"] = (null);
            row2["Fund Style"] = (null);
            row2["Expiry Date"] = (DateTime.Today.AddDays(5));
            table.Rows.Add(row2);

            return table;
        }

        /// <summary>
        /// Returns possible Mutual Fund names
        /// </summary>
        /// <returns></returns>
        public DataTable getMutualFundName()
        {
            string queryString = "SELECT  [Mutual Fund Name], [Search URL] FROM [MutualFund].[dbo].[MutualFundNames]";
            DataTable companyNames = new DataTable();

            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
            adapter.Fill(companyNames);

            connection.Close();
            return companyNames;
        }

        /// <summary>
        /// Helper method to scrape mutual fund data from web and return result as a data table
        /// </summary>
        /// <param name="dt"></param>
        public void scrapeHTML(DataTable dt)
        {
            HtmlWeb web = new HtmlWeb();

            //Scrape HTML for each mutual fund
            foreach (DataRow row in dt.Rows)
            {
                string MFName = row.ItemArray[0].ToString();
                string searchURL = row.ItemArray[1].ToString();

                web.PreRequest = delegate (HttpWebRequest webRequest)
                {
                    webRequest.KeepAlive = false;
                    webRequest.ServicePoint.Expect100Continue = true;
                    webRequest.Timeout = Timeout.Infinite;
                    //webRequest.ServicePoint.MaxIdleTime = 2000;
                    return true;
                };
                
                string urlParameter = searchURL.Substring(searchURL.LastIndexOf("=") + 1);

                //Get expense
                string expenseURL = ConfigurationManager.AppSettings["ExpenseURL"] + urlParameter;
                Thread.Sleep(500);
                var document1 = web.Load(expenseURL);
                var expenseNode = document1.DocumentNode.SelectNodes("//*[@id=\"fundHead\"]//div//div/table//tr/td");
                var expense = expenseNode.Select(node => node.InnerText).ToList();
                List<string> expenseValue = new List<string>();
                expenseValue = expense;
                string Expense = expenseValue[5].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Replace(" ", string.Empty);

                //Get Mean, Std Deviation, Sharpe, Sortino, Beta, Lapha in a list
                string performanceURL = ConfigurationManager.AppSettings["performanceURL"] + urlParameter;
                var document2 = web.Load(performanceURL);
                var performanceNode = document2.DocumentNode.SelectNodes("/html//body//div//div//div//div//div//table//tr");
                var performance = performanceNode.Select(node => node.InnerText).ToList();
                List<string> performanceValue = new List<string>();
                performanceValue = performance;
                string performance1 = performanceValue[11];
                string performance2 = performanceValue[12];
                string performance3 = performanceValue[14];

                //Get Portfolio
                string portfolioURL = ConfigurationManager.AppSettings["portfolioURL"] + urlParameter;
                var document3 = web.Load(portfolioURL);
                var portfolioNode = document3.DocumentNode.SelectNodes("/html/body//div//div//div//div//div//table//tr//td//table//tr");
                var portfolio = portfolioNode.Select(node => node.InnerText).ToList();
                var styleNode = document3.DocumentNode.SelectSingleNode("/html/body//div//div//div//div//div//table//tr//td//img");
                var style = styleNode.Attributes["src"].Value;
                List<string> portfolioValue = new List<string>();
                portfolioValue = portfolio;
                string pb = portfolioValue[7].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", string.Empty);
                string pe = portfolioValue[8].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", string.Empty);
                string fundStyle = style.Substring(style.Length - 6, 2);

                //Get Fund Manager Value
                string fundmanagerURL = ConfigurationManager.AppSettings["fundmanagerURL"] + urlParameter;
                var document4 = web.Load(fundmanagerURL);
                var fundManagerNode = document4.DocumentNode.SelectNodes("(/html/body//div//div//div//div//div//table)[2]//tr//td");
                var FundManagerList1 = fundManagerNode.Select(node => node.InnerText).ToList();
                var fundManager = fundManagerNode.Select(node => node.InnerText).ToList();
                List<string> fundmanagerValue = new List<string>();
                foreach (HtmlNode rows in fundManagerNode)
                {
                    var funManagerList = rows.SelectNodes("a/text()");
                    var FundManagerList = funManagerList.Select(node => node.InnerText).ToList();
                    fundmanagerValue.Add(FundManagerList[1]);
                }

                List<DateTime> fundManagerDates = new List<DateTime>();
                foreach(string value in fundmanagerValue)
                {
                    string datevalue = value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    datevalue = datevalue.Substring(datevalue.Length - 8);
                    fundManagerDates.Add(Convert.ToDateTime(datevalue));
                }
                DateTime fundManagerDate = fundManagerDates.Max();

                
                string rSquared = "0"; 
                table = fillTable(MFName, Expense, performance2, performance3, fundStyle, pb, pe, fundManagerDate, rSquared);
            }
        }
    }
}