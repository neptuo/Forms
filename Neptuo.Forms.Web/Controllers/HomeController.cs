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
            return View(new ListDetailArticleModel(PagingHelper.TakePage(ArticleService.GetList(Thread.CurrentThread.CurrentUICulture).Select(a => new DetailArticleModel
                {
                    Title = a.Title,
                    Content = a.Content,
                    Modified = a.Modified,
                    AuthorFullname = a.Author.Fullname
                }), page, PageSize),
                PagingHelper.CreateInfo(ArticleService.GetList(Thread.CurrentThread.CurrentUICulture), page, PageSize)
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

        #region Examples

        [Url("examples")]
        public ActionResult Examples()
        {
            return View();
        }

        [Url("examples/basic")]
        public ActionResult ExamplesBasic()
        {
            return View("Examples");
        }

        [Url("examples/contact-form")]
        public ActionResult ExamplesContactForm()
        {
            return View();
        }

        [Url("examples/file-upload")]
        public ActionResult ExamplesFileUpload()
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
