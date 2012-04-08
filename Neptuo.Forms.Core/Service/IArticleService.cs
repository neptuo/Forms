using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Neptuo.Forms.Core.Service
{
    public interface IArticleService
    {
        IQueryable<Article> GetList(CultureInfo culture);

        IQueryable<Article> GetList();

        Article Get(int id);

        CreateArticleStatus Create(string title, string content, CultureInfo culture, UserAccount author);

        UpdateArticleStatus Update(int id, string title, string content);

        bool Delete(int id);
    }

    public enum CreateArticleStatus
    {
        Created
    }

    public enum UpdateArticleStatus
    {
        Updated, NoSuchArticle
    }
}
