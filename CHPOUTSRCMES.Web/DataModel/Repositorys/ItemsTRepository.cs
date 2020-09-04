using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks.Interfaces;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Repositorys
{
    public class ItemsTRepository : GenericRepository<ITEMS_T>, IItemsTRepository
    {
        public ItemsTRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}