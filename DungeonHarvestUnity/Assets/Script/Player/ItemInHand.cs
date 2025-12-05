using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    public GameObject itemPosition;
    public GameObject inventoryManager;
    private GameObject selectedItem;
    private GameObject previousItem;
    private HotbarManager hotbar;

    void Start()
    {
        hotbar = inventoryManager.GetComponent<HotbarManager>();
    }

    void Update()
    {
        ItemSO selectedSO = hotbar.getSelectedItem();

        if (selectedSO == null)
        {
            ClearHand();
            previousItem = null;
            return;
        }

        selectedItem = selectedSO.prefab;

        // only update when selected item changes
        if (selectedItem != previousItem)
        {
            ClearHand();

            if (selectedItem != null)
            {
                GameObject itemInHand = Instantiate(selectedItem, itemPosition.transform);
                itemInHand.transform.localPosition = selectedSO.itemOffsetInHand;
            }

            previousItem = selectedItem;
        }
    }

    public ItemSO getSelectedSO()
    {
        return hotbar.getSelectedItem();
    }

    void ClearHand()
    {
        foreach (Transform child in itemPosition.transform)
            Destroy(child.gameObject);
    }
}
