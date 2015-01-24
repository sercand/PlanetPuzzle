using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameState state = GameState.Playing;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}

public enum GameState
{
    Intro,
    Playing,
    GameOver,
    Win
}