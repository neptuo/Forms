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
    public class FormsCore
    {
        public const string AdminFullname = "Forms Admin";
        public const string AdminEmail = "forms@neptuo.com";
        public const string AdminUsername = "admin";
        public const string AdminPassword = "f0rms.@dmin";

        public static FormsCore Instance { get; private set; }

        public UnityContainer UnityContainer { get; set; }

        public string BaseUrl { get; set; }

        public static void CreateInstance()
        {
            if (Instance != null)
                return;

            Instance = new FormsCore();
        }

        /// <summary>
        /// Register types for this assembly.
        /// </summary>
        /// <param name="container">Unity container isntance.</param>
        /// <param name="fileStorageType">File storage type.</param>
        /// <param name="loggerType">Logger type.</param>
        public void RegisterTypes(UnityContainer container)
        {
            (UnityContainer = container)
                .RegisterInstance<IMessageFormatter>(new DefaultMessageFormatter())
                .RegisterType<IActivityService, ActivityService>()
                .RegisterType<IRepository<UserAccount>, GenericRepository<UserAccount, DataContext>>()
                .RegisterType<IUserService, UserService>()
                .RegisterType<IRepository<Project>, GenericRepository<Project, DataContext>>()
                .RegisterType<IProjectService, ProjectService>()
                .RegisterType<IRepository<FormDefinition>, GenericRepository<FormDefinition, DataContext>>()
                .RegisterType<IRepository<FieldDefinition>, GenericRepository<FieldDefinition, DataContext>>()
                .RegisterType<IFormDefinitionService, FormDefinitionService>()
                .RegisterType<IRepository<FormData>, GenericRepository<FormData, DataContext>>()
                .RegisterType<IRepository<FieldData>, GenericRepository<FieldData, DataContext>>()
                .RegisterType<IFormDataService, FormDataService>()
                .RegisterType<IRepository<ProjectInvitation>, GenericRepository<ProjectInvitation, DataContext>>()
                .RegisterType<IInvitationService, InvitationService>()
                .RegisterType<ICleanUpService, DirectCleanUpService>()
                .RegisterType<IRepository<Article>, GenericRepository<Article, DataContext>>()
                .RegisterType<IArticleService, ArticleService>()
            ;
        }
    }
}
