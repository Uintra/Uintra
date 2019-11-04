namespace Uintra20.Core.Permissions.Models
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

        public static bool operator ==(IntranetMemberGroup a, IntranetMemberGroup b)
        {
            if (a is null && !(b is null)) return false;
            if (!(a is null) && b is null) return false;
            if (a is null && b is null) return true;
            return a.Id == b.Id;
        }

        public static bool operator !=(IntranetMemberGroup a, IntranetMemberGroup b)
        {
            if (a is null && !(b is null)) return true;
            if (!(a is null) && b is null) return true;
            if (a is null && b is null) return false;
            return a.Id != b.Id;
        }

        public bool Equals(IntranetMemberGroup other) =>
            !(other is null) && Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntranetMemberGroup other && Equals(other);
        }

        public override int GetHashCode() => Id;
    }
}