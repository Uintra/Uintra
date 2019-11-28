namespace Uintra20.Core.Feed.State
{
    public interface IFeedFilterStateService<T> where T : FeedFiltersState
    {
        void SaveFiltersState(T stateModel);
        T GetFiltersState();
        bool CentralFeedCookieExists();
    }
}