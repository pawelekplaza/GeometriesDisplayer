using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using PathDataDisplayer.Annotations;
using PathDataDisplayer.Helpers;

namespace PathDataDisplayer
{
    public class ViewModel : INotifyPropertyChanged
    {
        private List<XElement> _paths;

        public List<XElement> Paths
        {
            get => _paths;
            set { _paths = value; OnPropertyChanged(); }
        }

        public ViewModel()
        {
            SelectedFilesNotifier.Instance.FilesSelected += (s, e) => Paths = GeneratePaths(e.FileNames);
        }

        private List<XElement> GeneratePaths(string[] fileNames)
        {
            var pathDatas = GetPathDatas(fileNames);
            var pathGeometries = GetPathGeometries(fileNames);
            pathDatas.AddRange(pathGeometries);
            return pathDatas;
        }

        private List<XElement> GetPathDatas(string[] fileNames)
        {
            return GetData(fileNames, "Path.Data");
        }

        private List<XElement> GetPathGeometries(string[] fileNames)
        {
            return GetData(fileNames, "PathGeometry");
        }

        private List<XElement> GetData(string[] fileNames, string elementName)
        {
            var pathDatas = new List<XElement>();
            foreach (var file in fileNames)
            {                
                var document = XDocument.Load(file);               
                var descendants = document.Descendants().Where(v => v.Name.LocalName.Contains(elementName));
                pathDatas.AddRange(descendants);
            }

            return pathDatas;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
