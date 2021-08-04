using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSplitter.Services
{
    interface IVideoSplitterService
    {
        public void HandleTimeBasedSplitting(string file, string outputDir, long timeInterval);
        public void HandleFrameBasedSplitting(string file, string outputDir, long frameInterval);
        public void HandlejsonBasedSplitting(string jsonPath);

    }
}
