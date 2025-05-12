using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private VisualEffect playerDash;
    private Transform originalParent;

    private void Start()
    {
        originalParent = playerDash.transform.parent;
    }

    public void PlayPlayerDashVfx()
    {
        playerDash.transform.SetParent(null);
        playerDash.Play();
        StartCoroutine(ReturnToParentAfterDelay(1f));
    }


    private IEnumerator ReturnToParentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerDash.transform.SetParent(originalParent);
        playerDash.transform.position = originalParent.position + new Vector3(0 , 0.4f ,0);
    }
}
