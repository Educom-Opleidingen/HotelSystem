using HotelSystem.BusinessLayer.View;

namespace HotelSystem.Test
{
    internal class TestStandardDialog : IStandardDialog
    {
        public string Location { get; set; }

        public string GetExportFilename()
        {
            
            return Location;
        }
    }
}