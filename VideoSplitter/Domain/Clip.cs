using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSplitter
{
    public class Clip
    {
        public string Name { get; set; }
        public Mode Mode { get; set; }
        //public string OutputDir { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan Duration => End - Start;
    }

    public enum Mode
    {
        frame,
        time
    }
}
