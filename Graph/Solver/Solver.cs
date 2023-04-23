using System.Collections.Generic;

namespace ShortestPathTAB.Graph.Solve
{
    public abstract class Solver {
        public Solver(Graph g) {
            this.g = g;
        }

        /// <summary>
        /// The one and only graph
        /// </summary>
        internal Graph g;

        /// <summary>
        /// Solves the shortest path algorithm from the given start vertex.
        /// </summary>
        public abstract bool Solve(Vertex start);

        /// <summary>
        /// Gets the shortest path to the given vertex. (MUST RUN SOLVE FIRST)
        /// </summary>
        public abstract Vertex[] GetResultingPath(Vertex to);

        /// <summary>
        /// Gets a table to visualize the solving.
        /// </summary>
        public abstract string GetSolvingTable();
    }
}