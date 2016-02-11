using System;
using System.Web.Mvc;

namespace KK.JilTest.Controllers
{
    /// <summary>
    /// JilController
    /// </summary>
    public class HomeController : JilController
    {
        // GET: Home
        public ActionResult Index()
        {
            return Json(DateTime.Now, JsonRequestBehavior.AllowGet);

            //List<User> bigList = new User().GenSimData();
            //return Json(bigList, JsonRequestBehavior.AllowGet);
        }
    }

    ///// <summary>
    ///// JsonNetController
    ///// </summary>
    //public class JNetController : JsonNetController
    //{
    //    public ActionResult Index()
    //    {
    //        return Json(DateTime.Now, JsonRequestBehavior.AllowGet);

    //        //List<User> bigList = new User().GenSimData();
    //        //return Json(bigList, JsonRequestBehavior.AllowGet);
    //    }
    //}
}