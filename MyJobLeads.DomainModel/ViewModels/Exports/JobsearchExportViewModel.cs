using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyJobLeads.DomainModel.ViewModels.Exports
{
    public class JobsearchExportViewModel
    {
        public string FileName { get; set; }
        public byte[] ExportFileContents { get; set; }
    }
}
