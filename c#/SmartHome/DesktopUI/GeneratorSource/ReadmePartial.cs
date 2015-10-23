using System.Collections.Generic;

namespace DesktopUI.GeneratorSource
{
    public partial class Readme
    {
        public Readme(SegmentManager segmentManager, IEnumerable<Arduino> arduinos)
        {
            Segments = segmentManager.Segments;
            Arduinos = arduinos;
        }

        protected IEnumerable<Arduino> Arduinos { get; set; }

        protected ICollection<Segment> Segments { get; set; }
    }
}
