using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float health;
    [SerializeField] float maxHealth = 100;
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] UnityEvent OnHit;

    [Header("Hit Flash Settings")]
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashDuration = 0.05f;
    [SerializeField] int flashRepeat = 1;
    [SerializeField] Collider collider;
    private List<Material> materialInstances = new List<Material>();
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");


    private void Start()
    {
        health = maxHealth;

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
            Debug.Log(gameObject.transform + " dead");
            collider.enabled = false;
            return;
        }
        OnHit?.Invoke();
    }

    private IEnumerator FlashHitEffect()
    {
        for (int i = 0; i < flashRepeat; i++)
        {
            foreach (var mat in materialInstances)
                mat.SetColor(EmissionColorID, flashColor);

            yield return new WaitForSeconds(flashDuration);

            foreach (var mat in materialInstances)
                mat.SetColor(EmissionColorID, Color.black);

            yield return new WaitForSeconds(flashDuration);
        }
    }
}
