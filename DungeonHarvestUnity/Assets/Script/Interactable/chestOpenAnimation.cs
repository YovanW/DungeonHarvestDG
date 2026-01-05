using UnityEngine;

public class chestOpenAnimation : MonoBehaviour
{
    public GameObject chestLit;

    public void openState()
    {
        chestLit.transform.localRotation = Quaternion.Euler(-75, 0, 0);
    }

}
