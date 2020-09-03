using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Obsolete;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class ObsoleteUOW : TransferUOW
    {

        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_OBSOLETE_HEADER_T> trfObsoleteHeaderTRepositiory;
        private readonly IRepository<TRF_OBSOLETE_T> trfObsoleteTRepositiory;
        private readonly IRepository<TRF_OBSOLETE_HT> trfObsoleteHtRepositiory;

        public ObsoleteUOW(DbContext context)
        : base(context)
        {
            this.trfObsoleteHeaderTRepositiory = new GenericRepository<TRF_OBSOLETE_HEADER_T>(this);
            this.trfObsoleteTRepositiory = new GenericRepository<TRF_OBSOLETE_T>(this);
            this.trfObsoleteHtRepositiory = new GenericRepository<TRF_OBSOLETE_HT>(this);
        }




    }
}