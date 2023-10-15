using Ads.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data.Entities
{
	public class AdTypeEntity : IAuditEntity
	{
		public int Id { get; set; }
		public string TypeName { get; set; } = string.Empty;
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
		public DateTimeOffset DeletedAt { get; set; }
	}
}
