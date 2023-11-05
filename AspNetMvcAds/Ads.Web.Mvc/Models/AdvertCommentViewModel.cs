namespace Ads.Web.Mvc.Models
{
	public class AdvertCommentViewModel
	{
		public int Id { get; set; }
		public string? Comment { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; } = string.Empty;
		public int AdvertId { get; set; }
		public bool IsActive { get; set; }
		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
	}
}
