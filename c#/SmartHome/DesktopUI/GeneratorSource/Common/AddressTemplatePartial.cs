namespace DesktopUI.GeneratorSource.Common
{
    public partial class AddressTemplate
    {
        public AddressTemplate (SegmentManager segmentManager)
        {
            SegmentManager = segmentManager;
        }

        public SegmentManager SegmentManager
        {
            get; private set;
        }
    }
}
