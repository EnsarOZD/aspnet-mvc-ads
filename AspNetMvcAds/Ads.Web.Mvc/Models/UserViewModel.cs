namespace Ads.Web.Mvc.Models
{

    public class UserViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? Email { get; set; } 
        public string? Password { get; set; } 
        public string? Phone { get; set; } 
		    public string? Address { get; set; } 
		    public bool IsEmailConfirmed { get; set; }
        public string? UserImagePath { get; set; } 
    }

}
