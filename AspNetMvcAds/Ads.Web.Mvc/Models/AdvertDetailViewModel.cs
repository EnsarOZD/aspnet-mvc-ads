using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
	public class AdvertDetailViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTimeOffset CreatedAt { get; set; }= DateTimeOffset.Now;
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
        public List<string> ImagePaths { get; set; }
        public UserViewModel User { get; set; }
        public int StarCount { get; set; }
		public decimal Price { get; set; }
		public List<AdvertCommentViewModel> AdvertComments { get; set; } = new List<AdvertCommentViewModel>();
	}
}
