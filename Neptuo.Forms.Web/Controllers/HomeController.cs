using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Neptuo.Web.Mvc.Html;
using Neptuo.Web.Mvc.Models;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    public class HomeController : BaseController
    {
        private int pageSize = 10;

        [Dependency]
        public IArticleService ArticleService { get; set; }

        public ActionResult Index()
        {
            if(UserContext.IsAuthenticated())
                ShowMessage(String.Format((L)"Welcome back, {0}", UserContext.Account.Fullname), HtmlMessageType.Success);

            return View();
        }

        public ActionResult News(int page = 1)
        {
            return View(new ListDetailArticleModel(ArticleService.GetList(Thread.CurrentThread.CurrentUICulture).Select(a => new DetailArticleModel
                {
                    Title = a.Title,
                    Content = a.Content,
                    Modified = a.Modified,
                    AuthorFullname = a.Author.Fullname
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = ArticleService.GetList(Thread.CurrentThread.CurrentUICulture).Count()
                }
            ));
        }

        public ActionResult About()
        {
            return View();
        }

    }
}
