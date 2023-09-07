//SQL语句集合
namespace K3SalesOutNoticeReportTool.DB
{
    public class SqlList
    {
        //根据SQLID返回对应的SQL语句  
        private string _result;

        /// <summary>
        /// 根据‘开始日期’ ‘结束日期’ ‘客户编码列表’获取K3相关应收单记录集
        /// </summary>
        /// <param name="sdt"></param>
        /// <param name="edt"></param>
        /// <param name="customerlist"></param>
        /// <returns></returns>
        public string GetK3ArRecord(string sdt, string edt, string customerlist)
        {
            _result = $@"	SELECT 
                             X.客户ID,X.REMAKID,X.FID
	                        ,X.交货单位,X.终端客户,X.收货单位,X.客户开票名称
		                    ,X.二级客户,X.三级客户,X.销售订单号
		                    ,CONVERT(VARCHAR(10),X.单据日期,23) 单据日期
		                    ,CONVERT(VARCHAR(10),X.发货日期,23) 发货日期
		                    ,X.U订货单号,X.单据编号,X.摘要,X.托运货场地址
		                    ,X.产品名称,X.规格型号  
		                    ,X.实发罐数
		                    ,CASE X.REMAKID WHEN 1 THEN X.单价 ELSE 0 END  单价
		                    ,CASE X.REMAKID WHEN 1 THEN X.金额 ELSE 0 END 金额
		                    ,X.备注,X.开票人

	                FROM (	
			                --一级客户相关查询
			                SELECT '雅图高新材料股份有限公司' 交货单位
		                   ,ISNULL(Z0.FDATAVALUE,'') 终端客户
		                    ,X1.FNAME  收货单位,X2.FINVOICETITLE 客户开票名称
		                   ,ISNULL(J0.FNAME,'') 二级客户,ISNULL(K0.FNAME,'') 三级客户
		                   ,A.F_YTC_TEXT3 销售订单号,A.FDATE 单据日期
		                   ,A.F_YTC_DATE 发货日期  --复核日期
		                   ,A.F_YTC_TEXT13 U订货单号,A.FBILLNO 单据编号,A.F_YTC_TEXT2 摘要,A.F_YTC_TEXT1 托运货场地址
		                   ,C.FNAME 产品名称,C.FSPECIFICATION 规格型号  
		                   ,B.F_YTC_DECIMAL8 实发罐数  --实发数量
		                   ,B.F_YTC_DECIMAL2 单价         --实收单价
		                   ,B.FALLAMOUNTFOR 金额         --价税合计
		                   ,B.FCOMMENT 备注,Y0.FNAME 开票人
		                   ,A.FCUSTOMERID 客户ID --用于排序
		                   ,1 REMAKID  --1.用于设置二(三)级客户不显示‘单价’ ‘金额’ 信息 2.排序
		                   ,A.FID

			                FROM dbo.T_AR_RECEIVABLE A
			                INNER JOIN dbo.T_AR_RECEIVABLEENTRY B ON A.FID=B.FID
			                --INNER JOIN dbo.T_AR_RECEIVABLEFIN B1 ON A.FID=B1.FID
			                --物料相关
			                INNER JOIN dbo.T_BD_MATERIAL_L C ON B.FMATERIALID=C.FMATERIALID AND C.FLOCALEID='2052'
			                --一级客户相关
			                INNER JOIN dbo.T_BD_CUSTOMER X0 ON A.FCUSTOMERID=X0.FCUSTID
			                INNER JOIN dbo.T_BD_CUSTOMER_L X1 ON X0.FCUSTID=X1.FCUSTID AND X1.FLOCALEID='2052'
			                INNER JOIN dbo.T_BD_CUSTOMER_F X2 ON X0.FCUSTID=X2.FCUSTID
			                --二级客户相关
			                LEFT JOIN dbo.T_BD_CUSTOMER_L J0 ON A.F_YTC_BASE=J0.FCUSTID AND J0.FLOCALEID='2052'
			                --三级客户相关
			                LEFT JOIN dbo.T_BD_CUSTOMER_L K0 ON A.F_YTC_BASE1=K0.FCUSTID AND K0.FLOCALEID='2052'
			                --原厂终端客户
			                LEFT JOIN T_BAS_ASSISTANTDATAENTRY_L Z0 ON A.F_YTC_ASSISTANT9=Z0.FENTRYID
			                --创建人(开票人)相关
			                INNER JOIN dbo.T_SEC_USER Y0 ON A.FCREATORID=Y0.FUSERID

			                WHERE A.FDOCUMENTSTATUS='C'
			                AND CONVERT(VARCHAR(10),A.F_YTC_DATE,23)>='{sdt}' --FDATE
			                AND CONVERT(VARCHAR(10),A.F_YTC_DATE,23)<='{edt}' 
			                AND B.FALLAMOUNTFOR>=0.01    --价税合计
			                --AND A.F_YTC_DATE IS NOT NULL  --'复核日期'必填
			                AND X0.FNUMBER IN({customerlist})   --一级客户编码为条件

			                UNION

			                --二级客户相关查询
                            SELECT '雅图高新材料股份有限公司' 交货单位
		                   ,ISNULL(Z0.FDATAVALUE,'') 终端客户
		                    ,X1.FNAME  收货单位,X2.FINVOICETITLE 客户开票名称
		                   ,ISNULL(J1.FNAME,'') 二级客户,ISNULL(K0.FNAME,'') 三级客户
		                   ,A.F_YTC_TEXT3 销售订单号,A.FDATE 单据日期
		                   ,A.F_YTC_DATE 发货日期  --复核日期
		                   ,A.F_YTC_TEXT13 U订货单号,A.FBILLNO 单据编号,A.F_YTC_TEXT2 摘要,A.F_YTC_TEXT1 托运货场地址
		                   ,C.FNAME 产品名称,C.FSPECIFICATION 规格型号  
		                   ,B.F_YTC_DECIMAL8 实发罐数  --实发数量
		                   ,B.F_YTC_DECIMAL2 单价         --实收单价
		                   ,B.FALLAMOUNTFOR 金额         --价税合计
		                   ,B.FCOMMENT 备注,Y0.FNAME 开票人
		                   ,A.F_YTC_BASE 客户ID --用于排序
		                   ,2 REMAKID  --1.用于设置二(三)级客户不显示‘单价’ ‘金额’ 信息 2.排序
		                   ,A.FID

			                FROM dbo.T_AR_RECEIVABLE A
			                INNER JOIN dbo.T_AR_RECEIVABLEENTRY B ON A.FID=B.FID
			                --INNER JOIN dbo.T_AR_RECEIVABLEFIN B1 ON A.FID=B1.FID
			                --物料相关
			                INNER JOIN dbo.T_BD_MATERIAL_L C ON B.FMATERIALID=C.FMATERIALID AND C.FLOCALEID='2052'
			                --一级客户相关
			                INNER JOIN dbo.T_BD_CUSTOMER X0 ON A.FCUSTOMERID=X0.FCUSTID
			                INNER JOIN dbo.T_BD_CUSTOMER_L X1 ON X0.FCUSTID=X1.FCUSTID AND X1.FLOCALEID='2052'
			                INNER JOIN dbo.T_BD_CUSTOMER_F X2 ON X0.FCUSTID=X2.FCUSTID
			                --二级客户相关
			                INNER JOIN dbo.T_BD_CUSTOMER J0 ON A.F_YTC_BASE=J0.FCUSTID
			                INNER JOIN dbo.T_BD_CUSTOMER_L J1 ON J0.FCUSTID=J1.FCUSTID AND J1.FLOCALEID='2052'
			                --三级客户相关
			                LEFT JOIN dbo.T_BD_CUSTOMER_L K0 ON A.F_YTC_BASE1=K0.FCUSTID AND K0.FLOCALEID='2052'
			                --原厂终端客户
			                LEFT JOIN T_BAS_ASSISTANTDATAENTRY_L Z0 ON A.F_YTC_ASSISTANT9=Z0.FENTRYID
			                --创建人(开票人)相关
			                INNER JOIN dbo.T_SEC_USER Y0 ON A.FCREATORID=Y0.FUSERID

			                WHERE A.FDOCUMENTSTATUS='C'
			                AND CONVERT(VARCHAR(10),A.F_YTC_DATE,23)>='{sdt}'
			                AND CONVERT(VARCHAR(10),A.F_YTC_DATE,23)<='{edt}' 
			                AND B.FALLAMOUNTFOR>=0.01    --价税合计
			                --AND A.F_YTC_DATE IS NOT NULL  --'复核日期'必填
			                AND A.F_YTC_ASSISTANT='57946bda364e3f' --地区=‘中国区(原厂项目)’ 57946bda364e3f
			                AND J0.FNUMBER IN({customerlist})   --二级客户编码为条件

			                UNION

			                --三级客户相关查询
		                    SELECT '雅图高新材料股份有限公司' 交货单位
		                   ,ISNULL(Z0.FDATAVALUE,'') 终端客户
		                    ,X1.FNAME  收货单位,X2.FINVOICETITLE 客户开票名称
		                   ,ISNULL(J0.FNAME,'') 二级客户,ISNULL(K1.FNAME,'') 三级客户
		                   ,A.F_YTC_TEXT3 销售订单号,A.FDATE 单据日期
		                   ,A.F_YTC_DATE 发货日期  --复核日期
		                   ,A.F_YTC_TEXT13 U订货单号,A.FBILLNO 单据编号,A.F_YTC_TEXT2 摘要,A.F_YTC_TEXT1 托运货场地址
		                   ,C.FNAME 产品名称,C.FSPECIFICATION 规格型号  
		                   ,B.F_YTC_DECIMAL8 实发罐数  --实发数量
		                   ,B.F_YTC_DECIMAL2 单价         --实收单价
		                   ,B.FALLAMOUNTFOR 金额         --价税合计
		                   ,B.FCOMMENT 备注,Y0.FNAME 开票人
		                   ,A.F_YTC_BASE1 客户ID --用于排序
		                   ,3 REMAKID  --1.用于设置二(三)级客户不显示‘单价’ ‘金额’ 信息 2.排序
		                   ,A.FID

			                FROM dbo.T_AR_RECEIVABLE A
			                INNER JOIN dbo.T_AR_RECEIVABLEENTRY B ON A.FID=B.FID
			                --INNER JOIN dbo.T_AR_RECEIVABLEFIN B1 ON A.FID=B1.FID
			                --物料相关
			                INNER JOIN dbo.T_BD_MATERIAL_L C ON B.FMATERIALID=C.FMATERIALID AND C.FLOCALEID='2052'
			                --一级客户相关
			                INNER JOIN dbo.T_BD_CUSTOMER X0 ON A.FCUSTOMERID=X0.FCUSTID
			                INNER JOIN dbo.T_BD_CUSTOMER_L X1 ON X0.FCUSTID=X1.FCUSTID AND X1.FLOCALEID='2052'
			                INNER JOIN dbo.T_BD_CUSTOMER_F X2 ON X0.FCUSTID=X2.FCUSTID
			                --二级客户相关
			                LEFT JOIN dbo.T_BD_CUSTOMER_L J0 ON A.F_YTC_BASE=J0.FCUSTID AND J0.FLOCALEID='2052'
			                --三级客户相关
			                INNER JOIN T_BD_CUSTOMER K0 ON A.F_YTC_BASE1=K0.FCUSTID
			                INNER JOIN dbo.T_BD_CUSTOMER_L K1 ON K0.FCUSTID=K1.FCUSTID AND K1.FLOCALEID='2052'
			                --原厂终端客户
			                LEFT JOIN T_BAS_ASSISTANTDATAENTRY_L Z0 ON A.F_YTC_ASSISTANT9=Z0.FENTRYID
			                --创建人(开票人)相关
			                INNER JOIN dbo.T_SEC_USER Y0 ON A.FCREATORID=Y0.FUSERID

			                WHERE A.FDOCUMENTSTATUS='C'
			                AND CONVERT(VARCHAR(10),A.F_YTC_DATE,23)>='{sdt}'
			                AND CONVERT(VARCHAR(10),A.F_YTC_DATE,23)<='{edt}' 
			                AND B.FALLAMOUNT>=0.01    --价税合计本位币
			                --AND A.F_YTC_DATE IS NOT NULL  --'复核日期'必填
			                AND A.F_YTC_ASSISTANT='57946bda364e3f' --地区=‘中国区(原厂项目)’ 57946bda364e3f
			                AND A.F_YTC_BASE = 0  --二级客户为空(即为0)
			                AND K0.FNUMBER IN({customerlist})
	                  )X
	                 -- WHERE X.单据编号='AR00197269'
	                ORDER BY X.客户ID,X.REMAKID,X.发货日期
            ";

            return _result;
        }

        /// <summary>
        /// 根据从EXCEL导入的记录获取相关客户信息(包含FCUSTID)
        /// </summary>
        /// <param name="customerlist"></param>
        /// <returns></returns>
        public string SearchCustomList(string customerlist)
        {
            _result = $@"SELECT A.FCUSTID,A.FNUMBER,B.FNAME 
                        FROM dbo.T_BD_CUSTOMER A
                        INNER JOIN dbo.T_BD_CUSTOMER_L B ON A.FCUSTID=B.FCUSTID AND B.FLOCALEID='2052'
                        WHERE A.FNUMBER IN({customerlist})";

            return _result;
        }

    }
}
