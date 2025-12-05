using TMPro;
using UnityEngine;

public class statController : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defenseText;

    void Start()
    {
        setHealth(100);
        setStamina(100);

        setDamage(10);
        setDefense(5);
    }

    public void setHealth(int health)
    {
        healthText.text = health.ToString();
    }
    public void setStamina(int stamina)
    {
        staminaText.text = stamina.ToString();
    }
    public void setDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
    public void setDefense(int defense)
    {
        defenseText.text = defense.ToString();
    }

    public int getHealth()
    {
        return int.Parse(healthText.text);
    }
    public int getStamina()
    {
        return int.Parse(staminaText.text);
    }
    public int getDamage()
    {
        return int.Parse(damageText.text);
    }
    public int getDefense()
    {
        return int.Parse(defenseText.text);
    }
}
