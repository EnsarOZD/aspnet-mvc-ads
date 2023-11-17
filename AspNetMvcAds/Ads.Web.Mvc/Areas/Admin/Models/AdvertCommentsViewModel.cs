namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class AdvertCommentsViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }=string.Empty;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        public AdminUserViewModel? User { get; set; }
	}
}
