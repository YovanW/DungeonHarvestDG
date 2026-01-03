using System.Collections;
using UnityEngine;

public class PlayerCutting : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ObjectDetector ray;
    public ItemInHand itemHand;
    bool isSwinging = false;
    private float maxAxeDistance = 2.5f;

    void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var selectedSO = itemHand.getSelectedSO();      // get the item in hand
        if (ray.lookingAt == null || Time.deltaTime == 0) return;

        // Chop tree
        if (selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Chop)
        {
            TreeController tree = ray.lookingAt.GetComponent<TreeController>();
            if (tree == null) return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isSwinging) return;

                if (!tree.isChopped)
                {
                    float distance = Vector3.Distance(transform.position, tree.transform.position);
                    if (distance <= maxAxeDistance)
                    {
                        StartCoroutine(SwingAnimation(ray.lookingAt.transform));
                    }
                }
            }
        }
    }

    IEnumerator SwingAnimation(Transform hitpos)
    {
        if (isSwinging) yield break;
        isSwinging = true;

        // TODO: play animation here


        isSwinging = false;
    }
}
