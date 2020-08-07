using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mms_Server.DAL;
using Mms_Server.Model.Supplier;
using MSM_Server.Utility;
using Newtonsoft.Json;

namespace MSM_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogIActionFilterAttribute))]
    public class SupplierController : ControllerBase
    {
        /// <summary>
        /// 分页查询供应商信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("PageGetSupplierInfo")]
        [Authorize]
        public async Task<string> PageGetSupplierInfo(string info)
        {
            PageSupplierQueryInfo pageSupplierQueryInfo=new PageSupplierQueryInfo();
            if (!string.IsNullOrWhiteSpace(info))
            {
                pageSupplierQueryInfo = JsonConvert.DeserializeObject<PageSupplierQueryInfo>(info);
            }
            SupplierDal dal = new SupplierDal();
            var result = await dal.PageGetSupplierInfo(pageSupplierQueryInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToShortDateString()
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToShortDateString()
            });
        }

        /// <summary>
        /// 新增供应商信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("AddSupplierInfo")]
        [Authorize]
        public async Task<string> AddSupplierInfo(string info)
        {
            SupplierInfo supplierInfo = JsonConvert.DeserializeObject<SupplierInfo>(info);
            SupplierDal dal=new SupplierDal();
            var result = await dal.AddSupplierInfo(supplierInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToShortDateString()
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToShortDateString()
            });
        }

        /// <summary>
        /// 根据供应商ID获取供应商详细信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("GetSupplierInfoByID")]
        [Authorize]
        public async Task<string> GetSupplierInfoByID(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "供应商ID不能为空",
                    date = DateTime.Now
                });
            }
            int ID = Convert.ToInt32(info);
            SupplierDal dal=new SupplierDal();
            var result = await dal.GetSupplierInfoByID(ID);
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
        /// 编辑供应商信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPut("UpdateSupplierInfo")]
        [Authorize]
        public async Task<string> UpdateSupplierInfo(string info)
        {
            SupplierInfo supplierInfo = JsonConvert.DeserializeObject<SupplierInfo>(info);
            SupplierDal dal=new SupplierDal();
            var result = await dal.UpdateSupplierInfo(supplierInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToShortDateString()
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToShortDateString()
            });
        }

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("deleteSupplierInfo")]
        [Authorize]
        public async Task<string> deleteSupplierInfo(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "供应商ID不能为空",
                    date = DateTime.Now.ToShortDateString()
                });
            }
            int ID = Convert.ToInt32(info);
            SupplierDal dal=new SupplierDal();
            var result = await dal.deleteSupplierInfo(ID);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToShortDateString()
                });
            }

            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToShortDateString()
            });
        }
    }
}