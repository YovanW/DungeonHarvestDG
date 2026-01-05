using UnityEngine;
using System.Collections;

public class doorMove : MonoBehaviour
{
    public GameObject door;
    public float duration = 0.3f;   // animation speed

    private bool isDoorOpen = false;
    private bool isAnimating = false;
    private float closeDoorPos;

    void Start()
    {
        closeDoorPos = door.transform.rotation.eulerAngles.y;
    }

    public bool isDoorOpenNow()
    {
        if (isDoorOpen)
        {
            return true;
        }
        else return false;
    }

    public void ToggleDoor()
    {
        if (isAnimating) return;

        float targetY = isDoorOpen ? closeDoorPos : closeDoorPos - 90f;
        StartCoroutine(RotateDoor(targetY));

        isDoorOpen = !isDoorOpen;
    }

    IEnumerator RotateDoor(float targetY)
    {
        isAnimating = true;

        Quaternion startRot = door.transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, targetY, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            door.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        isAnimating = false;
    }
}
