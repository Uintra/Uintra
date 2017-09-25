namespace uIntra.Core.Links
{
    public interface IActivityLinks : IActivityCreateLinks
    {
        string Details { get; }
        string Edit { get; }
    }
}