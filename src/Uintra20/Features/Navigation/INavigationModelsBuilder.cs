using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Navigation
{
    public interface INavigationModelsBuilder
    {
        TopNavigationModel GetTopNavigationModel();
    }
}