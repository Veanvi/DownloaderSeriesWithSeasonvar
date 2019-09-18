﻿using DownloaderSeriesWithSeasonvar.Core;
using System;
using System.Text;
using System.Windows;

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
#if DEBUG
            tbUri.Text = "http://seasonvar.ru/serial-17482-Doktor_Kto-11-season.html";
#endif
        }

        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(tbUriList.Text);
            lbCurrentStageOfWork.Content = "Ссылки скопированы в буфер обмена";
            tbUri.Text = "";
            tbUriList.Text = "";
        }

        private async void GetSeason_Click(object sender, RoutedEventArgs e)
        {
            tbUriList.Text = "";
            if (tbUri.Text == "")
            {
                lbCurrentStageOfWork.Content = "Введите адрес сереала";
                return;
            }
            lbCurrentStageOfWork.Content = "Запущен браузер";
            var downloaderPlist = new DownloaderSeasonInfo(
                new Uri(tbUri.Text), (bool)cbTorProxy.IsChecked, (bool)cbHeadlessBrowser.IsChecked);
            Season season = null;

            try
            {
                season = await downloaderPlist.DownloadSeasonInfoAsync();
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

        private void OpenPlistInputWindow_Click(object sender, RoutedEventArgs e)
        {
            var plistWindow = new PlistInputWindow();
            Season season = null;

            plistWindow.Owner = this;
            if (plistWindow.ShowDialog() == true)
            {
                if (plistWindow.PatternStr == "")
                    season = new Season("", plistWindow.PlistStr);
                else
                {
                    season = new Season("");
                    season.SeriesList = PlaylistParser
                        .JsonPlaylistConvertToSeasonObject(plistWindow.PlistStr, plistWindow.PatternStr);
                }

                PrintSeriesUri(season);
            }
            lbCurrentStageOfWork.Content = "Все ссылки получены";
        }

        private void PrintSeriesUri(Season season)
        {
            var printString = new StringBuilder();
            foreach (var series in season.SeriesList)
                printString.AppendLine(series.FileUri.ToString());
            tbUriList.Text = printString.ToString();
        }
    }
}