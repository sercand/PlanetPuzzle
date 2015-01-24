// #
// # Created by Sercan Degirmenci on 2015.01.23
// #

using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance;
    private readonly List<PlanetPiece> bodies = new List<PlanetPiece>();
    [SerializeField] private float InteractionRange = 2;
    [SerializeField] private float InteractionCoefficient = 2;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterBody(PlanetPiece body)
    {
        if (!bodies.Contains(body))
            bodies.Add(body);
    }

    public void UnregisterBody(PlanetPiece body)
    {
        if (bodies.Contains(body))
            bodies.Remove(body);
    }

    public void Start()
    {

    }

    public void FixedUpdate()
    {
        for (var i = 0; i < bodies.Count; i++)
        {
            for (var j = i + 1; j < bodies.Count; j++)
            {
                // if (bodies[i].IsDragging || bodies[j].IsDragging) continue;
                var ri = bodies[i].rigidbody2D;
                var rj = bodies[j].rigidbody2D;
                var dp = ri.position - rj.position;
                var dist = dp.SqrMagnitude();
                dp.Normalize();
                var dp2 = -dp;
                var factor = (InteractionRange/dist)*InteractionCoefficient;
                ri.AddForce(dp*factor);
                rj.AddForce(dp2*factor);
            }
        }
    }
}
