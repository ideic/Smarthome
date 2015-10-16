using System.Collections.Generic;

namespace DesktopUI.GeneratorSource.Server
{
    public partial class ServerTemplate
    {
        private readonly List<Segment> _segments;

        public ServerTemplate()
        {
            _segments = new List<Segment>();
        }
        public IEnumerable<Segment> Segments
        {
            get { return _segments; }
        }

        public void AddSegmentItem(Segment segment)
        {
            _segments.Add(segment);
        }
    }
}
