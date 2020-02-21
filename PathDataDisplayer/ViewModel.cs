using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private List<XElement> GeneratePaths(IEnumerable<string> directoryPaths)
        {
            var fileNames = GetListOfFiles(directoryPaths).ToArray();
            var pathDatas = GetPathDatas(fileNames);
            var pathGeometries = GetPathGeometries(fileNames);
            pathDatas.AddRange(pathGeometries);
            return pathDatas;
        }

        private IEnumerable<string> GetListOfFiles(IEnumerable<string> directoryPaths)
        {
            var filePaths = new List<string>();
            foreach (var directoryPath in directoryPaths)
            {
                filePaths.AddRange(GetAllFiles(directoryPath, "*.xaml"));
            }

            return filePaths;
        }

        private IEnumerable<string> GetAllFiles(string directoryPath, string searchPattern)
        {
            var filesInside = new DirectoryInfo(directoryPath).EnumerateFileSystemInfos(searchPattern).Select(x => x.FullName).ToList();
            var directoriesInside = new DirectoryInfo(directoryPath).EnumerateDirectories();

            filesInside.AddRange(directoriesInside.SelectMany(x => GetAllFiles(x.FullName, searchPattern)));
            return filesInside;
        }

        private List<XElement> GetPathDatas(string[] fileNames)
        {
            return GetPaths(fileNames, "Path", x =>
            {
                if (!x.Attributes().Any(y => y.Name.LocalName.Equals("Key")))
                {
                    return null;
                }

                var attribute = x.Attributes().FirstOrDefault(y => y.Name == "Data");// || y.Name == "Figures");

                var element = new XElement(attribute?.Name ?? "Data");
                element.Value = attribute?.Value ?? x.Value;

                if (string.IsNullOrEmpty(element.Value))
                {
                    return null;
                }

                var key = x.Attributes().FirstOrDefault(v => v?.Name.LocalName.Equals("Key") == true);
                if (key != null)
                {
                    element.Add(new XAttribute("Key", key.Value));
                }

                return element.Value.Contains('{') == false ? element : null;
            });
        }

        private List<XElement> GetPathGeometries(string[] fileNames)
        {
            return GetPaths(fileNames, "PathGeometry", x =>
            {
                if (string.IsNullOrEmpty(x.Value))
                {
                    var figures = x.Attributes().FirstOrDefault(y => y.Name == "Figures");
                    if (figures != null)
                    {
                        x.Value = figures.Value;
                    }
                }

                if (x.Attributes().All(y => y.Name.LocalName != "Key"))
                {
                    var parentKey = GetParentKey(x);
                    if (!string.IsNullOrEmpty(parentKey))
                    {
                        x.Add(new XAttribute("Key", parentKey));
                    }
                }

                return string.IsNullOrEmpty(x.Value) ? null : x;
            });
        }

        private List<XElement> GetPaths(string[] fileNames, string elementName, Func<XElement, XElement> selector)
        {
            var pathDatas = new List<XElement>();
            foreach (var file in fileNames)
            {
                var document = XDocument.Load(file);
                var descendants = document.Descendants()
                    .Where(x => x.Name.LocalName.Equals(elementName))
                    .Select(selector);
                pathDatas.AddRange(descendants.Where(x => x != null));
            }

            return pathDatas;
        }

        private string GetParentKey(XElement element)
        {
            if (element == null)
            {
                return null;
            }

            var parentKey = element.Parent?.Attributes().FirstOrDefault(y => y.Name.LocalName == "Key");

            return string.IsNullOrEmpty(parentKey?.Value) ? GetParentKey(element.Parent) : parentKey.Value;
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
