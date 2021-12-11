using Adyen;
using Adyen.Model.Checkout;
using Adyen.Service;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using static Adyen.Model.Checkout.CheckoutBalanceCheckResponse;

namespace ECommerce1.Data.Services
{
    public class AdyenService : IAdyenService
    {
        private readonly Client _adyenClient;
        private readonly Checkout _adyenCheckout;
        private readonly IOptions<AdyenConfig> _adyenConfig;
        private bool _isResult;
        private string _resultMessage;

        public AdyenService(IOptions<AdyenConfig> adyenConfig)
        {
            _adyenConfig = adyenConfig;
            _adyenClient = new Client(_adyenConfig.Value.ApiKey, Adyen.Model.Enum.Environment.Test);
            _adyenCheckout = new Checkout(_adyenClient);
        }

        public AdyenPaymentResponse CheckoutUsingGCash(string reference, long amount, User customer)
        {
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
            var _paymentResponseDeserialize = JsonSerializer.Deserialize<AdyenPaymentResponse>(_paymentResponse.ToJson());

            return _paymentResponseDeserialize;
        }

        public (bool, string) HandlePaymentResponse(string redirectResult)
        {
            _isResult = true;
            var _paymentCompletionDetails = new PaymentCompletionDetails();
            if (redirectResult != null)
            {
                _paymentCompletionDetails.RedirectResult = redirectResult;
                var _paymentsDetailsRequest = new PaymentsDetailsRequest(details: _paymentCompletionDetails);
                var _adyenCheckout = new Checkout(_adyenClient);

                try
                {
                    var _paymentDetailsResponse = _adyenCheckout.PaymentDetails(_paymentsDetailsRequest);
                    if (_paymentDetailsResponse.PspReference != "")
                    {
                        switch (_paymentDetailsResponse.ResultCode)
                        {
                            case (PaymentDetailsResponse.ResultCodeEnum?)ResultCodeEnum.Authorised:
                                _resultMessage = "Success";
                                break;

                            case (PaymentDetailsResponse.ResultCodeEnum?)ResultCodeEnum.Pending:
                            case (PaymentDetailsResponse.ResultCodeEnum?)ResultCodeEnum.Received:
                                _isResult = false;
                                _resultMessage = "Pending";
                                break;

                            case (PaymentDetailsResponse.ResultCodeEnum?)ResultCodeEnum.Refused:
                                _isResult = false;
                                _resultMessage = "Refused";
                                break;

                            default:
                                {
                                    _isResult = false;
                                    _resultMessage = _paymentDetailsResponse.RefusalReason;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        _isResult = false;
                        _resultMessage = "No response from payment gateway";
                    }
                }
                catch (Adyen.HttpClient.HttpClientException err)
                {
                    _isResult = false;
                    _resultMessage = err.ToString();
                }
            }

            return (_isResult, _resultMessage);
        }
    }
}