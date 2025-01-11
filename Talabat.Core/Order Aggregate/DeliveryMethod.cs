using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Order_Aggregate
{
	public class DeliveryMethod : BaseEntity
	{
		public string ShortName { get; set; }
		public string Description { get; set; }
		public string DeliveryTime { get; set; }
		public decimal Cost { get; set; }

		#region Constructors
		public DeliveryMethod() { }

		public DeliveryMethod(string shortName, string description, string deliveryTime, decimal cost)
		{
			ShortName = shortName;
			Description = description;
			DeliveryTime = deliveryTime;
			Cost = cost;
		}
		#endregion
	}
}
