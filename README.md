A console application to display Dijkstra or the Bellman Ford algorithm step by step in a table.

## Disclaimer

The code is `not` optimized `at all`. But it's intended to be used on small graphs anyway, so it doesn't have to be.

Please do `not` use this in production `as is`.

## Building

Requires the [.Net 7 Sdk](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). But will propably compile just fine in 6 or 5.

If you simply want to run the application:

    dotnet run

Run following command to generate an executable:

    dotnet build

To target a specific platform use the `--os` parameter.
i.e. to build a linux executable:

    dotnet build --os linux

## Preview

After building your graph you can expect an output like the following.

Dijkstra:

    start vertex [A/B/C/D/F/H/E/G]: A
     |queue   |B    |C    |D    |F    |H    |E    |G    |
    A|B,C,    |1,A  |3,A  |inf  |inf  |inf  |inf  |inf  |
    B|C,D,E,  |1,A  |3,A  |2,B  |inf  |inf  |7,B  |inf  |
    D|C,E,F,  |1,A  |3,A  |2,B  |4,D  |inf  |7,B  |inf  |
    C|E,F,G,  |1,A  |3,A  |2,B  |4,D  |inf  |4,C  |7,C  |
    E|F,G,    |1,A  |3,A  |2,B  |4,D  |inf  |4,C  |6,E  |
    F|G,H,    |1,A  |3,A  |2,B  |4,D  |12,F |4,C  |6,E  |
    G|H,      |1,A  |3,A  |2,B  |4,D  |8,G  |4,C  |6,E  |
    H|        |1,A  |3,A  |2,B  |4,D  |8,G  |4,C  |6,E  |
    show path to vertex [A/B/C/D/F/H/E/G/back]: G
    A,C,E,G,

Bellman Ford:

    start vertex [A/B/C/D/F/H/E/G]: A
    iter.|B    |C    |D    |F    |H    |E    |G    |
    0    |1,A  |3,A  |2,B  |4,D  |8,G  |4,C  |6,E  |
    1    |1,A  |3,A  |2,B  |4,D  |8,G  |4,C  |6,E  |
    show path to vertex [A/B/C/D/F/H/E/G/back]: G
    A,C,E,G,

## Usage (of the graph editor)

Add one or multiple edges: `a [Vertex1](<)->[Vertex2],[Cost] ([VertexX](<)->[VertexY],[Cost] ...)`

`->` for directed edges and `<->` for undirected edges

Examples:

    a A->B,1
    a A<->B,9
    a A->B,2 B->C,1


Query used to create graph in preview section:

    a A<->B,1 A<->C,3 B<->D,1 D<->F,2 F<->H,8 B<->E,6 C<->E,1 C<->G,4 E<->G,2 F<->H,8 G<->H,2

Run `s` when you finished building your graph to start the solving algorithm.