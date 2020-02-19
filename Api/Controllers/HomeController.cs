using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestWebApi;

namespace Api.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : WebApi
    {
        /// <summary>
        /// index 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Index()
        {
            return 123;
        }
    }
}