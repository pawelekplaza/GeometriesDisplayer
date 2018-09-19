using System;
using PathDataDisplayer.Helpers.EventArguments;

namespace PathDataDisplayer.Helpers
{
    public class SelectedFilesNotifier
    {
        #region Singletone
        private static readonly Lazy<SelectedFilesNotifier> _instance = new Lazy<SelectedFilesNotifier>(() => new SelectedFilesNotifier());
        public static SelectedFilesNotifier Instance => _instance.Value;
        protected SelectedFilesNotifier() { }
        #endregion

        public event EventHandler<FilesSelectedArgs> FilesSelected;

        public void RaiseFilesSelected(object sender, FilesSelectedArgs e) =>
            FilesSelected?.Invoke(sender, e);
    }
}
