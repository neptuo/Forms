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
        /// <summary>
        /// Register types for this assembly.
        /// </summary>
        /// <param name="container">Unity container isntance.</param>
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IActivityService, ActivityService>()
                .RegisterType<IRepository<UserAccount>, GenericRepository<UserAccount, DataContext>>()
                .RegisterType<IUserService, UserService>()
                .RegisterType<IRepository<Project>, GenericRepository<Project, DataContext>>()
                .RegisterType<IProjectService, ProjectService>()
                .RegisterType<IRepository<FormDefinition>, GenericRepository<FormDefinition, DataContext>>()
                .RegisterType<IRepository<FieldDefinition>, GenericRepository<FieldDefinition, DataContext>>()
                .RegisterType<IFormDefinitionService, FormDefinitionService>()
                .RegisterType<IRepository<FormData>, GenericRepository<FormData, DataContext>>()
                .RegisterType<IFormDataService, FormDataService>()
                .RegisterType<IRepository<Article>, GenericRepository<Article, DataContext>>()
                .RegisterType<IArticleService, ArticleService>()
            ;
        }
    }
}
