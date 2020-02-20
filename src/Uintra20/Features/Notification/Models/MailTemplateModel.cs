using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Notification.Models
{
    public class MailTemplateModel : NodeModel
    {
        public PropertyModel<string> Subject { get; set; }
        public PropertyModel<string> Body { get; set; }
        public PropertyModel<string> ExtraTokens { get; set; }
        public PropertyModel<string> EmailType { get; set; }
        public PropertyModel<INodeModel> To { get; set; }
    }
}