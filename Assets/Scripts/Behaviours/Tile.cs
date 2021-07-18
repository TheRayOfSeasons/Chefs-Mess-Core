using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;
    public bool selected = false;

    private Plane draggingPlane;
    private Vector3 offset;
    private Camera mainCamera;

    void Start()
    {
        this.mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        this.draggingPlane = new Plane(this.mainCamera.transform.forward, transform.position);
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(camRay.origin, camRay.direction * 10, Color.green);

        float planeDistance;
        this.draggingPlane.Raycast(camRay, out planeDistance);
        this.offset = transform.position - camRay.GetPoint(planeDistance);
    }

    void OnMouseDrag()
    {
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        float planeDistance;
        this.draggingPlane.Raycast(camRay, out planeDistance);
        transform.position = camRay.GetPoint(planeDistance) + offset;
    }
}
