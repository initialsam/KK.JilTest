using KK.JilTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KK.JilTest.Controllers
{
    public class JilTestController : ApiController
    {
        public IHttpActionResult Get()
        {
            var dt = DateTime.Now;
            return Ok(dt);
        }

        //public IHttpActionResult Get()
        //{
        //    List <User> bigList = new User().GenSimData();
        //    return Ok(bigList);
        //}

    }
}
