﻿
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wells.Base;
using Wells.BaseView.Validators;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class NumericFilter<T> : BaseFilter<T>
    {
        public double Value { get; set; }

        public NumericFunctions Function { get; set; }
        public override string DisplayValue => Value.ToString("N2");

        public override bool IsDateRangeFilter => false;

        public NumericFilter() { }

        public NumericFilter(string propertyName, string displayName, double value, NumericFunctions function) :
            base(propertyName, displayName)
        {
            Value = value;
            Function = function;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   where CompareValue((double)Interaction.CallByName(o, PropertyName, CallType.Get))
                   select o;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            Value = filterViewModel.NumericValue;
            Function = (NumericFunctions)filterViewModel.SelectedMathFunction;
        }

        bool CompareValue(double objectValue)
        {
            return Function switch
            {
                NumericFunctions.Equal => objectValue.Equals(Math.Round(Value, 2)),
                NumericFunctions.Greater => objectValue > Value,
                NumericFunctions.GreaterOrEqual => objectValue >= Value,
                NumericFunctions.Lower => objectValue < Value,
                NumericFunctions.LowerOrEqual => objectValue <= Value,
                _ => false,
            };
        }


        public override string Description
        {
            get
            {
                return Function switch
                {
                    NumericFunctions.Equal => $"{DisplayPropertyName} = {DisplayValue}",
                    NumericFunctions.Greater => $"{DisplayPropertyName} > {DisplayValue}",
                    NumericFunctions.GreaterOrEqual => $"{DisplayPropertyName} >= {DisplayValue}",
                    NumericFunctions.Lower => $"{DisplayPropertyName} < {DisplayValue}",
                    _ => $"{DisplayPropertyName} <= {DisplayValue}",
                };
            }
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            Value = reader.ReadElementContentAsDouble();
            Function = reader.ReadContentAsEnum<NumericFunctions>();
            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.Write(nameof(Value), Value);
            writer.Write(nameof(Function), Function);
        }
    }
}
