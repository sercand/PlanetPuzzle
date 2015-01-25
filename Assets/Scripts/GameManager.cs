using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Planet planet1;
    [SerializeField] private Planet planet2;
	[SerializeField] private Meteor meteor;
    [SerializeField] private Text forceText;
    [SerializeField] private GameObject HomeScene;
    [SerializeField] private GameObject PlayScene;
    [SerializeField] private GameObject WinScene;
    [SerializeField] private GameObject GameOverScene;
	[SerializeField] private List<Planet> planetPrefabs;
	private Dictionary<int,Level> Levels;
	public Camera mainCamera;

    private GameScene scene = GameScene.Playing;
    private GameState state = GameState.Combine;
    public int CurrentLevelId = 0;
	public int TotalCompletedPlanets {get;set;}
	public Coroutine Spawner;
	private const int surviveFor = 10;
	public int SecondsToSurviveFor = surviveFor;
	
	public void HandlePlanetCompleted(Planet planet)
	{
		Debug.Log("HandlePlanetCompleted!");
		TotalCompletedPlanets++;
		if (TotalCompletedPlanets == Levels [CurrentLevelId].PlanetCount) {
			ChangeState(GameState.Survive);
		}
	}
	public void HandlePlanetDestroyed(Planet planet)
	{
		Debug.Log("HandlePlanetDestroyed!");
		TotalCompletedPlanets--;
		if (state == GameState.Survive) {
			ChangeState(GameState.Combine);
		}
	}
    private void Awake()
    {
	
        Instance = this;
		Levels = new Dictionary<int, Level> ();
		int levelIndex = 0;
		Levels.Add(levelIndex, new Level(levelIndex,2,new int[]{3,3}, 2.0f ));
		levelIndex++;
		Levels.Add(levelIndex, new Level(levelIndex,3,new int[]{3,3,3}, 2.0f ));
		levelIndex++;
		Levels.Add(levelIndex, new Level(levelIndex,4,new int[]{3,3,3,3}, 2.0f ));


    }

    // Use this for initialization
    private void Start()
    {
		ChangeScene (GameScene.Home);

    }
	private void clearScenes()
	{
		HomeScene.gameObject.SetActive(false);
		PlayScene.gameObject.SetActive(false);
		WinScene.gameObject.SetActive(false);
		GameOverScene.gameObject.SetActive(false);
	}
    // Update is called once per frame
    private void Update()
    {

    }
	public void CheckAllPlanetsCompleted()
	{
		
	}
	public void onReadyClicked()
	{
		ChangeScene (GameScene.Playing);
	}

    public void CreatePlanet(Planet p, Vector3 start)
    {
        var planet = (Planet) Instantiate(p);

		planet.PlanetCompletedEvent += HandlePlanetCompleted;
		planet.PlanetDestroyedEvent += HandlePlanetDestroyed;
		planetPrefabs.Add (planet);

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

    public void LoadLevel(int levelID)
    {
        CurrentLevelId = levelID;
		Level level;
		if(!Levels.TryGetValue (CurrentLevelId, out level))
		{
			Debug.LogError("LevelID not found: " + CurrentLevelId);
			return;
		}
		Debug.Log ("Level ID: " + level.LevelID);
		Debug.Log ("Planet Count: " + level.PlanetCount);
		for (int i = 0; i < level.PlanetCount; i++) {

			CreatePlanet(GetRandomPrefab(),new Vector3(i*Random.Range(-5,5),i*Random.Range(-5,5),0));

		}

    }
	private void ClearPlanets()
	{
		foreach (var planetFabs in planetPrefabs) {
			Destroy(planetFabs.gameObject);		
		}
		TotalCompletedPlanets = 0;
	}
	public Planet GetRandomPrefab()
	{
		int randomVal = Random.Range (0, 2);
		switch (randomVal) {
			case 0:
				return planet1;
			case 1:
				return planet2;
			default:
				return planet2;
		}

	}

	public void InitializePlayzone()
	{

		LoadLevel (++CurrentLevelId);

		//CreatePlanet(planetPrefab, new Vector3(0, -1f));
		//CreatePlanet(planetPrefab2, new Vector3(0, 1f));
	}

    public void ChangeScene(GameScene scene)
    {
		clearScenes ();
		switch (scene) {
			case GameScene.Home:
				HomeScene.SetActive (true);
				break;
			case GameScene.GameOver:
				GameOverScene.SetActive(true);
				break;
			case GameScene.Playing:
				PlayScene.SetActive (true);
				InitializePlayzone();
				break;
			case GameScene.Win:
				WinScene.SetActive (true);
				Invoke ("SwitchToHomeScreen",3.0f);
				break;
		}
		Debug.Log ("GameScene Changed: " + scene.ToString ());
	}
	public void SwitchToHomeScreen()
	{
		ChangeScene (GameScene.Home);
	}
	public void ChangeState(GameState stt)
    {
		state = stt;
		switch (state) {
			case GameState.Ready:
				ClearPlanets();
				break;
			case GameState.Combine:
				SecondsToSurviveFor  = surviveFor;
				break;
			case GameState.Survive:
				StartCoroutine(SpawnMeteors());
				StartCoroutine (VictoryTimer());
				break;
		}
		Debug.Log ("GameState Changed: " + state.ToString ());

    }
	private IEnumerator VictoryTimer()
	{
				
		while (true) {
			
			if(state == GameState.Survive)
			{
				SecondsToSurviveFor--;
				Debug.Log("Survice for! " + SecondsToSurviveFor);
				if(SecondsToSurviveFor == 0)
				{
					SecondsToSurviveFor = surviveFor;
					ChangeState(GameState.Ready);
					ChangeScene(GameScene.Win);
				}
			}
			
			yield return new WaitForSeconds (1);
			
			
		}
	}
	private IEnumerator SpawnMeteors()
	{

		Level lev = Levels[CurrentLevelId];

		while (true) {

			if(state == GameState.Survive)
			{
				Debug.Log("Creating Meteor!");
				Meteor met = (Meteor) Instantiate(meteor);
			}
		
			yield return new WaitForSeconds (lev.MeteorSpawnPeriodInSeconds);
			
			
		}
	}


}

public enum GameScene
{
    Home,
    Playing,
    GameOver,
    Win
}

public enum GameState
{
    Ready,
    Combine,
    Survive
}