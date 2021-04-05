
namespace Project1.Models.Settings
{
    public interface IApplicationSettings
    {
        string AppDataPath { get; set; }
        string DataDirectory { get; set; }
        string DataFile { get; set; }
        string FilesDirectory { get; set; }
        int ImageSize { get; set; }
    }
}
