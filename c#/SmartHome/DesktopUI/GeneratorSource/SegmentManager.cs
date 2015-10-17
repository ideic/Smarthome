using System.Collections.Generic;
using System.Linq;

namespace DesktopUI.GeneratorSource
{
    public class SegmentManager
    {
        private readonly List<Segment> _segments;

        public SegmentManager()
        {
            _segments = new List<Segment>();
        }
        public ICollection<Segment> Segments
        {
            get { return _segments; }
        }

        public void AddSegmentItem(Segment segment)
        {
            _segments.Add(segment);
        }

        public ICollection<BuildBlock> Switches
        {
            get { return _segments.SelectMany(segment => segment.Switches).ToList(); }
        }

        public ICollection<BuildBlock> Lightes
        {
            get { return _segments.SelectMany(segment => segment.Lights).ToList(); }
        } 


        public string SwitchNumbers
        {
            get { return Switches.Count.ToString();}
        }

        public string LightNumbers
        {
            get { return Lightes.Count.ToString(); }
        }
        
    }
}