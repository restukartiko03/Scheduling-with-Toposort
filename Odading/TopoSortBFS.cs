using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odading
{
    class TopoSortBFS
    {
        public static void Run(
            List<String> course,
            List<String> courseDependency,
            List<String> label,
            List<List<int>> adj,
            List<Tuple<int, int>> timestamp,
            List<int> semester,
            List<int> parent)
        {
            Queue<int> queue = new Queue<int>();

            foreach (String course_i in course)
            {
                label.Add(course_i);
                adj.Add(new List<int>());
            }
            for (int i=0; i<courseDependency.Count; i++)
            {
                String[] dependency = courseDependency[i].Split(',');
                for (int j=0; j<dependency.Length; j++)
                {
                    for (int k=0; k<label.Count; k++)
                    {
                        if (label[k].Equals(dependency[j]))
                        {
                            adj[k].Add(i);
                        }
                    }
                }
            }

            int[] Timestamp_1 = new int[course.Count];
            int[] Timestamp_2 = new int[course.Count];

            List<int> indir = new List<int>();
            for (int i=0; i<course.Count; i++)
            {
                parent.Add(-1);
                semester.Add(1);
                indir.Add(0);
            }

            foreach (List<int> x in adj)
            {
                foreach (int xx in x)
                {
                    indir[xx]++;
                    Debug.WriteLine(xx + " indir : " + indir[xx]);
                }
            }
            int stamp_counter = 1;
            //**************************************************************
            //Push all Adj with no prerequisite into queue
            for (int i = 0; i < course.Count; i++)
            {
                if (indir[i] == 0)
                {
                    queue.Enqueue(i);
                    Timestamp_1[i] = stamp_counter++;
                }
            }

            //*************************************************************
            //Do the BFS
            int curr;
            while (queue.Count > 0)
            {
                curr = queue.Peek();
                queue.Dequeue();
                foreach (int neighbor in adj[curr])
                {
                    indir[neighbor]--;
                    if (indir[neighbor] == 0)
                    {
                        queue.Enqueue(neighbor);
                        Timestamp_1[neighbor] = stamp_counter++;
                        semester[neighbor] = semester[curr] + 1;
                        parent[neighbor] = curr;
                    }
                }
                Timestamp_2[curr] = stamp_counter++;
                Debug.WriteLine(curr + " Semester :" + semester[curr] + " Parent :" + parent[curr]);
                Debug.WriteLine(Timestamp_1[curr] + " fi\n" + Timestamp_2[curr] + " se\n");
            }

            //Parent
            for (int i = 0; i < course.Count; i++)
            {
                timestamp.Add(new Tuple<int, int>(Timestamp_1[i], Timestamp_2[i]));
            }
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
