using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPathTAB.Graph.Solve
{
    public class DijkstraVertex : Vertex {
        public DijkstraVertex(Vertex vertex, DijkstraVertex? parent = null, int steps = int.MaxValue) : base(vertex.name) {
            this.parent = parent;
            this.steps = steps;
        }
        public DijkstraVertex? parent;
        public int steps;
    }
    public class Dijkstra : Solver
    {
        public Dijkstra(Graph g) : base(g)
        {
        }

        Dictionary<char, DijkstraVertex> queue = new Dictionary<char, DijkstraVertex>();
        Dictionary<char, DijkstraVertex> vertices = new Dictionary<char, DijkstraVertex>();
        Dictionary<char, DijkstraVertex> visited = new Dictionary<char, DijkstraVertex>();
        List<String[]> log = new List<string[]>();

        public override Vertex[] GetResultingPath(Vertex to)
        {
            List<Vertex> path = new List<Vertex>();
            if (vertices.ContainsKey(to.name)) {
                DijkstraVertex curr = vertices[to.name];
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
            visited.Clear();
            log.Clear();
            Vertex[] vertArr = g.GetVertices();
            foreach (Vertex v in vertArr) {
                vertices.Add(v.name, new DijkstraVertex(v));
            }

            queue.Add(vertices[start.name].name, vertices[start.name]);
            vertices[start.name].steps = 0;
            vertices[start.name].parent = vertices[start.name];

            while (visited.Count < vertices.Count) {
                DijkstraVertex? curr = null;

                //select vertex with fewest steps
                foreach (KeyValuePair<char, DijkstraVertex> v in queue) {
                    if ((curr == null || v.Value.steps < curr.steps) && v.Value.steps != int.MaxValue) {
                        curr = v.Value;
                    }
                }
                if (curr == null) {
                    //can't find new vertex. Maybe not all vertices are connected?
                    break;
                }

                //update neighbours
                List<Edge> neighbours = g.GetAllNeighbours(curr);
                for (int i = 0; i < neighbours.Count; i++) {
                    DijkstraVertex v = vertices[neighbours[i].to.name];
                    if (curr.steps + neighbours[i].cost < v.steps) {
                        v.parent = curr;
                        v.steps = curr.steps + neighbours[i].cost;
                    }
                    if (!visited.ContainsKey(v.name) && !queue.ContainsKey(v.name)) {
                        queue.Add(v.name, v);
                    }
                }
                //we are done with this vertex
                queue.Remove(curr.name);
                visited.Add(curr.name, curr);

                //build table content
                log.Add(new string[vertArr.Length + 2]);
                //curr vertex
                log[log.Count - 1][0] += curr.ToString();
                //queue column
                log[log.Count - 1][1] = "";
                foreach (Vertex v in queue.Values) {
                    log[log.Count - 1][1] += v.ToString() + ",";
                }
                //vertices
                for (int vertI = 0; vertI < vertArr.Length; vertI++) {
                    DijkstraVertex v = vertices[vertArr[vertI].name];
                    if (v.parent == null || v.steps == int.MaxValue) {
                        log[log.Count - 1][vertI + 2] = "inf";
                    } else {
                        log[log.Count - 1][vertI + 2] = v.steps + "," + v.parent?.ToString();
                    }
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
            
            //calculate space needed for the queue column
            int queueSpace = 8; 
            for (int i = 0; i < log.Count; i++) {
                queueSpace = Math.Max(log[i][1].Length + 1, queueSpace);
            }

            int space = 5;

            //header
            res += ConsoleTools.ConsoleTools.FillString("", 1);
            res += "|";
            res += ConsoleTools.ConsoleTools.FillString("queue", queueSpace);
            res += "|";
            DijkstraVertex[] vertArr = vertices.Values.ToArray();
            for (int j = 2; j < log[0].Length; j++) {
                if (vertArr[j - 2].steps != 0) { //skip start vertex
                    res += ConsoleTools.ConsoleTools.FillString(vertArr[j - 2].ToString(), space);
                    res += "|";
                }
            }

            //content
            for (int i = 0; i < log.Count; i++) {
                res += "\n";
                for (int j = 0; j < log[i].Length; j++) {
                    if (j < 2 || vertArr[j - 2].steps != 0) { //skip start vertex
                        res += ConsoleTools.ConsoleTools.FillString(log[i][j], j == 1? queueSpace : j == 0? 1 : space);
                        res += "|";
                    } 
                }
            }

            return res;
        }
    }
}