namespace Uintra.Core.Permissions.Models
{
    public class IntranetMemberGroup
    {      
        public int Id { get; }
        public string Name { get; }

        public IntranetMemberGroup(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static bool operator ==(IntranetMemberGroup a, IntranetMemberGroup b) =>
            a.Id == b.Id;

        public static bool operator !=(IntranetMemberGroup a, IntranetMemberGroup b) =>
            a.Id != b.Id;

        public bool Equals(IntranetMemberGroup other) =>
            Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntranetMemberGroup other && Equals(other);
        }

        public override int GetHashCode() => Id;
    }
}
