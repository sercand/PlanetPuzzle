using System.Collections.Generic;
using System.Reflection.Emit;
using Assets.Scripts.Temp;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameState state = GameState.Playing;
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private Planet planetPrefab2;
    [SerializeField] private Text forceText;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        CreatePlanet(planetPrefab, new Vector3(0, -1f));
        CreatePlanet(planetPrefab2, new Vector3(0, 1f));
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void CreatePlanet(Planet p, Vector3 start)
    {
        var planet = (Planet) Instantiate(p);
        planet.scaleRatio = 250;

        PuzzlePiece pp1 = new PuzzlePiece(), pp2 = new PuzzlePiece(), pp3 = new PuzzlePiece();
        var v1 = new Vector3(0, 0, 0);
        var v2 = new Vector3(512, 0, 0);
        var v3 = new Vector3(512, 240, 0);
        var v4 = new Vector3(512, 512, 0);
        var v5 = new Vector3(200, 512, 0);
        var v6 = new Vector3(0, 512, 0);
        var v7 = new Vector3(0, 182, 0);
        var v8 = new Vector3(200, 250, 0);

        var e1 = new Edge()
        {
            n1 = pp1,
            n2 = pp2,
            v1 = v7,
            v2 = v8
        };
        var e2 = new Edge()
        {
            n1 = pp1,
            n2 = pp3,
            v1 = v5,
            v2 = v8
        };
        var e3 = new Edge()
        {
            n1 = pp2,
            n2 = pp3,
            v1 = v3,
            v2 = v8
        };

        pp1.Vertices = new List<Vector3> {v6, v7, v8, v5};
        pp1.Triangenles = new List<int> {0, 1, 2, 2, 3, 0};
        pp1.Edges = new List<Edge> {e1, e2};

        pp2.Vertices = new List<Vector3> {v1, v2, v3, v8, v7};
        pp2.Triangenles = new List<int> {0, 1, 2, 2, 3, 0, 3, 4, 0};
        pp2.Edges = new List<Edge> {e1, e3};

        pp3.Vertices = new List<Vector3> {v3, v4, v5, v8,};
        pp3.Triangenles = new List<int> {0, 1, 2, 2, 3, 0};
        pp3.Edges = new List<Edge> {e2, e3};

        CreatePuzzlePiece(pp1, planet, start);
        CreatePuzzlePiece(pp2, planet, start);
        CreatePuzzlePiece(pp3, planet, start);
    }

    private GameObject CreatePuzzlePiece(PuzzlePiece piece, Planet planet, Vector3 initial)
    {
        var go = new GameObject();

        go.transform.SetParent(planet.transform, false);

        var ro = go.AddComponent<PlanetPiece>();
        go.AddComponent<Rigidbody2D>();
        ro.SetPiece(piece);
        ro.transform.localPosition += initial;
        return go;
    }

    private bool addForce = false;

    public void OnStartForce()
    {
        addForce = !addForce;
        foreach (var body in PhysicsManager.Instance.Bodies)
        {
            body.ApplyForceOthers = addForce;
        }
        forceText.text = addForce ? "StopForce" : "StartForce";
    }
}

public enum GameState
{
    Intro,
    Playing,
    GameOver,
    Win
}