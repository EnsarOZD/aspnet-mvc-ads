namespace Ads.Web.Mvc.Areas.Admin.Models
{
	public class UserViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string Phone { get; set; } = string.Empty;
		public string Address {  get; set; } = string.Empty;
		public bool IsEmailConfirmed { get; set; }
		public string Roles { get; set; } = string.Empty;
    }
}
