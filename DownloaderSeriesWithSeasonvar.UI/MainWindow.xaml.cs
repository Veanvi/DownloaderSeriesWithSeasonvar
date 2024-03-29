﻿using DownloaderSeriesWithSeasonvar.Core;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace DownloaderSeriesWithSeasonvar.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private IWebRequester webRequester;

        public MainWindow()
        {
            // TODO: Выяснить почему при сборке созона в объект не добавляется PlaylistJson

            InitializeComponent();
#if DEBUG
            tbUri.Text = "http://seasonvar.ru/serial-5583-Inspektor_Klot.html";
#endif
        }

        ~MainWindow()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (webRequester != null)
            {
                webRequester.Dispose();
                webRequester = null;
            }
        }

        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(tbUriList.Text);
            lbCurrentStageOfWork.Content = "Ссылки скопированы в буфер обмена";
            tbUri.Text = "";
            tbUriList.Text = "";
        }

        private ModelObjectsBuilder GetModelObjectsBuilder()
        {
            var isTorProxy = (bool)cbTorProxy.IsChecked;
            var isHeadless = (bool)cbHeadlessBrowser.IsChecked;

            if (webRequester == null)
                webRequester = new SeleniumWebRequester(isTorProxy, isHeadless);

            var objectBuilder = new ModelObjectsBuilder(
                new SeasonInfoDownloader(webRequester),
                new TvSeriesInfoDownloader(webRequester));
            return objectBuilder;
        }

        private async void GetSeason_Click(object sender, RoutedEventArgs e)
        {
            tbUriList.Text = "";
            if (string.IsNullOrEmpty(tbUri.Text))
            {
                lbCurrentStageOfWork.Content = "Введите адрес сереала";
                return;
            }
            lbCurrentStageOfWork.Content = "Запущен браузер";

            Season season;

            try
            {
                ModelObjectsBuilder objectBuilder = GetModelObjectsBuilder();
                season = await objectBuilder.BuildSeasonAsync(new Uri(tbUri.Text));
            }
            catch (Exception ex)
            {
                lbCurrentStageOfWork.Content = ex.Message;
                return;
            }

            lbCurrentStageOfWork.Content = "Все ссылки получены";
            PrintSeriesUri(season);
            lbCurrentStageOfWork.Content = "Работа закончена";
        }

        private async void GetTvSeries_Click(object sender, RoutedEventArgs e)
        {
            tbUriList.Text = "";
            if (string.IsNullOrEmpty(tbUri.Text))
            {
                lbCurrentStageOfWork.Content = "Введите адрес сереала";
                return;
            }
            lbCurrentStageOfWork.Content = "Пытаюсь получить все ссылки";

            TvSeries tvSeries;
            try
            {
                ModelObjectsBuilder objectBuilder = GetModelObjectsBuilder();
                tvSeries = await objectBuilder.BuildTvSeriesAsync(new Uri(tbUri.Text));
            }
            catch (Exception ex)
            {
                lbCurrentStageOfWork.Content = ex.Message;
                return;
            }

            lbCurrentStageOfWork.Content = "Все ссылки получены";
            PrintTvSeriesUri(tvSeries);
            lbCurrentStageOfWork.Content = "Работа закончена";
        }

        private void OpenPlistInputWindow_Click(object sender, RoutedEventArgs e)
        {
            var plistWindow = new PlistInputWindow();
            Season season;

            plistWindow.Owner = this;
            if (plistWindow.ShowDialog() == true)
            {
                ModelObjectsBuilder objectBuilder = GetModelObjectsBuilder();
                season = objectBuilder.BuildSeasonFromJson(plistWindow.PlistStr, plistWindow.PatternStr);

                PrintSeriesUri(season);
            }
            lbCurrentStageOfWork.Content = "Все ссылки получены";
        }

        private void PrintSeriesUri(Season season)
        {
            var printString = new StringBuilder();
            foreach (var series in season.EpisodeList)
                printString.AppendLine(series.FileUri.ToString());
            tbUriList.Text = printString.ToString();
        }

        private void PrintTvSeriesUri(TvSeries tvSeries)
        {
            var printString = new StringBuilder();
            foreach (var season in tvSeries.SeasonList)
                foreach (var series in season.EpisodeList)
                    printString.AppendLine(series.FileUri.ToString());
            tbUriList.Text = printString.ToString();
        }
    }
}