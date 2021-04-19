using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Project1.Models.DomainModels;


namespace Project1.Services
{
    public interface IDataManager
    {
        Task<List<DataItem>> GetDataItems();
        bool SaveNew(string text, IFormFile file);
    }
}
