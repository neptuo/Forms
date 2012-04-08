using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;

namespace Neptuo.Forms.Core.Service
{
    public class ArticleService : IArticleService
    {
        [Dependency]
        public IRepository<Article> Repository { get; set; }

        public IQueryable<Article> GetList(CultureInfo culture)
        {
            return Repository.Where(a => a.Culture == culture.TwoLetterISOLanguageName).OrderByDescending(a => a.ID);
        }

        public IQueryable<Article> GetList()
        {
            return Repository.OrderByDescending(a => a.ID);
        }

        public Article Get(int id)
        {
            return Repository.Get(id);
        }

        public CreateArticleStatus Create(string title, string content, CultureInfo culture, UserAccount author)
        {
            Article article = new Article
            {
                Title = title,
                Content = content,
                Culture = culture.TwoLetterISOLanguageName,
                AuthorUserID = author.ID,
                Modified = DateTime.Now
            };
            Repository.Insert(article);
            return CreateArticleStatus.Created;
        }

        public UpdateArticleStatus Update(int id, string title, string content)
        {
            Article article = Repository.Get(id);
            if (article != null)
            {
                article.Title = title;
                article.Content = content;
                article.Modified = DateTime.Now;
                Repository.Update(article);
                return UpdateArticleStatus.Updated;
            }
            return UpdateArticleStatus.NoSuchArticle;
        }

        public bool Delete(int id)
        {
            Article article = Repository.Get(id);
            if (article != null)
            {
                Repository.Delete(article);
                return true;
            }
            return false;
        }
    }
}
