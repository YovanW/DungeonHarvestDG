using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class playerFarming : MonoBehaviour
{
    public ObjectDetector ray;
    public ItemInHand itemHand;
    bool isSwinging = false;
    private float maxRakeDistance = 2.5f;


    void Update()
    {
        var selectedSO = itemHand.getSelectedSO();
        bool canRake = selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Rake;

        if (!canRake) return;
        if (ray.lookingAt == null) return;

        soilHitBox soil = ray.lookingAt.GetComponent<soilHitBox>();
        if (soil == null) return;

        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
        if (isSwinging) return;

        if (!soil.isRaked)
        {
            TryRake(soil);
        }

        // buat testing
        else { soil.isRaked = false; soil.plantArea.SetActive(false); }

    }

    void TryRake(soilHitBox soil)
    {
        float dist = Vector3.Distance(transform.position, soil.transform.position);
        if (dist > maxRakeDistance) return;

        StartCoroutine(SwingAnimation(soil.transform));

        soil.isRaked = true;
        soil.soilQuality = itemHand.getSelectedSO().extraInfo;
        soil.plantArea.SetActive(true);
    }


    IEnumerator SwingAnimation(Transform hitpos)
    {
        // stop spam action
        if (isSwinging) yield break;
        isSwinging = true;

        // TODO: rake swing animation
        // yield return new WaitForSeconds(1f);

        isSwinging = false;
        yield return null;
    }
}
