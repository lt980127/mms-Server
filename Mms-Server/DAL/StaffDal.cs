using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Mms_Server.Common;
using Mms_Server.Model.Staff;

namespace Mms_Server.DAL
{
    public class StaffDal
    {
        /// <summary>
        /// 分页获取员工信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="staffQueryInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<PageStaffInfo>> GetPageStaffInfo(int currentPage, int pageSize, StaffQueryInfo
            staffQueryInfo)
        {
            VMResult<PageStaffInfo> r=new VMResult<PageStaffInfo>();
            try
            {
                using (var conn=DapperHelper.CreateConnection())
                {
                    StringBuilder strSql=new StringBuilder();
                    StringBuilder strPageSql=new StringBuilder();
                    strSql.Append(@"SELECT * FROM Staff s WHERE 1=1");
                    if (string.IsNullOrWhiteSpace(staffQueryInfo.Account))
                    {
                        strSql.Append(" and s.Account='" + staffQueryInfo.Account + "'");
                    }

                    if (string.IsNullOrWhiteSpace(staffQueryInfo.Name))
                    {
                        strSql.Append(" and s.Name='" + staffQueryInfo.Name + "'");
                    }

                    int total = (await conn.QueryAsync<StaffInfo>(strSql.ToString())).Count();

                    int startNumber = (currentPage - 1) * pageSize;
                    strPageSql.Append(@"SELECT DATA.* FROM (" + strSql + ") DATA ORDER BY 1 OFFSET " + startNumber +
                                      "ROWS FETCH NEXT " + pageSize + "ROWS ONLY");
                    var value = await conn.QueryAsync<StaffInfo>(strPageSql.ToString());
                    if (value == null)
                    {
                        r.ResultMsg = "查询员工信息失败";
                    }
                    else
                    {
                        r.Data.StaffInfos = value;
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
        /// 新增员工信息
        /// </summary>
        /// <param name="addStaffInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> AddStaffInfo(StaffInfo addStaffInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @"INSERT INTO Staff (Account,Name,Age,Phone,Salary,EntryDate,Creater,CreateTime) 
                                            VALUES (@Account,@Name,@Age,@Phone,@Salary,@EntryDate,@Creater,@CreateTime)";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            addStaffInfo.Account,
                            addStaffInfo.Name,
                            addStaffInfo.Age,
                            addStaffInfo.Phone,
                            addStaffInfo.Salary,
                            addStaffInfo.EntryDate,
                            addStaffInfo.Creater,
                            CreateTime=DateTime.Now
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "新增员工信息失败";
                            return r;
                        }

                        r.Data = true;
                        r.ResultCode = 0;
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
        /// 根据ID获取员工信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<StaffInfo>> GetStaffInfoByID(int ID)
        {
            VMResult<StaffInfo> r=new VMResult<StaffInfo>();
            try
            {
                using (var conn=DapperHelper.CreateConnection())
                {
                    string strSql = @"SELECT * FROM Staff s WHERE s.ID=@ID";
                    var value = await conn.QueryFirstOrDefaultAsync<StaffInfo>(strSql, new
                    {
                        ID
                    });
                    if (value == null)
                    {
                        r.ResultMsg = "根据ID获取员工信息失败";
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
        /// 编辑员工信息
        /// </summary>
        /// <param name="updateStaffInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> UpdateStaffInfo(StaffInfo updateStaffInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @" UPDATE Staff SET Account=@Account,Name=@Name,Age=@Age,Phone=@Phone,Salary=@Salary,
                                            EntryDate=@EntryDate,Updater=@Updater,UpdateTime=@UpdateTime WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            updateStaffInfo.Account,
                            updateStaffInfo.Name,
                            updateStaffInfo.Age,
                            updateStaffInfo.Phone,
                            updateStaffInfo.Salary,
                            updateStaffInfo.EntryDate,
                            updateStaffInfo.Updater,
                            UpdateTime=DateTime.Now,
                            updateStaffInfo.ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "编辑员工信息失败";
                            return r;
                        }

                        r.Data = true;
                        r.ResultCode = 0;
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
        /// 删除员工信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> DeleteStaffInfo(int ID)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @"DELETE FROM Staff WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "删除员工信息失败";
                            return r;
                        }

                        r.Data = true;
                        r.ResultCode = 0;
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
