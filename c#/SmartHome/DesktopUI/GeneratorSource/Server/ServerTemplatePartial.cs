namespace DesktopUI.GeneratorSource.Server
{
    public partial class ServerTemplate
    {
        public ServerTemplate(SegmentManager segmentManager)
        {
            SegmentManager = segmentManager;
        }

        public SegmentManager SegmentManager
        {
            get; private set;
        }
    }
}
