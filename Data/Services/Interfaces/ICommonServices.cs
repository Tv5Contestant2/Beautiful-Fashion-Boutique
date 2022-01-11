using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICommonServices
    {
        public string NoImage { get; set; }

        string GetImageByte64StringFromSplit(string value);

        public IEnumerable<Modules> GetModules();

        public string GetPaymentMethod(int id);

        public string GetDeliveryMethod(int id);
    }
}