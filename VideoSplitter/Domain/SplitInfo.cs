using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSplitter
{
    public class SplitInfo
    {
        public string input { get; set; }
        public string output { get; set; }
        public List<Clip> clips { get; set; }
    }
}
