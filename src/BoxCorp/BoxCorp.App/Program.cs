using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommandLine;
using ServiceStack;

namespace BoxCorp.App
{
    class Program
    {
        public class Options
        {
            [Option('f', "file", Required = false, HelpText = "Input csv file of boxes.", Default = "boxes.csv")]
            public string File { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (string.IsNullOrWhiteSpace(o.File))
                    {
                        Console.WriteLine("Must specify a file path for boxes csv input.");
                        return;
                    }

                    var absolutePath = Path.IsPathRooted(o.File)
                        ? o.File
                        : Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location), o.File);

                    if (!File.Exists(absolutePath))
                    {
                        Console.WriteLine("File missing, not found, please specify absolute path or local to binary.");
                        return;
                    }

                    // Assume file format is good, skipping validation checks for simplicity, but check if parsed successfully.
                    var boxes = absolutePath.ReadAllText().FromCsv<List<CorpBox>>();
                    var noBoxesFound = boxes == null || boxes.Count == 0;

                    if (noBoxesFound)
                    {
                        Console.WriteLine("Can't find boxes in specified file.");
                        return;
                    }

                    var sw = new Stopwatch();
                    sw.Start();
                    // Find all rectangles that intersect
                    var result = boxes.FilterBoxes();
                    sw.Stop();
                    Console.WriteLine(
                        $"Keeping {result.Count} boxes, filtered {boxes.Count - result.Count} in {sw.ElapsedMilliseconds}");
                });
        }
    }
}