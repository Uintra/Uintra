namespace uIntra.Core.Controls
{
    public interface IEditorConfigProvider
    {
        GridEditorConfig GetConfig(string editorAlias);
    }
}