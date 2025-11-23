using System;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public GameObject lookingAt;
    private GameObject lastLookingAt;

    public Vector3 collision = Vector3.zero;
    public float maxInteractRange = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        // Physics.Raycast(ray);


        if (Physics.Raycast(ray, out hitInfo, maxInteractRange))
        {
            lookingAt = hitInfo.transform.gameObject;
            collision = hitInfo.point;


            if (lookingAt != lastLookingAt)
            {
                Debug.Log("Looking at: " + lookingAt.name);
                lastLookingAt = lookingAt;
            }
        }

        // // always update gizmo for testing
        // else { collision = transform.position + transform.forward * maxInteractRange; }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collision, 0.2f);
    }

}
