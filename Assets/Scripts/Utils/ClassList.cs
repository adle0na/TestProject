using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ### TableDatas ###

// 캐릭터 테이블 데이터
[Serializable]
public class CharacterData
{
    public int index;
    public string name;
    public string prefabName;
    public string iconName;
    public Attribute attribute;
    public int hp;
    public float hpIncrement;
    public int def;
    public float defIncrement;
    public int atk;
    public float atkIncrement;
    public int spd;
    public float spdIncrement;
    public int ats;
    public double atsIncrement;
    public int evd;
    public double evdIncrement;
    public int crt;
    public double crtIncrement;
    public int crtd;
    public double crtdIncrement;
    public string descript;
}

// 몬스터 테이블 데이터
[Serializable]
public class MonsterData
{
    public int index;
    public string title;
    public string name;
    public string prefabName;
    public string iconName;
    public int hp;
    public float hpIncrement;
    public int atk;
    public float atkIncrement;
    public int def;
    public float defIncrement;
    public Attribute attribute;
    public int minGrade;
    public string descript;
}

// 재료 테이블 데이터
[Serializable]
public class IngredientItem
{
    public int index;
    public string name;
    public int grade;
    public string iconName;
    public string descript;
}

// 소모품 테이블 데이터
[Serializable]
public class ConsumableItem
{
    public int index;
    public string name;
    public int grade;
    public string iconName;
    public string descript;
}

// 무기 테이블 데이터
[Serializable]
public class WeaponData
{
    public int index;
    public string name;
    public WeaponType weaponType;
    public Attribute attribute;
    public string handRPrefab;
    public string handLPrefab;
    public string attackEffect;
    public string projectile;
    public string skilleffect;
    public string iconName;
    public int atk;
    public int ap;
    public double crt;
    public float bornGrade;
    public int skillIndex;
}

// 무기 제작 테이블 데이터
[Serializable]
public class WeaponCraftData
{
    public int index;
    public string name;
    public int weaponIndex;
    public float grade;
    public int ingre1;
    public int ingre1Amount;
    public int ingre2;
    public int ingre2Amount;
    public int ingre3;
    public int ingre3Amount;
    public int ingre4;
    public int ingre4Amount;
    public int ingre5;
    public int ingre5Amount;
    public int ingre6;
    public int ingre6Amount;
    public int gold;
}

// 방어구 테이블 데이터
[Serializable]
public class ArmorData
{

}

// 방어구 제작 테이블 데이터
[Serializable]
public class ArmorCraftData
{
    
}

// 메인 이벤트 테이블 데이터
[Serializable]
public class AllEventData
{
    public int index;
    public string name;
    public bool isUsing;
    public double eventRate;
}

// 이벤트 1 ( 골드 획득 테이블 데이터 )
[Serializable]
public class Event1Data
{
    public int index;
    public string name;
    public bool isUsing;
    public int goldAmount;
    public double eventRate;
}

// 이벤트 2 ( 일반 재료 획득 테이블 데이터 )
[Serializable]
public class Event2Data
{

}

// 이벤트 3 ( 전투 테이블 데이터 )
[Serializable]
public class Event3Data
{

}

// 상점 정보 테이블 데이터
[Serializable]
public class ShopData
{

}

// 미션 정보 테이블 데이터
[Serializable]
public class MissionData
{

}

// 스킬 정보 테이블 데이터
[Serializable]
public class SkillData
{

}

// 비속어 필터 데이터
[Serializable]
public class InAppositeWordData
{
    public int number;
    public string word;
}

#endregion

#region ### UserDatas ###

// 유저 착용 정보, 재화 데이터
[Serializable]
public class UserPropertyData
{
    public string RowIndate;
    public int Gold;
    public int Dia;
    public int AccountLevel;
    public int Character;
    public int Weapon;
    public int Armor1;
    public int Armor2;
    public int Armor3;
    public int Armor4;
    public int Armor5;
    public string LastConnect;
}

// 유저 스테이터스 업그레이드 데이터
[Serializable]
public class UserCharacterUpgradeData
{
    public string RowIndate;
    public int HPLevel;
    public int DEFLevel;
    public int ATKLevel;
    public int SPDLevel;
    public int ATSLevel;
    public int EVDLevel;
    public int CRTLevel;
    public int CRTDLevel;
    public string LastConnect;
}

// 유저 보유 재료 인벤토리 데이터
[Serializable]
public class UserIngredientInvenData
{
    public string RowIndate;
    public List<int> ingreItems;
    public string LastConnect;
}

// 유저 보유 소모품 인벤토리 데이터
[Serializable]
public class UserConsumableItemInvenData
{
    public string RowIndate;
    public List<int> ConsumableItemList;
    public string LastConnect;
}

// 유저 보유 무기 인벤토리 데이터
[Serializable]
public class UserWeaponInvenData
{
    public string RowIndate;
    public List<int> WeaponLists;
    public string LastConnect;
}

// 유저 보유 방어구 인벤토리 데이터
[Serializable]
public class UserArmorInvenData
{
    public string RowIndate;
    public List<int> ArmorLists;
    public string LastConnect;
}

#endregion
