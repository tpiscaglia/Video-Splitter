using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void HandleTimeBasedSplitting(string file, string outputPath, long timeInterval)
        {
            CheckFileExists(file);

            if (timeInterval <= 0)
            {
                Console.WriteLine("Interval parameter is required when using the time base split method.");
                return;
            }
            if (string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine("Output parameter is required when using the time based split method.");
                return;
            }

            CheckFolderExists(outputPath);

            TimeSpan videoLength = _ffmpegService.GetVideoLength(file);
            SplitInfo splitInfo = new SplitInfo() { input = file, output = outputPath };

            for (long i = 0; i < videoLength.TotalSeconds; i = i + timeInterval)
            {
                long fileNumber = i / timeInterval + 1;
                string fileName = Path.GetFileName(file) + fileNumber;
                TimeSpan startTime = TimeSpan.FromSeconds(i);
                TimeSpan endTime = TimeSpan.FromSeconds(i + timeInterval);

                if (endTime > videoLength)
                {
                    endTime = videoLength;
                }

                Clip clip = new Clip()
                {
                    Start = startTime,
                    End = endTime,
                    Mode = Mode.time,
                    Name = fileName
                };

                splitInfo.clips.Add(clip);
            }

            SplitIntoClips(splitInfo);
        }

        public void HandleFrameBasedSplitting(string file, string outputPath, long frameInterval)
        {
            CheckFileExists(file);

            if (frameInterval <= 0)
            {
                Console.WriteLine("Interval parameter is required when using the frame base split method.");
                return;
            }
            if (string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine("Output parameter is required when using the frame based split method.");
                return;
            }
            if (!Directory.Exists(outputPath))
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

            SplitIntoClips(splitInfo);
        }

        private SplitInfo GetSplitInfoFromJson(string jsonPath)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SplitInfo>(File.ReadAllText(jsonPath));
        }

        private void SplitIntoClips(SplitInfo splitInfo)
        {
            FfmpegService service = new FfmpegService();

            foreach (var clip in splitInfo.clips)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.WriteLine($"{DateTime.Now} - Splitting clip {splitInfo.clips.IndexOf(clip) + 1} of {splitInfo.clips.Count}.");

                service.SplitVideoAsync(clip, splitInfo.input, splitInfo.output).Wait();
                stopwatch.Stop();

                Console.WriteLine($"{DateTime.Now} - Finished splitting clip {splitInfo.clips.IndexOf(clip) + 1} of of {splitInfo.clips.Count}.");
                Console.WriteLine($"Clip took {stopwatch.Elapsed} to split.");
            }
        }

        private void CheckFolderExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Folder {path} does not exist.");
            }
        }

        private void CheckFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
            }
        }
    }
}
