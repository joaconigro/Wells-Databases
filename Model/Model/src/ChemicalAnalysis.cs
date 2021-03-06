﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Wells.BaseModel.Attributes;
using Wells.BaseModel.Models;

namespace Wells.Model
{
    public abstract class ChemicalAnalysis : BusinessObject
    {
        protected ChemicalAnalysis() { }

        protected ChemicalAnalysis(Well well) { Well = well; WellId = well.Id; }

        #region Properties
        [Required]
        [Browsable(false)]
        public string WellId { get; set; }


        [Browsable(true), DisplayName("Pozo"), SortIndex(0)]
        public string WellName => Well?.Name;


        [DisplayName("Fecha"), Browsable(true), SortIndex(1)]
        public DateTime Date { get; set; }
        #endregion


        #region Static properties
        static Dictionary<string, ChemicalAnalysisType> flnaAnalysisTypes;
        static Dictionary<string, ChemicalAnalysisType> waterAnalysisTypes;
        static Dictionary<string, ChemicalAnalysisType> soilAnalysisTypes;

        public static Dictionary<string, ChemicalAnalysisType> FlnaAnalysisTypes => flnaAnalysisTypes;

        public static Dictionary<string, ChemicalAnalysisType> WaterAnalysisTypes => waterAnalysisTypes;

        public static Dictionary<string, ChemicalAnalysisType> SoilAnalysisTypes => soilAnalysisTypes;


        public static void CreateParamtersDictionary()
        {
            flnaAnalysisTypes = ReadParametersFromResource(Properties.Resources.FLNA);
            waterAnalysisTypes = ReadParametersFromResource(Properties.Resources.Water);
            soilAnalysisTypes = ReadParametersFromResource(Properties.Resources.Soil);
        }


        static Dictionary<string, ChemicalAnalysisType> ReadParametersFromResource(string resource)
        {
            Dictionary<string, ChemicalAnalysisType> dict = new Dictionary<string, ChemicalAnalysisType>();
            using (var sr = new StringReader(resource))
            {
                string line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var split = line.Trim().Split(new char[] { (char)9 }, StringSplitOptions.RemoveEmptyEntries);
                    dict.Add(split[1].ToLowerInvariant(), new ChemicalAnalysisType(split[1], split[0], split[2], split[3]));
                    line = sr.ReadLine();
                }
            }
            return dict;
        }


        #endregion


        #region Lazy-Load Properties
        /// <summary>
        /// The parent well.
        /// </summary>
        [ForeignKey("WellId")]
        [Browsable(false)]
        public virtual Well Well { get; set; }

        #endregion


        public abstract ChemicalAnalysisType GetChemicalAnalysisType(string propertyName);

        public override string ToString()
        {
            return Date.ToString(DateFormat);
        }

        public override int CompareTo(IBusinessObject other)
        {
            return Date.CompareTo((other as ChemicalAnalysis).Date);
        }
    }
}







