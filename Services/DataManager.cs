using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project1.Models.Settings;
using Project1.Models.DomainModels;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;

namespace Project1.Services
{
    public class DataManager : IDataManager
    {
        static object locker = new object();

        private string appDataPath;
        private string filesDirectory;
        private string dataFile;
        private int imageSize;
        private Dictionary<string, ImageFormat> fileTypes;
        public DataManager(IApplicationSettings applicationSettings)
        {
            fileTypes = new Dictionary<string, ImageFormat>();
            fileTypes[".jpg"] = ImageFormat.Jpeg;
            fileTypes[".png"] = ImageFormat.Png;

            appDataPath = !string.IsNullOrWhiteSpace(applicationSettings.AppDataPath) ? applicationSettings.AppDataPath : Environment.CurrentDirectory;

            filesDirectory = Path.Combine(appDataPath, applicationSettings.FilesDirectory);
            string dataDirectory = Path.Combine(appDataPath, applicationSettings.DataDirectory);
            dataFile = Path.Combine(dataDirectory, applicationSettings.DataFile);

            if (!Directory.Exists(filesDirectory))
            {
                Directory.CreateDirectory(filesDirectory);
            }

            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
            
            if (!File.Exists(dataFile))
            {
                File.WriteAllText(dataFile, "[]");
            }

            imageSize = applicationSettings.ImageSize;


        }
        public bool SaveNew(string text, IFormFile file)
        {
            try
            {
                Guid newId = Guid.NewGuid();
                string fileExtension = file.FileName.Substring(file.FileName.IndexOf('.'));
                List<Task> TaskList = new List<Task>();
                TaskList.Add(AddTodo(newId, text, newId.ToString(), fileExtension));
                TaskList.Add(SaveImage(file, filesDirectory, newId.ToString(), fileExtension));
                Task.WaitAll(TaskList.ToArray());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task AddTodo(Guid newId, string text, string imageName, string imageExtension){
            Task task = new Task(() => {
                        List<DataItem> data = JsonConvert.DeserializeObject<List<DataItem>>(File.ReadAllText(dataFile));
                        data.Add(new DataItem { 
                            Id = newId, 
                            Text = text, 
                            ImageName = imageName,
                            ImageExtension = imageExtension 
                        });
                        File.WriteAllText(dataFile, JsonConvert.SerializeObject(data));
                    });
            task.Start();
            return task;
        }

        public Task SaveImage(IFormFile file, string basePath, string fileName, string fileExtension)
        {
            Task task = new Task(() => {
                var tempFileName = Path.GetTempFileName();
                Stream fileStream = new FileStream(Path.Combine(basePath, tempFileName), FileMode.Create);

                file.CopyTo(fileStream);

                Bitmap bitmap = new Bitmap(Path.Combine(basePath, tempFileName));

                    if(bitmap.Width > imageSize || bitmap.Height > imageSize)
                    {
                        double indexWidth = ((double)(bitmap.Width))/((double)(imageSize));
                        double indexHeight = ((double)(bitmap.Height))/((double)(imageSize));
                        double index = (indexWidth > indexHeight) ? indexWidth : indexHeight;
                        int resultWidth = ((int)(bitmap.Width / index));
                        int resultHeight = ((int)(bitmap.Height / index));
                        Bitmap bitmapResult = (Bitmap)bitmap.GetThumbnailImage(resultWidth, resultHeight, null, IntPtr.Zero);
                        bitmapResult.Save(Path.Combine(basePath, $"{fileName}{fileExtension}"), fileTypes[fileExtension]);
                    }
                    else
                    {
                        bitmap.Save(Path.Combine(basePath, $"{fileName}{fileExtension}"), fileTypes[fileExtension]);
                    }
            });
            task.Start();
            return task;
        }

        public async Task<List<DataItem>> GetDataItems() => await Task.Run(() => { return JsonConvert.DeserializeObject<List<DataItem>>(File.ReadAllText(this.dataFile)); });
    }
}
