namespace HotelSystem.View
{
    public interface IStandardDialog
    {
        /// <summary>
        /// Opens SaveDialog asking for a filename for export
        /// </summary>
        /// <returns>string filename or NULL if dialog is cancelled </returns>
        string GetExportFilename();
    }
}