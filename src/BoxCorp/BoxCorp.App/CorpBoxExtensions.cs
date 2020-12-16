using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using QuadTrees;

namespace BoxCorp.App
{
    public static class CorpBoxExtensions
    {
        public static List<CorpBox> FilterBoxes(this List<CorpBox> corpBoxes)
        {
            var sw = new Stopwatch();
            sw.Start();
            var quadTreeRect = new QuadTreeRect<CorpBox>();
            // Copies for ease of use/readability
            var resultsList = new List<CorpBox>(corpBoxes);
            resultsList = resultsList.Where(x => x.Rank >= 0.5).OrderByDescending(x => x.Rank).ToList();
            quadTreeRect.AddRange(resultsList);
            sw.Stop();
            Console.WriteLine($"QuadTree gen took: {sw.ElapsedMilliseconds}ms");
            var results = new List<CorpBox>(resultsList);
            var boxesToRemove = new List<CorpBox>();
            // Real bad performance but somewhat straight forward.
            foreach (var corpBox in resultsList)
            {
                // Already removed
                if (boxesToRemove.Contains(corpBox))
                    continue;
                var rectIntersectsWith = quadTreeRect.GetObjects(corpBox.Rect);

                // No intersections, keep
                if (rectIntersectsWith.Count == 0)
                    continue;

                foreach (var box in rectIntersectsWith)
                {
                    // Same instance
                    if (box == corpBox)
                        continue;
                    var jIndex = corpBox.CalculateJaccardIndex(box);
                    if (jIndex > 0.4 && !boxesToRemove.Contains(box))
                    {
                        boxesToRemove.Add(box);
                        quadTreeRect.Remove(box);
                    }
                }
            }

            // Remove all boxes to remove, leaves final list of boxes.
            foreach (var corpBox in boxesToRemove)
            {
                results.Remove(corpBox);
            }

            return results;
        }

        private static double Area(this Rectangle rect)
        {
            return rect.Height * rect.Width;
        }

        private static double CalculateJaccardIndex(this CorpBox boxOne, CorpBox boxTwo)
        {
            var intersection = Rectangle.Intersect(boxOne.Rect, boxTwo.Rect);
            return intersection.Area() / (boxOne.Rect.Area() + boxTwo.Rect.Area() - intersection.Area());
        }
    }
}