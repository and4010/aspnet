using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class XXIF_CHP_P219_OSP_BATCH_ST
    {
        public string PROCESS_CODE { set; get; }

        public string SERVER_CODE { set; get; }

        public string BATCH_ID { set; get; }

        public long BATCH_LINE_ID { set; get; }

        public string STATUS_CODE { set; get; }

        public string ERROR_MSG { set; get; }

        public long PE_BATCH_ID { set; get; }

        public string BATCH_NO { set; get; }

        public string BATCH_TYPE { set; get; }

        public long BATCH_STATUS { set; get; }

        public string BATCH_STATUS_DESC { set; get; }

        public long ORG_ID { set; get; }

        public string ORG_NAME { set; get; }

        public long ORGANIZATION_ID { set; get; }

        public string ORGANIZATION_CODE { set; get; }

        public DateTime DUE_DATE { set; get; }

        public DateTime PLAN_START_DATE { set; get; }

        public DateTime PLAN_CMPLT_DATE { set; get; }

        public long PE_CREATED_BY { set; get; }

        public DateTime PE_CREATION_DATE { set; get; }

        public long PE_LAST_UPDATED_BY { set; get; }

        public DateTime PE_LAST_UPDATE_DATE { set; get; }

        public string LINE_TYPE { set; get; }

        public string LINE_NO { set; get; }

        public long INVENTORY_ITEM_ID { set; get; }

        public string INVENTORY_ITEM_NUMBER { set; get; }

        public string BASIC_WEIGHT { set; get; }

        public string SPECIFICATION { set; get; }

        public string GRAIN_DIRECTION { set; get; }

        public string ORDER_WEIGHT { set; get; }

        public string REAM_WT { set; get; }

        public string PACKING_TYPE { set; get; }

        public decimal PLAN_QTY { set; get; }
        
        public decimal WIP_PLAN_QTY { set; get; }

        public string DTL_UOM { set; get; }

        public long? HEADER_ID { set; get; }

        public long? ORDER_NUMBER { set; get; }

        public long? LINE { set; get; }

        public string LINE_NUMBER { set; get; }

        public long? CUSTOMER_UD { set; get; }

        public string CUSTOMER_NUMBER { set; get; }

        public string CUSTOMER_NAME { set; get; }

        public long? PR_NUMBER { set; get; }

        public long? PR_LINE_NUMBER { set; get; }

        public long? REQUISITION_LINE_ID { set; get; }

        public long? PO_NUMBER { set; get; }

        public long? PO_LINE_NUMBER { set; get; }

        public long? PO_LINE_ID { set; get; }

        public decimal? PO_UNIT_PRICE { set; get; }

        public long? PO_REVISION_NUM { set; get; }

        public string PO_STATUS { set; get; }

        public string PO_VENDOR_NUM { set; get; }

        public string OSP_REMARK { set; get; }

        public string SUBINVENTORY { set; get; }

        public long? LOCATOR_ID { set; get; }

        public string LOCATOR_CODE { set; get; }

        public string RESERVATION_UOM_CODE { set; get; }

        public decimal? RESERVATION_QUANTITY { set; get; }

        public long LINE_CREATED_BY { set; get; }

        public DateTime LINE_CREATION_DATE { set; get; }

        public long LINE_LAST_UPDATED_BY { set; get; }

        public DateTime LINE_LAST_UPDATE_DATE { set; get; }

        public decimal TRANSACTION_QUANTITY { set; get; }

        public string TRANSACTION_UOM { set; get; }

        public decimal PRIMARY_QUANTITY { set; get; }

        public string PRIMARY_UOM { set; get; }

        public decimal? SECONDARY_QUANTITY { set; get; }

        public string SECONDARY_UOM { set; get; }

        public string ATTRIBUTE1 { set; get; }

        public string ATTRIBUTE2 { set; get; }

        public string ATTRIBUTE3 { set; get; }

        public string ATTRIBUTE4 { set; get; }

        public string ATTRIBUTE5 { set; get; }

        public string ATTRIBUTE6 { set; get; }

        public string ATTRIBUTE7 { set; get; }

        public string ATTRIBUTE8 { set; get; }

        public string ATTRIBUTE9 { set; get; }

        public string ATTRIBUTE10 { set; get; }

        public string ATTRIBUTE11 { set; get; }

        public string ATTRIBUTE12 { set; get; }

        public string ATTRIBUTE13 { set; get; }

        public string ATTRIBUTE14 { set; get; }

        public string ATTRIBUTE15 { set; get; }


        public long? REQUEST_ID { set; get; }
        

        public long CREATED_BY { set; get; }

        public DateTime CREATION_DATE { set; get; }

        public long LAST_UPDATED_BY { set; get; }

        public DateTime LAST_UPDATE_DATE { set; get; }

        public long LAST_UPDATE_LOGIN { set; get; }
    }

}
