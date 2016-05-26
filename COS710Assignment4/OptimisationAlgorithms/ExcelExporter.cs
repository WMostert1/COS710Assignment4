using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisationAlgorithms
{
    public class ExcelExporter
    {
        public string FileName { get; set; }

        public string Output { get; set; }

        public ExcelExporter(string fileName)
        {
            FileName = fileName;
            Output = "";
        }
    }
}
