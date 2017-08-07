namespace uIntra.Core.Installer
{
    public interface IIntranetInstallationStep
    {
        string PackageName { get; }
        int Priority { get; }
        string Version { get; }
        void Execute();
    }
}
