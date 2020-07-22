using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Models.Purchase
{
    public class DataProcess
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OrgToHeader(MesContext context)
        {
            CTR_HEADER_T ctrheaderT = new CTR_HEADER_T();
            CTR_DETAIL_T ctrdetailT = new CTR_DETAIL_T();
            IRepository<CTR_ORG_T> OrgRepositiory = new GenericRepository<CTR_ORG_T>(context);
            IRepository<CTR_HEADER_T> CtrHeaderRepositiory = new GenericRepository<CTR_HEADER_T>(context);
            IRepository<CTR_DETAIL_T> CtrDetailRepositiory = new GenericRepository<CTR_DETAIL_T>(context);
            var org = OrgRepositiory.Get().ToList();
            CtrHeaderRepositiory.getContext().Configuration.AutoDetectChangesEnabled = false;
            CtrDetailRepositiory.getContext().Configuration.AutoDetectChangesEnabled = false;
            try
            {
                for (int i = 0; org.Count() > i; i++)
                {

                    ctrheaderT.OrgId = org[i].OrgId;
                    ctrheaderT.OrgName = org[i].OrgName;
                    ctrheaderT.LineId = org[i].LineId;
                    ctrheaderT.ContainerNo = org[i].ContainerNo;
                    ctrheaderT.MvContainerDate = org[i].MvContainerDate;
                    ctrheaderT.OrganizationId = org[i].OrganizationId;
                    ctrheaderT.OrganizationCode = org[i].OrganizationCode;
                    ctrheaderT.Subinventory = org[i].Subinventory;
                    ctrheaderT.Status = 1;
                    ctrheaderT.CreatedBy = org[i].CreatedBy;
                    ctrheaderT.CreationDate = org[i].CreationDate;
                    ctrheaderT.LastUpdateBy = org[i].LastUpdateBy;
                    ctrheaderT.LastUpdateDate = org[i].LastUpdateDate;


                    ctrdetailT.CtrHeaderId = ctrheaderT.CtrHeaderId;
                    ctrdetailT.ProcessCode = org[i].ProcessCode;
                    ctrdetailT.ServerCode = org[i].ServerCode;
                    ctrdetailT.BatchId = org[i].BatchId;
                    ctrdetailT.BatchLineId = org[i].BatchLineId;
                    ctrdetailT.HeaderId = org[i].HeaderId;
                    ctrdetailT.LineId = org[i].LineId;
                    ctrdetailT.DetailId = org[i].DetailId;
                    ctrdetailT.LocatorId = org[i].LocatorId;
                    ctrdetailT.LocatorCode = org[i].LocatorCode;
                    ctrdetailT.InventoryItemId = org[i].InventoryItemId;
                    ctrdetailT.ShipItemNumber = org[i].ShipItemNumber;
                    ctrdetailT.PaperType = org[i].PaperType;
                    ctrdetailT.BasicWeight = org[i].BasicWeight;
                    ctrdetailT.ReamWeight = org[i].ReamWeight;
                    ctrdetailT.RollReamQty = org[i].RollReamQty;
                    ctrdetailT.RollReamWt = org[i].RollReamWt;
                    ctrdetailT.TtlRollReam = org[i].TtlRollReam;
                    ctrdetailT.Specification = org[i].Specification;
                    ctrdetailT.PackingType = org[i].PackingType;
                    ctrdetailT.ShipMtQty = org[i].ShipMtQty;
                    ctrdetailT.TransactionQuantity = org[i].TransactionQuantity;
                    ctrdetailT.TransactionUom = org[i].TransactionUom;
                    ctrdetailT.PrimaryQuantity = org[i].PrimaryQuantity;
                    ctrdetailT.PrimaryUom = org[i].PrimaryUom;
                    ctrdetailT.SecondaryQuantity = org[i].SecondaryQuantity;
                    ctrdetailT.SecondaryUom = org[i].SecondaryUom;
                    ctrdetailT.LotNumber = org[i].LotNumber;
                    ctrdetailT.TheoryWeight = org[i].TheoryWeight;
                    if (org[i].LotNumber == "")
                    {
                        ctrdetailT.ItemCategory = "平張";
                    }
                    else
                    {
                        ctrdetailT.ItemCategory = "捲筒";
                    }
                    ctrdetailT.CreatedBy = org[i].CreatedBy;
                    ctrdetailT.CreationDate = org[i].CreationDate;
                    ctrdetailT.LastUpdateBy = org[i].LastUpdateBy;
                    ctrdetailT.LastUpdateDate = org[i].LastUpdateDate;

                    CtrHeaderRepositiory.Create(ctrheaderT, false);
                    CtrDetailRepositiory.Create(ctrdetailT, false);
                }
                CtrHeaderRepositiory.getContext().Configuration.AutoDetectChangesEnabled = true;
                CtrDetailRepositiory.getContext().Configuration.AutoDetectChangesEnabled = true;
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

        }

        public List<FullCalendarEventModel> FullCalender()
        {
            IRepository<CTR_HEADER_T> CtrHeaderRepositiory = new GenericRepository<CTR_HEADER_T>();
            var header = CtrHeaderRepositiory.Get().ToList();

            List<FullCalendarEventModel> fullCalendarEventModel = new List<FullCalendarEventModel>();
            UrlHelper objUrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            for (int i = 0; i < header.Count; i++)
            {
                if (header[i].Status == 1)
                {
                    fullCalendarEventModel.Add(new FullCalendarEventModel()
                    {
                        id = header[i].CtrHeaderId,
                        title = header[i].Subinventory + "\n" + header[i].ContainerNo + " 待入庫",
                        start = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        end = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        allDay = false,
                        url = objUrlHelper.Action("Detail", "Purchase", new
                        {
                            CONTAINER_NO = header[i].ContainerNo,
                            Start = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                            Status = header[i].Status,
                            Subinventory = header[i].Subinventory
                        }),
                        Status = header[i].Status
                    });
                }
                if (header[i].Status == 2)
                {
                    fullCalendarEventModel.Add(new FullCalendarEventModel()
                    {
                        id = header[i].CtrHeaderId,
                        title = header[i].Subinventory + "\n" + header[i].ContainerNo + "取消",
                        start = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        end = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        allDay = false,
                        Status = header[i].Status,
                        color = "#E60000"
                    });
                }

            }
            return fullCalendarEventModel;
        }



//        public void FlatHeader()
//        {
//            using(var mesContext = new MesContext())
//            {
//                StringBuilder query = new StringBuilder();
//                query.Append(
//                @"SELECT 
//d.CTR_HEADER_ID AS Id,
//h.Subinventory AS Subinventory, 
//d.InventoryItemId AS InventoryItemId,
//d.Category AS Category,
//d.State,
//d.EditedStaff,
//d.EditedTime
//FROM dbo.CTR_DETAIL_T d
//LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
//WHERE s.State = 0 AND s.SpecId = @id");
//                return mesContext.Database.SqlQuery<PurchaseViewModel>(query.ToString(), new SqlParameter("@id", id)).SingleOrDefault();
//            }
//        }
    }
}