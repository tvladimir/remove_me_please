using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models.DomainModels
{
    public class DataItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string FileExtension { get; set; }
    }
}
