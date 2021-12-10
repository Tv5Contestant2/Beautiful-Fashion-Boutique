using Adyen;
using Adyen.Model.Checkout;
using Adyen.Service;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECommerce1.Data.Services
{
    public class AdyenService : IAdyenService
    {
        private readonly Client _adyenClient;
        private readonly IOptions<AdyenConfig> _adyenConfig;

        public AdyenService(IOptions<AdyenConfig> adyenConfig)
        {
            _adyenConfig = adyenConfig;
            _adyenClient = new Client(_adyenConfig.Value.ApiKey, Adyen.Model.Enum.Environment.Test);
        }

        public AdyenPaymentResponse CheckoutUsingGCash(string reference, long amount, User customer)
        {
            var _adyenCheckout = new Checkout(_adyenClient);
            var _amount = new Adyen.Model.Checkout.Amount(_adyenConfig.Value.Currency, amount);
            var _paymentDetails = new Adyen.Model.Checkout.DefaultPaymentMethodDetails
            {
                Type = "gcash"
            };

            var _shopperDetail = new Adyen.Model.Checkout.Name(customer.FirstName
                , (Name.GenderEnum)customer.GenderId
                , string.Empty
                , customer.LastName);

            var _paymentsRequest = new Adyen.Model.Checkout.PaymentRequest
            {
                Reference = reference,
                Amount = _amount,
                ReturnUrl = _adyenConfig.Value.ReturnURL,
                MerchantAccount = _adyenConfig.Value.Merchant,
                ShopperName = _shopperDetail,
                PaymentMethod = _paymentDetails
            };

            var _paymentResponse = _adyenCheckout.Payments(_paymentsRequest);
            var _paymentResponseDeserialize = JsonSerializer.Deserialize<AdyenPaymentResponse>(_paymentResponse.ToJson()); ;

            return _paymentResponseDeserialize;
        }
    }
}