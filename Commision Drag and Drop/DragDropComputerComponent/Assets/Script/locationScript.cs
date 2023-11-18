using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class locationScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    DraggableObject draggable;
    private GameObject gameManager;
    bool locationDropped = false;
    GameObject component;

    private static bool isDelay = false;
    private static bool returnPosCalled = false;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (locationDropped) return;

        if (collision.CompareTag(gameObject.tag))
        {
            draggable = collision.GetComponent<DraggableObject>();
            if (draggable.getMoboDrag())
            {
                if(collision.CompareTag("ssd") ||
                    collision.CompareTag("ram") ||
                    collision.CompareTag("cpu") ||
                    collision.CompareTag("gpu"))
                {
                    return;
                }
            }

            if (!draggable.isDrop) return;
            if (draggable.isDragging) return;

            component = collision.gameObject;
            locationDropped = true;
            component.transform.parent = transform;
            gameManager.GetComponent<HealthManager>().increaseComponentDropped();

            //disable collision of correct component
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(delay());
        }
        else
        {
            if (isDelay) return;
            // check for wrong drop
            if (collision != null)
            {
                if (locationDropped) return;
                draggable = collision.GetComponent<DraggableObject>();
                if (draggable != null)
                {
                    if (draggable.isDragging)
                    {
                        returnPosCalled = false;
                        return;
                    }
                    if (!draggable.isDrop) return;
                }

                Collider2D collider = GetComponent<Collider2D>();
                bool isValidDrop = collider.IsTouchingLayers(collision.gameObject.layer);
                if (!isValidDrop)
                {
                    Debug.Log("invalid: " + collider.name);

                    if (!returnPosCalled)
                    {
                        if (draggable.getMoboDrag()) return;
                        StartCoroutine(wrongDrop());
                    }
                }
                else
                {
                    Debug.Log("valid");
                }
            }
        }
    }

    IEnumerator delay()
    {
        isDelay = true;
        yield return new WaitForSeconds(0.5f);
        isDelay = false;
    }
    IEnumerator wrongDrop()
    {
        if (draggable != null)
        {
            draggable.returnPos();
        }
        gameManager.GetComponent<HealthManager>().decreaseHeart();
        returnPosCalled = true;
        yield return new WaitForSeconds(0.5f);
    }
    private void Update()
    {
        if (component != null && locationDropped)
            component.transform.position = transform.position + offset;
    }
}
