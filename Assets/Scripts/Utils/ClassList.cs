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
    public float atsIncrement;
    public int evd;
    public float evdIncrement;
    public int crt;
    public float crtIncrement;
    public int crtd;
    public float crtdIncrement;
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
public class ItemData
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
    public int grade;
    public string prefabName;
    public string iconName;
    public int atk;
    public float atkIncrement;
    public int ap;
    public float apIncrement;
    public float crt;
    public float crtIncrement;
    public int bornGrade;
}

[Serializable]
public class ArmorData
{

}

[Serializable]
public class AllEventData
{

}

[Serializable]
public class Event1Data
{

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
