using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICommonServices
    {
        public string NoImage { get; set; }

        public List<Color> GetColors();
        public List<Size> GetSizes();
        public string GetImageByte64StringFromSplit(string value);
    }
}