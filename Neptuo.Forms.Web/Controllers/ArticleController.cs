using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using RiaLibrary.Web;
using Neptuo.Web.Mvc.Html;
using Neptuo.Web.Mvc.Models;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    [AuthorizeAdmin]
    public class ArticleController : BaseController
    {
        [Dependency]
        public IArticleService ArticleService { get; set; }

        [Url("admin/articles")]
        public ActionResult Index(int page = 1)
        {
            return View(new PageableModel<ListArticleModel>(PagingHelper.TakePage(ArticleService.GetList().Select(a => new ListArticleModel
                {
                    ID = a.ID,
                    Title = a.Title,
                    Culture = a.Culture,
                    Modified = a.Modified,
                    AuthorFullname = a.Author.Fullname
                }), page, PageSize),
                PagingHelper.CreateInfo(ArticleService.GetList(), page, PageSize)
            ));
        }

        [Url("admin/article/create")]
        public ActionResult Create()
        {
            return View("Edit", new EditArticleModel());
        }

        [Url("admin/article-{id}/edit")]
        public ActionResult Edit(int id)
        {
            Article article = ArticleService.Get(id);
            if (article != null)
            {
                return View(new EditArticleModel
                {
                    ID = article.ID,
                    Title = article.Title,
                    Content = article.Content,
                    Culture = article.Culture
                });
            }

            ShowMessage((L)"No such article", HtmlMessageType.Error);
            return RedirectToAction("index");
        }

        [HttpPost]
        [Url("admin/article-{id}/edit")]
        public ActionResult Edit(EditArticleModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsNew())
                {
                    CreateArticleStatus status = ArticleService.Create(model.Title, model.Content, CultureInfo.GetCultureInfo(model.Culture), UserContext.Account);
                    if (status == CreateArticleStatus.Created)
                    {
                        ShowMessage(String.Format((L)"Article '{0}' created.", model.Title));
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    UpdateArticleStatus status = ArticleService.Update(model.ID, model.Title, model.Content);
                    switch (status)
                    {
                        case UpdateArticleStatus.Updated:
                            ShowMessage(String.Format((L)"Article '{0}' updated.", model.Title));
                            break;
                        case UpdateArticleStatus.NoSuchArticle:
                            ShowMessage((L)"No such article!", HtmlMessageType.Error);
                            break;
                    }
                    return RedirectToAction("index");
                }
            }

            return View(model);
        }

        [HttpPost]
        [Url("admin/article-{id}/delete")]
        public ActionResult Delete(int id)
        {
            bool result = ArticleService.Delete(id);
            if (result)
                ShowMessage((L)"Article deleted.");
            else
                ShowMessage((L)"No such article!", HtmlMessageType.Error);

            return RedirectToAction("index");
        }
    }
}
