// #
// # Created by Sercan Degirmenci on 2015.01.24
// #

using System.Collections.Generic;
using Assets.Scripts.Temp;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PlanetCreator
    {
        public static PuzzlePiece[] Data3_1()
        {
            var r = new PuzzlePiece[3];

            PuzzlePiece pp1 = new PuzzlePiece(), pp2 = new PuzzlePiece(), pp3 = new PuzzlePiece();
            var v0 = new Vector3(0, 0, 0);
            var v1 = new Vector3(0, 0, 0);
            var v2 = new Vector3(512, 0, 0);
            var v3 = new Vector3(512, 240, 0);
            var v4 = new Vector3(512, 512, 0);
            var v5 = new Vector3(200, 512, 0);
            var v6 = new Vector3(0, 512, 0);
            var v7 = new Vector3(0, 182, 0);
            var v8 = new Vector3(200, 250, 0);
            var v9 = new Vector3(512, 240, 0);
            var v10 = new Vector3(512, 512, 0);
            var v11 = new Vector3(200, 512, 0);
            var v12 = new Vector3(0, 512, 0);
            var v13 = new Vector3(0, 182, 0);
            var v14 = new Vector3(200, 250, 0);

            var e1 = new Edge()
            {
                n1 = pp1,
                n2 = pp2,
                v1 = v13,
                v2 = v14
            };
            var e2 = new Edge()
            {
                n1 = pp1,
                n2 = pp3,
                v1 = v14,
                v2 = v4
            };
            var e3 = new Edge()
            {
                n1 = pp2,
                n2 = pp3,
                v1 = v14,
                v2 = v7
            };

            pp1.Vertices = new List<Vector3> {v14, v7, v8, v9, v10, v11, v12, v13};
            pp1.Triangenles = new List<int> {0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 7};
            pp1.Edges = new List<Edge> {e1, e2};

            pp2.Vertices = new List<Vector3> {v14, v13, v0, v1, v2, v3, v4};
            pp2.Triangenles = new List<int> {0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6};
            pp2.Edges = new List<Edge> {e1, e3};

            pp3.Vertices = new List<Vector3> {v14, v4, v5, v6, v7};
            pp3.Triangenles = new List<int> {0, 1, 2, 0, 2, 3, 0, 3, 4};
            pp3.Edges = new List<Edge> {e2, e3};

            return r;
        }
    }
}