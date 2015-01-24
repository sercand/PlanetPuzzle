// #
// # Created by Sercan Degirmenci on 2015.01.23
// #

using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance;
    [HideInInspector]
    public readonly List<PlanetPiece> Bodies = new List<PlanetPiece>();
	public readonly List<Planet> Planets = new List<Planet>();

    [SerializeField] private float InteractionRange = 2;
    [SerializeField] private float InteractionCoefficient = 2;
    [SerializeField] private float GrapRange = 0.02f;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterBody(PlanetPiece body)
    {
        if (!Bodies.Contains(body))
            Bodies.Add(body);
    }

    public void UnregisterBody(PlanetPiece body)
    {
        if (Bodies.Contains(body))
            Bodies.Remove(body);
    }
	public void RegisterPlanet(Planet planet)
    {
        if (!Planets.Contains(planet))
            Planets.Add(planet);
    }

	public void UnregisterPlanet(Planet planet)
    {
		if (Planets.Contains(planet))
			Planets.Remove(planet);
    }
    public void Start()
    {

    }

    public void FixedUpdate()
    {
        int draggingIndex = -1;
        for (var i = 0; i < Bodies.Count; i++)
        {
            if (Bodies[i].IsDragging)
            {
                draggingIndex = i;
                continue;
            }
            if (!Bodies[i].ApplyForceOthers) continue;

            for (var j = i + 1; j < Bodies.Count; j++)
            {
                if (!Bodies[j].ApplyForceOthers) continue;
                if (Bodies[j].IsDragging) continue;

                var ri = Bodies[i].rigidbody2D;
                var rj = Bodies[j].rigidbody2D;

                if (ri == null || rj == null) continue;

                var dp = ri.position - rj.position;
                var dist = dp.SqrMagnitude();
                dp.Normalize();
                var dp2 = -dp;
                var factor = (InteractionRange/dist)*(InteractionCoefficient*2);
                ri.AddForce(dp*factor);
                rj.AddForce(dp2*factor);
            }
        }
        for (var i = 0; i < Planets.Count; i++)
        {
            for (var j = i + 1; j < Planets.Count; j++)
            {
                var ri = Planets[i].Body.rigidbody2D;
                var rj = Planets[j].Body.rigidbody2D;

                if (ri == null || rj == null) continue;
                var dp = ri.position - rj.position;
                var dist = dp.SqrMagnitude();
                dp.Normalize();
                var dp2 = -dp;
                var factor = (InteractionRange/dist)*InteractionCoefficient;
                ri.AddForce(dp*factor);
                rj.AddForce(dp2*factor);
            }
        }

        Vector3 diff;
        if (draggingIndex != -1)
        {
            var body = Bodies[draggingIndex];
            for (var i = 0; i < Bodies.Count; i++)
            {
                if (i == draggingIndex) continue;
                if (CanGrab(body, Bodies[i], out diff))
                {
                    GrabTwo(body, Bodies[i], diff);
                    return;
                }
            }
        }
    }

    private void GrabTwo(PlanetPiece p1, PlanetPiece p2, Vector2 diff)
    {
        bool b1 = p1.IsGrabbed, b2 = p2.IsGrabbed;
        var planet = p1.Planet;
        var half_ps = new Vector3(planet.TextureWidth/planet.scaleRatio,
            planet.TextureHeight/planet.scaleRatio)/2;
        if (planet.BodyEnabled && !b1 && !b2)
        {
            return;
        }
        if (!planet.BodyEnabled)
        {
            planet.Body.transform.localPosition = p1.transform.localPosition - p1.mean + half_ps;
            planet.EnableBody();
        }
        if (b1 && b2) return;

        Debug.Log("Can Grap");
        if (b1)
        {
            // var d = p2.rigidbody2D.position + diff;

            //  p2.rigidbody2D.MovePosition(d);
            //  p2.rigidbody2D.MoveRotation(planet.Body.rigidbody2D.rotation);
            p2.rigidbody2D.MovePosition(p1.rigidbody2D.position + diff);

            p2.Grab();
            p2.transform.localRotation = p1.transform.localRotation;
            //p2.transform.localPosition = p2.mean - half_ps;
        }
        else if (b2)
        {
            var d = p1.rigidbody2D.position - diff;
            p1.rigidbody2D.MovePosition(d);
            //   p1.rigidbody2D.MoveRotation(planet.Body.rigidbody2D.rotation);
            p1.Grab();
            p1.transform.localRotation = p2.transform.localRotation;
            p1.transform.localPosition = p1.mean - half_ps;
        }
        else
        {
            p2.rigidbody2D.MovePosition(p2.rigidbody2D.position + diff);
            p2.rigidbody2D.MoveRotation(p1.rigidbody2D.rotation);
            p1.Grab();
            p2.Grab();

            //p2.transform.localPosition = p2.mean - half_ps;
        }
    }

    public bool CanGrab(PlanetPiece p1, PlanetPiece p2, out Vector3 diff)
    {
        Edge edge;
        if (p1.Planet.Id != p2.Planet.Id || !p1.IsNeighbourWith(p2, out edge))
        {
            diff = Vector3.zero;
            return false;
        }

        var p11 = p1.transform.rotation*p1.TransfromVertex(edge.v1) + p1.transform.position;
        var p12 = p1.transform.rotation*p1.TransfromVertex(edge.v2) + p1.transform.position;
        var p21 = p2.transform.rotation*p2.TransfromVertex(edge.v1) + p2.transform.position;
        var p22 = p2.transform.rotation*p2.TransfromVertex(edge.v2) + p2.transform.position;

        var p1m = (p11 + p12)/2;
        var p2m = (p21 + p22)/2;
        diff = p1m - p2m;
        return diff.sqrMagnitude <= GrapRange;
    }
}
