using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Data.EF.Entities
{
	public partial class Role : IdentityRole
	{
		public Role()
		{
			Accounts = new HashSet<Account>();
		}

		public Role(string roleName) : base(roleName)
		{
		}
		//public string? RoleName { get; set; }
		public virtual ICollection<Account> Accounts { get; set; }
	}
}
