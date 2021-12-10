using ECommerce1.Models;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IAdyenService
    {
        AdyenPaymentResponse CheckoutUsingGCash(string reference, long amount, User customer);

        (bool, string) HandlePaymentResponse(string redirectResult);
    }
}