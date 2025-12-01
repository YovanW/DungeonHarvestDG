using UnityEngine;

public class Ore : MonoBehaviour
{
    public oreData oreData;
    private int health;

    void Start()
    {
        health = oreData.hardness;
    }

    public void Mine(int power)
    {
        Debug.Log(health);
        Debug.Log("Power : " + power);


        // cek apakah pickaxe sesuai
        if (power < health)
        {
            Debug.Log("Your pickaxe is too weak");
        }
        else
        {
            health -= power;
        }

        // TODO: play sfx mining


        if (health <= 0)
        {
            SpawnDrops();
            Destroy(gameObject);
        }
    }

    void SpawnDrops()
    {
        int dropCount = Random.Range(1, oreData.dropAmount + 1);


        for (int i = 0; i < dropCount; i++)
        {
            // TODO: spawn mining ore drop

        }
    }
}
