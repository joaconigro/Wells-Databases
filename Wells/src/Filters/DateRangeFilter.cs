using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using Wells.Persistence.Repositories;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class DateRangeFilter<T> : BaseFilter<T>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string DisplayValue => $"Entre {StartDate.ToString("dd/MM/yy")} y {EndDate.ToString("dd/MM/yy")}";

        public override bool IsDateRangeFilter => true;

        public DateRangeFilter(string propertyName, string displayName, IBussinessObjectRepository repo, DateTime startDate) :
            base(propertyName, displayName, repo)
        {
            StartDate = startDate;
            EndDate = DateTime.Today;
        }

        public DateRangeFilter(string propertyName, string displayName, IBussinessObjectRepository repo, DateTime startDate, DateTime endDate) :
           base(propertyName, displayName, repo)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   where CompareValue((DateTime)Interaction.CallByName(o, PropertyName, CallType.Get))
                   select o;
        }

        bool CompareValue(DateTime objectValue)
        {
            return objectValue >= StartDate && objectValue <= EndDate;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            StartDate = filterViewModel.StartDate;
            EndDate = filterViewModel.EndDate;
        }

        public override string Description => $"{DisplayPropertyName} entre {StartDate.ToString("dd/MM/yy")} y {EndDate.ToString("dd/MM/yy")}";
    }
}
