using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace MutualFund
{

    public class GetMutualFudNamesFromWeb
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["MFConnectionString"].ConnectionString;

        /// <summary>
        /// Extracts possible mutual fund names form web and fills the table
        /// </summary>
        public void fillMutualFundNamesTable()
        {
            HtmlWeb web = new HtmlWeb();
            web.PreRequest = delegate (HttpWebRequest webRequest)
            {
                webRequest.KeepAlive = false;
                return true;
            };

            var document = web.Load("https://www.valueresearchonline.com/funds/fundSelector/default.asp?cat=equityAll&exc=susp&exc=close&exc=3Star&exc=2Star&exc=1Star&exc=notRated&x=16&y=13");

            var directPlanNodes = (HtmlNodeCollection)null;
            for (int i = 1; i <= 5; i++)
            {
                directPlanNodes = document.DocumentNode.SelectNodes("//*[@id=\"fundCatData\"]//table//tr//td[2]//a[@class = 'fundName']//text()[" + i + "]");

                foreach (var directPlanNode in directPlanNodes)
                {
                    string directPlanNodeString = directPlanNode.InnerText;
                    if (directPlanNodeString.Contains("Direct Plan") || directPlanNodeString.Contains("Invest Online"))
                    {
                        var parent = directPlanNode.ParentNode;
                        parent.Remove();
                    }
                }
            }

            var mutualFundListNodes = document.DocumentNode.SelectNodes("//*[@id=\"fundCatData\"]//table//tr//td[2]//a[@class = 'fundName']");
            var mutualFundList = mutualFundListNodes.Select(node => node.InnerText).ToList();
            List<string> mutualFundURLList = new List<string>();
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//*[@id=\"fundCatData\"]/table//tr//td[2]//a[@class = 'fundName'][@href]"))
            {
                mutualFundURLList.Add(link.Attributes["href"].Value.ToString());
            }

            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            string insertQuery = "";
            for (int i = 0; i < mutualFundList.Count; i++)
            {
                insertQuery = "insert into [MutualFund].[dbo].[MutualFundNames] values('" + mutualFundList[i].Replace("'", string.Empty) + "','" + mutualFundURLList[i] + "','" + DateTime.Today + "')";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}