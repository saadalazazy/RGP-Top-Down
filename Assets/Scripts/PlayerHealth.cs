using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    static public bool isDead = false;
    float heart;
    [SerializeField] float maxHeart = 5;
    [field: HideInInspector] public float HitImpact { get; private set; }
    [field: HideInInspector] public Vector3 LastHitDir { get; private set; }

    [SerializeField] UnityEvent OnDeath;
    [SerializeField] UnityEvent OnHit;

    [Header("Hit Flash Settings")]
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashDuration = 0.05f;
    [SerializeField] int flashRepeat = 1;
    [SerializeField] PlayerEffects playerEffects;
    private List<Material> materialInstances = new List<Material>();
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    private NavMeshAgent agent;

    [Header("UI")]
    [SerializeField] GameObject[] heartsUI;



    private void Start()
    {
        heart = maxHeart;
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
    public void DecreaseHeart(float damage, Vector3 hitDir, float hitImpact)
    {
        if (heart <= 0) return;

        heart = Mathf.Max(heart - damage, 0);
        StartCoroutine(FlashHitEffect());

        if (heart <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log(gameObject.transform + " dead");
            isDead = true;
            return;
        }
        OnHit?.Invoke();
        LastHitDir = hitDir;
        HitImpact = hitImpact;
        DecreseHeartUI();
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
    private void DecreseHeartUI()
    {
        for(int i = 0; i < heart;i++)
        {
            heartsUI[(int)heart].gameObject.active = true;
        }
        for(int i = (int)heart ;i < maxHeart;i++)
        {
            heartsUI[(int)heart].gameObject.active = false;
        }
    }
    public void IncreasHeart(int hearts)
    {
        if (heart == maxHeart) return; 
        heart += hearts;
        for (int i = 0; i < heart; i++)
        {
            heartsUI[(int)heart].gameObject.active = true;
        }
        for (int i = (int)heart; i < maxHeart; i++)
        {
            heartsUI[(int)heart].gameObject.active = false;
        }
    }
}
