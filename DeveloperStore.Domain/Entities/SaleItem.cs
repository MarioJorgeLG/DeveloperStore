using System;

namespace DeveloperStore.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => (Quantity * UnitPrice) - Discount;

        public Sale? Sale { get; set; }

        public SaleItem(
            Guid saleId,
            Guid productId,
            decimal unitPrice,
            int quantity,
            decimal totalPrice,
            decimal discount
        )
        {
            Id = Guid.NewGuid();
            SaleId = saleId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
        }
    }
}
