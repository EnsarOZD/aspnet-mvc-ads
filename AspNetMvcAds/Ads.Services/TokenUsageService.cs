using Ads.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services
{
    public class TokenUsageService
    {
        private readonly AppDbContext _dbContext;

        public TokenUsageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TokenUsage FindTokenUsage(string token)
        {
            return _dbContext.TokenUsages.FirstOrDefault(t => t.Token == token && !t.IsUsed);
        }

        public void MarkTokenAsUsed(TokenUsage tokenUsage)
        {
            tokenUsage.IsUsed = true;
            _dbContext.SaveChanges();
        }
    }
}
