using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Search.Elasticsearch;
using UBaseline.Search.Elasticsearch;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries.SearchByText
{
    public class SearchByTextSpecificationFactor : SearchByTextSpecificationFactory
    {
        public SearchByTextSpecificationFactor(IDependencyProvider dependencyProvider) : base(dependencyProvider)
        {
        }
    }
}