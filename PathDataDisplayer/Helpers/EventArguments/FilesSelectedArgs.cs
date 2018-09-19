using System;

namespace PathDataDisplayer.Helpers.EventArguments
{
    public class FilesSelectedArgs : EventArgs
    {
        public string[] FileNames { get; set; }

        public FilesSelectedArgs(string[] fileNames)
        {
            FileNames = fileNames;
        }
    }
}
