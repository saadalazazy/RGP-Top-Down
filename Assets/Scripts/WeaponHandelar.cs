using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandelar : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic1;
    [SerializeField] private GameObject weaponLogic2;
    [SerializeField] private GameObject sword1;
    [SerializeField] private GameObject sword2;
    [SerializeField] private GameObject crossbow;

    public void EnableOneWeapon()
    {
        weaponLogic1?.SetActive(true);
    }
    public void EnableWeaponTwo()
    {
        weaponLogic2?.SetActive(true);
    }
    public void DisableOneWeapon() 
    {
        weaponLogic1?.SetActive(false);
    }
    public void DisableWeaponTwo() 
    {
        weaponLogic2?.SetActive(false);
    }
    public void EnableTwoWeapon() 
    {
        weaponLogic1?.SetActive(true);
        weaponLogic2?.SetActive(true);
    }
    public void DisableTwoWeapon() 
    {
        weaponLogic1?.SetActive(false);
        weaponLogic2?.SetActive(false);
    }
    public void EnableSwords()
    {
        sword1?.SetActive(true);
        sword2?.SetActive(true);
        crossbow?.SetActive(false);
    }
    public void EnableCrossBow()
    {
        sword1?.SetActive(false);
        sword2?.SetActive(false);
        crossbow?.SetActive(true);
    }
}
