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
        public DataManager(IApplicationSettings applicationSettings)
        {
            appDataPath = applicationSettings.AppDataPath;
            if (string.IsNullOrWhiteSpace(appDataPath))
                appDataPath = Environment.CurrentDirectory;
            filesDirectory = Path.Combine(appDataPath, applicationSettings.FilesDirectory);
            string dataDirectory = Path.Combine(appDataPath, applicationSettings.DataDirectory);
            dataFile = Path.Combine(dataDirectory, applicationSettings.DataFile);

            if (!Directory.Exists(filesDirectory))
                Directory.CreateDirectory(filesDirectory);
            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);
            if (!File.Exists(dataFile))
            {
                File.WriteAllText(dataFile, "[]");
            }

            imageSize = applicationSettings.ImageSize;
        }
        public async Task<bool> SaveAsync(string text, IFormFile file)
        {
            return await Task.Run(() => {
                Guid newId = Guid.NewGuid();
                string fileExtension = file.FileName.Substring(file.FileName.IndexOf('.'));
                bool resultAddingData = true;
                bool resultWritingFile = true;
                Task[] tasks = new Task[2]
                {
                    new Task(() => {
                        try
                        {
                            List<DataItem> data = JsonConvert.DeserializeObject<List<DataItem>>(File.ReadAllText(dataFile));
                            data.Add(new DataItem { Id = newId, Text = text, FileExtension = ".jpg" });
                            File.WriteAllText(dataFile, JsonConvert.SerializeObject(data));
                        }
                        catch
                        {
                            resultAddingData = false;
                        }
                    }),
                    new Task(() => {
                        try
                        {
                            Stream fileStream = new FileStream(Path.Combine(filesDirectory, newId.ToString() + "_o" + fileExtension), System.IO.FileMode.Create);

                            file.CopyTo(fileStream);
                            fileStream.Close();
                            fileStream.Dispose();

                            Bitmap bitmap = new Bitmap(Path.Combine(filesDirectory, newId.ToString() + "_o" + fileExtension));
                            int width = bitmap.Width;
                            int height = bitmap.Height;
                            if(width > imageSize || height > imageSize)
                            {
                                double indexWidth = ((double)(width))/((double)(imageSize));
                                double indexHeight = ((double)(height))/((double)(imageSize));
                                double index;
                                if(indexWidth > indexHeight)
                                    index = indexWidth;
                                else
                                    index = indexHeight;
                                int resultWidth = ((int)(width / index));
                                int resultHeight = ((int)(height / index));
                                Bitmap bitmapResult = (Bitmap)bitmap.GetThumbnailImage(resultWidth, resultHeight, null, IntPtr.Zero);
                                bitmapResult.Save(Path.Combine(filesDirectory, newId.ToString() + ".jpg"), ImageFormat.Jpeg);
                                bitmapResult.Dispose();
                            }
                            else
                            {
                                bitmap.Save(Path.Combine(filesDirectory, newId.ToString() + ".jpg"), ImageFormat.Jpeg);
                            }
                            bitmap.Dispose();
                        }
                        catch(System.Exception ex)
                        {
                            resultWritingFile = false;
                        }
                    })
                };

                lock (locker)
                {
                    foreach (var t in tasks)
                        t.Start();

                    Task.WaitAll(tasks);
                }

                return (resultAddingData & resultWritingFile);
            });



        }

        public async Task<List<DataItem>> GetDataItems() => await Task.Run(() => { return JsonConvert.DeserializeObject<List<DataItem>>(File.ReadAllText(this.dataFile)); });
    }
}
