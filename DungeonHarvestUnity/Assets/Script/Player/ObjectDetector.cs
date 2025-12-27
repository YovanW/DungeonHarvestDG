using System;
using TMPro;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public GameObject lookingAt;
    private GameObject lastLookingAt;

    public Vector3 collision = Vector3.zero;
    public float maxInteractRange = 2.5f;

    // data untuk PlayerMining script
    public bool hitSomething;
    public RaycastHit hitInfoPublic;

    public TextMeshProUGUI intractInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractRange))
        {
            hitSomething = true;
            hitInfoPublic = hit;

            lookingAt = hit.transform.gameObject;
            collision = hit.point;

            if (lookingAt != lastLookingAt)
            {
                Debug.Log("Looking at: " + lookingAt.name);
                lastLookingAt = lookingAt;
            }
        }
        else
        {
            hitSomething = false;
            lookingAt = null;

            // // always update gizmo for testing
            // collision = transform.position + transform.forward * maxInteractRange;
        }

        // Hide intractInfo when inventory is open
        if (GameObject.FindGameObjectWithTag("CanvasController").GetComponent<InventoryMenu>().isOpen) { intractInfo.gameObject.SetActive(false); }
        else { intractInfo.gameObject.SetActive(true); }
        
        UpdateIntractInfo();
    }

    void UpdateIntractInfo()
    {
        // Default Empty
        if (lookingAt == null) intractInfo.text = "";

        // Chest
        if (lookingAt.TryGetComponent(out chestOpen chest))
        {
            intractInfo.text = "Press \"E\" to Open Chest";
            return;
        }

        // Door
        if (lookingAt.TryGetComponent(out doorMove door))
        {
            intractInfo.text = door.isDoorOpenNow()
                ? "Press \"E\" to Close"
                : "Press \"E\" to Open";
            return;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(collision, 0.05f);
    }

}
