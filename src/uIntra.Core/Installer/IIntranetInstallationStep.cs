namespace uIntra.Core.Installer
{
    public interface IIntranetInstallationStep
    {
        string PackageName { get; }
        int Priority { get; }
        void Execute();
    }
}
