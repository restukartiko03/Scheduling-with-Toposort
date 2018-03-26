using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Odading
{
    /// <summary>
    /// Interaction logic for CourseTree.xaml
    /// </summary>
    public partial class CourseTree : Window
    {
        private Canvas canvas;
        List<String> label;
        List<List<int>> adj;
        List<Tuple<int, int>> timestamp;
        List<int> semester;
        List<int> parent;
        private int counter;

        public CourseTree(
            List<String> label,
            List<List<int>> adj,
            List<Tuple<int, int>> timestamp,
            List<int> semester,
            List<int> parent)
        {
            InitializeComponent();

            this.canvas  = new Canvas();
            this.Title   = "Tree";
            this.Content = this.canvas;
            
            this.label = label;
            this.adj = adj;
            this.timestamp = timestamp;
            this.semester = semester;
            this.parent = parent;
        }

        public void Run()
        {
            DotBuilder dotBuilder = new DotBuilder(
                this.label, this.adj, this.timestamp, this.semester, this.parent
            );

            this.counter = 1;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(this.UpdateGraph);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();

        }

        public void UpdateGraph(Object sender, EventArgs e)
        {
            DotBuilder dotBuilder = new DotBuilder(
                this.label, this.adj, this.timestamp, this.semester, this.parent
            );
            this.DrawDot(dotBuilder.export(this.counter));
            this.counter++;
            if (this.counter > label.Count * 2 + 6)
            {
                this.counter = 0;
            }
        }
        
        private void DrawDot(String dotString)
        {
            Bitmap bitmap = FileDotEngine.Run(dotString);
            BitmapImage bitmapImage = BitmapToImageSource(bitmap);
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bitmapImage;
            this.canvas.Children.Clear();
            this.canvas.Children.Add(image);
            Canvas.SetTop(image, 20);
            Canvas.SetLeft(image, 20);
            
            this.Width = bitmapImage.PixelWidth + 60;
            this.Height = bitmapImage.PixelHeight + 80;
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
