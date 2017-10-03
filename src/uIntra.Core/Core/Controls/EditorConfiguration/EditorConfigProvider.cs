namespace uIntra.Core.Controls
{
    public class EditorConfigProvider : IEditorConfigProvider
    {
        public GridEditorConfig GetConfig(string editorAlias)
        {
            return new GridEditorConfig()
            {
                Config = new System.Collections.Generic.Dictionary<string, object>()
                { 
                    { "internalPicker", new { allowedAliases = new []{ "contentPage" } } }                   
                }
            };
        }
    }
}
