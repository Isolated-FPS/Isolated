using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
public class PlayerHUD : MonoBehaviour
{
   private WeaponBase weapon; //현재 정보가 출력된 무기

   [Header("Weapon Base")] 
   public TextMeshProUGUI textWeaponName; //무기 이름
   public Image imageWeaponIcon; //무기 아이콘
   public Sprite[] spritesWeaponIcons;// 무기 아이콘에 사용되는 sprite 배열
   public Vector2[] sizeWeaponIcons;//무기 아이콘 크키 배열

   [Header("Ammo")] 
   public TextMeshProUGUI textAmmo; //현재 탄 최대 탄 출력

   [Header("Magazine")] 
   public GameObject magazineUIPrefab; //탄창 프리팹
   public Transform magazineParent; //탄창 Panel
   public int maxMagazineCount;//처음 생성하는 최대 탄창 수
   
   private List<GameObject> magazineList; //탄창 리스트

   public void SetupAllWeapons(WeaponBase[] weapons)
   {
      SetupMagazine();
      
      //사용 가능한 모든 무기의 이벤트 등록
      for (int i = 0; i < weapons.Length; ++i)
      {
         weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
         weapons[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
      }
   }

   public void SwitchingWeapon(WeaponBase newWeapon)
   {
      weapon = newWeapon;
      
      SetupWeapon();
   }

   private void SetupWeapon()
   {
      textWeaponName.text = weapon.WeaponName.ToString();
      imageWeaponIcon.sprite = spritesWeaponIcons[(int)weapon.WeaponName];
      imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
   }
   private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
   {
      textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
   }

   private void SetupMagazine()
   {
      //최대 탄창 수만큼 이미지 생성
      //magazineParent 오브젝트의 자식으로 모두 등록 후 모두 비활성화, 리스트에 저장
      magazineList = new List<GameObject>();
      for (int i = 0; i < maxMagazineCount; i++)
      {
         GameObject clone = Instantiate(magazineUIPrefab);
         clone.transform.SetParent(magazineParent);
         clone.SetActive(false);
         
         magazineList.Add(clone);
      }
      
   }

   private void UpdateMagazineHUD(int currentMagazine)
   {
      //전부 비활성화 현재 탄창 개수만큼 활성화
      for (int i = 0; i < magazineList.Count; i++)
      {
        magazineList[i].SetActive(false); 
      }

      for (int i = 0; i < currentMagazine; i++)
      {
         magazineList[i].SetActive(true);
      }
   }


}
