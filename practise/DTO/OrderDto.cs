using System;

namespace practise.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int  ProductId { get; set; }
        public string ProductName { get; set; }
        public string  ProductType { get; set; }
        public decimal OrderPrice { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime Orderdate { get; set; }

    }
}
