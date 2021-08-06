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

        public async Task SplitVideoAsync(Clip clip, string filePath, string outputPath)
        {
            IConversion conversion = await FFmpeg.Conversions.FromSnippet.Split(filePath, Path.Combine(outputPath, clip.Name), clip.Start, clip.Duration);

            conversion = SetBitrates(conversion, clip, filePath);

            IConversionResult result = await conversion.Start();
        }

        public TimeSpan GetVideoLength(string filePath)
        {
            return FFmpeg.GetMediaInfo(filePath).Result.Duration;
        }

        public long GetVideoBitrate(string filePath)
        {
            return FFmpeg.GetMediaInfo(filePath).Result.VideoStreams.First().Bitrate;
        }
        public long GetAudioBitrate(string filePath)
        {
            return FFmpeg.GetMediaInfo(filePath).Result.AudioStreams.First().Bitrate;
        }

        private IConversion SetBitrates(IConversion conversion, Clip clip, string file)
        {
            long videoBitrate;
            long audioBitrate;
            
            videoBitrate = clip.VideoBitrate <= 0 ? 
                FFmpeg.GetMediaInfo(file).Result.VideoStreams.First().Bitrate : 
                clip.VideoBitrate;

            audioBitrate = clip.AudioBitrate <= 0 ?
                FFmpeg.GetMediaInfo(file).Result.AudioStreams.First().Bitrate :
                clip.AudioBitrate;

            conversion.SetVideoBitrate(videoBitrate);
            conversion.SetAudioBitrate(audioBitrate);

            return conversion;
        }
    }
}
