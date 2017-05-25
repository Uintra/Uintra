namespace uCommunity.Navigation.Core.Dashboard
{
    public class DocumentTypeState
    {
        public bool IsExists { get; set; }
    }

    public class CreateDocumentTypeState : DocumentTypeState
    {
        public bool IsUnknownParent { get; set; }
    }
}