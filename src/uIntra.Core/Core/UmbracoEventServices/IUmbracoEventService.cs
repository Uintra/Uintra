namespace uIntra.Core.UmbracoEventServices
{
   public  interface IUmbracoEventService <in TSender, in TArgs>
    {
        void Process(TSender sender, TArgs args);
    }
}
