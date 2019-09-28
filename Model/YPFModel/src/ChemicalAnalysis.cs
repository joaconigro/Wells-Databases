using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Wells.StandardModel.Models;

namespace Wells.YPFModel
{
    public abstract class ChemicalAnalysis : BusinessObject
    {
        protected ChemicalAnalysis() : base() { }

        protected ChemicalAnalysis(Well well) : base() { Well = well; WellId = well.Id; }

        #region Properties
        [Required]
        [Browsable(false)]
        public string WellId { get; set; }


        [Browsable(true), DisplayName("Pozo")]
        public string WellName => Well?.Name;


        [DisplayName("Fecha"), Browsable(true)]
        public DateTime Date { get; set; }      
        #endregion


        #region Static properties
        static Dictionary<string, ChemicalAnalysisType> _FLNAAnalysisTypes;
        static Dictionary<string, ChemicalAnalysisType> _WaterAnalysisTypes;
        static Dictionary<string, ChemicalAnalysisType> _SoilAnalysisTypes;

        public static Dictionary<string, ChemicalAnalysisType> FLNAAnalysisTypes => _FLNAAnalysisTypes;

        public static Dictionary<string, ChemicalAnalysisType> WaterAnalysisTypes => _WaterAnalysisTypes;

        public static Dictionary<string, ChemicalAnalysisType> SoilAnalysisTypes => _SoilAnalysisTypes;


        public static void CreateParamtersDictionary()
        {
            _FLNAAnalysisTypes = ReadParametersFromResource(Properties.Resources.FLNA);
            _WaterAnalysisTypes = ReadParametersFromResource(Properties.Resources.Water);
            _SoilAnalysisTypes = ReadParametersFromResource(Properties.Resources.Soil);
        }


        static Dictionary<string, ChemicalAnalysisType> ReadParametersFromResource(string resource)
        {
            Dictionary<string, ChemicalAnalysisType> dict = new Dictionary<string, ChemicalAnalysisType>();
            using (var sr = new StringReader(resource))
            {
                string line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var split = line.Trim().Split(new char[] {(char)9}, StringSplitOptions.RemoveEmptyEntries);
                    dict.Add(split[1], new ChemicalAnalysisType(split[1], split[0], split[2], split[3]));
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







