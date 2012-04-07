using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Web.Mvc.Html;

namespace Neptuo.Forms.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if(UserContext.IsAuthenticated())
                ShowMessage(String.Format((L)"Welcome back, {0}", UserContext.Account.Fullname), HtmlMessageType.Success);

            return View();
        }

    }
}
