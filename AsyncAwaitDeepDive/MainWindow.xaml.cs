using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace AsyncAwaitDeepDive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NormalExecute_Click(object sender, RoutedEventArgs e)
        {
            resultsWindow.Text = "Downloading...";
            Stopwatch? watch = Stopwatch.StartNew();

            var results = DemoMethods.RunDownloadSync();
            PrintResults(results);

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: {elapsedMs}";
        }


        private async void AsyncExecute_Click(object sender, RoutedEventArgs e)
        {
            resultsWindow.Text = "Downloading...";
            Progress<ProgressReportModel> progress = new();
            progress.ProgressChanged += ReportProgress;

            Stopwatch? watch = Stopwatch.StartNew();            

            var results = await DemoMethods.RunDownloadAsync(progress);
            //PrintResults(results);

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: {elapsedMs}";

        }

        private void ReportProgress(object? sender, ProgressReportModel e)
        {
            downloadProgress.Value = e.PercentageComplete;
            PrintResults(e.SitesDownloaded);
        }

        private async void ParallelExecute_Click(object sender, RoutedEventArgs e)
        {
            resultsWindow.Text = "Downloading...";
            Stopwatch? watch = Stopwatch.StartNew();

            var results = await DemoMethods.RunDownloadParalleAsync();
            PrintResults(results);

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: {elapsedMs}";
        }

        private async void CancelOperation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintResults(List<WebSiteDataModel> results)
        {
            resultsWindow.Text = string.Empty;
            foreach (var item in results)
            {
                resultsWindow.Text += $"{ item.WebsiteUrl } downloaded: { item.WebsiteData.Length } characters long. { Environment.NewLine }";
            }
        }
    }
}
