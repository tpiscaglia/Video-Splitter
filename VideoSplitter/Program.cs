using System;
using System.IO;
using CommandLine;
using VideoSplitter.Services;
using Microsoft.Extensions.DependencyInjection;

namespace VideoSplitter
{
    public class Program
    {
        private static IVideoSplitterService service;

        public static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IFfmpegService, FfmpegService>()
                .AddSingleton<IVideoSplitterService, VideoSplitterService>()
                .BuildServiceProvider();

            service = serviceProvider.GetService<IVideoSplitterService>();

            Parser.Default.ParseArguments<HelpCommand, SplitCommand>(args)
                .WithParsed<ICommand>(x => x.Execute());
        }

        internal interface ICommand
        {
            void Execute();
        }

        [Verb("help", HelpText = "Display this help screen.")]
        internal class HelpCommand : ICommand
        {
            public void Execute()
            {

            }
        }

        [Verb("split", HelpText = "Splits the video file into multiple video files based on option parameters.")]
        internal class SplitCommand : ICommand
        {
            [Option('f', "file", Required = false, HelpText = "The fully qualified path to the video file to split.")]
            public string FilePath { get; set; }

            [Option('m', "mode", Required = true, HelpText = "The method to split the file.  Valid options are: time, frame, or json.")]
            public string SplitMethod { get; set; }
            [Option('j', "json", Required = false, HelpText = "The fully qualified path to the json file to read.")]
            public string jsonPath { get; set; }

            [Option('i', "interval", Required = false, HelpText = "The interval to split the file on.  Only applies to frame and time split methods.  Seconds for time based and frame numbers for frame based.")]
            public long Interval { get; set; }

            [Option('o', "output", Required = false, HelpText = "The directory to output to.  Only applies to frame and time split methods.")]
            public string OutputDir { get; set; }

            public void Execute()
            {
                Console.WriteLine($"Splitting video file {FilePath} using {SplitMethod} method.  Optional argument interval is set to {Interval}");

                if(SplitMethod.ToLower() != "json" && string.IsNullOrEmpty(FilePath))
                {
                    Console.WriteLine("File path is required if using a split method other than JSON.");
                }
                switch (SplitMethod.ToLower())
                {
                    case("time"):
                        service.HandleTimeBasedSplitting(FilePath, OutputDir, Interval);
                        break;
                    case ("frame"):
                        service.HandleFrameBasedSplitting(FilePath, OutputDir, Interval);
                        break;
                    case ("json"):
                        service.HandlejsonBasedSplitting(jsonPath);
                        break;
                    default:
                        Console.WriteLine("Invalid split method.  Valid methods are: time, frame, or json.");
                        return;
                }
            }
        }  
    }
}
