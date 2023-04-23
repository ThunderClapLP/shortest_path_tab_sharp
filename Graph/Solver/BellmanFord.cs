using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPathTAB.Graph.Solve
{
    public class BellmanFordVertex : Vertex {
        public BellmanFordVertex(Vertex vertex, BellmanFordVertex? parent = null, int steps = int.MaxValue) : base(vertex.name) {
            this.parent = parent;
            this.steps = steps;
        }
        public BellmanFordVertex? parent;
        public int steps;
    }
    public class BellmanFord : Solver
    {
        public BellmanFord(Graph g) : base(g)
        {
        }

        Dictionary<char, BellmanFordVertex> vertices = new Dictionary<char, BellmanFordVertex>();
        List<String[]> log = new List<string[]>();

        public override Vertex[] GetResultingPath(Vertex to)
        {
            List<Vertex> path = new List<Vertex>();
            int start = Array.IndexOf(g.GetVertices(), to);
            if (start != -1 && vertices.Count > start) {
                BellmanFordVertex curr = vertices[to.name];
                path.Add(curr);
                while (curr.parent != null && !curr.Equals(curr.parent)) {
                    curr = curr.parent;
                    path.Insert(0, curr); //reverse because we walk the path backwards here
                }
            }

            return path.ToArray();
        }

        public override bool Solve(Vertex start)
        {
            //reset
            vertices.Clear();
            log.Clear();
            Vertex[] vertArr = g.GetVertices();
            Edge[] edgeArr = g.GetEdges();
            foreach (Vertex v in vertArr) {
                vertices.Add(v.name, new BellmanFordVertex(v));
            }

            vertices[start.name].steps = 0;
            vertices[start.name].parent = vertices[start.name];
            for (int i = 0; i < vertArr.Length * 2 - 2; i++) {
                bool changed = false;
                foreach (Edge edge in edgeArr) {
                    BellmanFordVertex from = vertices[edge.from.name];
                    BellmanFordVertex to = vertices[edge.to.name];
                    if (from.steps != int.MaxValue && from.steps + edge.cost < to.steps || from.steps == int.MinValue) {
                        if (i < vertArr.Length - 1) {
                            //first v-1 iterations - get minimal stepsS
                            to.steps = from.steps + edge.cost;
                            to.parent = from;
                            changed = true;
                        } else {
                            //second v-1 iterations - detect circles
                            if (to.steps != int.MinValue)
                                changed = true;
                            to.steps = int.MinValue;
                        }
                    }
                }
                //build table content
                log.Add(new string[vertArr.Length + 1]);
                log[log.Count - 1][0] = "";
                //iteration column
                log[log.Count - 1][0] = i.ToString();
                //vertices
                for (int vertI = 0; vertI < vertArr.Length; vertI++) {
                    BellmanFordVertex v = vertices[vertArr[vertI].name];
                    if (v.parent == null || v.steps == int.MaxValue) {
                        log[log.Count - 1][vertI + 1] = "inf";
                    } else if (v.steps == int.MinValue) {
                        log[log.Count - 1][vertI + 1] = "-inf";
                    } else {
                        log[log.Count - 1][vertI + 1] = v.steps + "," + v.parent?.ToString();
                    }
                }
                if (!changed) {
                    //we already have the minimum distance
                    break;
                }
            }

            return true;
        }

        public override string GetSolvingTable()
        {
            if (log.Count <= 0) {
                //table is not yet calculated. Run Solve first
                return "failed";
            }

            string res = "";

            int firstSpace = 5; //iteration column width
            int space = 5;

            //header
            res += ConsoleTools.ConsoleTools.FillString("iter.", firstSpace);
            res += "|";
            BellmanFordVertex[] vertArr = vertices.Values.ToArray();
            for (int j = 1; j < log[0].Length; j++) {
                if (vertArr[j - 1].steps != 0) { //skip start vertex
                    res += ConsoleTools.ConsoleTools.FillString(vertArr[j - 1].ToString(), space);
                    res += "|";
                }
            }

            //content
            for (int i = 0; i < log.Count; i++) {
                res += "\n";
                for (int j = 0; j < log[i].Length; j++) {
                    if (j == 0 || vertArr[j - 1].steps != 0) { //skip start vertex
                        res += ConsoleTools.ConsoleTools.FillString(log[i][j], j == 0? firstSpace : space);
                        res += "|";
                    } 
                }
            }

            return res;
        }
    }
}