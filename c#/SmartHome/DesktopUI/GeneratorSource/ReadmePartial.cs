using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
