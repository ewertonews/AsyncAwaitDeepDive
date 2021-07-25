using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace AsyncAwaitDeepDive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Controls weather or not we cancel any method execution.
        /// Signals to a cancelationToken that it should be cancelled
        /// </summary>
        private readonly CancellationTokenSource cts = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NormalExecute_Click(object sender, RoutedEventArgs e)
        {
            resultsWindow.Text = "Downloading...";
            Stopwatch? watch = Stopwatch.StartNew();

            //var results = DemoMethods.RunDownloadSync();
            var results = DemoMethods.RunDownloadParallelSync();
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

            try
            {
                var results = await DemoMethods.RunDownloadAsync(progress, cts.Token);
                //PrintResults(results);
            }
            catch (OperationCanceledException ex)
            {
                resultsWindow.Text += $"{ex.Message} { Environment.NewLine }";
            }

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

        private void CancelOperation_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
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
