namespace uCommunity.Navigation.Core.Dashboard
{
    public interface INavigationService
    {
        void CreateHomeNavigationComposition(string folderId);
        void CreateNavigationComposition(string folderId);

        bool IsNavigationCompositionExist();
        bool IsHomeNavigationCompositionExist();
    }
}