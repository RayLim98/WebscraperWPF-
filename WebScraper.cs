using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebScrapperWPF
{
    public class Scraper
    {
        //Contianer of objects
        //Type EntryModel { Title }
        private ObservableCollection<EntryModel> _entries = new ObservableCollection<EntryModel>();

        //Gets and Sets Articles that are obtained from scraping website
        //Used to data bind to a data-grid
        public ObservableCollection<EntryModel> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        //Decodes HTML 
        //Returns String
        private async Task<string> DecodeHtmlNode(HtmlNode node)
        {
            //Decodes 
            var decodedHtml = await Task.Run(() => HttpUtility.HtmlDecode(node.SelectSingleNode(".//h3").InnerText));
            return decodedHtml;
        }
        //Gets and stores inner text data into ObserableCollection
        public async Task getData(string page)
        {
            var web = new HtmlWeb();
            //get htmldocument of page
            var doc = await Task.Run(() => web.Load(page));

            //scrape website" http://books.toscrape.com/, searches for any elements under specified conditions
            //and reutrns a list of all articles under the same catagory 
            var Articles = doc.DocumentNode.SelectNodes("//*[@class = 'product_pod']");

            //for each article found 
            foreach (var article in Articles)
            {
                var title = await Task.Run(() => DecodeHtmlNode(article));
                _entries.Add(new EntryModel { Title = title });
            }
        }
    }
}
