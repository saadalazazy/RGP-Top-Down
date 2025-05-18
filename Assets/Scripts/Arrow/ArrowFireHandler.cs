using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowFireHandler : MonoBehaviour
{
    [SerializeField] Transform ShootingPoint;
    [SerializeField] Transform Character;
    [SerializeField] GameObject arrow;
    [SerializeField] public float impact;
    [field: SerializeField] public float ArrowCount { get; private set; }
    public float delay;
    public bool canShoot = true;

    [SerializeField] TextMeshProUGUI arrowBundleCountText;


    public void ShootingArrow()
    {
        if (canShoot)
            StartCoroutine(ShootingArrowDelay());
    }
    public void DcreaseArrowCount()
    {
        if(ArrowCount > 0)
        {
            ArrowCount--;
            arrowBundleCountText.text = ArrowCount.ToString();
        }
    }
    public void IncreaseArrowCount(int count)
    {
        ArrowCount+= count;
    }
    IEnumerator ShootingArrowDelay()
    {
        canShoot = false;
        Instantiate(arrow, ShootingPoint.position, Character.rotation);
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
