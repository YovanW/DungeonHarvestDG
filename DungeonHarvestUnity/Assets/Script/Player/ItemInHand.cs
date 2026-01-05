using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    public GameObject itemPosition;
    public GameObject inventoryManager;
    private GameObject selectedItem;
    private GameObject previousItem;
    private HotbarManager hotbar;
    private ItemSO selectedSO;

    void Start()
    {
        hotbar = inventoryManager.GetComponent<HotbarManager>();
    }

    void Update()
    {
        selectedSO = hotbar.getSelectedItem();

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
            RefreshHand();

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

    public void RefreshHand()
    {
        ClearHand();

        selectedItem = hotbar.getSelectedItem()?.prefab;
        selectedSO = hotbar.getSelectedItem();

        if (selectedItem != null)
        {
            GameObject itemInHand = Instantiate(selectedItem, itemPosition.transform);
            itemInHand.transform.localPosition = selectedSO.itemOffsetInHand;
        }
    }

}
