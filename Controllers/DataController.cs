using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project1.Services;
using Project1.Models.DomainModels;
using Project1.Models.Settings;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private IDataManager dataManager;
        private IApplicationSettings applicationSettings;
        public DataController(IDataManager dataManager, IApplicationSettings applicationSettings)
        {
            this.dataManager = dataManager;
            this.applicationSettings = applicationSettings;
        }

        [HttpGet]
        [Route("data")]
        public async Task<List<DataItem>> GetData() => await dataManager.GetDataItems();

        [HttpPost, DisableRequestSizeLimit]
        [Route("saveNew")]
        public async Task<bool> SaveNew()
        {
            var file = Request.Form.Files[0];
            string text = Request.Form["text"];
            return await dataManager.SaveAsync(Request.Form["text"], file);
        }

        [HttpGet]
        [Route("image/{filename}")]
        public async Task<PhysicalFileResult> GetImage()
        {
            return await Task.Run(() => {

                string fileName = Request.RouteValues["filename"].ToString();

                string fileFullPath =
                Path.Combine(
                    string.IsNullOrEmpty(applicationSettings.AppDataPath) ? Environment.CurrentDirectory : applicationSettings.AppDataPath,
                    applicationSettings.FilesDirectory,
                    fileName);

                string fileExtension = fileName.Substring(fileName.IndexOf('.'));

                string fileType = string.Empty;
                switch (fileExtension)
                {
                    case ".jpg":
                        fileType = "image/jpeg";
                        break;
                    case ".png":
                        fileType = "image/png";
                        break;
                }

                return PhysicalFile(fileFullPath, fileType, fileName);
            });

        }
    }

}
