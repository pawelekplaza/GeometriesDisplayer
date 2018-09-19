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
using Microsoft.Win32;
using PathDataDisplayer.Helpers;
using PathDataDisplayer.Helpers.EventArguments;

namespace PathDataDisplayer
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

        private void BrowseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BrowseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var browseFileDialog = new OpenFileDialog
            {
                Filter = "Xaml files (*.xaml)|*.xaml",
                Multiselect = true
            };
            browseFileDialog.ShowDialog();
            if (browseFileDialog.FileNames.Length == 0)
                return;

            SelectedFilesNotifier.Instance.RaiseFilesSelected(this, new FilesSelectedArgs(browseFileDialog.FileNames));
        }
    }
}
