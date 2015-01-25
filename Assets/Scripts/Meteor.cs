using UnityEngine;
using System.Collections.Generic;


namespace Assets.Scripts
{
	public class Meteor: MonoBehaviour
	{

		public Meteor ()
		{


		}
		private void Awake()
		{
			var v = GameManager.Instance.mainCamera.ScreenToWorldPoint (new Vector2 (Random.Range(0,Screen.width), Screen.height+20.0f));
			v.z = 0;
			transform.position = v;
			rigidbody2D.velocity = new Vector2 (Random.Range(-5,5), -Random.Range(transform.position.y/2,transform.position.y));
			Destroy (this.gameObject, 10.0f);
		}

		private void Start()
		{

		}
		void OnCollisionEnter2D(Collision2D coll) {
			if (coll.gameObject.tag == "planet") {
				Debug.Log("Planet collision!!");	
				var r = coll.gameObject.GetComponent<PlanetBody>().ParentPlanet.childs;
				foreach(var planetPiece in r)
				{
					planetPiece.BecomeFree();
				}
			}
			
		}
		
	}
}

