using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace QC.MF.EntityFramework.Repositories
{
    public abstract class MFRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<MFDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected MFRepositoryBase(IDbContextProvider<MFDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class MFRepositoryBase<TEntity> : MFRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected MFRepositoryBase(IDbContextProvider<MFDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
