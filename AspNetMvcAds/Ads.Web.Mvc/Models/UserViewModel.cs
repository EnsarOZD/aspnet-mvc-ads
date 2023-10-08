namespace Ads.Web.Mvc.Models
{
	public class UserViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;
	}
}
