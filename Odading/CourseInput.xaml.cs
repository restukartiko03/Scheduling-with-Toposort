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
using System.Windows.Shapes;

namespace Odading
{
    /// <summary>
    /// Interaction logic for CourseInput.xaml
    /// </summary>
    public partial class CourseInput : Window
    {
        const int ROW_HEIGHT = 30;
        const int COURSE_COLUMN_WIDTH = 100;
        const int DEPENDENCY_COLUMN_WIDTH = 200;
        const String SAMPLE_COURSE = "KU1101";
        const String SAMPLE_COURSE_DEPENDENCY = "KU1102,KU1103";

        Canvas canvas;
        Grid grid;
        Button addButton;
        List<String> course;
        List<String> courseDependency;

        public CourseInput(List<String> course, List<String> courseDependency)
        {
            InitializeComponent();

            canvas = new Canvas();
            grid = new Grid();
            this.course = course;
            this.courseDependency = courseDependency;
            this.gridInit();
            this.update();

            canvas.Children.Add(grid);
            Canvas.SetTop(grid, 75);
            Canvas.SetLeft(grid, 20);

            Button doneButton = new Button();
            doneButton.Content = "Done!";
            doneButton.Padding = new Thickness(5);
            doneButton.Click += new RoutedEventHandler(this.EndCourseInput);
            canvas.Children.Add(doneButton);
            Canvas.SetRight(doneButton, 20);
            Canvas.SetTop(doneButton, 20);

            Button loadButton = new Button();
            loadButton.Content = "Load From Files...";
            loadButton.Padding = new Thickness(5);
            loadButton.Click += new RoutedEventHandler(this.LoadFromFile);
            canvas.Children.Add(loadButton);
            Canvas.SetLeft(loadButton, 20);
            Canvas.SetTop(loadButton, 20);

            this.Width = 400;
            this.Height = 300;
            this.Content = canvas;
            this.Title = "Add Course";

            this.adjustHeight();
        }

        private void adjustHeight()
        {
            while (this.Height < (this.course.Count + 2) * CourseInput.ROW_HEIGHT + 150)
            {
                this.Height += CourseInput.ROW_HEIGHT;
            }
        }

        private void gridInit()
        {
            this.AddRow();
            this.AddRow();

            ColumnDefinition firstColumn = new ColumnDefinition();
            ColumnDefinition secondColumn = new ColumnDefinition();
            firstColumn.Width = new GridLength(CourseInput.COURSE_COLUMN_WIDTH);
            secondColumn.Width = new GridLength(CourseInput.DEPENDENCY_COLUMN_WIDTH);
            grid.ColumnDefinitions.Add(firstColumn);
            grid.ColumnDefinitions.Add(secondColumn);

            TextBlock columnHeader = new TextBlock();
            columnHeader.Text = "Course";
            columnHeader.FontSize = 14;
            columnHeader.FontWeight = FontWeights.Bold;
            columnHeader.Foreground = new SolidColorBrush(Colors.Black);
            columnHeader.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(columnHeader, 0);
            Grid.SetColumn(columnHeader, 0);

            addButton = new Button();
            addButton.Content = "Add New Course";
            addButton.Padding = new Thickness(5);
            addButton.Click += new RoutedEventHandler(this.AddCourse);
            Grid.SetRow(addButton, 1);
            Grid.SetColumn(addButton, 0);

            grid.Children.Add(columnHeader);
            grid.Children.Add(addButton);
        }

        private void AddRow()
        {
            RowDefinition generalRow = new RowDefinition();
            generalRow.Height = new GridLength(CourseInput.ROW_HEIGHT);
            grid.RowDefinitions.Add(generalRow);
        }

        public void AddCourse(object sender, EventArgs e)
        {
            this.course.Add(CourseInput.SAMPLE_COURSE);
            this.courseDependency.Add(CourseInput.SAMPLE_COURSE_DEPENDENCY);

            this.AddCourseRow(
                CourseInput.SAMPLE_COURSE,
                CourseInput.SAMPLE_COURSE_DEPENDENCY,
                this.course.Count
            );
        }

        private void AddCourseRow(String course, String courseDependency, int rowIndex)
        {
            this.AddRow();

            TextBox courseTextBox = new TextBox();
            courseTextBox.Uid = (this.course.Count - 1).ToString();
            courseTextBox.Text = course;
            courseTextBox.TextChanged += new TextChangedEventHandler(ChangeCourse);
            Grid.SetRow(courseTextBox, rowIndex);
            Grid.SetColumn(courseTextBox, 0);
            grid.Children.Add(courseTextBox);

            TextBox dependencyTextBox = new TextBox();
            dependencyTextBox.Uid = (this.course.Count - 1).ToString();
            dependencyTextBox.Text = courseDependency;
            dependencyTextBox.TextChanged += new TextChangedEventHandler(ChangeCourseDependency);
            Grid.SetRow(dependencyTextBox, rowIndex);
            Grid.SetColumn(dependencyTextBox, 1);
            grid.Children.Add(dependencyTextBox);

            Grid.SetRow(addButton, rowIndex + 1);

            this.adjustHeight();
        }

        public void ChangeCourse(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int id = Convert.ToInt32(textBox.Uid);
            this.course[id] = textBox.Text;
        }

        public void ChangeCourseDependency(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int id = Convert.ToInt32(textBox.Uid);
            this.courseDependency[id] = textBox.Text;
        }

        public void EndCourseInput(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void LoadFromFile(object sender, EventArgs e)
        {
            String tmp;
            int idx;
            String[] myString;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            myString = System.IO.File.ReadAllLines(dlg.FileName);

            
            this.course.Clear();
            this.courseDependency.Clear();
            for (int i=0; i<myString.Length; i++)
            {
                tmp = "";
                idx = 0;
                while(idx < myString[i].Length && myString[i][idx] != ',' && myString[i][idx] != '.')
                  tmp += myString[i][idx++];
                this.course.Add(tmp);
                tmp = "";
                idx += 2;
                for(;idx<myString[i].Length;idx++)
                  tmp += myString[i][idx];
                tmp = tmp.Replace('.',',').Replace(' ',',').Replace(",,",",");
                this.courseDependency.Add(tmp);
            }
            this.update();
        }

        private void update()
        {
            grid.Children.Clear();
            grid.RowDefinitions.RemoveRange(0, grid.RowDefinitions.Count);
            grid.ColumnDefinitions.RemoveRange(0, grid.ColumnDefinitions.Count);
            this.gridInit();

            for (int i=0; i<course.Count; i++)
            {
                this.AddCourseRow(
                    this.course[i],
                    this.courseDependency[i],
                    i+1
                );
            }
        }
    }
}
