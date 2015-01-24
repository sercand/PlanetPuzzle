// #
// # Created by Sercan Degirmenci on 2015.01.24
// #

using Assets.Scripts;
using UnityEngine;


public class PlanetBody : MonoBehaviour, IPhysicsObject
{
    public PhysicsObjectType Type
    {
        get { return PhysicsObjectType.Planet; }
    }

    public bool IsDragging { get; set; }

    public bool IsGrabbed { get; private set; }

	public Planet ParentPlanet { get; set;}

    private void OnEnable()
    {
		PhysicsManager.Instance.RegisterPlanet(ParentPlanet);
	}
	
    private void OnDisable()
    {
		PhysicsManager.Instance.UnregisterPlanet(ParentPlanet);
	}
}
