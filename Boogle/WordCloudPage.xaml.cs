using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using directoryPath;
using System.IO;

namespace Boogle
{
    /// <summary>
    /// Interaction logic for WordCloudPage.xaml
    /// </summary>
    public partial class WordCloudPage : Page
    {
        public WordCloudPage(string filePath)
        {
            InitializeComponent();

            // Set the image source in C#
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new System.Uri(filePath); // Replace with your image path
            bitmap.EndInit();

            myImage.Source = bitmap;
        }
    }
}
