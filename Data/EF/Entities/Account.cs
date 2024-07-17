using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Data.EF.Entities
{
	public partial class Account : IdentityUser
	{
		public Account()
		{
			Ratings = new HashSet<Rating>();
            Orders = new HashSet<Order>();
        }

		//public string? AccountFullName { get; set; }
		public string? AccountAddress { get; set; }
		public DateTime? AccountBirthday { get; set; }
		public string? RoleId { get; set; }
		public int? CartId { get; set; }
		public virtual Role? Role { get; set; }
		public virtual Cart? Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
	}
}
