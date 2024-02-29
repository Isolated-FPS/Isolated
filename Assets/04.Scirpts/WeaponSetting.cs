// 무기의 종류가 여러 종류일때 공용으로 사용하는 변수들 구조체로 묶어 정의

public enum WeaponName { AK47 = 0, DesertEagle, CombatKnife, Grenade }

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName;
    public int damege;// 데미지
    public int currentMagazine; //현재탄창
    public int maxMagazine; //최대 탄창
    public int currentAmmo; //현재 탄약
    public int maxAmmo; //최대 탄약
    public float attackRate; //공격속도
    public float attakDistance; //사거리
    public bool isAutomaticAttack; // 연발사격 여부
}
