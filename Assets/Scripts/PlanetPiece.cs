// #
// # Created by Sercan Degirmenci on 2015.01.23
// #

using System.Collections.Generic;
using Assets.Scripts.Temp;
using UnityEngine;


[RequireComponent(typeof (PolygonCollider2D), typeof (Rigidbody2D))]
public class PlanetPiece : MonoBehaviour
{
    public PuzzlePiece Piece;
    public bool IsDragging = false;
    private Planet m_planet;
    private Vector2[] uvs;
    private Vector3 mean;

    private Planet Planet
    {
        get { return m_planet ?? (m_planet = gameObject.GetComponentInParent<Planet>()); }
    }

    public int VertexCount
    {
        get { return Piece.Length; }
    }

    public void Render(List<Vector3> vertices, List<Vector2> uvList, List<Color> colors, List<int> triangles)
    {
        var c = vertices.Count;
        for (var i = 0; i < Piece.Vertices.Count; i++)
        {
            var vertex = Piece.Vertices[i];
            var v = transform.localRotation*vertex;
            v += transform.localPosition;
            vertices.Add(v);
            uvList.Add(uvs[i]);
            colors.Add(IsDragging ? Color.yellow : Color.white);
        }
        for (var i = 0; i < Piece.Triangenles.Count; i++)
        {
            triangles.Add(Piece.Triangenles[i] + c);
        }
    }

    private void OnEnable()
    {
        Planet.RegisterPiece(this);
        PhysicsManager.Instance.RegisterBody(this);
    }

    private void OnDisable()
    {
        Planet.UnregisterPiece(this);
        PhysicsManager.Instance.UnregisterBody(this);

    }

    public void SetPiece(PuzzlePiece p)
    {
        Piece = p;
        var col = GetComponent<PolygonCollider2D>();
        var path = new Vector2[p.Length];
        var ratio = Planet.scaleRatio;
        uvs = new Vector2[Piece.Length];
        var total = Vector3.zero;
        for (int i = 0; i < Piece.Length; i++)
        {
            var v = p.Vertices[i];
            uvs[i] = new Vector2(v.x/Planet.TextureWidth, v.y/Planet.TextureHeight);
            v /= ratio;
            total += v;
            p.Vertices[i] = v;
        }
        mean = total/Piece.Length;
        for (var i = 0; i < Piece.Length; i++)
        {
            Piece.Vertices[i] -= mean;
        }
        for (int i = 0; i < Piece.Length; i++)
        {
            var v = p.Vertices[i];
            path[i] = new Vector2(v.x, v.y);
        }
        col.SetPath(0, path);
        transform.localPosition = mean;
    }
}
