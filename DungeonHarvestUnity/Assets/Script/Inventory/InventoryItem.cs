using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSO item;
    public Image image;
    public Transform parentAfterDrag;
    public int count = 1;
    public TextMeshProUGUI countText;

    public void Start()
    {
        image = GetComponent<Image>();

        InitialiseItem(item);
    }

    void Update()
    {
        refreshCount();
    }

    public void refreshCount()
    {
        if (countText == null) return;
        countText.text = count.ToString();
        if (countText.gameObject != null) countText.gameObject.SetActive(count > 1);
    }

    public void InitialiseItem(ItemSO newItem)
    {
        item = newItem;
        if (image == null) image = GetComponent<Image>();
        if (image != null && newItem != null) image.sprite = newItem.icon;
        refreshCount();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;

        Transform canvas = GetComponentInParent<Canvas>().transform;
        transform.SetParent(canvas);

        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.blocksRaycasts = true;

        // check what we dropped on
        GameObject obj = eventData.pointerEnter;

        if (obj != null)
        {
            // if drop target is the highlight selector, switch to its parent instead
            if (obj.CompareTag("SelectedImage"))
            {
                obj = obj.transform.parent.gameObject;
            }

            InventorySlot slot = obj.GetComponent<InventorySlot>();
            if (slot != null)
            {
                InventoryItem other = slot.GetComponentInChildren<InventoryItem>();

                // kalau slot kosong langsung taruh
                if (other == null)
                {
                    transform.SetParent(slot.transform);
                    transform.localPosition = Vector3.zero;
                    return;
                }

                // kalau slot ada item → swap
                Transform otherParent = other.transform.parent;
                other.transform.SetParent(parentAfterDrag);
                other.transform.localPosition = Vector3.zero;

                transform.SetParent(otherParent);
                transform.localPosition = Vector3.zero;
                return;
            }

            // if drop on same item
            


            // if drop target is another inventory item, swap places
            if (obj.GetComponent<InventoryItem>() != null)
            {
                Transform otherItemParent = obj.transform.parent;
                obj.transform.SetParent(parentAfterDrag);
                obj.transform.localPosition = Vector3.zero;

                transform.SetParent(otherItemParent);
                transform.localPosition = Vector3.zero;
                return;
            }

            // delete slot
            if (obj.CompareTag("DeleteSlot"))
            {
                Destroy(gameObject);
                return;
            }
        }

        // default fallback if nothing valid was hit
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
    }



}