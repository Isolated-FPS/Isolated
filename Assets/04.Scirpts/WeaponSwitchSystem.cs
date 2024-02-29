using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerHUD playerHUD;

    public WeaponBase[] weapons;
    private WeaponBase currentWeapon;
    private WeaponBase previousWeapon;

    private void Awake()
    {
        playerHUD.SetupAllWeapons(weapons);

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

        SwtchingWeapon(WeaponType.Main);
    }

    private void Update()
    {
        if(Store.isStore == false)//상점 메뉴가 꺼져있다면
            UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if(!Input.anyKeyDown) return;

        int inputIndex = 0;
        if (int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex < 4))
        {
            SwtchingWeapon((WeaponType)(inputIndex - 1));
        }
    }

    private void SwtchingWeapon(WeaponType weaponType)
    {
        if (weapons[(int)weaponType] == null)
        {
            return;
        }

        if (currentWeapon != null)
            previousWeapon = currentWeapon;

        currentWeapon = weapons[(int)weaponType];
        
        if(currentWeapon == previousWeapon)
            return;
        
        playerController.SwithchingWeapon(currentWeapon);
        playerHUD.SwitchingWeapon(currentWeapon);

        if (previousWeapon != null)
            previousWeapon.gameObject.SetActive(false);
        
        currentWeapon.gameObject.SetActive(true);
    }
}
