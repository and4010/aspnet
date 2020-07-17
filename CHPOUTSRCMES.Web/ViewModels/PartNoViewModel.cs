using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class PartNoViewModel
    {
       
        public PartNoModel partNoModel { set; get; }

        public static List<PartNoModel> model = new List<PartNoModel>();

        public List<PartNoModel> GetPartNo()
        {
            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "4.433.DM00",
                Category_name_inv = "YFY Inventory",
                Category_code_cost = "OP",
                Category_name_cost = "YFY Cost",
                Category_code_control = "",
                Category_name_control = "YFY Item Control",
                Item_number = "4DM00A03500214K512K",
                Inventory_item_id = 504029,
                Item_desc_eng = "COATED D-B(G-B)",
                Item_desc_sch = "全塗灰銅卡",
                Item_desc_tch = "全塗灰銅卡",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "Active",
                Item_type = "16",
                Catalog_elem_val_010 = "塗佈白紙板",
                Catalog_elem_val_020 = "DM00",
                Catalog_elem_val_030 = "A",
                Catalog_elem_val_040 = "03500",
                Catalog_elem_val_050 = "214K512K",
                Catalog_elem_val_060 = "274.27",
                Catalog_elem_val_070 = "平版",
                Catalog_elem_val_080 = "N",
                Catalog_elem_val_090 = "雙邊特規",
                Catalog_elem_val_100 = "L",
                Catalog_elem_val_110 = "令包",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "全塗灰銅卡",
                Catalog_elem_val_140 = "COATED D-B(G-B)",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });


            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "4.433.DM00",
                Category_name_inv = "YFY Inventory",
                Category_code_cost = "OP",
                Category_name_cost = "YFY Cost",
                Category_code_control = "",
                Category_name_control = "YFY Item Control",
                Item_number = "4DM00P03000310K446K",
                Inventory_item_id = 505468,
                Item_desc_eng = "COATED D-B(G-B)",
                Item_desc_sch = "全塗灰銅卡",
                Item_desc_tch = "全塗灰銅卡",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "Active",
                Item_type = "16",
                Catalog_elem_val_010 = "塗佈白紙板",
                Catalog_elem_val_020 = "DM00",
                Catalog_elem_val_030 = "P",
                Catalog_elem_val_040 = "03000",
                Catalog_elem_val_050 = "284K446K",
                Catalog_elem_val_060 = "295.27",
                Catalog_elem_val_070 = "平版",
                Catalog_elem_val_080 = "N",
                Catalog_elem_val_090 = "單邊特規",
                Catalog_elem_val_100 = "L",
                Catalog_elem_val_110 = "無令打件",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "全塗灰銅卡",
                Catalog_elem_val_140 = "COATED D-B(G-B)",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });


            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "4.433.DM00",
                Category_name_inv = "YFY Inventory",
                Category_code_cost = "OP",
                Category_name_cost = "YFY Cost",
                Category_code_control = "",
                Category_name_control = "YFY Item Control",
                Item_number = "4DM00P03000297K476K",
                Inventory_item_id = 504271,
                Item_desc_eng = "COATED D-B(G-B)",
                Item_desc_sch = "全塗灰銅卡",
                Item_desc_tch = "全塗灰銅卡",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "Active",
                Item_type = "16",
                Catalog_elem_val_010 = "塗佈白紙板",
                Catalog_elem_val_020 = "DM00",
                Catalog_elem_val_030 = "P",
                Catalog_elem_val_040 = "03000",
                Catalog_elem_val_050 = "297K476K",
                Catalog_elem_val_060 = "304.35",
                Catalog_elem_val_070 = "平版",
                Catalog_elem_val_080 = "N",
                Catalog_elem_val_090 = "雙邊特規",
                Catalog_elem_val_100 = "L",
                Catalog_elem_val_110 = "無令打件",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "全塗灰銅卡",
                Catalog_elem_val_140 = "COATED D-B(G-B)",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "4.433.DM00",
                Category_name_inv = "YFY Inventory",
                Category_code_cost = "OP",
                Category_name_cost = "YFY Cost",
                Category_code_control = "",
                Category_name_control = "YFY Item Control",
                Item_number = "4A003A01000310K266K",
                Inventory_item_id = 514029,
                Item_desc_eng = "COATED D-B(G-B)",
                Item_desc_sch = "試抄紙品",
                Item_desc_tch = "試抄紙品",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "Active",
                Item_type = "16",
                Catalog_elem_val_010 = "試抄紙品",
                Catalog_elem_val_020 = "A003",
                Catalog_elem_val_030 = "A",
                Catalog_elem_val_040 = "01000",
                Catalog_elem_val_050 = "310K266K",
                Catalog_elem_val_060 = "274.27",
                Catalog_elem_val_070 = "平版",
                Catalog_elem_val_080 = "N",
                Catalog_elem_val_090 = "雙邊特規",
                Catalog_elem_val_100 = "L",
                Catalog_elem_val_110 = "令包",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "試抄紙品",
                Catalog_elem_val_140 = "COATED D-B(G-B)",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "4.433.DM00",
                Category_name_inv = "YFY Inventory",
                Category_code_cost = "OP",
                Category_name_cost = "YFY Cost",
                Category_code_control = "",
                Category_name_control = "YFY Item Control",
                Item_number = "4AB23P00699350K250K",
                Inventory_item_id = 514030,
                Item_desc_eng = "COATED D-B(G-B)",
                Item_desc_sch = "雪面銅版紙",
                Item_desc_tch = "雪面銅版紙",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "Active",
                Item_type = "16",
                Catalog_elem_val_010 = "雪面銅版紙",
                Catalog_elem_val_020 = "AB23",
                Catalog_elem_val_030 = "A",
                Catalog_elem_val_040 = "00699",
                Catalog_elem_val_050 = "350K250K",
                Catalog_elem_val_060 = "274.27",
                Catalog_elem_val_070 = "平版",
                Catalog_elem_val_080 = "N",
                Catalog_elem_val_090 = "雙邊特規",
                Catalog_elem_val_100 = "L",
                Catalog_elem_val_110 = "無令打件",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "雪面銅版紙",
                Catalog_elem_val_140 = "COATED D-B(G-B)",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4FHIZA03000787RL00",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒金典銅西",
                Item_desc_tch = "捲筒金典銅西",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "FHIZ",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "03000",
                Catalog_elem_val_050 = "787",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4FHIZA02500787RL00",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒金典銅西",
                Item_desc_tch = "捲筒金典銅西",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "FHIZ",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "02500",
                Catalog_elem_val_050 = "787",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4FHIZA02000787RL00",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒金典銅西",
                Item_desc_tch = "捲筒金典銅西",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "FHIZ",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "02000",
                Catalog_elem_val_050 = "787",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4AKMXA01000280KRL00",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒特級雙面銅版紙(M)",
                Item_desc_tch = "捲筒特級雙面銅版紙(M)",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "AKMXA",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "01000",
                Catalog_elem_val_050 = "280",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4AEMXA0100007110965",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒特級雙面銅版紙(M)",
                Item_desc_tch = "捲筒特級雙面銅版紙(M)",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "AEMXA",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "01000",
                Catalog_elem_val_050 = "7110965",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4AKMXA01200350KRL00",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒特級雙面銅版紙(M)",
                Item_desc_tch = "捲筒特級雙面銅版紙(M)",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "AKMXA",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "01200",
                Catalog_elem_val_050 = "350",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            model.Add(new PartNoModel
            {
                Organization_code = "FTY",
                Category_code_inv = "",
                Category_name_inv = "",
                Category_code_cost = "",
                Category_name_cost = "",
                Category_code_control = "",
                Category_name_control = "",
                Item_number = "4AEMXA0120008890635",
                Inventory_item_id = 30456,
                Item_desc_eng = "",
                Item_desc_sch = "捲筒特級雙面銅版紙(M)",
                Item_desc_tch = "捲筒特級雙面銅版紙(M)",
                Primary_uom_code = "KG",
                Secondary_uom_code = "RE",
                Inventory_item_status_code = "",
                Item_type = "",
                Catalog_elem_val_010 = "",
                Catalog_elem_val_020 = "AKMXA",
                Catalog_elem_val_030 = "",
                Catalog_elem_val_040 = "01200",
                Catalog_elem_val_050 = "8890635",
                Catalog_elem_val_060 = "",
                Catalog_elem_val_070 = "捲筒",
                Catalog_elem_val_080 = "",
                Catalog_elem_val_090 = "",
                Catalog_elem_val_100 = "",
                Catalog_elem_val_110 = "",
                Catalog_elem_val_120 = "",
                Catalog_elem_val_130 = "",
                Catalog_elem_val_140 = "",
                Created_by = "華紙管理員",
                Creation_Date = DateTime.Parse("2019-2-1 10:15:30"),
                Last_Updated_by = "華紙管理員",
                Last_Update_Date = DateTime.Parse("2019-2-1 10:15:30")
            });

            return model;

        }

        public IEnumerable<SelectListItem> GetItem_No()
        {
            List<ListItem> Catalog_elem_val_050 = new List<ListItem>();
            Catalog_elem_val_050.Add(new ListItem("全部", "*"));
            Catalog_elem_val_050.Add(new ListItem("214K512K", "214K512K"));
            Catalog_elem_val_050.Add(new ListItem("284K446K", "284K446K"));
            Catalog_elem_val_050.Add(new ListItem("297K476K", "297K476K"));
            Catalog_elem_val_050.Add(new ListItem("310K266K", "310K266K"));
            Catalog_elem_val_050.Add(new ListItem("350K250K", "350K250K"));
            Catalog_elem_val_050.Add(new ListItem("787", "787"));
            Catalog_elem_val_050.Add(new ListItem("280", "280"));
            Catalog_elem_val_050.Add(new ListItem("7110965", "7110965"));
            Catalog_elem_val_050.Add(new ListItem("350", "350"));
            Catalog_elem_val_050.Add(new ListItem("8890635", "8890635"));
            return Catalog_elem_val_050.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public IEnumerable<SelectListItem> GetTypePaper()
        {
            List<ListItem> Catalog_elem_val_020 = new List<ListItem>();
            Catalog_elem_val_020.Add(new ListItem("全部", "*"));
            Catalog_elem_val_020.Add(new ListItem("DM00", "DM00"));
            Catalog_elem_val_020.Add(new ListItem("FHIZ", "FHIZ"));
            Catalog_elem_val_020.Add(new ListItem("A003", "A003"));
            Catalog_elem_val_020.Add(new ListItem("AKMXA", "AKMXA"));
            Catalog_elem_val_020.Add(new ListItem("AKMXA", "AKMXA"));
            Catalog_elem_val_020.Add(new ListItem("AB23", "AB23"));
            return Catalog_elem_val_020.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public IEnumerable<SelectListItem> Get070()
        {
            List<ListItem> Catalog_elem_val_070 = new List<ListItem>();
            Catalog_elem_val_070.Add(new ListItem("全部", "*"));
            Catalog_elem_val_070.Add(new ListItem("平版", "平版"));
            Catalog_elem_val_070.Add(new ListItem("捲筒", "捲筒"));
            return Catalog_elem_val_070.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public IEnumerable<SelectListItem> GetOrganization_code()
        {
            List<ListItem> Organization_code = new List<ListItem>();
            Organization_code.Add(new ListItem("全部", "*"));
            Organization_code.Add(new ListItem("FTY", "FTY"));
            return Organization_code.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public List<PartNoModel> search(string Catalog_elem_val_050, string Catalog_elem_val_020, string Catalog_elem_val_070,string Organization_code)
        {
            //ResultModel result = new ResultModel(true, "搜尋成功");
            try
            {

                var query = model.Where(
                  x =>
                  (Catalog_elem_val_050 == "*" || x.Catalog_elem_val_050 != null && x.Catalog_elem_val_050.ToLower() == Catalog_elem_val_050.ToLower()) &&
                  (Catalog_elem_val_020 == "*" || x.Catalog_elem_val_020 != null && x.Catalog_elem_val_020.ToLower() == Catalog_elem_val_020.ToLower()) &&
                  (Catalog_elem_val_070 == "*" || x.Catalog_elem_val_070 != null && x.Catalog_elem_val_070.ToLower() == Catalog_elem_val_070.ToLower()) &&
                  (Organization_code == "*" || x.Organization_code != null && x.Organization_code.ToLower() == Organization_code.ToLower())
                  ).ToList();

                //dtData = query;
                return query;
            }
            catch (Exception e)
            {
                //result.Msg = e.Message;
                //result.Success = false;
                return new List<PartNoModel>();
            }
            //return result;
        }


        public static IOrderedEnumerable<PartNoModel> Order(List<Order> orders, IEnumerable<PartNoModel> models)
        {
            IOrderedEnumerable<PartNoModel> orderedModel = null;
            if (orders.Count() > 0)
            {
                orderedModel = OrderBy(orders[0].Column, orders[0].Dir, models);
            }

            for (int i = 1; i < orders.Count(); i++)
            {
                orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
            }
            return orderedModel;
        }

        private static IOrderedEnumerable<PartNoModel> OrderBy(int column, string dir, IEnumerable<PartNoModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_inv) : models.OrderBy(x => x.Category_code_inv);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_cost) : models.OrderBy(x => x.Category_code_cost);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_control) : models.OrderBy(x => x.Category_code_control);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_tch) : models.OrderBy(x => x.Item_desc_tch);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Primary_uom_code) : models.OrderBy(x => x.Primary_uom_code);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Secondary_uom_code) : models.OrderBy(x => x.Secondary_uom_code);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_number) : models.OrderBy(x => x.Item_number);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Inventory_item_status_code) : models.OrderBy(x => x.Inventory_item_status_code);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_type) : models.OrderBy(x => x.Item_type);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_010) : models.OrderBy(x => x.Catalog_elem_val_010);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_020) : models.OrderBy(x => x.Catalog_elem_val_020);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_030) : models.OrderBy(x => x.Catalog_elem_val_030);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_040) : models.OrderBy(x => x.Catalog_elem_val_040);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_050) : models.OrderBy(x => x.Catalog_elem_val_050);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_060) : models.OrderBy(x => x.Catalog_elem_val_060);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_070) : models.OrderBy(x => x.Catalog_elem_val_070);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_100) : models.OrderBy(x => x.Catalog_elem_val_100);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_110) : models.OrderBy(x => x.Catalog_elem_val_110);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_by) : models.OrderBy(x => x.Last_Updated_by);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Update_Date) : models.OrderBy(x => x.Last_Update_Date);
   
            }
        }

        private static IOrderedEnumerable<PartNoModel> ThenBy(int column, string dir, IOrderedEnumerable<PartNoModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_inv) : models.OrderBy(x => x.Category_code_inv);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_cost) : models.OrderBy(x => x.Category_code_cost);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_control) : models.OrderBy(x => x.Category_code_control);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_tch) : models.OrderBy(x => x.Item_desc_tch);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Primary_uom_code) : models.OrderBy(x => x.Primary_uom_code);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Secondary_uom_code) : models.OrderBy(x => x.Secondary_uom_code);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_number) : models.OrderBy(x => x.Item_number);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Inventory_item_status_code) : models.OrderBy(x => x.Inventory_item_status_code);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_type) : models.OrderBy(x => x.Item_type);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_010) : models.OrderBy(x => x.Catalog_elem_val_010);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_020) : models.OrderBy(x => x.Catalog_elem_val_020);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_030) : models.OrderBy(x => x.Catalog_elem_val_030);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_040) : models.OrderBy(x => x.Catalog_elem_val_040);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_050) : models.OrderBy(x => x.Catalog_elem_val_050);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_060) : models.OrderBy(x => x.Catalog_elem_val_060);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_070) : models.OrderBy(x => x.Catalog_elem_val_070);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_100) : models.OrderBy(x => x.Catalog_elem_val_100);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_110) : models.OrderBy(x => x.Catalog_elem_val_110);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_by) : models.OrderBy(x => x.Last_Updated_by);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Update_Date) : models.OrderBy(x => x.Last_Update_Date);

            }
        }


        public static List<PartNoModel> Search(DataTableAjaxPostViewModel data, List<PartNoModel> model)
        {
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (
                    !string.IsNullOrEmpty(p.Organization_code) && p.Organization_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Category_code_inv.ToString()) && p.Category_code_inv.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Category_code_cost) && p.Category_code_cost.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Category_code_control) && p.Category_code_control.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_desc_tch) && p.Item_desc_tch.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Primary_uom_code) && p.Primary_uom_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Secondary_uom_code) && p.Secondary_uom_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_number) && p.Item_number.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Inventory_item_status_code) && p.Inventory_item_status_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_type) && p.Item_type.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_010) && p.Catalog_elem_val_010.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_020) && p.Catalog_elem_val_020.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_030) && p.Catalog_elem_val_030.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_040) && p.Catalog_elem_val_040.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_050) && p.Catalog_elem_val_050.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_060) && p.Catalog_elem_val_060.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_070) && p.Catalog_elem_val_070.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_100) && p.Catalog_elem_val_100.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_110) && p.Catalog_elem_val_110.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_Updated_by.ToString()) && p.Last_Updated_by.ToString().ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }





    }
}