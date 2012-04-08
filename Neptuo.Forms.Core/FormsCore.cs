using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;
using Neptuo.Web.DataAccess.EntityFramework;
using Neptuo.Forms.Core.DataAccess;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Core
{
    public static class FormsCore
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IRepository<UserAccount>, GenericRepository<UserAccount, DataContext>>()
                .RegisterType<IUserService, UserService>()
                .RegisterType<IRepository<Article>, GenericRepository<Article, DataContext>>()
                .RegisterType<IArticleService, ArticleService>()
            ;
        }
    }
}
