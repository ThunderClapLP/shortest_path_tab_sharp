using System.Collections.Generic;
using System.Linq;

namespace ShortestPathTAB.Graph
{
    /// <summary>
    /// Simple vertex of a graph
    /// <example>
    /// <code>
    /// Vertex v = new Vertex('A');
    /// </code>
    /// </example>
    /// </summary>
    public class Vertex {
        public Vertex(char name) {
            this.name = name;
        }
        /// <summary> identifier of the vertex </summary>
        public readonly char name;

        public override string ToString()
        {
            return name.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || (obj.GetType() != this.GetType() && !obj.GetType().IsSubclassOf(this.GetType()) && !this.GetType().IsSubclassOf(obj.GetType()))) {
                return false;
            }
            return ((Vertex)obj).name == this.name;
        }

        public override int GetHashCode()
        {
            return (int)name;
        }
    }


    /// <summary>
    /// Simple (directional) edge of a graph
    /// <example>
    /// <code>
    /// Vertex a = new Vertex('A');
    /// Vertex b = new Vertex('B');
    /// Edge e = new Edge(a, b);
    /// </code>
    /// </example>
    /// </summary>
    public class Edge {
        public Edge(Vertex from, Vertex to, int cost) {
            this.from = from;
            this.to = to;
            this.cost = cost;
        }

        public readonly Vertex from;
        public readonly Vertex to;

        public int cost;

        public bool IsReverse(Edge edge) {
            if (edge == null) {
                return false;
            }
            return edge.to.Equals(this.from) && edge.from.Equals(this.to);
        }

        public override string ToString() {
            if (from == null || to == null) {
                return "invalid edge";
            } 
            return from.ToString() + "->" + to.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != this.GetType()) {
                return false;
            }
            return (((Edge)obj).from.Equals(this.from) && ((Edge)obj).to.Equals(this.to));
        }

        public override int GetHashCode()
        {
            return (int)from.name | ((int)to.name << 16);
        }
    }

    /// <summary>
    /// A (directed) graph
    /// </summary>
    public class Graph {
        private Dictionary<char, Vertex> vertices = new Dictionary<char, Vertex>();
        //private List<Vertex> vertices = new List<Vertex>();
        private List<Edge> edges = new List<Edge>();
        private Dictionary<Edge, Edge> edgeDict = new Dictionary<Edge, Edge>();

        public bool AddEdge(char from, char to, int cost, bool directional = true) {
            bool success = false;
            Vertex a = new Vertex(from);
            Vertex b = new Vertex(to);
            AddVertex(a);
            AddVertex(b);
            success = AddEdge(new Edge(a, b, cost));
            if (!directional) {
                if (AddEdge(new Edge(b, a, cost))) {
                    success = true;
                }
            }
            return success;
        }
        public bool AddEdge(Edge edge) {
            if (edge.from.Equals(edge.to)) {
                return false;
            }
            if (edgeDict.ContainsKey(edge)) {
                edgeDict[edge].cost = edge.cost;
                //edge is already in graph
                return false;
            }

            //check if edge exists in revers order
            if (edgeDict.ContainsKey(new Edge(edge.to, edge.from, edge.cost))) {
                //edge is now undirected
                int revIdx = edges.IndexOf(new Edge(edge.to, edge.from, edge.cost));
                edges.Insert(revIdx + 1, edge);
            } else {
                edges.Add(edge);
            }
            edgeDict.Add(edge, edge);
            return true;
        }

        public bool AddVertex(Vertex vertex) {
            if (vertices.ContainsKey(vertex.name)) {
                //vertex is already in graph
                return false;
            }
            vertices.Add(vertex.name, vertex);
            return true;
        }

        public bool RemoveEdge(char from, char to, bool directional = true) {
            bool success = false;
            Vertex a = new Vertex(from);
            Vertex b = new Vertex(to);

            success = RemoveEdge(new Edge(a, b, 0));
            if (!directional) {
                if (RemoveEdge(new Edge(b, a, 0))) {
                    success = true;
                }
            }

            //true if at least one edge is removed successfully
            return success;
        }

        public bool RemoveEdge(Edge edge, bool cleanVerts = true) {
            bool res = edges.Remove(edge) && edgeDict.Remove(edge);
            if (!res || !cleanVerts)
                return res;

            //remove vertices if no longer needed
            bool fromUsed = false;
            bool toUsed = false;
            for (int i = 0; i < edges.Count; i++) {
                if (edge.from.Equals(edges[i].from) || edge.from.Equals(edges[i].to)) {
                    fromUsed = true;
                }
                if (edge.to.Equals(edges[i].from) || edge.to.Equals(edges[i].to)) {
                    toUsed = true;
                }
            }

            //we can't use RemoveVertex here. would cause an infinite recursion
            if (!fromUsed)
                vertices.Remove(edge.from.name);
            if (!toUsed)
                vertices.Remove(edge.to.name);
            
            return true;
        }

        public bool RemoveVertex(Vertex v) {
            bool res = vertices.Remove(v.ToString()[0]);
            if (!res)
                return false;
            
            //remove all connected edges
            for (int i = 0; i < edges.Count; i++) {
                if (edges[i].from.Equals(v) || edges[i].to.Equals(v)) {
                    RemoveEdge(edges[i]);
                }
            }
            
            return true;
        }

        public void Clear() {
            vertices.Clear();
            edges.Clear();
            edgeDict.Clear();
        }

        public Vertex[] GetVertices() {
            return vertices.Values.ToArray();
        }

        public Edge[] GetEdges() {
            return edges.ToArray();
        }

        public List<Edge> GetAllNeighbours(Vertex v) {
            List<Edge> res = new List<Edge>();

            for (int i = 0; i < edges.Count; i++) {
                if (edges[i].from.Equals(v)) {
                    res.Add(edges[i]);
                }
            }

            return res;
        }

        public List<string> GetEdgeStrings() {
            List<string> edgeStrings = new List<string>();
            for (int i = 0; i < edges.Count; i++) {
                if (i < edges.Count - 1 && edges[i + 1].IsReverse(edges[i]) && edges[i].cost == edges[i + 1].cost) {
                    //edge is undirectional
                    edgeStrings.Add(edges[i].from + "<->" + edges[i].to + "," + edges[i].cost);
                    i++;
                } else {
                    //edge is directional
                    edgeStrings.Add(edges[i].ToString() + "," + edges[i].cost);
                }
            }
            return edgeStrings;
        }

        public string GetEdgesFormated(int indent = 0, int width = 50) {
            string res = "";
            List<string> edgeStrings = GetEdgeStrings();
            string currLine = "";
            for (int i = 0; i < edgeStrings.Count; i++) {
                if (edgeStrings[i].Length + currLine.Length + 2 > width) {
                    //add to res and clear currLine
                    res += currLine + "\n";
                    currLine = "";
                }
                if (currLine == "") {
                    //indent
                    for (int j = 0; j < indent; j++) {
                        currLine += " ";
                    }
                }
                currLine += ConsoleTools.ConsoleTools.FillString(edgeStrings[i], 8) + " ";
            }
            res += currLine; //add last line
            return res;
        }
    }
}