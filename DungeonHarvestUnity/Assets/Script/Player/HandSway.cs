using UnityEngine;

public class HandSway : MonoBehaviour
{
    public float swayAmount = 0.05f;
    public float swaySmooth = 6f;

    private Vector3 basePos;

    void Start()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 sway = new Vector3(-mouseX, -mouseY, 0) * swayAmount;
        Vector3 target = basePos + sway;

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * swaySmooth);
    }
}
