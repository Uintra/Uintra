using Compent.uIntra.Core.Constants;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace Compent.uIntra.Core
{
    public class DocumentTypeProvider : IDocumentTypeAliasProvider
    {
        public virtual string GetNavigationComposition()
        {
            return DocumentTypeAliasConstants.NavigationComposition;
        }

        public virtual string GetContentPage()
        {
            return DocumentTypeAliasConstants.ContentPage;
        }

        public virtual string GetHeading()
        {
            return DocumentTypeAliasConstants.Heading;
        }

        public virtual string GetSearchResultPage()
        {
            return DocumentTypeAliasConstants.SearchResultPage;
        }

        public virtual string GetProfilePage()
        {
            return DocumentTypeAliasConstants.ProfilePage;
        }

        public virtual string GetProfileEditPage()
        {
            return DocumentTypeAliasConstants.ProfileEditPage;
        }

        public virtual string GetNotificationPage()
        {
            return DocumentTypeAliasConstants.NotificationPage;
        }

        public virtual string GetHomePage()
        {
            return DocumentTypeAliasConstants.HomePage;
        }

        public string GetErrorPage()
        {
            return DocumentTypeAliasConstants.ErrorPage;
        }

        public virtual string GetOverviewPage(IIntranetType type)
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

        public virtual string GetEditPage(IIntranetType type)
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

        public virtual string GetDetailsPage(IIntranetType type)
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

        public virtual string GetCreatePage(IIntranetType type)
        {
            switch (type.Id)
            {
                case (int)IntranetActivityTypeEnum.News: return DocumentTypeAliasConstants.NewsCreatePage;
                case (int)IntranetActivityTypeEnum.Events: return DocumentTypeAliasConstants.EventsCreatePage;
                default:
                    return null;
            }
        }

        public virtual string GetMailTemplateFolder()
        {
            return DocumentTypeAliasConstants.MailTemplatesFolder;
        }

        public virtual string GetMailTemplate()
        {
            return DocumentTypeAliasConstants.MailTemplate;
        }

        public virtual string GetDataFolder()
        {
            return DocumentTypeAliasConstants.DataFolder;
        }

        public virtual string GetSystemLink()
        {
            return DocumentTypeAliasConstants.SystemLink;
        }

        public virtual string GetSystemLinkFolder()
        {
            return DocumentTypeAliasConstants.SystemLinkFolder;
        }

        public virtual string GetGroupOverviewPage()
        {
            return DocumentTypeAliasConstants.GroupsOverviewPage;
        }

        public virtual string GetGroupCreatePage()
        {
            return DocumentTypeAliasConstants.GroupsCreatePage;
        }

        public virtual string GetGroupRoomPage()
        {
            return DocumentTypeAliasConstants.GroupsRoomPage;
        }

        public virtual string GetGroupEditPage()
        {
            return DocumentTypeAliasConstants.GroupsEditPage;
        }

        public virtual string GetGroupMyGroupsOverviewPage()
        {
            return DocumentTypeAliasConstants.GroupsMyGroupsOverviewPage;
        }

        public virtual string GetGroupDeactivatedPage()
        {
            return DocumentTypeAliasConstants.GroupsDeactivatedGroupPage;
        }
    }
}