using System;
using System.Collections.Generic;

namespace Data.EF.Entities
{
	public partial class Rating
	{
		public int RatingId { get; set; }
		public int? ProductId { get; set; }
		public string? AccountId { get; set; }
		public int? RatingValue { get; set; }
		public DateTime? RatingDate { get; set; }

		public virtual Account? Account { get; set; }
		public virtual Product? Product { get; set; }
	}
}
