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
        public const string AdminFullname = "Forms Admin";
        public const string AdminEmail = "forms@neptuo.com";
        public const string AdminUsername = "admin";
        public const string AdminPassword = "f0rms.@dmin";

        /// <summary>
        /// Register types for this assembly.
        /// </summary>
        /// <param name="container">Unity container isntance.</param>
        /// <param name="fileStorageType">File storage type.</param>
        /// <param name="loggerType">Logger type.</param>
        public static void RegisterTypes(UnityContainer container, FileStorageType fileStorageType, LoggerType loggerType)
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
                .RegisterType<IRepository<ProjectInvitation>, GenericRepository<ProjectInvitation, DataContext>>()
                .RegisterType<IInvitationService, InvitationService>()
                .RegisterType<ICleanUpService, DirectCleanUpService>()
                .RegisterType<IRepository<Article>, GenericRepository<Article, DataContext>>()
                .RegisterType<IArticleService, ArticleService>()
            ;

            switch (fileStorageType)
            {
                case FileStorageType.Azure:
                    throw new NotImplementedException();
                case FileStorageType.FileSystem:
                    throw new NotImplementedException();
                case FileStorageType.Memory:
                    container.RegisterType<IFileStorage, MemoryFileStorage>();
                    break;
            }

            switch (loggerType)
            {
                case LoggerType.Azure:
                    throw new NotImplementedException();
                case LoggerType.Trace:
                    container.RegisterType<ILogger, TraceLogger>();
                    break;
            }
        }
    }

    public enum FileStorageType
    {
        Azure, FileSystem, Memory, Custom
    }

    public enum LoggerType
    {
        Azure, Trace, Custom
    }
}
