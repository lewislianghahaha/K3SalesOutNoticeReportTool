using System;
using System.Data;

//临时表
namespace K3SalesOutNoticeReportTool.DB
{
    public class TempDtList
    {
        /// <summary>
        /// 导出报表记录集使用
        /// </summary>
        /// <returns></returns>
        public DataTable BatchConfirmDtTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 21; i++)
            {
                var dc = new DataColumn();
                switch (i)
                {
                    //客户ID
                    case 0:
                        dc.ColumnName = "FCUSTID";
                        dc.DataType = Type.GetType("System.Int32"); 
                        break;
                    //应收单ID
                    case 1:
                        dc.ColumnName = "FID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //终端客户
                    case 2:
                        dc.ColumnName = "FDATAVALUE";
                        dc.DataType = Type.GetType("System.String"); 
                        break;
                    //收货单位
                    case 3:
                        dc.ColumnName = "ReceiveFNAME";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //客户开票名称
                    case 4:
                        dc.ColumnName = "ReceiveFNAME1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //二级客户
                    case 5:
                        dc.ColumnName = "TwoCustomerName";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //三级客户
                    case 6:
                        dc.ColumnName = "ThreeCustomerName";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //摘要
                    case 7:
                        dc.ColumnName = "Remark";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //单据日期
                    case 8:
                        dc.ColumnName = "FDATE";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //U订货单号
                    case 9:
                        dc.ColumnName = "UOrderNo";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //单据编号
                    case 10:
                        dc.ColumnName = "FBILLNO";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //托运货场地址
                    case 11:
                        dc.ColumnName = "FAddress";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //产品名称
                    case 12:
                        dc.ColumnName = "FMaterialName";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //规格
                    case 13:
                        dc.ColumnName = "KuiName";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //实发罐数
                    case 14:
                        dc.ColumnName = "FQty";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //单价
                    case 15:
                        dc.ColumnName = "FPrice";
                        dc.DataType = Type.GetType("System.Decimal"); 
                        break;
                    //金额
                    case 16:
                        dc.ColumnName = "FAmount";
                        dc.DataType = Type.GetType("System.Double"); 
                        break;
                    //备注
                    case 17:
                        dc.ColumnName = "FNote";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //开票人
                    case 18:
                        dc.ColumnName = "FCeateName";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //发货日期
                    case 19:
                        dc.ColumnName = "FSendDate";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //销售订单号
                    case 20:
                        dc.ColumnName = "SaleOrderNo";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }
    }
}
