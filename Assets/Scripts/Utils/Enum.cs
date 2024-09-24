using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 로그인 타입
public enum LoginType
{
    Guest,
    Google,
    Apple
}

// 유저 데이터 타입
public enum UserDataType
{
    UserProperty,
    UserCharacterUpgrade
}

// 테이블 데이터 타입
public enum TableDataType
{
    Character,
    Monster,
    IngredientItem,
    ConsumableItem,
    Weapon,
    WeaponCraft,
    Armor,
    ArmorCraft,
    AllEvent,
    Event1,
    Event2,
    Event3,
    Shop,
    Mission,
    Skill,
    InAppositeWord
}

// 게임 로그 타입
public enum GameLogType
{
    SignIn,
    LogIn,
    AddItem,
    UseItem,
    UpgradeWeapon,
    UpgradeArmor
}

// UI 타입
public enum CurrentUIStatus
{
    UI,
    ABBPopup,
    NoneABBPopup
}

// 가격 타입
public enum PriceType
{
    IAP,
    Gold,
    Dia,
}

// 속성 타입
public enum Attribute
{
    Forest,
    Dessert,
    IceLand,
    FireGround
}

// 무기 타입
public enum WeaponType
{
    AxeShield,
    GreatSword,
    Bow,
    Staff
}

// 방어구 타입
public enum ArmorType
{
    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5
}

// 서버 타입
public enum ServerType
{
    Dev,
    Live
}

// 서버 데이터 송수신 타입
public enum TransactionType
{
    Insert,
    Update,
    SetGet
}

// 저장 형식
public enum SaveType
{
    Insert,
    Update,
    SetGet
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}