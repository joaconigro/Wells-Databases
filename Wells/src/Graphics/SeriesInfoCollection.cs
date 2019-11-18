using System.Collections.Generic;

namespace Wells.View.Graphics
{
    public class SeriesInfoCollection
    {
        public string Title { get; set; }
        public List<SeriesInfo> Values { get; set; }

        public SeriesInfoCollection() { Values = new List<SeriesInfo>(); }

        public void Add(SeriesInfo item)
        {
            Values.Add(item);
        }

        public void Remove(SeriesInfo item)
        {
            Values.Remove(item);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
