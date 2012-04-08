using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Neptuo.Web.Mvc.Html;
using Neptuo.Web.Mvc.Models;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    [Authorize]
    public class ArticleController : BaseController
    {
        private int pageSize = 10;

        [Dependency]
        public IArticleService ArticleService { get; set; }

        public ActionResult Index(int page = 1)
        {
            return View(new ListArticleModel(ArticleService.GetList().Select(a => new ListItemArticleModel
                {
                    ID = a.ID,
                    Title = a.Title,
                    Culture = a.Culture,
                    Modified = a.Modified,
                    AuthorFullname = a.Author.Fullname
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = ArticleService.GetList().Count()
                }
            ));
        }

        public ActionResult Create()
        {
            return View("Edit", new EditArticleModel());
        }

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
