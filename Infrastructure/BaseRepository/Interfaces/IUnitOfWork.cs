using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
  public  interface IUnitOfWork
    {
        /// <summary>
        /// Save changes/ends transaction
        /// </summary>
        /// <param name="applicationUser">Application user.</param>
        /// <returns>Status of save change.</returns>
        bool SaveChanges();

        /// <summary>
        /// Retrieve Generic Repository
        /// </summary>
        /// <typeparam name="T">Generic repository entity type.</typeparam>
        /// <returns>Repository instance.</returns>
        IBaseRepository<T> GetRepository<T>() where T : class;
    }
}
