using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VideoSplitter.Services
{
    public interface IFfmpegService
    {
        public Task SplitVideoAsync(Clip clip, string file, string outputDir);
    }
}
