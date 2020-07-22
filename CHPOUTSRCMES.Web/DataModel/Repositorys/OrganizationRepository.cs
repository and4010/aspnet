using CHPOUTSRCMES.Web.DataModel.Entiy.Information;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.DataModel.Repositorys
{
    public class OrganizationRepository : GenericRepository<ORGANIZATION_T>
    {
        public List<SelectListItem> GetOrganizationList()
        {
            List<ListItem> organizationList = new List<ListItem>();
            organizationList.Add(new ListItem("全部", "*"));

            var query = from table in Get()
                        group table by new { table.OrganizationCode, table.OrganizationID } into g
                        select new ListItem
                        {
                            Text = g.Key.OrganizationCode,
                            Value = g.Key.OrganizationID.ToString()
                        };
         
            organizationList.AddRange(query.ToList());
            return organizationList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value }).ToList();
        }
    }
}