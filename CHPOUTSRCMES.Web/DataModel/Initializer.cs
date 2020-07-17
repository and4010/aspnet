using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MesContext>
    {
        protected override void Seed(MesContext context)
        {

        }
    }
}