using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace VideoSplitter.Services
{
    public class FfmpegService : IFfmpegService
    {
        public FfmpegService()
        {
            FFmpeg.SetExecutablesPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Resources");
        }

        public async Task SplitVideoAsync(Clip clip, string file, string outputDir)
        {
            IConversion conversion = await FFmpeg.Conversions.FromSnippet.Split(file, Path.Combine(outputDir, clip.Name), clip.Start, clip.Duration);
            conversion.SetVideoBitrate(FFmpeg.GetMediaInfo(file).Result.VideoStreams.First().Bitrate);
            conversion.SetAudioBitrate(FFmpeg.GetMediaInfo(file).Result.AudioStreams.First().Bitrate);
            IConversionResult result = await conversion.Start();
        }
    }
}
