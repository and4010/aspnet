using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using NLog.Time;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Util
{
    public class ExcelImport
    {

        public ResultModel PaperRollDetail(HttpPostedFileBase file, ref List<DetailModel.RollDetailModel> PaperRollDetail, long CtrHeaderId, ref ResultModel result,
            string createby, string userName)
        {
            PurchaseViewModel purchaseView = new PurchaseViewModel();
            var RollHeader = purchaseView.GetRollHeader(CtrHeaderId);
            IWorkbook workbook = null;
            if (file.FileName.Substring(file.FileName.LastIndexOf(".")).Equals(".xls"))
            {
                //xls
                workbook = new HSSFWorkbook(file.InputStream);
            }
            else
            {
                //xlsx
                workbook = new XSSFWorkbook(file.InputStream);
            }

            ISheet sheet = workbook.GetSheetAt(0);
            IRow header = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = header.LastCellNum;


            try
            {
                ICell Id_cell = null;
                ICell Subinventory_cell = null;
                ICell Locator_cell = null;
                ICell Barocde_cell = null;
                ICell PartNo_cell = null;
                ICell PaperType_cell = null;
                ICell BaseWeight_cell = null;
                ICell Specification_cell = null;
                ICell TheoreticalWeight_cell = null;
                ICell TransactionQuantity_cell = null;
                ICell TransactionUom_cell = null;
                ICell PrimanyQuantity_cell = null;
                ICell PrimaryUom_cell = null;
                ICell LotNumber_cell = null;
                ICell Status_cell = null;
                ICell Remark_cell = null;

                PartNo_cell = ExcelUtil.FindCell("料號", sheet);
                if (PartNo_cell == null)
                {
                    throw new Exception("找不到料號欄位");
                }

                PaperType_cell = ExcelUtil.FindCell("紙別", sheet);
                if (PaperType_cell == null)
                {
                    throw new Exception("找不到紙別欄位");
                }

                BaseWeight_cell = ExcelUtil.FindCell("基重", sheet);
                if (BaseWeight_cell == null)
                {
                    throw new Exception("找不到基重欄位");
                }

                Specification_cell = ExcelUtil.FindCell("規格", sheet);
                if (Specification_cell == null)
                {
                    throw new Exception("找不到規格欄位");
                }

                TheoreticalWeight_cell = ExcelUtil.FindCell("重量", sheet);
                if (TheoreticalWeight_cell == null)
                {
                    throw new Exception("找不到重量欄位");
                }

                PrimaryUom_cell = ExcelUtil.FindCell("單位", sheet);
                if (PrimaryUom_cell == null)
                {
                    throw new Exception("找不到單位欄位");
                }

                LotNumber_cell = ExcelUtil.FindCell("捲號", sheet);
                if (LotNumber_cell == null)
                {
                    throw new Exception("找不到捲號欄位");
                }

                int rowCount = sheet.LastRowNum;
                for (int j = 0; j < RollHeader.Count; j++)
                {
                    for (int i = PartNo_cell.RowIndex + 1; i <= rowCount; i++)
                    {

                        try
                        {

                            var excelPartNo = ExcelUtil.GetStringCellValue(i, PartNo_cell.ColumnIndex, sheet).Trim();
                            if (RollHeader[j].Item_No.Equals(excelPartNo))
                            {
                                //if(RollHeader[j].PrimanyQuantity == decimal.Parse(ExcelUtil.GetStringCellValue(i, TheoreticalWeight_cell.ColumnIndex, sheet).Trim()))
                                //{
                                var model = new DetailModel.RollDetailModel();
                                model.Id = i;
                                model.Barcode = "";
                                model.Item_No = ExcelUtil.GetStringCellValue(i, PartNo_cell.ColumnIndex, sheet).Trim();
                                model.PaperType = ExcelUtil.GetStringCellValue(i, PaperType_cell.ColumnIndex, sheet).Trim();
                                model.BaseWeight = ExcelUtil.GetStringCellValue(i, BaseWeight_cell.ColumnIndex, sheet).Trim();
                                model.Specification = ExcelUtil.GetStringCellValue(i, Specification_cell.ColumnIndex, sheet).Trim();
                                model.TheoreticalWeight = ExcelUtil.GetStringCellValue(i, TheoreticalWeight_cell.ColumnIndex, sheet).Trim();
                                model.TransactionQuantity = ExcelUtil.GetDecimalCellValue(i, TheoreticalWeight_cell.ColumnIndex, sheet);
                                model.TransactionUom = RollHeader[j].TransactionUom;
                                model.PrimanyQuantity = ExcelUtil.GetDecimalCellValue(i, TheoreticalWeight_cell.ColumnIndex, sheet);
                                model.PrimaryUom = ExcelUtil.GetStringCellValue(i, PrimaryUom_cell.ColumnIndex, sheet).Trim();
                                model.LotNumber = ExcelUtil.GetStringCellValue(i, LotNumber_cell.ColumnIndex, sheet).Trim();
                                model.Status = "待入庫";
                                //model.Remark = ExcelUtil.GetCellString(i, Remark_cell.ColumnIndex, sheet).Trim();
                                model.Subinventory = RollHeader[j].Subinventory;
                                model.Locator = RollHeader[j].Locator;
                                PaperRollDetail.Add(model);
                                //}
                                //else
                                //{
                                //    result.Msg = "數量不正確";
                                //    result.Success = true;
                                //    break;
                                //}

                            }

                        }
                        catch (Exception e)
                        {
                            result.Success = false;
                            result.Msg = e.Message;
                        }

                    }
                }

            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Success = false;
                return result;
            }

            var CheckQty = false;
            for (int j = 0; j < RollHeader.Count; j++)
            {
                var totle = 0M;
                for (int i = 0; i < PaperRollDetail.Count; i++)
                {
                    if (RollHeader[j].Item_No == PaperRollDetail[i].Item_No)
                    {
                        totle += PaperRollDetail[i].PrimanyQuantity;
                    }
                        
                }
                if(RollHeader[j].PrimanyQuantity == totle)
                {
                    CheckQty = true;
                }
                else
                {
                    CheckQty = false;
                    break;
                }
            }


            if (CheckQty)
            {
                result = purchaseView.ImportPaperRollPickT(CtrHeaderId, PaperRollDetail, createby, userName);
            }
            else
            {
                result.Msg = "匯入資料與表頭資料不正確";
                result.Success = false;
            }

            workbook = null;
            sheet = null;

            return result;
        }






        public ResultModel TransferPaperRoll(HttpPostedFileBase file, ref List<StockTransferBarcodeDT> stockTransferBarcodeDTs, ref ResultModel result, string ddlInSubinventory, string ddlInLocator)
        {
            IWorkbook workbook = null;
            DataTable dt = new DataTable();
            if (file.FileName.Substring(file.FileName.LastIndexOf(".")).Equals(".xls"))
            {
                //xls
                workbook = new HSSFWorkbook(file.InputStream);
            }
            else
            {
                //xlsx
                workbook = new XSSFWorkbook(file.InputStream);
            }

            ISheet sheet = workbook.GetSheetAt(0);
            IRow header = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = header.LastCellNum;


            try
            {
                ICell Id_cell = null;
                ICell Subinventory_cell = null;
                ICell Locator_cell = null;
                ICell Barocde_cell = null;
                ICell ItemNo_cell = null;
                ICell PaperType_cell = null;
                ICell BaseWeight_cell = null;
                ICell Specification_cell = null;
                ICell TheoreticalWeight_cell = null;
                ICell TransactionQuantity_cell = null;
                ICell TransactionUom_cell = null;
                ICell PrimanyQuantity_cell = null;
                ICell PrimaryUom_cell = null;
                ICell LotNumber_cell = null;
                ICell Status_cell = null;
                ICell Remark_cell = null;


                ItemNo_cell = ExcelUtil.FindCell("料號", sheet);
                if (ItemNo_cell == null)
                {
                    throw new Exception("找不到料號欄位");
                }

                PaperType_cell = ExcelUtil.FindCell("紙別", sheet);
                if (PaperType_cell == null)
                {
                    throw new Exception("找不到紙別代碼欄位");
                }

                BaseWeight_cell = ExcelUtil.FindCell("基重", sheet);
                if (BaseWeight_cell == null)
                {
                    throw new Exception("找不到基重欄位");
                }

                Specification_cell = ExcelUtil.FindCell("規格", sheet);
                if (Specification_cell == null)
                {
                    throw new Exception("找不到規格欄位");
                }

                TheoreticalWeight_cell = ExcelUtil.FindCell("重量", sheet);
                if (TheoreticalWeight_cell == null)
                {
                    throw new Exception("找不到重量欄位");
                }



                PrimaryUom_cell = ExcelUtil.FindCell("單位", sheet);
                if (PrimaryUom_cell == null)
                {
                    throw new Exception("找不到單位欄位");
                }

                LotNumber_cell = ExcelUtil.FindCell("捲號", sheet);
                if (LotNumber_cell == null)
                {
                    throw new Exception("找不到捲號欄位");
                }



                int rowCount = sheet.LastRowNum;
                for (int i = ItemNo_cell.RowIndex + 1; i <= rowCount; i++)
                {

                    try
                    {

                        var excelPartNo = ExcelUtil.GetStringCellValue(i, ItemNo_cell.ColumnIndex, sheet).Trim();
                        var model = new StockTransferBarcodeDT();
                        model.ID = i;
                        //model.BARCODE = "B200619000" + (i).ToString();
                        model.ITEM_NUMBER = ExcelUtil.GetStringCellValue(i, ItemNo_cell.ColumnIndex, sheet).Trim();
                        model.PAPERTYPE = ExcelUtil.GetStringCellValue(i, PaperType_cell.ColumnIndex, sheet).Trim();
                        model.Base_Weight = ExcelUtil.GetStringCellValue(i, BaseWeight_cell.ColumnIndex, sheet).Trim();
                        model.Specification = ExcelUtil.GetStringCellValue(i, Specification_cell.ColumnIndex, sheet).Trim();
                        model.PRIMARY_QUANTITY = ExcelUtil.GetDecimalCellValue(i, TheoreticalWeight_cell.ColumnIndex, sheet);
                        model.PRIMARY_UOM = ExcelUtil.GetStringCellValue(i, PrimaryUom_cell.ColumnIndex, sheet).Trim();
                        model.LOT_NUMBER = ExcelUtil.GetStringCellValue(i, LotNumber_cell.ColumnIndex, sheet).Trim();
                        //model.Status = "待入庫";
                        model.Subinventory = ddlInSubinventory;
                        model.Locator = ddlInLocator == "請選擇" ? "" : ddlInLocator;
                        //model.Remark = ExcelUtil.GetCellString(i, Remark_cell.ColumnIndex, sheet).Trim();
                        //model.Lo = RollHeader[j].Locator;
                        stockTransferBarcodeDTs.Add(model);

                    }
                    catch (Exception e)
                    {
                        result.Success = false;
                        result.Msg = e.Message;
                    }

                }

            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Success = false;
                return result;
            }

            if (stockTransferBarcodeDTs.Count != 0)
            {
                result.Msg = "成功";
                result.Success = true;
            }
            else
            {
                result.Msg = "資料錯誤";
                result.Success = false;
            }



            workbook = null;
            sheet = null;

            return result;
        }



        public ResultModel TransferFlat(HttpPostedFileBase file, ref List<StockTransferDT> stockTransferDTs, ref ResultModel result, string ddlInSubinventory, string ddlInLocator)
        {

            IWorkbook workbook;

            if (file.FileName.Substring(file.FileName.LastIndexOf(".")).Equals(".xls"))
            {
                //xls
                workbook = new HSSFWorkbook(file.InputStream);
            }
            else
            {
                //xlsx
                workbook = new XSSFWorkbook(file.InputStream);
            }

            ISheet sheet = workbook.GetSheetAt(0);
            IRow header = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = header.LastCellNum;


            try
            {
                ICell Id_cell = null;
                ICell Subinventory_cell = null;
                ICell Locator_cell = null;
                ICell Barocde_cell = null;
                ICell ItemNo_cell = null;
                ICell ReamWeight_cell = null;
                ICell PackingType_cell = null;
                ICell EveyReam_cell = null;
                ICell TotalReam_cell = null;
                ICell Qty_cell = null;
                ICell Status_cell = null;
                ICell Remark_cell = null;

                ItemNo_cell = ExcelUtil.FindCell("料號", sheet);
                if (ItemNo_cell == null)
                {
                    throw new Exception("找不到料號欄位");
                }

                ReamWeight_cell = ExcelUtil.FindCell("棧板數", sheet);
                if (ReamWeight_cell == null)
                {
                    throw new Exception("找不到棧板數欄位");
                }

                PackingType_cell = ExcelUtil.FindCell("包裝方式", sheet);
                if (PackingType_cell == null)
                {
                    throw new Exception("找不到包裝方式欄位");
                }

                EveyReam_cell = ExcelUtil.FindCell("每件令數", sheet);
                if (EveyReam_cell == null)
                {
                    throw new Exception("找不到每件令數欄位");
                }

                TotalReam_cell = ExcelUtil.FindCell("總令數", sheet);
                if (TotalReam_cell == null)
                {
                    throw new Exception("找不到總令數欄位");
                }

                Qty_cell = ExcelUtil.FindCell("數量(噸)", sheet);
                if (Qty_cell == null)
                {
                    throw new Exception("找不到數量(噸)欄位");
                }


                int rowCount = sheet.LastRowNum;
                for (int i = ItemNo_cell.RowIndex + 1; i <= rowCount; i++)
                {

                    try
                    {

                        var excelPartNo = ExcelUtil.GetStringCellValue(i, ItemNo_cell.ColumnIndex, sheet).Trim();
                        var model = new StockTransferDT();
                        model.ID = i;
                        //model.BARCODE = "B200619000" + (i).ToString();

                        model.ITEM_NUMBER = ExcelUtil.GetStringCellValue(i, ItemNo_cell.ColumnIndex, sheet).Trim();
                        model.PACKING_TYPE = ExcelUtil.GetStringCellValue(i, PackingType_cell.ColumnIndex, sheet).Trim();
                        model.REQUESTED_QUANTITY = ExcelUtil.GetDecimalCellValue(i, Qty_cell.ColumnIndex, sheet);
                        model.REQUESTED_QUANTITY2 = ExcelUtil.GetDecimalCellValue(i, TotalReam_cell.ColumnIndex, sheet);
                        model.ROLL_REAM_WT = ExcelUtil.GetDecimalCellValue(i, EveyReam_cell.ColumnIndex, sheet);
                        model.ROLL_REAM_QTY = ExcelUtil.GetDecimalCellValue(i, ReamWeight_cell.ColumnIndex, sheet);

                        //model.Status = "待入庫";
                        model.IN_SUBINVENTORY_CODE = ddlInSubinventory == "請選擇" ? "" : ddlInSubinventory;
                        model.IN_LOCATOR_ID = ddlInLocator == "請選擇" ? "" : ddlInLocator;
                        stockTransferDTs.Add(model);

                    }
                    catch (Exception e)
                    {
                        result.Success = false;
                        result.Msg = e.Message;
                    }

                }

            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Success = false;
                return result;
            }

            if (stockTransferDTs.Count != 0)
            {
                result.Msg = "成功";
                result.Success = true;
            }
            else
            {
                result.Msg = "資料錯誤";
                result.Success = false;
            }


            workbook = null;
            sheet = null;

            return result;

        }

    }
}