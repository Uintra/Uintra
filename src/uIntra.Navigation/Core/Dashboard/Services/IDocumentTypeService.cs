namespace Uintra.Navigation.Dashboard
{
    public interface IDocumentTypeService
    {
        CreateDocumentTypeState Create(string parentIdOrAlias);
        CreateDocumentTypeState Create(int? parentId);
        DocumentTypeState Delete();
        bool IsExists();
    }
}