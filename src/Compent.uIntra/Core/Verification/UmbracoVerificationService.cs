using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Verification
{
    public class UmbracoVerificationService : IUmbracoVerificationService
    {
        private readonly IContentTypeService _contentTypeService;

        public UmbracoVerificationService(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        public void VerifyDocumnetTypes()
        {
            var missedContentTypes = CollectMissedContentTypes().ToList();

            if (missedContentTypes.Any())
            {
                var msg = missedContentTypes.JoinWith(Environment.NewLine);

                throw new Exception("Some content types are missed: " + Environment.NewLine + msg);
            }
        }

        private IEnumerable<string> CollectMissedContentTypes()
        {
            var types = CollectDocTypesClasses().ToList();
            var docTypes = CollectDocTypeAliasses(types).ToList();

            foreach (var dt in docTypes)
            {
                var contentType = _contentTypeService.GetContentType(dt);
                if (contentType == null)
                {
                    yield return dt;
                }
            }
        }

        private IEnumerable<Type> CollectDocTypesClasses()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            foreach (var type in assemblyTypes)
            {
                if (type.GetCustomAttributes(typeof(UmbracoDocumentTypeVerificationAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        private IEnumerable<string> CollectDocTypeAliasses(IEnumerable<Type> classes)
        {
            foreach (var @class in classes)
            {
                foreach (var field in @class.GetFields())
                {
                    yield return field.GetRawConstantValue().ToString();
                }
            }
        }
    }
}
