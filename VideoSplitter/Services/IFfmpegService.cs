using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VideoSplitter.Services
{
    public interface IFfmpegService
    {
        public Task SplitVideoAsync(Clip clip, string filePath, string outputDir);
        public TimeSpan GetVideoLength(string filePath);
        public double GetVideoFrameRate(string filePath);
        public double GetVideoTotalFrameCount(string filePath);
        public long GetVideoBitrate(string filePath);
        public long GetAudioBitrate(string filePath);
    }
}
