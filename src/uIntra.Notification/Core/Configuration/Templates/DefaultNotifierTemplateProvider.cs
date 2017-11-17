using uIntra.Core.Activity;
using uIntra.Core.Extensions;

namespace uIntra.Notification.Configuration
{
    public class DefaultNotifierTemplateProvider : 
        IDefaultNotifierTemplateProvider<EmailNotifierTemplate>,
        IDefaultNotifierTemplateProvider<UiNotifierTemplate>
    {
        private readonly IDefaultTemplateReader _defaultTemplateReader;

        public DefaultNotifierTemplateProvider(IDefaultTemplateReader defaultTemplateReader)
        {
            _defaultTemplateReader = defaultTemplateReader;
        }

        public EmailNotifierTemplate GetTemplate(ActivityEventIdentity notificationType)
        {
            throw new System.NotImplementedException();
        }

        UiNotifierTemplate IDefaultNotifierTemplateProvider<UiNotifierTemplate>.GetTemplate(ActivityEventIdentity notificationType)
        {
            var dto = _defaultTemplateReader.ReadTemplate(notificationType).Deserialize<UiNotifierTemplateDto>();
            return new UiNotifierTemplate()
            {
                Message = dto.Message
            };
        }
    }

    internal class UiNotifierTemplateDto
    {
        public string Message { get; set; }
    }
}