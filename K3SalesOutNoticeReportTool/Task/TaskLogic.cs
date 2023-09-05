//中转(功能分配)

using System.Data;
using NPOI.POIFS.Dev;

namespace K3SalesOutNoticeReportTool.Task
{
    public class TaskLogic
    {
        SearchDt serchDt = new SearchDt();
        Generate generate = new Generate();
        ImportDt importDt = new ImportDt();

        #region 变量参数
        private int _taskid;
        private string _sdt;           //开始日期(运算时使用)
        private string _edt;           //结束日期(运算时使用)
        private string _customerlist;  //客户列表(运算时使用)
        private string _fileAddress;   //文件地址('自定义批量导出'-导入EXCEL 及 导出地址收集使用)
        private DataTable _custdtlist; //获取前端的客户列表DT(自定义批量导出功能使用)

        private string _resultmark;    //返回是否成功标记
        private DataTable _resultImportDt;  //返回导入EXCEL信息

        #endregion

        #region Set
        /// <summary>
        /// 中转ID
        /// </summary>
        public int TaskId { set { _taskid = value; } }

        /// <summary>
        ///开始日期(运算时使用)
        /// </summary>
        public string Sdt { set { _sdt = value; } }

        /// <summary>
        ///结束日期(运算时使用)
        /// </summary>
        public string Edt { set { _edt = value; } }

        /// <summary>
        /// 客户列表(运算时使用)
        /// </summary>
        public string Customerlist { set { _customerlist = value; } }
        /// <summary>
        /// 接收文件地址信息
        /// </summary>
        public string FileAddress { set { _fileAddress = value; } }
        /// <summary>
        /// 获取前端的客户列表DT(自定义批量导出功能使用)
        /// </summary>
        public DataTable Custdtlist { set { _custdtlist = value; } }
        #endregion

        #region Get
        /// <summary>
        ///  返回是否成功标记
        /// </summary>
        public string ResultMark => _resultmark;

        /// <summary>
        /// 返回导入EXCEL结果
        /// </summary>
        public DataTable ResultImportDt => _resultImportDt;
        #endregion

        public void StartTask()
        {
            switch (_taskid)
            {
                //导入
                case 0:
                    ImportExcelRecord(_fileAddress);
                    break;
                //运算
                case 1:
                    GenerateRdAndExportPdf(_sdt,_edt,_customerlist, _custdtlist, _fileAddress);
                    break;
            }
        }

        /// <summary>
        /// 导入EXCEL
        /// </summary>
        /// <param name="fileadd"></param>
        private void ImportExcelRecord(string fileadd)
        {
            _resultImportDt = importDt.OpenExcelImporttoDt(fileadd).Copy();
        }

        /// <summary>
        /// 根据相关条件进行运算并最后整合数据至PDF
        /// </summary>
        /// <param name="sdt"></param>
        /// <param name="edt"></param>
        /// <param name="customerlist"></param>
        /// <param name="custlistdt"></param>
        /// <param name="exportaddress"></param>
        private void GenerateRdAndExportPdf(string sdt, string edt, string customerlist, DataTable custlistdt,string exportaddress)
        {
            _resultmark = generate.GenerateRdAndExportPdf(sdt,edt,customerlist,custlistdt,exportaddress);
        }

    }
}
