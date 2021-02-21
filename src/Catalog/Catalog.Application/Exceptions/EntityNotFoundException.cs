using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Application.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(
            Type entityType,
            Exception innerException = null) : this(entityType, Enumerable.Empty<object>(), innerException)
        {

        }

        public EntityNotFoundException(
            Type entityType,
            Exception innerException = null,
            params object[] searchParams) : this(entityType, searchParams, innerException)
        {

        }

        public EntityNotFoundException(
            Type entityType,
            IEnumerable<object> searchParams,
            Exception innerException = null) : base($"{entityType} not found", innerException)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            SearchParams = new List<object>(searchParams ?? Enumerable.Empty<object>());
        }

        public Type EntityType { get; }
        public List<object> SearchParams { get; }
    }
}
