using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mms_Server.DAL;
using Mms_Server.Model.Goods;
using MSM_Server.Utility;
using Newtonsoft.Json;

namespace MSM_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogIActionFilterAttribute))]
    public class GoodsController : ControllerBase
    {
        /// <summary>
        /// 分页获取商品信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("GetPageGoodsInfo")]
        public async Task<string> GetPageGoodsInfo(string info)
        {
            pageGoodsQueryInfo pageGoodsQueryInfo = JsonConvert.DeserializeObject<pageGoodsQueryInfo>(info);
            GoodsDal dal=new GoodsDal();
            var result = await dal.GetPageGoodsInfo(pageGoodsQueryInfo.CurrentPage, pageGoodsQueryInfo.PageSize,
                pageGoodsQueryInfo.GoodsQueryInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now
            });
        }

        /// <summary>
        /// 新增商品信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("AddGoodsInfo")]
        public async Task<string> AddGoodsInfo(string info)
        {
            GoodsInfo goodsInfo = JsonConvert.DeserializeObject<GoodsInfo>(info);
            GoodsDal dal=new GoodsDal();
            var result = await dal.AddGoodsInfo(goodsInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now
            });
        }

        /// <summary>
        /// 根据商品ID获取商品详情
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("GetGoodsInfoByID")]
        public async Task<string> GetGoodsInfoByID(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "商品ID不能为空",
                    date = DateTime.Now
                });
            }
            int ID = Convert.ToInt32(info);
            GoodsDal dal = new GoodsDal();
            var result = await dal.GetGoodsInfoByID(ID);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now
            });
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPut("UpdateGoodsInfo")]
        public async Task<string> UpdateGoodsInfo(string info)
        {
            GoodsInfo goodsInfo = JsonConvert.DeserializeObject<GoodsInfo>(info);
            GoodsDal dal=new GoodsDal();
            var result = await dal.UpdateGoodsInfo(goodsInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now
            });
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpDelete("DeleteGoodsInfo")]
        public async Task<string> DeleteGoodsInfo(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "需要删除的商品ID不能为空",
                    date = DateTime.Now
                });
            }

            int ID = Convert.ToInt32(info);
            GoodsDal dal=new GoodsDal();
            var result = await dal.DeleteGoodsInfo(ID);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now
            });
        }
    }
}