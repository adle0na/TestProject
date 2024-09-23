using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using BackEnd.BackndLitJson;
using UnityEditor;
using UnityEngine;
using JsonData = LitJson.JsonData;

public class DataManager : Singleton<DataManager>
{
    public static DataManager instance;

    public bool LoadStatus;
    
    public LoginType loginType;
    
    public DateTime LocalTime;
    
    public string TimeValue;
    public string ServerTimeValue;
    
    public SystemLanguage systemLanguage;
    
    private bool isServerTimeUpdate = false;
    private bool isChangeServerTime = false;
    private float oldTime = 0;
    private DateTime serverTime;
    
    List<Dictionary<string, object>> DataDic = new List<Dictionary<string, object>>();
    
    public UserPropertyData            userPropertyData            = new();
    public UserWeaponInvenData         userWeaponInvenData         = new();
    public UserArmorInvenData          userArmorInvenData          = new();
    public UserIngredientInvenData     userIngredientInvenData     = new();
    public UserConsumableItemInvenData userConsumableItemInvenData = new();
    public UserCharacterUpgradeData    userCharacterUpgradeData    = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize();
    }
    
    private void FixedUpdate()
    {
        if (isServerTimeUpdate)
        {
            if (isChangeServerTime)
            {
                oldTime = Time.fixedUnscaledTime;
                LocalTime = serverTime;
                isChangeServerTime = false;
            }

            var delta = Time.fixedUnscaledTime - oldTime;
            oldTime = Time.fixedUnscaledTime;

            LocalTime = LocalTime.AddMilliseconds(delta * 1000.0f);

            TimeValue = LocalTime.ToString();

            ServerTimeValue = serverTime.ToString();
        }
    }

    public void Initialize()
    {
        systemLanguage = SystemLanguage.Korean;
        
        DataDic.Clear();
    }
    
    public void SetLocalTime(DateTime servertime)
    {
        if (serverTime < servertime)
        {
            serverTime = servertime;
            isServerTimeUpdate = true;
            isChangeServerTime = true;
        }
    }
    
    // 초기값 모두 세팅
    public void SetDefaultData()
    {
        SetUserData();
    }
    
    public void LoginWithType()
    {
        switch (loginType)
        {
            case LoginType.Guest :
                BackendManager.Instance.GuestLoginSequense();
                Debug.Log("Guest 로그인 시도");
                break;
            case LoginType.Google :
                Debug.Log("Google 로그인 시도");
                break;
            case LoginType.Apple :
                Debug.Log("Apple 로그인 시도");
                break;
        }
    }
    
    // 유저 데이터 별 구분자 삽입
    public void SetRowInDate(UserDataType table, string inDate)
    {
        switch (table)
        {
            case UserDataType.UserProperty:
                userPropertyData.RowIndate = inDate;
                break;
            case UserDataType.UserCharacterUpgrade:
                userCharacterUpgradeData.RowIndate = inDate;
                break;
        }
    }
    
    //유저 데이터 모든 값 초기화
    public void SetUserData()
    {
        BackendManager.Instance.Nickname = Backend.UserNickName;

        userPropertyData.Gold = 0;
        userPropertyData.Dia = 0;
        userPropertyData.AccountLevel = 1;
        userPropertyData.Character = 10000000;
        userPropertyData.Weapon = 50000000;
        userPropertyData.Armor1 = 70000000;
        userPropertyData.Armor2 = 70100000;
        userPropertyData.Armor3 = 70200000;
        userPropertyData.Armor4 = 70300000;
        userPropertyData.Armor5 = 70400000;
        userPropertyData.LastConnect = LocalTime.ToString();

        userCharacterUpgradeData.HPLevel = 1;
        userCharacterUpgradeData.DEFLevel = 1;
        userCharacterUpgradeData.ATKLevel = 1;
        userCharacterUpgradeData.SPDLevel = 1;
        userCharacterUpgradeData.ATSLevel = 1;
        userCharacterUpgradeData.EVDLevel = 1;
        userCharacterUpgradeData.CRTLevel = 1;
        userCharacterUpgradeData.CRTDLevel = 1;
        
        userWeaponInvenData.WeaponLists.Clear();
        userWeaponInvenData.WeaponLists.Add(50000000);
        
        userArmorInvenData.ArmorLists.Clear();
        userArmorInvenData.ArmorLists.Add(70000000);
        userArmorInvenData.ArmorLists.Add(70100000);
        userArmorInvenData.ArmorLists.Add(70200000);
        userArmorInvenData.ArmorLists.Add(70300000);
        userArmorInvenData.ArmorLists.Add(70400000);
        
        userConsumableItemInvenData.ConsumableItemList.Clear();
        userConsumableItemInvenData.ConsumableItemList.Add(40000000);
    }
    
    // 유저 데이터 값 타입별로 저장
    public void SetUserData(UserDataType type, JsonData json)
    {
        switch (type)
        {
            case UserDataType.UserProperty:
                userPropertyData.RowIndate = json["inDate"].ToString();
                
                userPropertyData.Gold         = int.Parse(json["UserPropertyData"]["Gold"].ToString());
                userPropertyData.Dia          = int.Parse(json["UserPropertyData"]["Dia"].ToString());
                userPropertyData.AccountLevel = int.Parse(json["UserPropertyData"]["AccountLevel"].ToString());
                userPropertyData.Character    = int.Parse(json["UserPropertyData"]["Character"].ToString());
                userPropertyData.Weapon       = int.Parse(json["UserPropertyData"]["Weapon"].ToString());
                userPropertyData.Armor1       = int.Parse(json["UserPropertyData"]["Armor1"].ToString());
                userPropertyData.Armor2       = int.Parse(json["UserPropertyData"]["Armor2"].ToString());
                userPropertyData.Armor3       = int.Parse(json["UserPropertyData"]["Armor3"].ToString());
                userPropertyData.Armor4       = int.Parse(json["UserPropertyData"]["Armor4"].ToString());
                userPropertyData.Armor5       = int.Parse(json["UserPropertyData"]["Armor5"].ToString());
                
                userPropertyData.LastConnect = json["LastConnect"].ToString();
                break;
            case UserDataType.UserCharacterUpgrade:
                userCharacterUpgradeData.RowIndate = json["inDate"].ToString();
                
                userCharacterUpgradeData.HPLevel   = int.Parse(json["UserCharacterUpgradeData"]["HPLevel"].ToString());
                userCharacterUpgradeData.DEFLevel  = int.Parse(json["UserCharacterUpgradeData"]["DEFLevel"].ToString());
                userCharacterUpgradeData.ATKLevel  = int.Parse(json["UserCharacterUpgradeData"]["ATKLevel"].ToString());
                userCharacterUpgradeData.SPDLevel  = int.Parse(json["UserCharacterUpgradeData"]["SPDLevel"].ToString());
                userCharacterUpgradeData.ATSLevel  = int.Parse(json["UserCharacterUpgradeData"]["ATSLevel"].ToString());
                userCharacterUpgradeData.EVDLevel  = int.Parse(json["UserCharacterUpgradeData"]["EVDLevel"].ToString());
                userCharacterUpgradeData.CRTLevel  = int.Parse(json["UserCharacterUpgradeData"]["CRTLevel"].ToString());
                userCharacterUpgradeData.CRTDLevel = int.Parse(json["UserCharacterUpgradeData"]["CRTDLevel"].ToString());
                break;
        }
    }
    
    public void SaveAllDataAtFirst()
    {
        Debug.Log("신규 데이터 삽입");
        
        SaveUserPropertyData(SaveType.Insert);
        SaveUserCharacterUpgradeData(SaveType.Insert);
        
        BackendManager.Instance.SendTransaction(TransactionType.Insert, this);
    }
    
    public void SaveUserPropertyData(SaveType type)
    {
        Param paramUserPropertyData = new Param();
        
        userPropertyData.LastConnect = LocalTime.ToString("yyyy-MM-dd HH:mm:ss");
        
        paramUserPropertyData.Add("Gold", userPropertyData.Gold);
        paramUserPropertyData.Add("Dia", userPropertyData.Dia);
        paramUserPropertyData.Add("AccountLevel", userPropertyData.AccountLevel);
        paramUserPropertyData.Add("Character", userPropertyData.Character);
        paramUserPropertyData.Add("Weapon", userPropertyData.Weapon);
        paramUserPropertyData.Add("Armor1", userPropertyData.Armor1);
        paramUserPropertyData.Add("Armor2", userPropertyData.Armor2);
        paramUserPropertyData.Add("Armor3", userPropertyData.Armor3);
        paramUserPropertyData.Add("Armor4", userPropertyData.Armor4);
        paramUserPropertyData.Add("Armor5", userPropertyData.Armor5);
        
        paramUserPropertyData.Add("LastConnect", userPropertyData.LastConnect);
        
        if(type == SaveType.Insert)
            BackendManager.Instance.AddTransactionInsert(UserDataType.UserProperty, paramUserPropertyData);
        else
            BackendManager.Instance.AddTransactionUpdate(UserDataType.UserProperty, userPropertyData.RowIndate, paramUserPropertyData);
    }
    
    public void SaveUserCharacterUpgradeData(SaveType type)
    {
        Param paramUserCharacterUpgradeData = new Param();
        
        paramUserCharacterUpgradeData.Add("HPLevel", userCharacterUpgradeData.HPLevel);
        paramUserCharacterUpgradeData.Add("DEFLevel", userCharacterUpgradeData.DEFLevel);
        paramUserCharacterUpgradeData.Add("ATKLevel", userCharacterUpgradeData.ATKLevel);
        paramUserCharacterUpgradeData.Add("SPDLevel", userCharacterUpgradeData.SPDLevel);
        paramUserCharacterUpgradeData.Add("ATSLevel", userCharacterUpgradeData.ATSLevel);
        paramUserCharacterUpgradeData.Add("EVDLevel", userCharacterUpgradeData.EVDLevel);
        paramUserCharacterUpgradeData.Add("CRTLevel", userCharacterUpgradeData.CRTLevel);
        paramUserCharacterUpgradeData.Add("CRTDLevel", userCharacterUpgradeData.CRTDLevel);

        if(type == SaveType.Insert)
            BackendManager.Instance.AddTransactionInsert(UserDataType.UserCharacterUpgrade, paramUserCharacterUpgradeData);
        else
            BackendManager.Instance.AddTransactionUpdate(UserDataType.UserCharacterUpgrade, userCharacterUpgradeData.RowIndate, paramUserCharacterUpgradeData);
    }
}

