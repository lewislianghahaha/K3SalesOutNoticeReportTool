using System;
using System.Data;
using System.Windows.Forms;
using K3SalesOutNoticeReportTool.DB;
using Stimulsoft.Report;

//运算
namespace K3SalesOutNoticeReportTool.Task
{
    public class Generate
    {
        SearchDt searchDt = new SearchDt();
        TempDtList tempDt = new TempDtList();

        /// <summary>
        /// 根据相关条件进行运算并最后整合数据至PDF
        /// </summary>
        /// <param name="sdt">开始日期</param>
        /// <param name="edt">结束日期</param>
        /// <param name="customerlist">EXCEL导入整理的客户列表信息</param>
        /// <param name="exportaddress">输出地址</param>
        /// <returns></returns>
        public string GenerateRdAndExportPdf(string sdt, string edt, string customerlist,string exportaddress)
        {
            var result = "Finish";

            try
            {
                //定义-‘导出’模板
                var exportdt = tempDt.BatchConfirmDtTemp();

                //todo:根据‘customerlist’获取相关记录集
                var custdt = searchDt.SearchCustomList(customerlist).Copy();
                //todo:根据‘sdt’ ‘edt’ ‘customerlist’获取相关应收单记录集
                var ardt = searchDt.GetK3ArRecord(sdt,edt,customerlist).Copy();
                //todo:根据ardt获取相关‘发货日期’记录-去从 row[11]
                var sendoutdatedt = GetSendOutDateDt(ardt).Copy();

                //todo:循环顺序-->1.按客户 2.按发货日期 3.按1 2 3 客户等级ID
                foreach (DataRow custrows in custdt.Rows)
                {
                   // var count = custdt.Rows.Count;
                    var custid = Convert.ToInt32(custrows[0]);

                    foreach (DataRow sendoutrows in sendoutdatedt.Rows)
                    {
                        var senddt = Convert.ToString(sendoutrows[0]);

                        //todo:按照从1开始循环--表示客户等级ID（REMAKID）
                        for (var i = 1; i <= 3; i++)
                        {
                            //todo:使用‘客户ID’ ‘发货日期’ 及 ‘REMAKID’放到ardt进行查询并获取数据集,最后整合到exportdt内(重)
                            var dtlrows = ardt.Select("客户ID='"+ custid + "' and 发货日期='"+senddt+ "' and REMAKID='"+i+"'");

                            if (dtlrows.Length > 0)
                            {
                                //todo:对dtlrows进行循环插入
                                exportdt.Merge(GetExportDt(exportdt,dtlrows));
                            }
                        }
                    }

                    //var b = exportdt.Copy();

                    //todo:当判断exportdt>0,进行转换成PDF并输出至指定的地址内--(注:当循环完起一个客户后执行)
                    if (exportdt.Rows.Count > 0)
                    {
                        ExportDtToPdf(sdt, edt, Convert.ToString(custrows[2]),exportaddress, exportdt);
                        //todo:最后将exportdt初始化,便于下一个循环使用
                        exportdt.Rows.Clear();
                    }                   
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 将计算的DT输出至指定位置的PDF
        /// </summary>
        /// <returns></returns>
        private bool ExportDtToPdf(string sdt,string edt,string custname,string exportaddress,DataTable resultdt)
        {
            var result = true;
            var stiReport = new StiReport();
           // var date = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                var filepath = Application.StartupPath + "/Report/" + "SalesOutListReport.mrt";
               
                var pdfFileAddress = exportaddress + "\\" + $"{custname}-" + $"销售发货清单-{sdt}至{edt}" +".pdf";

                stiReport.Load(filepath);                                      //读取STI模板
                stiReport.RegData("SalesOutList", resultdt);                   //填充数据至STI模板内
                stiReport.Render(false);                                       //重点-没有这个生成的文件会提示“文件已损坏”
                stiReport.ExportDocument(StiExportFormat.Pdf, pdfFileAddress); //生成指定格式文件   
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 将记录集插入至tempdt内
        /// </summary>
        /// <param name="tempdt"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        private DataTable GetExportDt(DataTable tempdt,DataRow[] rows)
        {
            for (var i = 0; i < rows.Length; i++)
            {
                var newrow = tempdt.NewRow();

                newrow[0] = Convert.ToString(rows[i][0]);                     //客户ID
                newrow[1] = Convert.ToString(rows[i][2]);                     //应收单ID
                newrow[2] = Convert.ToString(rows[i][4]);                     //终端客户
                newrow[3] = Convert.ToString(rows[i][5]);                     //收货单位
                newrow[4] = Convert.ToString(rows[i][6]);                     //客户开票名称
                newrow[5] = Convert.ToString(rows[i][7]);                     //二级客户
                newrow[6] = Convert.ToString(rows[i][8]);                     //三级客户
                newrow[7] = Convert.ToString(rows[i][14]);                    //摘要
                newrow[8] = Convert.ToString(rows[i][10]);                    //单据日期
                newrow[9] = Convert.ToString(rows[i][12]);                    //U订货单号
                newrow[10] = Convert.ToString(rows[i][13]);                   //单据编号
                newrow[11] = Convert.ToString(rows[i][15]);                   //托运货场地址
                newrow[12] = Convert.ToString(rows[i][16]);                   //产品名称
                newrow[13] = Convert.ToString(rows[i][17]);                   //规格
                newrow[14] = Convert.ToInt32(rows[i][18]);                    //实发罐数

                newrow[15] = Convert.ToDecimal(rows[i][19]) == 0
                    ? (object) DBNull.Value
                    : Math.Round(Convert.ToDecimal(rows[i][19]), 2);          //单价

                newrow[16] = Convert.ToDecimal(rows[i][20]) == 0
                    ? (object)DBNull.Value
                    : Math.Round(Convert.ToDecimal(rows[i][20]), 2);          //金额

                newrow[17] = Convert.ToString(rows[i][21]);                   //备注
                newrow[18] = Convert.ToString(rows[i][22]);                   //开票人
                newrow[19] = Convert.ToString(rows[i][11]);                   //发货日期
                newrow[20] = Convert.ToString(rows[i][9]);                    //销售订单号

                newrow[21] = Convert.ToDecimal(rows[i][19]) == 0
                    ? (object)DBNull.Value
                    : Convert.ToString(Math.Round(Convert.ToDecimal(rows[i][19]), 2)); //FPriceShow-用于显示在STI报表的单价

                newrow[22] = Convert.ToDecimal(rows[i][20]) == 0
                    ? (object)DBNull.Value
                    : Convert.ToString(Math.Round(Convert.ToDecimal(rows[i][20]), 2)); //FAmountShow-用于显示在STI报表的金额

                newrow[23] =Convert.ToString(rows[i][1]);                     //FRemarkid-用于在STI报表分组使用-客户等级ID

                tempdt.Rows.Add(newrow);
            }

            return tempdt;
        }

        /// <summary>
        /// 根据获取的应收单记录集,对‘发货日期’rows[11]进行去从处理
        /// </summary>
        /// <param name="sourcedt"></param>
        /// <returns></returns>
        private DataTable GetSendOutDateDt(DataTable sourcedt)
        {
            var temp = string.Empty;
            var tempdt = tempDt.GetRecordSalesOutDateDt();
            var resultdt = tempdt.Clone();

            foreach (DataRow rows in sourcedt.Rows)
            {
                if (string.IsNullOrEmpty(temp))
                {
                    var newrow = tempdt.NewRow();
                    newrow[0] = Convert.ToString(rows[11]);
                    temp = Convert.ToString(rows[11]);
                    tempdt.Rows.Add(newrow);
                }
                else if (temp !=Convert.ToString(rows[11]) && tempdt.Select("发货日期='"+Convert.ToString(rows[11])+"'").Length==0)
                {
                    var newrow = tempdt.NewRow();
                    newrow[0] = Convert.ToString(rows[11]);
                    temp = Convert.ToString(rows[11]);
                    tempdt.Rows.Add(newrow);
                }
            }

            //todo:对整合的结果进行排序
            var dv = tempdt.DefaultView;
            dv.Sort = "发货日期 ASC";

            //todo:最后循环dv并将相关记录插入至DT内
            foreach (DataRowView drv in dv)
            {
                var newrow = resultdt.NewRow();
                newrow[0] = Convert.ToString(drv["发货日期"]);
                resultdt.Rows.Add(newrow);
            }
            //var a = resultdt;

            return resultdt;
        }

    }
}
