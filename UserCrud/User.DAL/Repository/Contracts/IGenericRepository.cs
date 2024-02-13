using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace User.DAL.Repository.Contracts
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<TModel> GetDataDetails(Expression<Func<TModel, bool>> filter);
        Task<TModel> CreateData(TModel model);
        Task<bool> UpdateData(TModel model);
        Task<bool> RemoveData(TModel model);
        Task<IQueryable<TModel>> ValidateDataExistence(Expression<Func<TModel, bool>> filter = null);

    }

}
