using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Mms_Server.Common;
using Mms_Server.Model.Supplier;

namespace Mms_Server.DAL
{
    public class SupplierDal
    {
        /// <summary>
        /// 分页查询供应商信息
        /// </summary>
        /// <param name="pageSupplierQueryInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<PageSupplierInfo>> PageGetSupplierInfo(PageSupplierQueryInfo pageSupplierQueryInfo)
        {
            VMResult<PageSupplierInfo> r=new VMResult<PageSupplierInfo>();
            try
            {
                using (var conn=DapperHelper.CreateConnection())
                {
                    StringBuilder strSql=new StringBuilder();
                    StringBuilder strPageSql=new StringBuilder();
                    DynamicParameters paras=new DynamicParameters();
                    strSql.Append(@"SELECT * FROM Supplier s where 1=1");
                    if (!string.IsNullOrWhiteSpace(pageSupplierQueryInfo.supplierQueryInfo.Name))
                    {
                        strSql.Append(" and s.Name='" + pageSupplierQueryInfo.supplierQueryInfo.Name + "'");
                    }

                    if (!string.IsNullOrWhiteSpace(pageSupplierQueryInfo.supplierQueryInfo.Mobile))
                    {
                        strSql.Append(" and s.Mobile='" + pageSupplierQueryInfo.supplierQueryInfo.Mobile + "'");
                    }

                    if (!string.IsNullOrWhiteSpace(pageSupplierQueryInfo.supplierQueryInfo.LinkName))
                    {
                        strSql.Append(" and s.LinkName='" + pageSupplierQueryInfo.supplierQueryInfo.LinkName + "'");
                    }

                    int total = (await conn.QueryAsync<SupplierInfo>(strSql.ToString())).Count();

                    int startNumber = (pageSupplierQueryInfo.CurrentPage - 1) * pageSupplierQueryInfo.PageSize;
                    strPageSql.Append(@"SELECT DATA.* FROM (" + strSql + ") DATA ORDER BY 1 OFFSET " + startNumber +
                                      "ROWS FETCH NEXT " + pageSupplierQueryInfo.PageSize + "ROWS ONLY");
                    var value = await conn.QueryAsync<SupplierInfo>(strPageSql.ToString());
                    if (value == null)
                    {
                        r.ResultMsg = "分页查询失败";
                    }
                    else
                    {
                        r.Data.SupplierInfos = value;
                        r.Data.total = total;
                        r.ResultCode = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                r.ResultMsg = ex.Message;
            }

            return r;
        }

        /// <summary>
        /// 新增供应商信息
        /// </summary>
        /// <param name="addSupplierInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> AddSupplierInfo(SupplierInfo addSupplierInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @"INSERT INTO Supplier (Name,LinkName,Mobile,Remark,Creater,CreateTime) VALUES (@Name,@LinkName,@Mobile,@Remark,@Creater,@CreateTime)";
                        var value = await conn.ExecuteAsync(strSql,new
                        {
                            addSupplierInfo.Name,
                            addSupplierInfo.LinkName,
                            addSupplierInfo.Mobile,
                            addSupplierInfo.Remark,
                            addSupplierInfo.Creater,
                            CreateTime=DateTime.Now
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "新增供应商信息失败";
                            return r;
                        }

                        r.ResultCode = 0;
                        r.Data = true;
                        transaction.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                r.ResultMsg = ex.Message;
            }

            return r;
        }

        /// <summary>
        /// 根据供应商ID获取供应商详细信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<SupplierInfo>> GetSupplierInfoByID(int ID)
        {
            VMResult<SupplierInfo> r=new VMResult<SupplierInfo>();
            try
            {
                using (var conn=DapperHelper.CreateConnection())
                {
                    string strSql = @"SELECT * FROM Supplier s WHERE s.ID=@ID";
                    var value = await conn.QueryFirstOrDefaultAsync<SupplierInfo>(strSql, new
                    {
                        ID
                    });
                    if (value == null)
                    {
                        r.ResultMsg = "根据ID获取供应商信息失败";
                        return r;
                    }

                    r.Data = value;
                    r.ResultCode = 0;
                }
            }
            catch (Exception ex)
            {
                r.ResultMsg = ex.Message;
            }

            return r;
        }

        /// <summary>
        /// 编辑供应商信息
        /// </summary>
        /// <param name="updatesSupplierInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> UpdateSupplierInfo(SupplierInfo updatesSupplierInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @"UPDATE Supplier SET Name=@Name,LinkName=@LinkName, Mobile=@Mobile,Remark=@Remark,Updater=@Updater,UpdateTime=@UpdateTime 
                                            WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql,new
                        {
                            updatesSupplierInfo.Name,
                            updatesSupplierInfo.LinkName,
                            updatesSupplierInfo.Mobile,
                            updatesSupplierInfo.Remark,
                            updatesSupplierInfo.Updater,
                            UpdateTime=DateTime.Now,
                            updatesSupplierInfo.ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "编辑供应商信息失败";
                            return r;
                        }

                        r.ResultCode = 0;
                        r.Data = true;
                        transaction.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                r.ResultMsg= ex.Message;
            }

            return r;
        }

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> deleteSupplierInfo(int ID)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @"DELETE FROM Supplier WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql,new
                        {
                            ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "编辑供应商信息失败";
                            return r;
                        }

                        r.ResultCode = 0;
                        r.Data = true;
                        transaction.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                r.ResultMsg = ex.Message;
            }

            return r;
        }
    }
}
