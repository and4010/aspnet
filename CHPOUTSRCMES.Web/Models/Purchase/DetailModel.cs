using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Purchase
{
    public class DetailModel
    {

        public class RollModel
        {
            public long SubId { set; get; }

            public long Id { get; set; }

            [Display(Name = "倉庫")]
            public string Subinventory { get; set; }

            [Display(Name = "儲位")]
            public string Locator { get; set; }

            [Display(Name ="料號")]
            public string Item_No { get; set; }

            [Display(Name = "紙別")]
            public string PaperType { get; set; }

            [Display(Name = "基重")]
            public string BaseWeight { get; set; }

            [Display(Name = "規格")]
            public string Specification { get; set; }

            [Display(Name = "捲數/棧板數")]
            public decimal RollReamQty { get; set; }

            [Display(Name = "交易數量")]
            public decimal TransactionQuantity { get; set; }

            [Display(Name = "交易單位")]
            public string TransactionUom { get; set; }

            [Display(Name = "主要數量")]
            public decimal PrimanyQuantity { set; get; }

            [Display(Name = "主要單位")]
            public string PrimaryUom { set; get; }

        }


        public class RollDetailModel
        {
            public long SubId { set; get; }

            public long Id { get; set; }

            [Display(Name = "倉別")]
            public string Subinventory { get; set; }

            [Display(Name = "儲位")]
            public string Locator { set; get; }

            [Display(Name = "條碼號")]
            public string Barcode { get; set; }

            [Display(Name = "料號")]
            public string Item_No { get; set; }

            [Display(Name = "紙別")]
            public string PaperType { get; set; }

            [Display(Name = "基重")]
            public string BaseWeight { get; set; }

            [Display(Name = "規格")]
            public string Specification { get; set; }

            [Display(Name = "理論重(KG)")]
            public string TheoreticalWeight { get; set; }

            [Display(Name = "交易數量")]
            public decimal TransactionQuantity { get; set; }

            [Display(Name = "交易單位")]
            public string TransactionUom { get; set; }

            [Display(Name = "主要數量")]
            public decimal PrimanyQuantity { set; get; }

            [Display(Name = "主要單位")]
            public string PrimaryUom { set; get; }

            [Display(Name = "捲號")]
            public string LotNumber { set; get; }

            [Display(Name = "入庫狀態")]
            public string Status { get; set; }

            [Display(Name = "原因")]
            public string Reason { get; set; }

            [Display(Name = "備註")]
            public string Remark { get; set; }

            [Display(Name = "更新時間")]
            public DateTime? CreationDate { get; set; }

            [Display(Name = "更新人員")]
            public string CreatedUserName { get; set; }
        }


        public class FlatModel
        {
            public long SubId { set; get; }

            public long Id { get; set; }

            [Display(Name = "倉別")]
            public string Subinventory { get; set; }

            [Display(Name = "儲位")]
            public string Locator { get; set; }

            [Display(Name = "料號")]
            public string Item_No { get; set; }

            [Display(Name = "令重")]
            public string ReamWeight { get; set; }

            [Display(Name = "捲數/棧板數")]
            public decimal RollReamQty { get; set; }

            [Display(Name = "包裝方式")]
            public string PackingType { get; set; }

            [Display(Name = "每件令數")]
            public decimal Pieces_Qty { get; set; }

            [Display(Name = "交易數量")]
            public decimal TransactionQuantity { set; get; }

            [Display(Name = "交易單位")]
            public string TransactionUom { set; get; }

            [Display(Name = "總令數")]
            public decimal TtlRollReam { get; set; }

            [Display(Name = "總令數單位")]
            public string TtlRollReamUom { get; set; }

            [Display(Name = "總公斤")]
            public decimal DeliveryQty { get; set; }

            [Display(Name = "總公斤單位")]
            public string DeliveryUom { get; set; }

        }

        public class FlatDetailModel
        {
            public long SubId { set; get; }

            public long Id { get; set; }

            [Display(Name = "倉別")]
            public string Subinventory { get; set; }

            [Display(Name = "儲位")]
            public string Locator { get; set; }

            [Display(Name = "條碼號")]
            public string Barcode { get; set; }

            [Display(Name = "料號")]
            public string Item_No { get; set; }

            [Display(Name = "令重")]
            public string ReamWeight { get; set; }

            [Display(Name = "包裝方式")]
            public string PackingType { get; set; }

            [Display(Name = "每件令數")]//ROLL_REAM_WT
            public decimal Pieces_Qty { get; set; }

            [Display(Name = "數量(頓)")]
            public decimal Qty { get; set; }

            [Display(Name = "入庫狀態")]
            public string Status { get; set; }

            [Display(Name = "原因")]
            public string Reason { get; set; }

            [Display(Name = "備註")]
            public string Remark { get; set; }

            [Display(Name = "更新時間")]
            public DateTime? CreationDate { get; set; }

            [Display(Name = "更新人員")]
            public string CreatedUserName { get; set; }
        }

        public class ReasonModel
        {
            public Int64 Id { set; get; }

            public string ReasonMsg{set;get;}
        }


        public class PhotoModel
        {
            /// <summary>
            /// 加這個，PhotoModel new出來時，存取PhotoFileNames才不會發生NULL例外
            /// </summary>
            private List<string> _PhotoFileNames = new List<string>();
            /// <summary>
            /// 多張照片的檔案名稱
            /// </summary>
            public List<string> PhotoFileNames
            {
                get
                {
                    return this._PhotoFileNames;
                }
                set
                {
                    this._PhotoFileNames = value;
                }
            }
        }

    }
}