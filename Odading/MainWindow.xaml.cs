using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Odading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<String> course;
        List<String> courseDependency;

        public MainWindow()
        {
            InitializeComponent();

            course = new List<String>();
            courseDependency = new List<string>();
            Canvas canvas = new Canvas();

            Button createGraphButton = new Button();
            createGraphButton.Content = "Input Course";
            createGraphButton.Padding = new Thickness(5);
            createGraphButton.Click += new RoutedEventHandler(this.CreateGraph);
            canvas.Children.Add(createGraphButton);
            Canvas.SetTop(createGraphButton, 50);
            Canvas.SetLeft(createGraphButton, 40);

            Button topoSortBFSButton = new Button();
            topoSortBFSButton.Content = "Find Tree with BFS";
            topoSortBFSButton.Uid = "BFS";
            topoSortBFSButton.Padding = new Thickness(5);
            topoSortBFSButton.Click += new RoutedEventHandler(this.TopoSort);
            canvas.Children.Add(topoSortBFSButton);
            Canvas.SetTop(topoSortBFSButton, 75);
            Canvas.SetLeft(topoSortBFSButton, 140);

            Button topoSortDFSButton = new Button();
            topoSortDFSButton.Content = "Find Tree with DFS";
            topoSortDFSButton.Uid = "DFS";
            topoSortDFSButton.Padding = new Thickness(5);
            topoSortDFSButton.Click += new RoutedEventHandler(this.TopoSort);
            canvas.Children.Add(topoSortDFSButton);
            Canvas.SetTop(topoSortDFSButton, 30);
            Canvas.SetLeft(topoSortDFSButton, 140);

            this.Height = 175;
            this.Width = 300;
            this.Content = canvas;
            this.Title = "Odading";
        }

        private void CreateGraph(object sender, EventArgs e)
        {
            CourseInput courseInputWindow = new CourseInput(this.course, this.courseDependency);
            courseInputWindow.Show();
        }

        private void TopoSort(object sender, EventArgs e)
        {
            List<String> label = new List<String>();
            List<List<int>> adj = new List<List<int>>();
            List<Tuple<int, int>> timestamp = new List<Tuple<int, int>>();
            List<int> semester = new List<int>();
            List<int> parent = new List<int>();
            Button senderButton = (Button) sender;

            if (senderButton.Uid.Equals("BFS"))
            {
                TopoSortBFS.Run(
                    this.course, this.courseDependency,
                    label, adj, timestamp, semester, parent
                );
            }
            else
            {
                TopoSortDFS.Run(
                    this.course, this.courseDependency,
                    label, adj, timestamp, semester, parent
                );
            }
            

            CourseTree courseTreeWindow = new CourseTree(
                label, adj, timestamp, semester, parent
            );
            courseTreeWindow.Show();
            courseTreeWindow.Run();
        }

    }
}
