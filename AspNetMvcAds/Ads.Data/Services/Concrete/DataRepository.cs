using Ads.Data.Services.Abstract;
using Ads.Utilites.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ads.Data.Services.Concrete
{
    public class DataRepository<T> : IRepository<T> where T : class
    {
        private AppDbContext _dbContext;
        private readonly ILogger<AppDbContext> _logger;

		public DataRepository(AppDbContext dbContext,ILogger<AppDbContext> logger) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<OperationResult<T>> Add(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return OperationResult<T>.SuccessResult(entity, "Entity added");

            }
            catch (Exception ex)
            {
                return OperationResult<T>.FailureResult("An error occurred while adding the entity");
            }
        }

        public async Task<OperationResult<T>> Delete(T entity)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return OperationResult<T>.SuccessResult(entity, "Entity deleted");

            }
            catch (Exception ex)
            {

                return OperationResult<T>.FailureResult("An error occurred while deleting the entity");
            }
          
        }

        public IQueryable<T> GetAll() => _dbContext.Set<T>();
        

        public async Task<OperationResult<T>> GetById(int? id)
        {
            var entity=await _dbContext.Set<T>().FindAsync(id);
            if (entity==null)
            {
                return OperationResult<T>.FailureResult("Entity not found");
            }
            return OperationResult<T>.SuccessResult(entity, "Entity found");
        }

        public async Task<OperationResult<T>> Update(T entity)
        {
            try
            {
                _dbContext.Set<T>().Update(entity);
                await _dbContext.SaveChangesAsync();
                return OperationResult<T>.SuccessResult(entity, "Entity updated");
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message);
				if (ex.InnerException != null)
				{
					_logger.LogError("İç hata: {InnerException}", ex.InnerException);
				}
				return OperationResult<T>.FailureResult($"An error occurred while updating the entity:{ex.Message}");
            }
           
        }
    }
}
