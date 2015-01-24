﻿// #
// # Created by Sercan Degirmenci on 2015.01.24
// #

using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [SerializeField] private Camera Camera;
    private Rigidbody2D body;
    private PlanetPiece draggingObject;
    private Vector2 nextPosition = Vector2.zero;
    private Vector2 previousPosition;
    private Vector2 positionChange;
    private Vector2 velocity;

    public void OnPointerDown(PointerEventData eventData)
    {
        previousPosition = Camera.ScreenToWorldPoint(eventData.position);
        var hit = Physics2D.Raycast(previousPosition, -Vector2.up);

        if (hit.collider != null)
        {
            body = hit.collider.attachedRigidbody;
            draggingObject = hit.collider.gameObject.GetComponent<PlanetPiece>();
            draggingObject.IsDragging = true;
            body.velocity = Vector2.zero;
            nextPosition = body.position;
            positionChange = Vector2.zero;
            velocity = Vector2.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (body == null) return;
        Vector2 dp = Camera.ScreenToWorldPoint(eventData.position);
        positionChange = dp - previousPosition;
        nextPosition += positionChange;

        previousPosition = dp;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (body == null) return;
        body.velocity = velocity;
        draggingObject.IsDragging = false;
        body = null;
    }

    public void FixedUpdate()
    {
        if (body == null) return;
        
        var newVelocity = positionChange/Time.deltaTime;
        velocity = Vector2.Lerp(velocity, newVelocity, Time.deltaTime*10);
        positionChange = Vector2.zero;

        body.MovePosition(nextPosition);
    }
}
