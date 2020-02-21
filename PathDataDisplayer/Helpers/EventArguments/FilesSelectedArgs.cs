using System;
using System.Collections.Generic;

namespace PathDataDisplayer.Helpers.EventArguments
{
    public class FilesSelectedArgs : EventArgs
    {
        public IEnumerable<string> FileNames { get; set; }

        public FilesSelectedArgs(IEnumerable<string> fileNames)
        {
            FileNames = fileNames;
        }
    }
}
