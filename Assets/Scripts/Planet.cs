using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode, RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
public class Planet : MonoBehaviour
{
    [SerializeField] private Material material;
    public float scaleRatio = 100f;
    public PlanetBody Body;
    private List<Vector3> vertices;
    private List<Vector2> uvs;
    private List<Color> colors;
    private List<int> triangles;
    public readonly List<PlanetPiece> childs = new List<PlanetPiece>();
    private MeshFilter meshFilter;
    public bool BodyEnabled = false;
    private static int lastId = 0;
	private bool isComplete = false;

    public int Id;
    public void EnableBody()
    {
		Body.ParentPlanet = this;
        Body.gameObject.SetActive(true);
        BodyEnabled = true;
    }
    public int TextureWidth
    {
        get { return material.mainTexture.width; }
    }

    public int TextureHeight
    {
        get { return material.mainTexture.height; }
    }
	public delegate void  PlanetCompletedEventHandler(Planet planet);
	public delegate void  PlanetDestroyedEventHandler(Planet planet);
	public event PlanetCompletedEventHandler PlanetCompletedEvent;
	public event PlanetDestroyedEventHandler PlanetDestroyedEvent;

    private void Awake()
    {
        Id = ++lastId;
    }

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        var m = new Mesh();

        m.MarkDynamic();
        meshFilter.mesh = m;
        meshFilter.sharedMesh = m;
        renderer.material = material;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var vCount = VertexCount;
        vertices = new List<Vector3>(vCount);
        uvs = new List<Vector2>(vCount);
        colors = new List<Color>(vCount);
        triangles = new List<int>();
        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].Render(vertices, uvs, colors, triangles);
        }
        var mesh = meshFilter.sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
    }

    public void RegisterPiece(PlanetPiece o)
    {
        if (!childs.Contains(o)) childs.Add(o);
    }

    public void UnregisterPiece(PlanetPiece o)
    {
        if (childs.Contains(o)) childs.Remove(o);
    }
	public bool IsComplete()
	{
		foreach (var child in childs) {
			if(!child.IsGrabbed)		
			{
				if(isComplete)
				{
					isComplete = false;
					if(PlanetDestroyedEvent != null)
						PlanetDestroyedEvent (this);
					else
						Debug.Log("PlanetDestroyedEvent Null");
				}
				return false;
			}
				
		}
		isComplete = true;
		if(PlanetCompletedEvent != null)
			PlanetCompletedEvent (this);
		else
			Debug.Log("PlanetComplete Null");

		return true;
	}
    public int VertexCount
    {
        get
        {
            var c = 0;
            for (var i = 0; i < childs.Count; i++)
            {
                c += childs[i].VertexCount;
            }
            return c;
        }
    }
}