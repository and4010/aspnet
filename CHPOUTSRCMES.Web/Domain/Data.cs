using CHPOUTSRCMES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Domain
{
    public class Data
    {
        public IEnumerable<Navbar> navbarItems(string role)
        {

            var menu = new List<Navbar>();
            //一般 (第一層選單)
            menu.Add(new Navbar { Id = 1, nameOption = "入庫", controller = "Purchase", action = "Index", imageClass = "fa fa-calendar-plus-o", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 2, nameOption = "加工", controller = "Process", action = "Index", imageClass = "fa fa-puzzle-piece", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 3, nameOption = "出貨", controller = "Delivery", action = "Index", imageClass = "fa fa-calendar-minus-o", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 4, nameOption = "查詢", imageClass = "fa fa-table", status = true, isParent = true, parentId = 0 });
            menu.Add(new Navbar { Id = 5, nameOption = "基本資料", imageClass = "fa fa-briefcase", status = true, isParent = true, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "庫存異動", imageClass = "fa fa-pie-chart", status = true, isParent = true, parentId = 0 });
            //menu.Add(new Navbar { Id = 7, nameOption = "報表", imageClass = "fa fa-table", status = false, isParent = true, parentId = 0 });

            //查詢 (第二層選單)
            menu.Add(new Navbar { Id = 401, nameOption = "庫存查詢", controller = "Stock", action = "Query", status = true, isParent = false, parentId = 4 });
            menu.Add(new Navbar { Id = 402, nameOption = "庫存異動記錄", controller = "Stock", action = "Transaction", status = true, isParent = false, parentId = 4 });
            if (role.CompareTo("系統管理員") == 0 || role.CompareTo("華紙使用者") == 0)
                menu.Add(new Navbar { Id = 403, nameOption = "SOA傳輸記錄", controller = "Soa", action = "Index", status = true, isParent = false, parentId = 4 });


            //基本資料 (第二層選單)
            if(role.CompareTo("系統管理員") == 0 || role.CompareTo("華紙使用者") == 0)
                menu.Add(new Navbar { Id = 501, nameOption = "使用者", controller = "Account", action = "Index", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 502, nameOption = "貨故原因", controller = "Reason", action = "Index", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 503, nameOption = "料號", controller = "PartNo", action = "Index", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 504, nameOption = "組織倉庫", controller = "OrgSubinventory", action = "Index", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 505, nameOption = "板令對照", controller = "Yszmpckq", action = "Index", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 506, nameOption = "機台", controller = "MachinePaperType", action = "Index", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 507, nameOption = "餘切規格", controller = "OspRelated", action = "Index", status = true, isParent = false, parentId = 5 });

            //庫存異動 (第二層選單)
            menu.Add(new Navbar { Id = 601, nameOption = "庫存移轉", controller = "StockTransaction", action = "Index", status = true, isParent = false, parentId = 6 });
            menu.Add(new Navbar { Id = 601, nameOption = "雜項異動", controller = "Miscellaneous", action = "Index", status = true, isParent = false, parentId = 6 });
            menu.Add(new Navbar { Id = 601, nameOption = "盤點", controller = "StockInventory", action = "Index", status = true, isParent = false, parentId = 6 });
            menu.Add(new Navbar { Id = 601, nameOption = "存貨報廢", controller = "Obsolete", action = "Index", status = true, isParent = false, parentId = 6 });

            //報表 (第二層選單)
            //menu.Add(new Navbar { Id = 701, nameOption = "工單得率報表", controller = "Home", action = "Icons", status = false, isParent = false, parentId = 7 });
            //menu.Add(new Navbar { Id = 702, nameOption = "裁切加工報表", controller = "Home", action = "Icons", status = false, isParent = false, parentId = 7 });
            return menu.ToList();
        }
    }
}