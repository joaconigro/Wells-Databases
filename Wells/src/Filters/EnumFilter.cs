﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using Wells.Base;
using Wells.Persistence.Repositories;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class EnumFilter<T> : BaseFilter<T>
    {
        public int Value { get; set; }

        readonly Type EnumType;
        public override string DisplayValue => Common.GetEnumDescription(EnumType, Value);

        public override bool IsDateRangeFilter => false;

        public override string Description
        {
            get
            {
                return $"{DisplayPropertyName} = {DisplayValue}";
            }
        }

        public EnumFilter(string propertyName, string displayName, IBussinessObjectRepository repo, int value, Type enumType) :
            base(propertyName, displayName, repo)
        {
            Value = value;
            EnumType = enumType;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   where (int)Interaction.CallByName(o, PropertyName, CallType.Get) == Value
                   select o;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            Value = filterViewModel.SelectedEnumValue;
        }
    }
}
