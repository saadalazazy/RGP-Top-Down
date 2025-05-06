using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFireHandler : MonoBehaviour
{
    [SerializeField] Transform ShootingPoint;
    [SerializeField] Transform Player;
    [SerializeField] GameObject arrow;
    [SerializeField] public float impact;
    public float delay;
    public bool canShoot = true;

    public void ShootingArrow()
    {
        if (canShoot)
            StartCoroutine(ShootingArrowDelay());
    }

    IEnumerator ShootingArrowDelay()
    {
        canShoot = false;
        Instantiate(arrow, ShootingPoint.position, Player.rotation);
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
