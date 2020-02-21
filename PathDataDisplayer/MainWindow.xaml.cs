using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
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
            using (var selectFolderDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = true
            })
            {
                selectFolderDialog.ShowDialog();

                if (!selectFolderDialog.FileNames.Any())
                {
                    return;
                }

                SelectedFilesNotifier.Instance.RaiseFilesSelected(this, new FilesSelectedArgs(selectFolderDialog.FileNames));
            }
        }

        private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.OriginalSource is Button button))
            {
                return;
            }

            if (!(button.DataContext is XElement element))
            {
                return;
            }

            var key = element.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals("Key"));
            if (key == null)
            {
                return;
            }

            Clipboard.SetText(key.Value);
        }
    }
}
