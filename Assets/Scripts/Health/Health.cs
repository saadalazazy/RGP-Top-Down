using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float health;
    [SerializeField] float maxHealth = 100;
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] UnityEvent OnHit;

    [Header("Hit Flash Settings")]
    [SerializeField] Color[] flashColor;
    [SerializeField] float flashDuration = 0.05f;
    [SerializeField] int flashRepeat = 1;
    [SerializeField] Collider collider;

    private List<Material> materialInstances = new List<Material>();
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    private NavMeshAgent agent;
    float hitStopTime = 0.25f;

    [SerializeField] bool isBoss = false;

    private void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            Material[] mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].EnableKeyword("_EMISSION");
                materialInstances.Add(mats[i]);
            }
        }
    }

    public void DealDamage(float damage)
    {
        if (health <= 0) return;

        health = Mathf.Max(health - damage, 0);
        StartCoroutine(FlashHitEffect());

        if (health <= 0)
        {
            OnDeath?.Invoke();
            StartCoroutine(HitStop());
            Debug.Log(gameObject.transform + " dead");
            collider.enabled = false;
            if (isBoss) return;
            agent.enabled = false;
            return;
        }
        OnHit?.Invoke();
        CinemaMahchineShake.instance.ShakeCamera(1,0.2f);
    }

    private IEnumerator FlashHitEffect()
    {
        for (int i = 0; i < flashRepeat; i++)
        {
            foreach (var mat in materialInstances)
                mat.SetColor(EmissionColorID, flashColor[i]);

            yield return new WaitForSeconds(flashDuration);

            foreach (var mat in materialInstances)
                mat.SetColor(EmissionColorID, Color.black);

            yield return new WaitForSeconds(flashDuration);
        }
    }
    private IEnumerator HitStop()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1;
    }
}
