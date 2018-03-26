using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odading
{
    class TopoSortDFS
    {
        static bool[] Visited;
        static int[] Timestamp_1;
        static int[] Timestamp_2;
        static int stamp_counter;
        static List<int> indir = new List<int>();
        static List<List<int>> Adj;
        static List<int> Parent;
        public static void DFS(int node, int node_before)
        {
            Visited[node] = true;
            Timestamp_1[node] = stamp_counter++;
            Parent[node] = node_before;
            foreach (int neighbor in Adj[node])
                indir[neighbor]--;
            foreach (int neighbor in Adj[node])
                if (!Visited[neighbor] && indir[neighbor] == 0)
                    DFS(neighbor, node);
            Timestamp_2[node] = stamp_counter++;
        }
        public static void Run(
            List<String> course,
            List<String> courseDependency,
            List<String> label,
            List<List<int>> adj,
            List<Tuple<int, int>> timestamp,
            List<int> semester,
            List<int> parent)
        {
            stamp_counter = 1;
            Adj = adj;
            Parent = parent;
            List<Tuple<int, int>> tupleSemester = new List<Tuple<int, int>>();
            
            Timestamp_1 = new int[course.Count];
            Timestamp_2 = new int[course.Count];
            Visited = new bool[course.Count];

            for (int i = 0; i < course.Count; i++) Visited[i] = false;

            foreach (String course_i in course)
            {
                label.Add(course_i);
                Adj.Add(new List<int>());
            }

            for (int i = 0; i < courseDependency.Count; i++)
            {
                String[] dependency = courseDependency[i].Split(',');
                for (int j = 0; j < dependency.Length; j++)
                {
                    for (int k = 0; k < label.Count; k++)
                    {
                        if (label[k].Equals(dependency[j]))
                            Adj[k].Add(i);
                    }
                }
            }

            for (int i = 0; i < course.Count; i++)
            {
                Parent.Add(-1);
                semester.Add(1);
                indir.Add(0);
            }

            //calculate edge in
            foreach (List<int> x in adj)
            {
                foreach (int xx in x)
                    indir[xx]++;
            }

            //*************************************************************
            //Do the DFSfor (int i = 0; i < course.Count; i++)
            for (int i = 0; i < course.Count; i++)
            {
                if (indir[i] == 0 && !Visited[i])
                    DFS(i, -1);
            }

            for (int i = 0; i < course.Count; i++)
            {
                tupleSemester.Add(new Tuple<int, int>(Timestamp_2[i], i));
                timestamp.Add(new Tuple<int, int>(Timestamp_1[i], Timestamp_2[i]));
            }

            tupleSemester.Sort(Comparer<Tuple<int, int>>.Default);
            for (int i = 0; i < course.Count; i++)
                semester[tupleSemester[i].Item2] = course.Count - i;
            adj = Adj;
            parent = Parent;
        }

        public static void Run(
            List<String> label,
            List<List<int>> adj,
            List<Tuple<int, int>> timestamp,
            List<int> semester,
            List<int> parent)
        {
            // sample graph
            label.Add("IF2111");
            label.Add("IF2112");
            label.Add("IF2113");
            label.Add("IF2114");
            label.Add("IF2115");
            label.Add("IF2116");

            adj.Add(new List<int>());
            adj[0].Add(1);
            adj[0].Add(2);
            adj[0].Add(3);
            adj.Add(new List<int>());
            adj[1].Add(4);
            adj[1].Add(5);
            adj.Add(new List<int>());
            adj[2].Add(3);
            adj.Add(new List<int>());
            adj.Add(new List<int>());
            adj.Add(new List<int>());

            timestamp.Add(new Tuple<int, int>(1, 12));
            timestamp.Add(new Tuple<int, int>(2, 7));
            timestamp.Add(new Tuple<int, int>(8, 11));
            timestamp.Add(new Tuple<int, int>(9, 10));
            timestamp.Add(new Tuple<int, int>(3, 4));
            timestamp.Add(new Tuple<int, int>(5, 6));

            semester.Add(1);
            semester.Add(2);
            semester.Add(2);
            semester.Add(3);
            semester.Add(3);
            semester.Add(3);

            parent.Add(0);
            parent.Add(0);
            parent.Add(0);
            parent.Add(2);
            parent.Add(1);
            parent.Add(1);
            // end of sample graph
        }
    }
}
