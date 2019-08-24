namespace Wells.YPFModel
{
    public struct ChemicalAnalysisType
    {
        public string Parameter;
        public string Group;
        public string Unit;
        public string Technique;

        public ChemicalAnalysisType(string parameter, string group, string unit, string technique)
        {
            Parameter = parameter;
            Group = group;
            Unit = unit;
            Technique = technique;
        }
    }
}