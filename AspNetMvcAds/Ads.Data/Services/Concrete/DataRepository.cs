using Ads.Data.Services.Abstract;
using Ads.Utilites.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data.Services.Concrete
{
    public class DataRepository<T> : IRepository<T> where T : class
    {
        private AppDbContext _dbContext;

        public DataRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;       
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

                return OperationResult<T>.FailureResult("An error occurred while updating the entity");
            }
           
        }
    }
}
