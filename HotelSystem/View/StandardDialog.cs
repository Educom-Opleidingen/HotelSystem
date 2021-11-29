using HotelSystem.BusinessLayer.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.View
{
    public class StandardDialog : IStandardDialog
    {
        public string GetExportFilename()
        {
            var saveDialog = new SaveFileDialog
            {
                DefaultExt = ".tsv",
                Filter = "Excel table (.tsv)|*.tsv"
            };
            var result = saveDialog.ShowDialog();
            var defaultLocation = result == true ? saveDialog.FileName : null;

            return defaultLocation;
        }
    }
}