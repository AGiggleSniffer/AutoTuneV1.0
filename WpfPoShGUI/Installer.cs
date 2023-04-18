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
using System.Xml;

namespace WpfPoShGUI
{

    public class Installer
    {
        // Initialize HTTPClient Class. Only needs to be done once!
        static readonly HttpClient client = new HttpClient();

        // List of strings kinda like an array but fancier
        public static List<string> PrepData()
        {
            List<string> output = new List<string>();

            output.Add( "https://adwcleaner.malwarebytes.com/adwcleaner?channel=release" );

            return output;
        }

        // Download Async
        public static async Task<List<WebsiteDataModel>> RunDownloadParallelAsync(IProgress<ProgressReportModel> progress)
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> output = new List<Task<WebsiteDataModel>>();
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
            {
                Parallel.ForEach<string>(websites, (site) =>
                {
                    WebsiteDataModel results = DownloadWebsiteAsync(site);
                    output.Add(results);

                    report.SitesDownloaded = output;
                    report.PercentageComplete = (output.Count * 100) / websites.Count;
                    progress.Report(report);
                });
            });

            return output;
        }

        // Download Website data
        public static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();

            output.WebsiteUrl = websiteURL;
            var stream = await client.GetStreamAsync(websiteURL);
            var response = await client.GetAsync(websiteURL, HttpCompletionOption.ResponseHeadersRead);
            var fileStream = System.IO.File.Create(@"C:\Users\Public\Downloads\ADWCleaner.exe");

            foreach ( var header in response.Headers)
            {
                output.WebsiteData = $"{header.Key}={header.Value.First()}";
            }
            using (fileStream)
            {
                await stream.CopyToAsync(fileStream);
            }
            await client.Download;

            return output;
        }

        // Report progress data to text field
        private string ReportWebsiteInfo(WebsiteDataModel data)
        {
            return data.WebsiteUrl;
        }
    }
}
