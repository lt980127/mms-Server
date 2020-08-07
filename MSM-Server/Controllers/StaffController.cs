using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mms_Server.DAL;
using Mms_Server.Model.Staff;
using MSM_Server.Utility;
using Newtonsoft.Json;

namespace MSM_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogIActionFilterAttribute))]
    public class StaffController : ControllerBase
    {
        /// <summary>
        /// 分页获取员工信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("GetPageStaffInfo")]
        public async Task<string> GetPageStaffInfo(string info)
        {
            PageStaffQueryInfo pageStaffQueryInfo = JsonConvert.DeserializeObject<PageStaffQueryInfo>(info);
            StaffDal dal=new StaffDal();
            var result = await dal.GetPageStaffInfo(pageStaffQueryInfo.CurrentPage, pageStaffQueryInfo.PageSize,
                pageStaffQueryInfo.StaffQueryInfo);
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
        /// 新增员工信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("AddStaffInfo")]
        public async Task<string> AddStaffInfo(string info)
        {
            StaffInfo staffInfo = JsonConvert.DeserializeObject<StaffInfo>(info);
            StaffDal dal=new StaffDal();
            var result = await dal.AddStaffInfo(staffInfo);
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
        /// 根据员工ID获取员工详细信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("GetStaffInfoByID")]
        public async Task<string> GetStaffInfoByID(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "员工ID不能为空",
                    date = DateTime.Now
                });
            }
            int ID = Convert.ToInt32(info);
            StaffDal dal=new StaffDal();
            var result = await dal.GetStaffInfoByID(ID);
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
        /// 编辑员工信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPut("UpdateStaffInfo")]
        public async Task<string> UpdateStaffInfo(string info)
        {
            StaffInfo staffInfo = JsonConvert.DeserializeObject<StaffInfo>(info);
            StaffDal dal=new StaffDal();
            var result = await dal.UpdateStaffInfo(staffInfo);
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
        /// 删除员工信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpDelete("DeleteStaffInfo")]
        public async Task<string> DeleteStaffInfo(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "需要删除的员工ID不能为空",
                    date = DateTime.Now
                });
            }
            int ID = Convert.ToInt32(info);
            StaffDal dal=new StaffDal();
            var result = await dal.DeleteStaffInfo(ID);
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