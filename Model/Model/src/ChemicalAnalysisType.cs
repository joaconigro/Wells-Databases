namespace Wells.Model
{
    public struct ChemicalAnalysisType
    {
        public string PropertyName { get; }
        public string Group { get; }
        public string Unit { get; }
        public string Technique { get; }

        public ChemicalAnalysisType(string propertyName, string group, string technique, string unit)
        {
            PropertyName = propertyName;
            Group = group;
            Unit = unit;
            Technique = technique;
        }
    }
}