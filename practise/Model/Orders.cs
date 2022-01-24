using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace practise.Model
{
    public partial class Orders
    {
        public int OredrId { get; set; }
        public decimal OrderPrice { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime Orderdate { get; set; }
        public int? ProductId { get; set; }
    }
}
