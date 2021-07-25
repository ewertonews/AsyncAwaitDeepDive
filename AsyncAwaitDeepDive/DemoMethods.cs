using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitDeepDive
{
    internal class DemoMethods
    {
        private static readonly HttpClient httpClient = new HttpClient();

        internal static List<WebSiteDataModel> RunDownloadSync()
        {
            List<string>? websites = WebsitesToDownload();
            List<WebSiteDataModel> output = new();

            foreach (string site in websites)
            {
                WebSiteDataModel results = DownloadWebsite(site);
                output.Add(results);
            }
            return output;
        }


        internal static async Task<List<WebSiteDataModel>> RunDownloadAsync(
            IProgress<ProgressReportModel> progress, 
            CancellationToken cancellationToken)
        {
            List<string>? websites = WebsitesToDownload();
            List<WebSiteDataModel> output = new();
            ProgressReportModel report = new();

            foreach (string site in websites)
            {
                WebSiteDataModel results = await DownloadWebsiteAsync(site);
                output.Add(results);
                cancellationToken.ThrowIfCancellationRequested(); // <====
                report.SitesDownloaded = output;
                report.PercentageComplete = output.Count * 100 / websites.Count;
                progress.Report(report);
            }
            return output;
        }


        internal static async Task<List<WebSiteDataModel>> RunDownloadParalleAsync()
        {
            List<string>? websites = WebsitesToDownload();
            List<Task<WebSiteDataModel>> tasks = new();

            foreach (string site in websites)
            {
                tasks.Add(DownloadWebsiteAsync(site));
            }

            WebSiteDataModel[]? results = await Task.WhenAll(tasks);

            return new List<WebSiteDataModel>(results);
        }

        internal static List<WebSiteDataModel> RunDownloadParallelSync()
        {
            List<string>? websites = WebsitesToDownload();
            List<WebSiteDataModel> output = new();

            Parallel.ForEach(websites, website =>
            {
                WebSiteDataModel result = DownloadWebsite(website);
                output.Add(result);
            });

            return output;
        }

        private static WebSiteDataModel DownloadWebsite(string websiteUrl)
        {
            WebSiteDataModel output = new();
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = httpClient.GetStringAsync(websiteUrl).Result;
            return output;
        }

        private static async Task<WebSiteDataModel> DownloadWebsiteAsync(string websiteUrl)
        {
            WebSiteDataModel output = new();
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = await httpClient.GetStringAsync(websiteUrl);
            return output;
        }

        private static List<string> WebsitesToDownload()
        {
            List<string> websites = new()
            {
                "https://www.yahoo.com",
                "https://www.google.com",
                "https://www.microsoft.com",
                "https://www.cnn.com",
                "https://www.amazon.com",
                "https://www.facebook.com",
                "https://www.twitter.com",
                "https://www.codeproject.com",
                "https://www.stackoverflow.com",
                "https://en.wikipedia.org/wiki/.NET_Framework",
            };

            return websites;
        }
    }
}