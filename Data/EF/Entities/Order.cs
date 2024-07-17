using System;
using System.Collections.Generic;

namespace Data.EF.Entities
{
	public partial class Order
	{
		public Order()
		{
			OrderDetails = new HashSet<OrderDetail>();
		}

		public int OrderId { get; set; }
		public string AccountID { get; set; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhoneNumber { set; get; }
        public DateTime OrderDate { get; set; }
		public double TotalAmount { get; set; }
		public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public virtual Account? Account { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
	}
}
