namespace Uintra20.Features.CentralFeed
{
    public interface IFeedFilterStateService<T> where T : FeedFiltersState
    {
        void SaveFiltersState(T stateModel);
        T GetFiltersState();
        bool CentralFeedCookieExists();
    }
}