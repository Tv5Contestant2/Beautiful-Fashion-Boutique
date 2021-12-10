using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string CustomersId { get; set; }
        public string Instructions { get; set; }

        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingBlock { get; set; }
        public string ShippingLot { get; set; }
        public string ShippingBarangay { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingContactNumber { get; set; }
        public string ShippingEmail { get; set; }
        public bool IsBillingSame { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingBlock { get; set; }
        public string BillingLot { get; set; }
        public string BillingBarangay { get; set; }
        public string BillingCity { get; set; }
        public string BillingContactNumber { get; set; }
        public string BillingEmail { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsGCash { get; set; }
        public bool IsPickup { get; set; }
        public bool IsPayMaya { get; set; }
        public bool IsCashOnDelivery { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public Customers Customers { get; set; }
    }
}