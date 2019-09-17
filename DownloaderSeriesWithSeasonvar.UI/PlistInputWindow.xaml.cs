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
using System.Windows.Shapes;

namespace DownloaderSeriesWithSeasonvar.UI
{
    /// <summary>
    /// Логика взаимодействия для PlistInputWindow.xaml
    /// </summary>
    public partial class PlistInputWindow : Window
    {
        public PlistInputWindow()
        {
            InitializeComponent();
        }

        public string PlistStr { get; set; }
        public string PatternStr { get; set; }

        private void OkDialog_Click(object sender, RoutedEventArgs e)
        {
            if (tbPlist.Text.Count() > 0)
            {
                DialogResult = true;
                PlistStr = tbPlist.Text;
                PatternStr = tbPattern.Text;
                Close();
            }
            else
                Close();

        }

        private void CopySelector_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(xpathSelector.Text);
        }
    }
}
