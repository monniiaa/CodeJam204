using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class drag : MonoBehaviour
{
    private Edit editMode;
    private InputManager inputManager;
    public List<RectTransform> canDragList;
    private RectTransform elmToDrag;
    private Vector3 initialPosition;
    private Transform prevHitElm;


    void Start()
    {
        editMode = Edit.Instance;
        inputManager = InputManager.Instance;
        inputManager.PerformedHoldEvent += MoveElement;
        inputManager.StartHoldEvent += SetMoveableElement;
        inputManager.EndHoldEvent += SetElementPosition;
    }
    private void OnDisable()
    {
        inputManager.PerformedHoldEvent -= MoveElement;
        inputManager.StartHoldEvent -= SetMoveableElement;
        inputManager.EndHoldEvent -= SetElementPosition;
    }
    /// <summary>
    /// Gets the element that the finger is holding down
    /// </summary>
    /// <param name="pos">The position of the finger</param>
    private void SetMoveableElement(Vector2 pos)
    {
        if (editMode.EditMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit2D Hit = Physics2D.GetRayIntersection(ray);
            if (Hit.collider != null)
            {
                elmToDrag = Hit.collider.gameObject.GetComponent<RectTransform>();
            }
        }
    }

    /// <summary>
    /// Moves the elements y position depending on the fingers location
    /// </summary>
    /// <param name="pos"> The positon of the finger </param>
    private void MoveElement(Vector2 pos)
    {
        if (elmToDrag != null)
        {
            //Gets the distance between the elm to drag and the camera
            float camDistance = Vector2.Distance(elmToDrag.position, Camera.main.transform.position);
            //Casts a new ray from the position of the finger
            Ray ray = Camera.main.ScreenPointToRay(pos);
            //Changes the y position of the elmToDrag.position. The ray.GetPoint(camDistance) will return a point on the ray that is the distance between the 2 objects along the ray
            //We only want the y position of this since we only want to move the object along the y axis
            elmToDrag.transform.position = new Vector2(elmToDrag.transform.position.x, ray.GetPoint(camDistance).y);

        }
    }


    /// <summary>
    /// Drops the element in the position of the closest element or on the position of the element it passed last
    /// </summary>
    private void SetElementPosition(Vector2 pos)
    {
        if (elmToDrag != null)
        {
            canDragList.Remove(elmToDrag);
            RectTransform elm = FindClosestElement(elmToDrag, canDragList);
            if (elm == elmToDrag)
            {
                elmToDrag.position = initialPosition;
            }
            elmToDrag.SetSiblingIndex(elm.GetSiblingIndex());
            canDragList.Add(elmToDrag);
            elmToDrag = null;
        }
    }

    /// <summary>
    /// Finds the element in the list closest to the elmToDrag
    /// </summary>
    /// <param name="elmToDrag">The element used to find the closest elements</param>
    /// <param name="list"> The list of elements </param>
    /// <returns></returns>
    private RectTransform FindClosestElement(RectTransform elmToDrag, List<RectTransform> list)
    {
        RectTransform closestElm = elmToDrag;
        float smallestDistance = Mathf.Sqrt(Mathf.Pow((initialPosition.x - elmToDrag.transform.position.x), 2) +
                                        Mathf.Pow((initialPosition.y - elmToDrag.transform.position.y), 2));
        for (int i = 0; i < list.Count; i++)
        {
            float distance = Mathf.Sqrt(Mathf.Pow((list[i].position.x - elmToDrag.transform.position.x), 2) +
                                        Mathf.Pow((list[i].position.y - elmToDrag.transform.position.y), 2));
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestElm = list[i];
            }
        }
        return closestElm;
    }

}