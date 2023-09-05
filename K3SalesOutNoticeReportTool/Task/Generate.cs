using System;
using System.Data;

//运算
namespace K3SalesOutNoticeReportTool.Task
{
    public class Generate
    {
        /// <summary>
        /// 根据相关条件进行运算并最后整合数据至PDF
        /// </summary>
        /// <param name="sdt"></param>
        /// <param name="edt"></param>
        /// <param name="customerlist"></param>
        /// <param name="custlistdt"></param>
        /// <param name="exportaddress"></param>
        /// <returns></returns>
        public string GenerateRdAndExportPdf(string sdt, string edt, string customerlist,DataTable custlistdt,string exportaddress)
        {
            var result = "Finish";

            try
            {
                //todo:


            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        } 



    }
}
