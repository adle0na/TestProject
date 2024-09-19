using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoginType
{
    Guest,
    Google,
    Apple
}

public enum UserDataType
{
    UserAccount,
    UserProperty,
    UserCharacterUpgrade
}

public enum TableDataType
{
    Character,
    Monster,
    Item,
    Weapon,
    Armor,
    AllEvent,
    Event1,
    Event2,
    Event3,
    Shop,
    Mission,
    Skill,
    InAppositeWord
}

public enum GameLogType
{
    SignIn,
    LogIn,
    AddItem,
    UseItem,
    UpgradeWeapon,
    UpgradeArmor
}

public enum CurrentUIStatus
{
    UI,
    ABBPopup,
    NoneABBPopup
}

public enum PriceType
{
    IAP,
    Gold,
    Dia,
}

public enum Attribute
{
    Forest,
    Dessert,
    IceLand,
    FireGround
}

public enum WeaponType
{
    SwordShield,
    GreatSword,
    Bow,
    Staff
}

public enum ServerType
{
    Dev,
    Live
}

public enum TransactionType
{
    Insert,
    Update,
    SetGet
}

public enum SaveType
{
    Insert,
    Update,
    SetGet
}