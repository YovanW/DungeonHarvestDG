using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRecipie : MonoBehaviour
{
    public Image materialIcon1;
    public TextMeshProUGUI amount1;

    public Image materialIcon2;
    public TextMeshProUGUI amount2;

    public Image resultIcon;
    public CraftingRecipe recipe;
    private InventoryManager inventoryManager;
    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }
    }

    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }

    public void Setup(CraftingRecipe data)
    {

        recipe = data;

        // Material 1
        materialIcon1.sprite = data.Material[0].item.icon;
        SetAmount(amount1, data.Material[0].amount);

        // Material 2
        materialIcon2.sprite = data.Material[1].item.icon;
        SetAmount(amount2, data.Material[1].amount);

        // Result
        resultIcon.sprite = data.Result[0].item.icon;
    }

    void SetAmount(TextMeshProUGUI text, int amount)
    {
        if (amount <= 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
            text.text = amount.ToString();
        }
    }

    void OnClick()
    {
        Debug.Log("Crafting " + recipe.Result[0].item.name + "...");

        if (!inventoryManager.HasMaterials(recipe))
            return;

        // Remove materials
        foreach (var mat in recipe.Material)
        {
            for (int i = 0; i < mat.amount; i++)
            {
                inventoryManager.RemoveItem(mat.item);
            }
        }

        // Add result
        for (int i = 0; i < recipe.Result[0].amount; i++)
        {
            inventoryManager.AddItem(recipe.Result[0].item);
        }
    }

}
