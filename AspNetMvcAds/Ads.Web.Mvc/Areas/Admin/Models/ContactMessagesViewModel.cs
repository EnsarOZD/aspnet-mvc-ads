using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class ContactMessagesViewModel
    {

        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Context { get; set; }
        public DateTime DateTime { get; set; }
        public bool Status { get; set; }
    }
}
