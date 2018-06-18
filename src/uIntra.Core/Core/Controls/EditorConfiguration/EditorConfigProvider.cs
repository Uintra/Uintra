using System;
using System.Collections.Generic;
using uIntra.Core.Constants;

namespace uIntra.Core.Controls
{
    public abstract class EditorConfigProvider : IEditorConfigProvider
    {
        public GridEditorConfig GetConfig(string editorAlias)
        {
            switch (editorAlias)
            {
                case GridEditorConstants.ContentPanelAlias:
                    return GetForInternalLinkPicker();
                default:
                    throw new ArgumentOutOfRangeException(nameof(editorAlias));
            }
        }

        protected virtual GridEditorConfig GetForInternalLinkPicker()
        {
            return new GridEditorConfig()
            {
                Config = new Dictionary<string, object>
                {
                    { "linksList", new
                        {
                            internalPicker = new
                            {
                                allowedAliases = GetAllowedAliasesForInternalLinkPicker()
                            }
                        }
                    }
                }
            };
        }

        protected abstract IEnumerable<string> GetAllowedAliasesForInternalLinkPicker();
    }
}
