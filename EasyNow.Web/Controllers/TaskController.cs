using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNow.Dto;
using EasyNow.Dto.Script;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyNow.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ScriptInfo> GetTaskInfoList()
        {
            return new List<ScriptInfo>();
        }
    }
}
