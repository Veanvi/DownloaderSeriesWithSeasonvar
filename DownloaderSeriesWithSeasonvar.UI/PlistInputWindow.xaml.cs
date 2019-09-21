using System.Linq;
using System.Windows;

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

        public string PatternStr { get; set; }
        public string PlistStr { get; set; }

        private void CopySelector_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(xpathSelector.Text);
        }

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
    }
}