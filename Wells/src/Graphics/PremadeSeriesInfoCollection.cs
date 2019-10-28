using System.Collections.Generic;

namespace Wells.View.Graphics
{
    public class PremadeSeriesInfoCollection
    {
        public string Title { get; set; }
        public List<PremadeSeriesInfo> Values { get; set; }

        public PremadeSeriesInfoCollection() { Values = new List<PremadeSeriesInfo>(); }

        public void Add(PremadeSeriesInfo item)
        {
            Values.Add(item);
        }

        public void Remove(PremadeSeriesInfo item)
        {
            Values.Remove(item);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
