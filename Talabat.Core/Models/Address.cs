using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models
{
	public class Address
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string Country { get; set; }

		public string AppUserId { get; set; } // FK
		public AppUser User { get; set; } // Navigational Property
	}
}
