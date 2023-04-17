using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPoShGUI
{

    public class Installer
    {
        // Initialize HTTPClient Class. Only needs to be done once!
        static readonly HttpClient client = new HttpClient();

        // List of strings kinda like an array but fancier
        private List<string> PrepData()
        {
            List<string> output = new List<string>();

            output.Add( "https://adwcleaner.malwarebytes.com/adwcleaner?channel=release" );

            return output;
        }

        // Download Async
        private async Task<List<WebsiteDataModel>> RunDownloadParallelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            foreach ( string site in websites )
            {
                tasks.Add(DownloadWebsiteAsync(site));
            }

            var results = await Task.WhenAll(tasks);

            return new List<WebsiteDataModel>(results);
        }

        // Download Website data
        private async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();

            output.WebsiteUrl = websiteURL;
            var stream = await client.GetStreamAsync(websiteURL);
            output.WebsiteData = "To Be Added";

            return output;
        }

        // Report progress data to text field
        private string ReportWebsiteInfo(WebsiteDataModel data)
        {
            return data.WebsiteUrl;
        }
    }
}
