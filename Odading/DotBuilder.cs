using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odading
{
    class DotBuilder
    {
        List<String> label;
        List<List<int>> adj;
        List<Tuple<int,int>> timestamp;
        List<int> semester;
        List<int> parent;

        public DotBuilder()
        {
            this.label     = new List<String>();
            this.adj       = new List<List<int>>();
            this.timestamp = new List<Tuple<int,int>>();
            this.semester  = new List<int>();
            this.parent    = new List<int>();
        }

        public DotBuilder(
            List<String> label,
            List<List<int>> adj,
            List<Tuple<int,int>> timestamp,
            List<int> semester,
            List<int> parent)
        {
            this.label     = label;
            this.adj       = adj;
            this.timestamp = timestamp;
            this.semester  = semester;
            this.parent    = parent;
        }

        public String export(int step)
        {
            String dot = "digraph g {";

            for (int i=0; i<label.Count; i++)
            {
                dot += i.ToString() + " [label=\"" + label[i] + "\" shape=box ";
                if (timestamp[i].Item2 < step) {
                    dot += "color=blue";
                }
                else if (timestamp[i].Item1 < step)
                {
                    dot += "color=red";
                }
                dot += "]; ";
            }
            for (int i=0; i<adj.Count; i++)
            {
                for (int j=0; j<adj[i].Count; j++)
                {
                    dot += i.ToString() + " -> " + adj[i][j].ToString();
                    if (parent[adj[i][j]] == i)
                    {
                        if (timestamp[adj[i][j]].Item2 < step)
                        {
                            dot += " [color=blue]";
                        }
                        else if (timestamp[adj[i][j]].Item1 < step)
                        {
                            dot += " [color=red]";
                        }
                    }
                    dot += "; ";
                }
            }
            
            List<List<int>> rank = new List<List<int>>();
            for (int i=0; i<semester.Count; i++)
            {
                while (rank.Count <= semester[i])
                {
                    rank.Add(new List<int>());
                }

                rank[semester[i]].Add(i);
            }
            for (int i=0; i<rank.Count; i++)
            {
                if (rank[i].Count > 0)
                {
                    dot += "{ rank=same; ";
                    for (int j = 0; j < rank[i].Count; j++)
                    {
                        dot += rank[i][j] + " ";
                    }
                    dot += " } ";
                }
            }

            dot += "}";
            Debug.WriteLine(dot);
            return dot;
        }
    }
}
