using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField]
    private Transform otherBoundary;
    [SerializeField]
    private bool horizontalBoundary;
    [SerializeField]
    private float offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (horizontalBoundary)
            {
                Vector3 newPos = collision.gameObject.transform.position;
                newPos.y = otherBoundary.position.y + offset;
                collision.gameObject.transform.position = newPos;
            }
            else
            {
                Vector3 newPos = collision.gameObject.transform.position;
                newPos.x = otherBoundary.position.x + offset;
                collision.gameObject.transform.position = newPos;
            }
        }
    }
}
