using CHPOUTSRCMES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Domain
{
    public class Data
    {
        public IEnumerable<Navbar> navbarItems()
        {
            var menu = new List<Navbar>();
            menu.Add(new Navbar { Id = 1, nameOption = "首頁", controller = "Home", action = "Index", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 2, nameOption = "入庫", controller = "Purchase", action = "Index", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 3, nameOption = "加工", controller = "Process", action = "Index", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 4, nameOption = "出貨", controller = "Delivery", action = "Index", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 4, nameOption = "盤點", controller = "Inventory", action = "Index", status = true, isParent = false, parentId = 0 });
            //menu.Add(new Navbar { Id = 5, nameOption = "調撥", status = false, isParent = true, parentId = 0 });
            //menu.Add(new Navbar { Id = 6, nameOption = "調撥出庫", controller = "Home", action = "Forms", status = false, isParent = false, parentId = 5 });
            //menu.Add(new Navbar { Id = 7, nameOption = "調撥入庫", controller = "Home", action = "Forms", status = false, isParent = false, parentId = 5 });

            //menu.Add(new Navbar { Id = 8, nameOption = "庫存", status = false, isParent = true, parentId = 0 });
            //menu.Add(new Navbar { Id = 9, nameOption = "查詢", controller = "Home", action = "Buttons", status = false, isParent = false, parentId = 8 });
            //menu.Add(new Navbar { Id = 10, nameOption = "盤點", controller = "Home", action = "Notifications", status = false, isParent = false, parentId = 8 });
            //menu.Add(new Navbar { Id = 11, nameOption = "異動記錄", controller = "Home", action = "Typography", status = false, isParent = false, parentId = 8 });

            //menu.Add(new Navbar { Id = 12, nameOption = "報表", status = false, isParent = true, parentId = 0 });
            //menu.Add(new Navbar { Id = 13, nameOption = "工單得率", controller = "Home", action = "Icons", status = false, isParent = false, parentId = 12 });

            menu.Add(new Navbar { Id = 14, nameOption = "基本資料", status = false, isParent = true, parentId = 0});
            menu.Add(new Navbar { Id = 15, nameOption = "使用者", controller = "Account", action = "Index", status = true, isParent = false, parentId = 14 });
            //menu.Add(new Navbar { Id = 16, nameOption = "公司", status = false, isParent = false, parentId = 14 });
            menu.Add(new Navbar { Id = 16, nameOption = "貨故原因", controller = "Reason", action = "Index", status = true, isParent = false, parentId = 14 });
            menu.Add(new Navbar { Id = 16, nameOption = "料號", controller = "PartNo", action = "Index", status = true, isParent = false, parentId = 14 });
            menu.Add(new Navbar { Id = 17, nameOption = "組織倉庫", controller = "OrgSubinventory", action = "Index", status = true, isParent = false, parentId = 14 });
            menu.Add(new Navbar { Id = 17, nameOption = "板令對照", controller = "Yszmpckq", action = "Index", status = true, isParent = false, parentId = 14 });
            menu.Add(new Navbar { Id = 18, nameOption = "機台", controller = "MachinePaperType", action = "Index", status = true, isParent = false, parentId = 14 });
            menu.Add(new Navbar { Id = 19, nameOption = "餘切規格", controller = "OspRelated", action = "Index", status = true, isParent = false, parentId = 14 });

            menu.Add(new Navbar { Id = 20, nameOption = "庫存異動", status = false, isParent = true, parentId = 0 });
            menu.Add(new Navbar { Id = 20, nameOption = "庫存移轉", controller = "StockTransaction", action = "Index", status = true, isParent = false, parentId = 20 });
            menu.Add(new Navbar { Id = 20, nameOption = "雜項異動", controller = "Miscellaneous", action = "Index", status = true, isParent = false, parentId = 20 });
            menu.Add(new Navbar { Id = 20, nameOption = "盤點", controller = "StockInventory", action = "Index", status = true, isParent = false, parentId = 20 });
            menu.Add(new Navbar { Id = 20, nameOption = "存貨報廢", controller = "Obsolete", action = "Index", status = true, isParent = false, parentId = 20 });

            //menu.Add(new Navbar { Id = 20, nameOption = "傳輸記錄", controller = "Home", action = "Blank", status = false, isParent = false, parentId = 0 });

            menu.Add(new Navbar { Id = 21, nameOption = "登入", controller = "Home", action = "Login", status = true, isParent = false, parentId = 0 });

            menu.Add(new Navbar { Id = 22, nameOption = "測試資料", controller = "Home", action = "TestData", status = true, isParent = false, parentId = 0 });

            //menu.Add(new Navbar { Id = 19, nameOption = "Purchase Page", controller = "PartNo", action = "Index", status = true, isParent = false, parentId = 0 });
            return menu.ToList();
        }
    }
}