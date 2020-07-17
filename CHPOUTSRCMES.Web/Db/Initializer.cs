using CHPOUTSRCMES.Web.Data;
using CHPOUTSRCMES.Web.Db.Entiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MesContext>
    {
        protected override void Seed(MesContext context)
        {

        }
    }
}