using System;
using System.Collections.Generic;

namespace Uintra.Core.Context.Models
{
    public class ContextData
    {
        public Enum Type { get; set; }
        public Guid? EntityId { get; set; }

        public override bool Equals(object obj) =>
            obj is ContextData data &&
            EqualityComparer<Enum>.Default.Equals(Type, data.Type) &&
            EqualityComparer<Guid?>.Default.Equals(EntityId, data.EntityId);

        public override int GetHashCode()
        {
            var hashCode = 1264222156;
            hashCode = hashCode * -1521134295 + EqualityComparer<Enum>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid?>.Default.GetHashCode(EntityId);
            return hashCode;
        }
    }
}