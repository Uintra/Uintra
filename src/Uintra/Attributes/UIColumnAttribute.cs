﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra.Features.UserList.Models;

namespace Uintra.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class UIColumnAttribute : Attribute
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string DisplayName { get; set; }
        public ColumnType Type { get; set; }
        public string PropertyName { get; set; }
        public bool SupportSorting { get; set; }

        public UIColumnAttribute(int order, string backofficeDisplayName, string propertyName, ColumnType type, bool supportSorting = false, string alias = null)
        {
            Id = order;
            DisplayName = backofficeDisplayName;
            Type = type;
            PropertyName = propertyName;
            SupportSorting = supportSorting;
            Alias = alias ?? DisplayName?.Replace(" ", string.Empty);
        }
    }
}