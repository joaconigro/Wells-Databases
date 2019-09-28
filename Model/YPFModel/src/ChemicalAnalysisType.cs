namespace Wells.YPFModel
{
    public struct ChemicalAnalysisType
    {
        public string PropertyName;
        public string Group;
        public string Unit;
        public string Technique;

        public ChemicalAnalysisType(string propertyName, string group, string technique, string unit)
        {
            PropertyName = propertyName;
            Group = group;
            Unit = unit;
            Technique = technique;
        }
    }
}