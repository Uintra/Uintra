namespace uIntra.Notification.Configuration
{
    /// <summary>
    /// Represents node in umbraco backoffice tree.
    /// </summary>
    public class TreeNodeModel
    {
        public string Id { get; }
        public string Name { get; }
        public string Icon { get; }
        public string ViewPath { get; }

        public TreeNodeModel(string id, string name, string icon, string viewPath)
        {
            Id = id;
            Name = name;
            Icon = icon;
            ViewPath = viewPath;
        }

        public TreeNodeModel WithViewPath(string viewPath) => new TreeNodeModel(Id, Name, Icon, viewPath);
        public TreeNodeModel WithId(string id) => new TreeNodeModel(id, Name, Icon, ViewPath);
        public TreeNodeModel WithIcon(string icon) => new TreeNodeModel(Id, Name, icon, ViewPath);
        public TreeNodeModel WithName(string name) => new TreeNodeModel(Id, name, Icon, ViewPath);
    }
}