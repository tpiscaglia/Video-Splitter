using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSplitter
{
    public class Clip
    {
        public string Name { get; set; }
        public Mode Mode { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan Duration => End - Start;
        public long VideoBitrate { get; set; }
        public long AudioBitrate { get; set; }
    }

    public enum Mode
    {
        frame,
        time
    }
}
