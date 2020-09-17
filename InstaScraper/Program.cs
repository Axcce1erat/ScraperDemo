using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InstaScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();

            //keeps console open
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            var url = "https://www.ebay.com/b/Polaris-ATV-Side-by-Side-UTV-Body-Frame/43963/bn_116336949";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var feedsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("b-list__items_nofooter srp-results srp-grid")).ToList();

            var feedLists = feedsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("s-item")).ToList();

            //counting items
            Console.WriteLine(feedLists.Count());
            Console.WriteLine();

            foreach (var feedList in feedLists)
            {              
                // out: product title
                Console.WriteLine(feedList.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__title")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    );

                // out: price
                Console.WriteLine(
                    "current price: " +
                    // regex to remove $
                    Regex.Match(
                    feedList.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("s-item__price")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    ,@"\d+.\d+")
                    );

                // out URL
                Console.WriteLine(feedList.Descendants("a").FirstOrDefault().GetAttributeValue("href", "")                
                );

                Console.WriteLine();
            }          
        }
    }
}