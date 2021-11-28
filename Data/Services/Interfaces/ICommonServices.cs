namespace ECommerce1.Data.Services.Interfaces
{
    public interface ICommonServices
    {
        public string NoImage { get; set; }

        string GetImageByte64StringFromSplit(string value);
    }
}