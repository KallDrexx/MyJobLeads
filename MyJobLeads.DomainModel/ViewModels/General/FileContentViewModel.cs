using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.General
{
    public class FileContentViewModel
    {
        public string FileName { get; set; }
        public byte[] ExportFileContents { get; set; }
        public string Mimetype { get; set; }
    }
}
