using UnityEngine;
using System.Collections;

public class WallResizer : MonoBehaviour {

	// Use this for initialization
	[SerializeField] private GameObject WallNorth;
	[SerializeField] private GameObject WallEast;
	[SerializeField] private GameObject WallWest;
	[SerializeField] private GameObject WallSouth;
	[SerializeField] private Camera Camera;


	void Start () {
		WallNorth.transform.position = Camera.ScreenToWorldPoint(new Vector2(Screen.width/2,Screen.height));
		WallEast.transform.position = Camera.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height/2));
		WallWest.transform.position = Camera.ScreenToWorldPoint(new Vector2(0,Screen.height/2));
		WallSouth.transform.position = Camera.ScreenToWorldPoint(new Vector2(Screen.width/2,0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
