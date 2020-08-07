using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Mms_Server.Common;
using Mms_Server.Model.Goods;

namespace Mms_Server.DAL
{
    public class GoodsDal
    {
        /// <summary>
        /// 分页查询商品信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="goodsQueryInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<pageGoodsInfo>> GetPageGoodsInfo(int currentPage, int pageSize,
            GoodsQueryInfo goodsQueryInfo)
        {
            VMResult<pageGoodsInfo> r=new VMResult<pageGoodsInfo>();
            try
            {
                using (var conn = DapperHelper.CreateConnection())
                {
                    StringBuilder strSql=new StringBuilder();
                    StringBuilder strPageSql=new StringBuilder();
                    strSql.Append(@"SELECT g.* ,s.Name AS SupplierName FROM Goods g LEFT JOIN Supplier s ON s.ID=g.SupplierID WHERE 1 = 1");
                    if (!string.IsNullOrWhiteSpace(goodsQueryInfo.Name))
                    {
                        strSql.Append(" and g.Name='" + goodsQueryInfo.Name + "'");
                    }

                    if (!string.IsNullOrWhiteSpace(goodsQueryInfo.Code))
                    {
                        strSql.Append(" and g.Code='" + goodsQueryInfo.Code + "'");
                    }

                    if (goodsQueryInfo.SupplierId != 0)
                    {
                        strSql.Append(" and g.SupplierID='" + goodsQueryInfo.SupplierId + "'");
                    }

                    int total = (await conn.QueryAsync<GoodsInfo>(strSql.ToString())).Count();

                    int startNumber = (currentPage - 1) * pageSize;

                    strPageSql.Append(@"SELECT DATA.* FROM (" + strSql + ") DATA ORDER BY 1 OFFSET " + startNumber +
                                      "ROWS FETCH NEXT " + pageSize + "ROWS ONLY");
                    var value = await conn.QueryAsync<GoodsInfo>(strPageSql.ToString());
                    if (value == null)
                    {
                        r.ResultMsg = "查询商品信息失败";
                    }
                    else
                    {
                        r.Data.goodsInfos = value;
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
        /// <param name="addGoodsInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> AddGoodsInfo(GoodsInfo addGoodsInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn = DapperHelper.CreateConnection())
                    {
                        string strSql = @"INSERT INTO Goods (Name,Code,Spec,RetailPrice,PurchasePrice,StorageNum,SupplierID, Creater,CreateTime)
                                            VALUES (@Name,@Code,@Spec,@RetailPrice,@PurchasePrice,@StorageNum,@SupplierID,@Creater,@CreateTime)";
                        var value = await conn.ExecuteAsync(strSql,new
                        {
                            addGoodsInfo.Name,
                            addGoodsInfo.Code,
                            addGoodsInfo.Spec,
                            addGoodsInfo.RetailPrice,
                            addGoodsInfo.PurchasePrice,
                            addGoodsInfo.StorageNum,
                            addGoodsInfo.SupplierID,
                            addGoodsInfo.Creater,
                            CreateTime=DateTime.Now
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "新增商品信息失败";
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
        /// 根据ID获取商品信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<GoodsInfo>> GetGoodsInfoByID(int ID)
        {
            VMResult<GoodsInfo> r=new VMResult<GoodsInfo>();
            try
            {
                using (var conn = DapperHelper.CreateConnection())
                {
                    string strSql = @"SELECT g.*,s.Name AS SupplierName FROM Goods g
                                          LEFT JOIN Supplier s ON s.ID=g.SupplierID
                                          WHERE g.ID=@ID";
                    var value = await conn.QueryFirstOrDefaultAsync<GoodsInfo>(strSql, new
                    {
                        ID
                    });
                    if (value == null)
                    {
                        r.ResultMsg = "根据ID获取商品信息失败";
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
        /// 编辑商品信息
        /// </summary>
        /// <param name="updateGoodsInfo"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> UpdateGoodsInfo(GoodsInfo updateGoodsInfo)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn = DapperHelper.CreateConnection())
                    {
                        string strSql = @"UPDATE Goods SET Name=@Name,Code=@Code,Spec=@Spec,RetailPrice=@RetailPrice,PurchasePrice=@PurchasePrice
                                            ,StorageNum=@StorageNum,SupplierID=@SupplierID, Updater=@Updater,UpdateTime=@UpdateTime WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            updateGoodsInfo.Name,
                            updateGoodsInfo.Code,
                            updateGoodsInfo.Spec,
                            updateGoodsInfo.RetailPrice,
                            updateGoodsInfo.PurchasePrice,
                            updateGoodsInfo.StorageNum,
                            updateGoodsInfo.SupplierID,
                            updateGoodsInfo.Updater,
                            UpdateTime=DateTime.Now,
                            updateGoodsInfo.ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "编辑商品信息失败";
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
        /// 删除商品信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<VMResult<bool>> DeleteGoodsInfo(int ID)
        {
            VMResult<bool> r=new VMResult<bool>();
            r.Data = false;
            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn = DapperHelper.CreateConnection())
                    {
                        string strSql = @"DELETE FROM Goods WHERE ID=@ID";
                        var value = await conn.ExecuteAsync(strSql, new
                        {
                            ID
                        });
                        if (value == 0)
                        {
                            r.ResultMsg = "删除商品信息失败";
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
