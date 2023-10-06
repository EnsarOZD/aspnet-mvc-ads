namespace Ads.Web.Mvc.Models
{
	public class AdvertDetailViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
        public List<string> ImagePaths { get; set; }
        public UserViewModel User { get; set; }
    }
}
