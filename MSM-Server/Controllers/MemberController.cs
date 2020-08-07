using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mms_Server.Common;
using Mms_Server.DAL;
using Mms_Server.Model;
using Mms_Server.Model.Member;
using MSM_Server.Utility;
using Newtonsoft.Json;

namespace MSM_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        /// <summary>
        /// 条件查询会员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMemberInfo")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        [Authorize]
        public async Task<string> GetMemberInfo(string info)
        {
            MemberQueryInfo memberQueryInfo=new MemberQueryInfo();
            if (!string.IsNullOrWhiteSpace(info))
            {
                memberQueryInfo = JsonConvert.DeserializeObject<MemberQueryInfo>(info);
            }
            MemberDal dal=new MemberDal();
            var result =await dal.GetMemberInfo(memberQueryInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    statue = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return JsonConvert.SerializeObject(new
            {
                status="success",
                data=result.Data,
                date=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        /// <summary>
        /// 分页查询会员信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("PageGetMemberInfo")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        [Authorize]
        public async Task<string> PageGetMemberInfo(string info)
        {
            PageMemberQueryInfo pageMemberQueryInfo =new PageMemberQueryInfo();
            MemberQueryInfo memberQueryInfo=new MemberQueryInfo();
            if (!string.IsNullOrWhiteSpace(info))
            {
                pageMemberQueryInfo = JsonConvert.DeserializeObject<PageMemberQueryInfo>(info);
            }

            if (pageMemberQueryInfo.memberQueryInfo != null)
            {
                memberQueryInfo = pageMemberQueryInfo.memberQueryInfo;
            }
            MemberDal dal = new MemberDal();
            var result = await dal.PageGetMemberInfo(pageMemberQueryInfo.CurrentPage, pageMemberQueryInfo.PageSize,
                memberQueryInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    statue = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        /// <summary>
        /// 新增会员信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("AddMemberInfo")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        [Authorize]
        public async Task<string> AddMemberInfo(string info)
        {
            MemberInfo memberInfo=new MemberInfo();
            if (!string.IsNullOrWhiteSpace(info))
            {
                memberInfo = JsonConvert.DeserializeObject<MemberInfo>(info);
            }
            MemberDal dal = new MemberDal();
            var result = await dal.AddMemberInfo(memberInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    statue = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        /// <summary>
        /// 根据会员ID获取会员详细信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpGet("GetMemberInfoByID")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        [Authorize]
        public async Task<string> GetMemberInfoByID(string info)
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
            MemberDal dal=new MemberDal();
            var result = await dal.GetMemberInfoByID(ID);
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
        /// 修改会员信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPut("AddMemberInfo")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        [Authorize]
        public async Task<string> UpdateMemberInfo(string info)
        {
            MemberInfo memberInfo = new MemberInfo();
            if (!string.IsNullOrWhiteSpace(info))
            {
                memberInfo = JsonConvert.DeserializeObject<MemberInfo>(info);
            }
            MemberDal dal = new MemberDal();
            var result = await dal.UpdateMemberInfo(memberInfo);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    statue = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        /// <summary>
        /// 删除会员信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMemberInfo")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        [Authorize]
        public async Task<string> DeleteMemberInfo(string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                return JsonConvert.SerializeObject(new
                {
                    statue = "fail",
                    message = "请选择要删除的行",
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            int ID = Convert.ToInt32(info);
            MemberDal dal = new MemberDal();
            var result = await dal.DeleteMemberInfo(ID);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    statue = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }
}