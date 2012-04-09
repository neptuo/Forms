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
using RiaLibrary.Web;

namespace Neptuo.Forms.Web.Controllers
{
    public class HomeController : BaseController
    {
        private int pageSize = 10;

        [Dependency]
        public IArticleService ArticleService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [Url("news")]
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

        [Url("about")]
        public ActionResult About()
        {
            return View();
        }

        [Url("learn")]
        public ActionResult Learn()
        {
            return View();
        }

        [Url("features")]
        public ActionResult Features()
        {
            return View();
        }
    }
}
