using System;
using System.Data;
using System.Data.SqlClient;
using K3SalesOutNoticeReportTool.DB;

//查询
namespace K3SalesOutNoticeReportTool.Task
{
    public class SearchDt
    {
        ConDb conDb = new ConDb();
        SqlList sqlList = new SqlList();

        /// <summary>
        /// 根据SQL语句查询得出对应的DT
        /// </summary>
        /// <param name="sqlscript"></param>
        /// <returns></returns>
        private DataTable UseSqlSearchIntoDt(string sqlscript)
        {
            var resultdt = new DataTable();

            try
            {
                var sqlDataAdapter = new SqlDataAdapter(sqlscript, conDb.GetK3CloudConn());
                sqlDataAdapter.Fill(resultdt);
            }
            catch (Exception)
            {
                resultdt.Rows.Clear();
                resultdt.Columns.Clear();
            }

            return resultdt;
        }

        /// <summary>
        /// 根据‘开始日期’ ‘结束日期’ ‘客户编码列表’获取K3相关应收单记录集
        /// </summary>
        /// <param name="sdt"></param>
        /// <param name="edt"></param>
        /// <param name="customerlist"></param>
        /// <returns></returns>
        public DataTable GetK3ArRecord(string sdt, string edt, string customerlist)
        {
            var dt = UseSqlSearchIntoDt(sqlList.GetK3ArRecord(sdt, edt, customerlist));
            return dt;
        }

    }
}
