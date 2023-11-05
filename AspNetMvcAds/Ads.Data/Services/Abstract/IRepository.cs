using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ads.Utilites.Services;

namespace Ads.Data.Services.Abstract
{
    public interface IRepository<T>where T : class
    {
        Task<OperationResult<T>> GetById(int? id);
        IQueryable<T> GetAll();
        Task<OperationResult<T>> Add(T entity);
        Task<OperationResult<T>> Update(T entity);
        Task<OperationResult<T>> Delete(T entity);
    }
}
