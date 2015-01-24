// #
// # Created by Sercan Degirmenci on 2015.01.23
// #

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PuzzlePiece
    {
        public int Length
        {
            get { return Vertices.Count; }
        }

        public List<Vector3> Vertices;
        public List<int> Triangenles;
        public List<Edge> Edges;
    }

    public class Edge
    {
        public Vector3 v1;
        public Vector3 v2;
        public PuzzlePiece n1;
        public PuzzlePiece n2;
    }
}