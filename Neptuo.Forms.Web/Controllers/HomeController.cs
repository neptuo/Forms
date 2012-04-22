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
    //[OutputCache(Duration = Int32.MaxValue, VaryByParam="lang")]
    public class HomeController : BaseController
    {
        private int pageSize = 10;

        [Dependency]
        public IArticleService ArticleService { get; set; }

        #region Home

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult IndexWelcome()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult QuickNews()
        {
            return PartialView(new QuickListArticleModel(ArticleService.GetList(Thread.CurrentThread.CurrentUICulture).Take(5).Select(a => new DetailArticleModel
            {
                Title = a.Title,
                Content = a.Content,
                Modified = a.Modified,
                AuthorFullname = a.Author.Fullname
            })));
        }

        #endregion

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

        #region Learn

        [Url("learn")]
        public ActionResult Learn()
        {
            return View();
        }

        [Url("learn/rest")]
        public ActionResult LearnRestApi()
        {
            return View();
        }

        [Url("learn/webservice")]
        public ActionResult LearnWebService()
        {
            return View();
        }

        [Url("learn/javascript")]
        public ActionResult LearnJavascript()
        {
            return View();
        }

        #endregion

        [Url("features")]
        public ActionResult Features()
        {
            return View();
        }
    }
}
