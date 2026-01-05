using TMPro;
using UnityEngine;

public enum CraftingType { Forge, Craft }


public class CraftingUIManager : MonoBehaviour
{

    public CraftingType currentType;
    public CraftingRecipe[] craftingTable;
    public CraftingRecipe[] furnace;
    public Transform contentParent;
    public ButtonRecipie recipeButtonPrefab;
    private InventoryManager inventoryManager;

    void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }

        // if (currentType == CraftingType.Craft)
        //     GenerateCraftingUI();

        // if (currentType == CraftingType.Forge)
        //     GenerateForgeUI();
    }

    public void SetCraftingType(CraftingType type)
    {
        currentType = type;
        RefreshUI();
    }

    void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        if (currentType == CraftingType.Craft)
            GenerateCraftingUI();
        else
            GenerateForgeUI();
    }

    void GenerateCraftingUI()
    {
        foreach (var recipe in craftingTable)
        {
            ButtonRecipie btn =
                Instantiate(recipeButtonPrefab, contentParent);

            btn.Setup(recipe);

            bool canCraft = inventoryManager.HasMaterials(recipe);
            btn.SetInteractable(canCraft);
        }
    }

    void GenerateForgeUI()
    {
        foreach (var recipe in furnace)
        {
            ButtonRecipie btn =
                Instantiate(recipeButtonPrefab, contentParent);

            btn.Setup(recipe);

            bool canCraft = inventoryManager.HasMaterials(recipe);
            btn.SetInteractable(canCraft);
        }
    }

    void OnEnable()
    {
        InventoryManager.OnInventoryChanged += UpdateButtons;
    }

    void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= UpdateButtons;
    }

    void UpdateButtons()
    {
        foreach (Transform child in contentParent)
        {
            ButtonRecipie btn = child.GetComponent<ButtonRecipie>();
            if (btn == null || btn.recipe == null) continue;

            bool canCraft = inventoryManager.HasMaterials(btn.recipe);
            btn.SetInteractable(canCraft);
        }
    }

}
