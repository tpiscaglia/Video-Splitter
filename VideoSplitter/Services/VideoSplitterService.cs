using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VideoSplitter.Services
{
    public class VideoSplitterService : IVideoSplitterService
    {
        private readonly IFfmpegService _ffmpegService;

        public VideoSplitterService(IFfmpegService ffmpegService)
        {
            _ffmpegService = ffmpegService;
        }

        public void HandleTimeBasedSplitting(string file, string outputDir, long timeInterval)
        {
            CheckFileExists(file);

            if (timeInterval <= 0)
            {
                Console.WriteLine("Interval parameter is required when using the time base split method.");
                return;
            }
            if (string.IsNullOrEmpty(outputDir))
            {
                Console.WriteLine("Output parameter is required when using the time based split method.");
                return;
            }
            if (!Directory.Exists(outputDir))
            {
                //TODO: Maybe just create the directory instead of erroring if it doesn't exist?
                Console.WriteLine("Output directory does not exist");
                return;
            }

            //Do time based splitting
            Console.WriteLine("Time based splitting is not supported at this time.");
        }

        public void HandleFrameBasedSplitting(string file, string outputDir, long frameInterval)
        {
            CheckFileExists(file);

            if (frameInterval <= 0)
            {
                Console.WriteLine("Interval parameter is required when using the frame base split method.");
                return;
            }
            if (string.IsNullOrEmpty(outputDir))
            {
                Console.WriteLine("Output parameter is required when using the frame based split method.");
                return;
            }
            if (!Directory.Exists(outputDir))
            {
                //TODO: Maybe just create the directory instead of erroring if it doesn't exist?
                Console.WriteLine("Output directory does not exist");
                return;
            }

            //Do time based splitting
            Console.WriteLine("Frame based splitting is not supported at this time");
        }

        public void HandlejsonBasedSplitting(string jsonPath)
        {
            CheckFileExists(jsonPath);

            SplitInfo splitInfo = GetSplitInfoFromJson(jsonPath);

            FfmpegService service = new FfmpegService();


            foreach (var clip in splitInfo.clips)
            {
                Console.WriteLine($"Splitting clip {splitInfo.clips.IndexOf(clip) + 1} of {splitInfo.clips.Count}.");

                service.SplitVideoAsync(clip, splitInfo.input, splitInfo.output).Wait();
            }
        }

        private SplitInfo GetSplitInfoFromJson(string jsonPath)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SplitInfo>(File.ReadAllText(jsonPath));
        }

        private void CheckFileExists(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }
        }
    }
}
