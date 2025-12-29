using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    public RectTransform panel;
    public TextMeshProUGUI text;
    public Vector2 offset = new Vector2(30f, -30f);

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string itemName, Vector2 screenPos)
    {
        text.text = itemName;
        panel.gameObject.SetActive(true);
        panel.position = screenPos + offset;
    }

    public void UpdatePosition(Vector2 screenPos)
    {
        panel.position = screenPos + offset;
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
