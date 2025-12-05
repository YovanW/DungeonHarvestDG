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
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }

    public void InitialiseItem(ItemSO newItem)
    {
        item = newItem;
        image.sprite = newItem.icon;
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
                transform.SetParent(slot.transform);
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