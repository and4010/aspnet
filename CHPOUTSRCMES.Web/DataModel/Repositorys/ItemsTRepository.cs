using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using EntityFramework.Utilities;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Information;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys
{
    public class ItemsTRepository : GenericRepository<ITEMS_T>, IItemsTRepository
    {
        public ItemsTRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}