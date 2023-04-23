using System;
using System.IO;
using ShortestPathTAB.ConsoleTools;
using ShortestPathTAB.Graph;
using ShortestPathTAB.Graph.Solve;

namespace ShortestPathTAB
{
    class Program
    {
        static Graph.Graph g = new Graph.Graph();
        static bool skipClear = false;
        static void Main(string[] args)
        {
            //clear the screen and make sure we have a console window
            try { Console.Clear(); } catch (IOException) {Console.WriteLine("Not a console! TERMINATING"); return;};

            //Benchmark b = new Benchmark();
            //b.Run();
            //return;

            //main menue
            while (true) {
                if (!skipClear) {
                    Console.Clear(); //redundent at first iteration I know.
                    Console.WriteLine("Current graph:");
                    Console.WriteLine(g.GetEdgesFormated(indent: 10, width: ConsoleTools.ConsoleTools.DockRight()));
                }
                skipClear = false;

                string input = ConsoleTools.ConsoleTools.InputPrompt("graph editor", new string[] {"a", "r", "s", "c", "q", "help"});

                string[] splits = input.TrimEnd().Split(" ");
                string error = "";
                switch (splits[0]) {
                    case "a":
                        error = "";
                        for (int edges = 1; edges < splits.Length; edges++) {
                            string[] costSplit = splits[edges].Split(",");  //0:edge 1:cost
                            string[] dirEdge = costSplit[0].Split("->");    //0:vertex1 1:vertex2
                            string[] undirEdge = costSplit[0].Split("<->"); //0:vertex1 1:vertex2
                            int cost = 0;
                            if (costSplit.Length >= 2) {
                                int.TryParse(costSplit[1], out cost);
                            }
                            if (undirEdge.Length == 2 && undirEdge[0].Length == 1 && undirEdge[1].Length == 1) {
                                //undirectional edge
                                g.AddEdge(undirEdge[0][0], undirEdge[1][0], cost);
                                g.AddEdge(undirEdge[1][0], undirEdge[0][0], cost);
                            } else if (dirEdge.Length == 2 && dirEdge[0].Length == 1 && dirEdge[1].Length == 1 && undirEdge.Length < 2) {
                                //directional edge
                                g.AddEdge(dirEdge[0][0], dirEdge[1][0], cost);
                            } else if (splits[edges] != "") {
                                error = splits[edges];
                            }
                        }

                        if (error != "" || splits.Length <= 1) {
                            if (error == "")
                                error = "a";
                            Console.WriteLine("Invalid syntax at: \"" + error + "\". Usage: a [Vertex1](<)->[Vertex2],[Cost]");
                            Console.WriteLine("Example: \"a A->B,1\" or \"a A<->B,9\" or \"a A->B,2 B->C,1\"");
                            skipClear = true;
                        }
                        break;
                    case "r":
                        error = "";
                        for (int edges = 1; edges < splits.Length; edges++) {
                            string[] costSplit = splits[edges].Split(",");  //0:edge 1: anything else. We don't care. In case user forgets to delete the cost
                            string[] dirEdge = costSplit[0].Split("->");    //0:vertex1 1:vertex2
                            string[] undirEdge = costSplit[0].Split("<->"); //0:vertex1 1:vertex2
                            
                            if (undirEdge.Length == 2 && undirEdge[0].Length == 1 && undirEdge[1].Length == 1) {
                                //remove both directions
                                g.RemoveEdge(undirEdge[0][0], undirEdge[1][0]);
                                g.RemoveEdge(undirEdge[1][0], undirEdge[0][0]);
                            } else if (dirEdge.Length == 2 && dirEdge[0].Length == 1 && dirEdge[1].Length == 1 && undirEdge.Length < 2) {
                                //remove only one direction
                                g.RemoveEdge(dirEdge[0][0], dirEdge[1][0]);
                            } else if (splits[edges] != "") {
                                error = splits[edges];
                            }
                        }
                        if (error != "" || splits.Length <= 1) {
                            if (error == "")
                                error = "r";
                            Console.WriteLine("Invalid syntax at: \"" + error + "\". Usage: r [Vertex1](<)->[Vertex2]");
                            Console.WriteLine("Example: \"r A->B\" or \"r A<->B\" or \"r A->B B->C\"");
                            skipClear = true;
                        }
                        break;
                    case "s":
                        //select an algorithm
                        Console.WriteLine("Choose an algorithm:\n");
                        Console.WriteLine("1: Dijkstra");
                        Console.WriteLine("2: Bellman Ford");
                        Console.WriteLine("");

                        string algo = ConsoleTools.ConsoleTools.InputPrompt("algorithm", new string[] {"1-2"});
                        string[] verts = new string[g.GetVertices().Length];
                        for (int i = 0; i < verts.Length; i++) {
                            verts[i] = g.GetVertices()[i].ToString();
                        }
                        //select start vertex
                        string start = ConsoleTools.ConsoleTools.InputPrompt("start vertex", verts);

                        if (start.Length == 0 || Array.IndexOf(g.GetVertices(), new Vertex(start[0])) == -1){
                            Console.WriteLine("Invalid start index");
                            skipClear = true;
                            break;
                        }

                        Solver? solv = null;
                        switch (algo) {
                            case "1":
                                solv = new Dijkstra(g);
                                break;
                            case "2":
                                solv = new BellmanFord(g);
                                break;
                            default:
                                Console.WriteLine("Invalid algorithm");
                                skipClear = true;
                                break;
                        }
                        if (solv == null)
                            break;
                        
                        //perform solve and print table
                        solv.Solve(new Vertex(start[0]));
                        Console.WriteLine(solv.GetSolvingTable());
                        while (true) {
                            //prints path to the given points
                            string[] toArgs = new string[verts.Length + 1];
                            verts.CopyTo(toArgs, 0);
                            toArgs[toArgs.Length - 1] = "back";
                            string to = ConsoleTools.ConsoleTools.InputPrompt("show path to vertex", toArgs);
                            if (to == "back") {
                                //we are done here. Back to the graph editor
                                break;
                            }
                            if (to.Length == 0 || Array.IndexOf(g.GetVertices(), new Vertex(to[0])) == -1){
                                Console.WriteLine("Invalid index");
                            }
                            Vertex[] path = solv.GetResultingPath(new Vertex(to[0]));

                            for (int i = 0; i < path.Length; i++) {
                                Console.Write(path[i] + ",");
                            }
                            Console.WriteLine();

                        }
                        break;
                    case "c":
                        g.Clear();
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "help":
                        Console.WriteLine("a: Adds one or more edges");
                        Console.WriteLine("  Usage: a [Vertex1](<)->[Vertex2],[Cost]");
                        Console.WriteLine("  Example: \"a A->B,1\" or \"a A<->B,9\" or \"a A->B,2 B->C,1\"");

                        Console.WriteLine("r: Removes one or more edges");
                        Console.WriteLine("  Usage: r [Vertex1](<)->[Vertex2]");
                        Console.WriteLine("  Example: \"r A->B\" or \"r A<->B\" or \"r A->B B->C\"");

                        Console.WriteLine("s: Solves in specified algorithm");
                        Console.WriteLine("c: Clears the graph");
                        Console.WriteLine("q: Quits the program");
                        skipClear = true;
                        break;
                    default:
                        break;
                }

            }
        }
    }
}