using DownloaderSeriesWithSeasonvar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DownloaderSeriesWithSeasonvar.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
#if DEBUG
            TbUri.Text = "http://seasonvar.ru/serial-17482-Doktor_Kto-11-season.html";
#endif
        }


        private async void GetSeason_Click(object sender, RoutedEventArgs e)
        {
            tbUriList.Text = "";
            var downloader = new Downloader(new Uri(TbUri.Text));
            await DownloaderWorkAsync(downloader);

        }

        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            //Clipboard.SetText(tbUriList.Text);
            var downloaderPlist = new DownloaderSeasonInfo(new Uri(TbUri.Text), true, false);

            Season season = downloaderPlist.DownloadSeasonInfo();

            var printString = new StringBuilder();
            foreach (var series in season.SeriesList)
                printString.AppendLine(series.FileUri.ToString());
            tbUriList.Text = printString.ToString();

        }

        private async void OpenPlistInputWindow_Click(object sender, RoutedEventArgs e)
        {
            var plistWindow = new PlistInputWindow();

            plistWindow.Owner = this;
            if(plistWindow.ShowDialog() == true)
            {
                Downloader downloader = null;
                if(plistWindow.PatternStr == "")
                    downloader = new Downloader(plistWindow.PlistStr);
                else
                    downloader = new Downloader(
                        plistWindow.PlistStr, plistWindow.PatternStr);

                await DownloaderWorkAsync(downloader);
            }
            
        }

        private async Task DownloaderWorkAsync(Downloader downloader)
        {
            downloader.NewStageOfWork += (s, m) =>
            {
                Dispatcher.Invoke(() =>
                {
                    lbCurrentStageOfWork.Content = m;
                });
            };

            Season season = await downloader.FillOutSeasonInformationAsync();

            var printString = new StringBuilder();
            foreach (var series in season.SeriesList)
                printString.AppendLine(series.FileUri.ToString());
            tbUriList.Text = printString.ToString();
        }
    }
}
