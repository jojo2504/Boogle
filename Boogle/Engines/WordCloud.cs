using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;

namespace Boogle.Engines{

    public class WordCloudGenerator
    {
        /// <summary>
        /// Generates a word cloud from a dictionary of words and their occurrences
        /// </summary>
        /// <param name="wordOccurrences">Dictionary of words and their occurrences</param>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <param name="minimumCount">Minimum number of occurrences to appear</param>
        /// <returns>Bitmap containing the word cloud</returns>
        public static Bitmap CreateWordCloud(
            Dictionary<string, int> wordOccurrences,
            int width = 800,
            int height = 600,
            int minimumCount = 1)
        {
            // Filter words based on minimum threshold
            var filteredWords = wordOccurrences
                .Where(w => w.Value >= minimumCount)
                .OrderByDescending(w => w.Value)
                .ToList();

            // Create bitmap
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // White background
                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Calculate maximum occurrence
                int maxOccurrence = filteredWords.Any() ? filteredWords.Max(w => w.Value) : 1;

                // Random number generator
                Random random = new Random();

                // Color palette
                Color[] colors = new Color[]
                {
                    Color.FromArgb(26, 188, 156),   // Turquoise
                    Color.FromArgb(46, 204, 113),   // Emerald
                    Color.FromArgb(52, 152, 219),   // Platform Blue
                    Color.FromArgb(155, 89, 182),   // Amethyst
                    Color.FromArgb(52, 73, 94)      // Night Blue
                };

                // List to track occupied areas
                List<RectangleF> occupiedAreas = new List<RectangleF>();

                // Process each word
                foreach (var word in filteredWords)
                {
                    // Calculate proportional font size
                    float fontSize = CalculateFontSize(word.Value, maxOccurrence);
                    
                    // Choose font
                    using (Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold))
                    {
                        // Measure word size
                        SizeF wordSize = graphics.MeasureString(word.Key, font);

                        // Find non-overlapping position
                        PointF position = FindFreePosition(
                            occupiedAreas,
                            width,
                            height,
                            wordSize,
                            random);

                        // Random color
                        Color color = colors[random.Next(colors.Length)];
                        
                        using (SolidBrush brush = new SolidBrush(color))
                        {
                            // Optional rotation
                            //graphics.RotateTransform(random.Next(-30, 30));
                            
                            // Draw the word
                            graphics.DrawString(word.Key, font, brush, position);
                            
                            // Reset transformation
                            graphics.ResetTransform();

                            // Add occupied area
                            occupiedAreas.Add(new RectangleF(position, wordSize));
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Calculates font size based on occurrence
        /// </summary>
        private static float CalculateFontSize(int occurrence, int maxOccurrence)
        {
            // Minimum and maximum size
            const float MIN_SIZE = 10f;
            const float MAX_SIZE = 60f;

            // Proportional calculation
            float size = MIN_SIZE +
                ((occurrence / (float)maxOccurrence) * (MAX_SIZE - MIN_SIZE));

            return size;
        }

        /// <summary>
        /// Finds a free position without overlap
        /// </summary>
        private static PointF FindFreePosition(
            List<RectangleF> occupiedAreas,
            int width,
            int height,
            SizeF wordSize,
            Random random)
        {
            PointF position;
            RectangleF wordArea;
            int attempts = 0;

            do
            {
                // Random position
                position = new PointF(
                    random.Next(0, width - (int)wordSize.Width),
                    random.Next(0, height - (int)wordSize.Height)
                );

                wordArea = new RectangleF(position, wordSize);

                // Check for overlaps
                if (!occupiedAreas.Any(a => a.IntersectsWith(wordArea)))
                    return position;

                attempts++;
            }
            while (attempts < 100);

            // Fallback if too many attempts
            return new PointF(random.Next(width), random.Next(height));
        }

        /// <summary>
        /// Saves the word cloud
        /// </summary>
        public static void SaveWordCloud(Bitmap bitmap, string filePath)
        {
            bitmap.Save(filePath);
        }
    }

    // Usage example
    class Program
    {
        static void Main()
        {
            // Example word occurrences dictionary
            var wordOccurrences = new Dictionary<string, int>
            {
                {"Programming", 150},
                {"C#", 120},
                {"Development", 100},
                {"Artificial Intelligence", 80},
                {"Cloud", 70},
                {"Word Cloud", 60},
                {"Interface", 50},
                {"Algorithm", 40},
                {"Data", 30},
                {"Machine Learning", 20}
            };

            // Generate word cloud
            Bitmap cloud = WordCloudGenerator.CreateWordCloud(wordOccurrences);
            
            // Save the image
            WordCloudGenerator.SaveWordCloud(cloud, "word_cloud.png");
        }
    }
}