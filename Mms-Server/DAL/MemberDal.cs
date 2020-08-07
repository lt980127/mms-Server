using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Mms_Server.Common;
using Mms_Server.Model;
using Mms_Server.Model.Member;

namespace Mms_Server.DAL
{
    public class MemberDal
    {
        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="memberQueryInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<PageMemberInfo>> GetMemberInfo(MemberQueryInfo memberQueryInfo)
        {
            VMResult<PageMemberInfo> r = new VMResult<PageMemberInfo>();
            try
            {
                using (var conn = DapperHelper.CreateConnection())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"SELECT * FROM Member m where 1=1");
                    if (!string.IsNullOrWhiteSpace(memberQueryInfo.UserName))
                    {
                        strSql.Append(" and m.Name like'%" + memberQueryInfo.UserName + "%'");
                    }

                    if (!string.IsNullOrWhiteSpace(memberQueryInfo.CardNumber))
                    {
                        strSql.Append(" and m.CardNum = '" + memberQueryInfo.CardNumber + "'");
                    }

                    if (memberQueryInfo.PayTypeID != 0)
                    {
                        strSql.Append(" and m.PayType = '" + memberQueryInfo.PayTypeID + "'");
                    }

                    if (memberQueryInfo.Birthday!=null)
                    {
                        strSql.Append(" and m.Birthday='" + memberQueryInfo.Birthday + "'");
                    }
                    var value = await conn.QueryAsync<MemberInfo>(strSql.ToString());
                    if (value != null)
                    {
                        r.Data.MemberInfos = value;
                        r.Data.total = value.ToList().Count; ;
                        r.ResultCode = 0;
                    }
                    else
                    {
                        r.ResultMsg = "查询用户信息为空!";
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
        /// 分页查询会员信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="memberQueryInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<PageMemberInfo>> PageGetMemberInfo(int currentPage, int pageSize,
            MemberQueryInfo memberQueryInfo)
        {
            VMResult<PageMemberInfo> r = new VMResult<PageMemberInfo>();
            try
            {
                using (var conn = DapperHelper.CreateConnection())
                {
                    StringBuilder strSql=new StringBuilder();
                    StringBuilder strPageSql=new StringBuilder();
                    strSql.Append(@"SELECT * FROM Member m where 1=1");
                    if (!string.IsNullOrWhiteSpace(memberQueryInfo.UserName))
                    {
                        strSql.Append(" and m.Name like'%" + memberQueryInfo.UserName + "%'");
                    }

                    if (!string.IsNullOrWhiteSpace(memberQueryInfo.CardNumber))
                    {
                        strSql.Append(" and m.CardNum = '" + memberQueryInfo.CardNumber + "'");
                    }

                    if (memberQueryInfo.PayTypeID != 0)
                    {
                        strSql.Append(" and m.PayType = '" + memberQueryInfo.PayTypeID + "'");
                    }

                    if (memberQueryInfo.Birthday != null)
                    {
                        strSql.Append(" and m.Birthday='" + memberQueryInfo.Birthday + "'");
                    }

                    //总条数
                    int TotalCount = (await conn.QueryAsync<MemberInfo>(strSql.ToString())).Count();

                    //开始条数
                    int startNumber = (currentPage - 1) * pageSize;
                    strPageSql.Append(@"SELECT DATA.* FROM (" + strSql + ") DATA ORDER BY 1 OFFSET " + startNumber +
                                      "ROWS FETCH NEXT " + pageSize + "ROWS ONLY");
                    var value = await conn.QueryAsync<MemberInfo>(strPageSql.ToString());
                    if (value != null)
                    {
                        r.Data.MemberInfos = value;
                        r.Data.total = TotalCount;
                        r.ResultCode = 0;
                    }
                    else
                    {
                        r.ResultMsg = "分页查询失败";
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
        /// 新增会员信息
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> AddMemberInfo(MemberInfo memberInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn = DapperHelper.CreateConnection())
                    {
                        string CardNumSql = @"SELECT m.CardNum FROM Member m WHERE m.CardNum=@CardNum";
                        var CardNum = await conn.QueryFirstOrDefaultAsync<string>(CardNumSql,new{memberInfo.CardNum});
                        if (!string.IsNullOrWhiteSpace(CardNum))
                        {
                            r.ResultMsg = "此会员卡号已存在";
                        }
                        else
                        {
                            string strSql = @"INSERT INTO Member (CardNum,Name,Birthday,Money,Integral,PayType,Phone,Address,Creater,CreateTime) VALUES 
                                                (@CardNum,@Name,@Birthday,@Money,@Integral,@PayType,@Phone,@Address,@Creater,@CreateTime)";
                            var value = await conn.ExecuteAsync(strSql, new
                            {
                                memberInfo.CardNum,
                                memberInfo.Name,
                                memberInfo.Birthday,
                                memberInfo.Money,
                                memberInfo.Integral,
                                memberInfo.PayType,
                                memberInfo.Phone,
                                memberInfo.Address,
                                memberInfo.Creater,
                                CreateTime=DateTime.Now
                            });
                            if (value == 0)
                            {
                                r.ResultMsg = "新增会员信息失败";
                            }
                            else
                            {
                                r.Data = true;
                                r.ResultCode = 0;
                                transaction.Complete();
                            }
                        }
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
        /// 根据会员ID获取会员详细信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<MemberInfo>> GetMemberInfoByID(int ID)
        {
            VMResult<MemberInfo> r=new VMResult<MemberInfo>();
            try
            {
                using (var conn=DapperHelper.CreateConnection())
                {
                    string strSql = @"SELECT * FROM Member m WHERE m.ID=@ID";
                    var value = await conn.QueryFirstOrDefaultAsync<MemberInfo>(strSql, new
                    {
                        ID
                    });
                    if (value == null)
                    {
                        r.ResultMsg = "根据ID获取会员信息失败";
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
        /// 更新会员信息
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> UpdateMemberInfo(MemberInfo memberInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn = DapperHelper.CreateConnection())
                    {
                        string strSql = @"UPDATE Member set CardNum=@CardNum,Name=@Name,Birthday=@Birthday,Money=@Money,Integral=@Integral,PayType=@PayType,
                                            Phone=@Phone,Address=@Address,Updater=@Updater,UpdateTime=@UpdateTime  
                                            WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            memberInfo.CardNum,
                            memberInfo.Name,
                            memberInfo.Birthday,
                            memberInfo.Money,
                            memberInfo.Integral,
                            memberInfo.PayType,
                            memberInfo.Phone,
                            memberInfo.Address,
                            memberInfo.Updater,
                            UpdateTime=DateTime.Now,
                            memberInfo.ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "修改会员信息失败";
                        }
                        else
                        {
                            r.Data = true;
                            r.ResultCode = 0;
                            transaction.Complete();
                        }
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
        /// 删除会员信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> DeleteMemberInfo(int ID)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn=DapperHelper.CreateConnection())
                    {
                        string strSql = @"DELETE FROM Member WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "删除会员信息失败";
                        }
                        else
                        {
                            r.Data = true;
                            r.ResultCode = 0;
                            transaction.Complete();
                        }
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
