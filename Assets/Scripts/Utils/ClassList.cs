using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ### TableDatas ###

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

[Serializable]
public class IngredientItem
{
    public int index;
    public string name;
    public int grade;
    public string iconName;
    public string descript;
}

[Serializable]
public class ConsumableItem
{
    public int index;
    public string name;
    public int grade;
    public string iconName;
    public string descript;
}

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

[Serializable]
public class ArmorData
{

}

[Serializable]
public class AllEventData
{
    public int index;
    public string name;
    public bool isUsing;
    public double eventRate;
}

[Serializable]
public class Event1Data
{
    public int index;
    public string name;
    public bool isUsing;
    public int goldAmount;
    public double eventRate;
}

[Serializable]
public class Event2Data
{

}

[Serializable]
public class Event3Data
{

}

[Serializable]
public class ShopData
{

}

[Serializable]
public class MissionData
{

}

[Serializable]
public class SkillData
{

}

[Serializable]
public class InAppositeWordData
{
    public int number;
    public string word;
}

#endregion

#region ### UserDatas ###

[Serializable]
public class UserAccountData
{
    public string RowIndate;
    public int AccountLevel;
    public string LastConnect;
}

[Serializable]
public class UserPropertyData
{
    public string RowIndate;
    public int Gold;
    public int Dia;
    public string LastConnect;
}

[Serializable]
public class UserCharacterUpgradeData
{
    public string RowIndate;
    // 여기에 강화 정보 추가
    public string LastConnect;
}

[Serializable]
public class UserWeaponInvenData
{
    public string RowIndate;
    // 여기에 유저 보유 무기 정보 추가
    public string LastConnect;
}

[Serializable]
public class UserArmorInvenData
{
    public string RowIndate;
    // 여기에 방어구 보유 정보 추가
    public string LastConnect;
}

[Serializable]
public class UserItemInvenData
{
    public string RowIndate;
    // 여기에 소모품 정보 추가
    public string LastConnect;
}




#endregion
