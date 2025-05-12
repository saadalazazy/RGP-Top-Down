using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private VisualEffect playerDash;
    [SerializeField] private VisualEffect playerHeal;
    [SerializeField] private ParticleSystem collectable;
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
    public void PlayPlayerHealVfx()
    {
        playerHeal.Play();
    }

    public void PlayPlayerCollectVfx()
    {
        Instantiate(collectable ,transform.position + new Vector3(0 ,1,0), Quaternion.identity,transform);
    }

    private IEnumerator ReturnToParentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerDash.transform.SetParent(originalParent);
        playerDash.transform.position = originalParent.position + new Vector3(0 , 0.4f ,0);
    }
}
