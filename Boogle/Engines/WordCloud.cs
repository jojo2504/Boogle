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
            Player player,
            int maxOccurrence,
            Dictionary<string, int> wordOccurrences,
            int width = 800,
            int height = 600,
            int minimumCount = 1)
        {
            var filteredWords = wordOccurrences
                .Where(w => w.Value >= minimumCount)
                .OrderByDescending(w => w.Value)
                .ToList();
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {

                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Random random = new Random();

                Color[] colors = new Color[]
                {
                    Color.FromArgb(26, 188, 156),   
                    Color.FromArgb(46, 204, 113),   
                    Color.FromArgb(52, 152, 219),   
                    Color.FromArgb(155, 89, 182),   
                    Color.FromArgb(52, 73, 94)      
                };

                List<RectangleF> occupiedAreas = new List<RectangleF>();

                float fontSize = CalculateFontSize(maxOccurrence, maxOccurrence);

                using (Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold))
                    {
                        SizeF wordSize = graphics.MeasureString(player.Name, font);

                        PointF position = new PointF(200,230);

                        Color color = Color.FromArgb(0,0,0);
                        
                        using (SolidBrush brush = new SolidBrush(color))
                        {
                            graphics.DrawString($"{player.Name} won !", font, brush, position);

                            graphics.ResetTransform();

                            occupiedAreas.Add(new RectangleF(position, wordSize));
                        }
                    }

                foreach (var word in filteredWords)
                {
                    fontSize = CalculateFontSize(word.Value, maxOccurrence);
                    
                    using (Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold))
                    {

                        SizeF wordSize = graphics.MeasureString(word.Key, font);

                        PointF position = FindFreePosition(
                            occupiedAreas,
                            width,
                            height,
                            wordSize,
                            random);

                        Color color = colors[random.Next(colors.Length)];
                        
                        using (SolidBrush brush = new SolidBrush(color))
                        {

                            graphics.DrawString(word.Key, font, brush, position);

                            graphics.ResetTransform();

                            occupiedAreas.Add(new RectangleF(position, wordSize));
                        }
                    }
                }
            }

            return bitmap;
        }

        
        /// <summary>
        /// Calculate a proportional font size
        /// </summary>
        /// <param name="occurrence"></param>
        /// <param name="maxOccurrence"></param>
        /// <returns></returns>
        private static float CalculateFontSize(int occurrence, int maxOccurrence)
        {
            const float MIN_SIZE = 10f;
            const float MAX_SIZE = 60f;

            float size = MIN_SIZE +
                ((occurrence / (float)maxOccurrence) * (MAX_SIZE - MIN_SIZE));

            return size;
        }

        /// <summary>
        /// Try to find a position that isn't occupied
        /// </summary>
        /// <param name="occupiedAreas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="wordSize"></param>
        /// <param name="random"></param>
        /// <returns></returns>
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
                position = new PointF(
                    random.Next(0, width - (int)wordSize.Width),
                    random.Next(0, height - (int)wordSize.Height)
                );

                wordArea = new RectangleF(position, wordSize);

                if (!occupiedAreas.Any(a => a.IntersectsWith(wordArea)))
                    return position;

                attempts++;
            }
            while (attempts < 100);

            return new PointF(random.Next(width), random.Next(height));
        }

        /// <summary>
        /// Save the Bitmap as a png at the file path
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="filePath"></param>
        public static void SaveWordCloud(Bitmap bitmap, string filePath)
        {
            bitmap.Save(filePath);
        }
    }
}