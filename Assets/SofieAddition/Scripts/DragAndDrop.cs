using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDrop : MonoBehaviour
{
    private Edit editMode;
    private InputManager inputManager;
    public  List<GameObject> canDragList;
    private RectTransform elmToDrag;
    private Vector3 newPosition;
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
            if (Hit.collider && canDragList.Contains(Hit.collider.gameObject))
            {
                elmToDrag = Hit.collider.gameObject.GetComponent<RectTransform>();
                newPosition = elmToDrag.transform.position;
                prevHitElm = elmToDrag;
            }
        }
    }

    /// <summary>
    /// Moves the elements y position depending on the fingers location
    /// </summary>
    /// <param name="pos"> The positon of the finger </param>
    private void MoveElement(Vector2 pos)
    {
        if (elmToDrag)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            elmToDrag.transform.position = new Vector2(elmToDrag.transform.position.x, ray.GetPoint(pos.y).y);
            //vector2.zero means that it will only intersect with the objects directly under the finger
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            foreach (RaycastHit2D obj in hit)
            {
                if (obj.collider.transform != prevHitElm.transform && obj.collider.transform != elmToDrag.transform)
                {
                    MovePassedElement(obj.collider.transform);
                }
            }
            if (hit.Length == 1)
            {
                prevHitElm = elmToDrag;
            }
        }
    }

    /// <summary>
    /// Changes the Sibling index of the element that was just passed by the element being dragged
    /// Also updates the initialposition 
    /// </summary>
    /// <param name="hitElm"> The element the finger is passing</param>
    private void MovePassedElement(Transform hitElm)
    {
        if (elmToDrag.transform.position.y != hitElm.position.y)
        {
            newPosition = hitElm.position;
            hitElm.SetSiblingIndex(elmToDrag.GetSiblingIndex());
            prevHitElm = hitElm;
        }
    }

    /// <summary>
    /// Drops the element in the position of the closest element or on the position of the element it passed last
    /// </summary>
    private void SetElementPosition(Vector2 pos)
    {
        if (elmToDrag)
        {
            elmToDrag.position = newPosition;
            elmToDrag = null;
        }
    }
}

  


