using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class TokenUsage
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; }
    }
}
