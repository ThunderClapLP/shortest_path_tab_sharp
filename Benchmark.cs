using System;
using System.Diagnostics;
using ShortestPathTAB.Graph;
using ShortestPathTAB.Graph.Solve;

namespace ShortestPathTAB
{
    class Benchmark
    {
        public void Run() {
            Graph.Graph g = new Graph.Graph();
            Random rnd = new Random(100);
            int vertCount = (int)char.MaxValue;
            int edgePerVert = 2;

            Console.WriteLine("Building graph of " + vertCount + " verices and " + vertCount * edgePerVert + " edges...");

            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < vertCount; i++) {
                g.AddVertex(new Vertex((char)i));
            }

            Vertex[] vertices = g.GetVertices();
            foreach (Vertex v in vertices) {
                for (int i = 0; i < edgePerVert; i++) {
                    g.AddEdge(new Edge(v, vertices[rnd.Next(0, vertices.Length - 1)], rnd.Next(1, 30)));
                }
            }
            Console.WriteLine("Building graph took: " + s.ElapsedMilliseconds + "ms");
            s.Reset();
            s.Start();

            Solver solv = new Dijkstra(g);
            solv.Solve(vertices[0]);

            Console.WriteLine("solving took: " + s.ElapsedMilliseconds + "ms");

            System.IO.File.WriteAllText("Benchmark.txt", solv.GetSolvingTable());

        }
    }
}