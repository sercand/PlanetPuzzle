// #
// # Created by Sercan Degirmenci on 2015.01.23
// #

using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;


[RequireComponent(typeof (PolygonCollider2D))]
public class PlanetPiece : MonoBehaviour, IPhysicsObject
{
    public PuzzlePiece Piece;
    public bool IsDragging { get; set; }
    private bool m_grabbed=false;
    public bool IsGrabbed
    {
        get { return m_grabbed; }
    }

    private Planet m_planet;
    private Vector2[] uvs;
    public Vector3 mean;
    public bool ApplyForceOthers = false;
    public Planet Planet
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
            var v = transform.rotation*vertex;
            v += transform.position;
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

    public void Grab()
    {
        if (rigidbody2D != null)
        {
            IsDragging = false;
            m_grabbed = true;
            transform.SetParent(Planet.Body.transform, true);
            Destroy(GetComponent<Rigidbody2D>());
        }
    }

    //todo:
    public void BecomeFree()
    {
        
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

    public bool IsNeighbourWith(PlanetPiece other, out Edge edge)
    {
        for (int i = 0; i < Piece.Edges.Count; i++)
        {
            if (Piece.Edges[i].n1 == other.Piece || Piece.Edges[i].n2 == other.Piece)
            {
                edge = Piece.Edges[i];
                return true;
            }
        }
        edge = null;
        return false;
    }

    public Vector3 TransfromVertex(Vector3 v)
    {
        v /= Planet.scaleRatio;
        v -= mean;
        return v;
    }


    public PhysicsObjectType Type
    {
        get { return PhysicsObjectType.Piece; }
    }
}
