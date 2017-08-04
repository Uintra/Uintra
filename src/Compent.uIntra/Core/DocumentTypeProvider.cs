using System;
using Compent.uIntra.Core.Constants;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace Compent.uIntra.Core
{
    public class DocumentTypeProvider : IDocumentTypeAliasProvider
    {
        public string Get(string alias)
        {
            throw new NotImplementedException();
        }

        public string GetNavigationComposition()
        {
            return DocumentTypeAliasConstants.NavigationComposition;
        }

        public string GetContentPage()
        {
            return DocumentTypeAliasConstants.ContentPage;
        }

        public string GetSearchResultPage()
        {
            return DocumentTypeAliasConstants.SearchResultPage;
        }

        public string GetProfilePage()
        {
            return DocumentTypeAliasConstants.ProfilePage;
        }

        public string GetProfileEditPage()
        {
            return DocumentTypeAliasConstants.ProfileEditPage;
        }

        public string GetNotificationPage()
        {
            return DocumentTypeAliasConstants.NotificationPage;
        }

        public string GetHomePage()
        {
            return DocumentTypeAliasConstants.HomePage;
        }

        public string GetOverviewPage(IIntranetType type)
        {
            switch (type.Id)
            {
                case (int)IntranetActivityTypeEnum.News: return DocumentTypeAliasConstants.NewsOverviewPage;
                case (int)IntranetActivityTypeEnum.Events: return DocumentTypeAliasConstants.EventsOverviewPage;
                case (int)IntranetActivityTypeEnum.Bulletins: return DocumentTypeAliasConstants.BulletinsOverviewPage;
                default:
                    return null;
            }
        }

        public string GetEditPage(IIntranetType type)
        {
            switch (type.Id)
            {
                case (int)IntranetActivityTypeEnum.News: return DocumentTypeAliasConstants.NewsEditPage;
                case (int)IntranetActivityTypeEnum.Events: return DocumentTypeAliasConstants.EventsEditPage;
                case (int)IntranetActivityTypeEnum.Bulletins: return DocumentTypeAliasConstants.BulletinsEditPage;
                default:
                    return null;
            }
        }

        public string GetDetailsPage(IIntranetType type)
        {
            switch (type.Id)
            {
                case (int)IntranetActivityTypeEnum.News: return DocumentTypeAliasConstants.NewsDetailsPage;
                case (int)IntranetActivityTypeEnum.Events: return DocumentTypeAliasConstants.EventsDetailsPage;
                case (int)IntranetActivityTypeEnum.Bulletins: return DocumentTypeAliasConstants.BulletinsDetailsPage;
                default:
                    return null;
            }
        }

        public string GetCreatePage(IIntranetType type)
        {
            switch (type.Id)
            {
                case (int)IntranetActivityTypeEnum.News: return DocumentTypeAliasConstants.NewsCreatePage;
                case (int)IntranetActivityTypeEnum.Events: return DocumentTypeAliasConstants.EventsCreatePage;
                default:
                    return null;
            }
        }

        public string GetMailTemplateFolder()
        {
            return DocumentTypeAliasConstants.MailTemplatesFolder;
        }

        public string GetMailTemplate()
        {
            return DocumentTypeAliasConstants.MailTemplate;
        }

        public string GetDataFolder()
        {
            return DocumentTypeAliasConstants.DataFolder;
        }

        public string GetSystemLink()
        {
            return DocumentTypeAliasConstants.SystemLink;
        }

        public string GetSystemLinkFolder()
        {
            return DocumentTypeAliasConstants.SystemLinkFolder;
        }
    }
}