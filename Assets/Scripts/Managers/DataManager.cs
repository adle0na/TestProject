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
    
    private DateTime serverTime;
    private bool isServerTimeUpdate;
    private bool isChangeServerTime;
 
    public DateTime LocalTime;
    
    List<Dictionary<string, object>> DataDic = new List<Dictionary<string, object>>();
    
    public UserAccountData     userAccountData        = new();
    public UserPropertyData    userPropertyData       = new();
    public UserWeaponInvenData userWeaponInvenData    = new();
    public UserArmorInvenData  userArmorInvenData     = new();
    public UserItemInvenData   userItemInvenData      = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        //SetData();
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
    
    
    public void SetRowInDate(UserDataType table, string inDate)
    {
        switch (table)
        {
            case UserDataType.UserAccount:
                userAccountData.RowIndate  = inDate;
                break;
            case UserDataType.UserProperty:
                userPropertyData.RowIndate = inDate;
                break;
        }
    }
    
    //유저 초기값
    public void SetUserData()
    {
        BackendManager.Instance.Nickname = Backend.UserNickName;

        userAccountData.AccountLevel = 1;
        userAccountData.LastConnect = LocalTime.ToString();
    }
    
    // 유저 값 저장
    public void SetUserData(UserDataType type, JsonData json)
    {
        switch (type)
        {
            case UserDataType.UserAccount:
                userAccountData.RowIndate = json["inDate"].ToString();
                
                userAccountData.AccountLevel = int.Parse(json["AccountLevel"].ToString());
                userAccountData.LastConnect  = json["LastConnect"].ToString();
                break;
            case UserDataType.UserProperty:
                userPropertyData.RowIndate = json["inDate"].ToString();
                
                userPropertyData.Gold = int.Parse(json["UserPropertyData"]["Gold"].ToString());
                userPropertyData.Dia  = int.Parse(json["UserPropertyData"]["Dia"].ToString());
                break;
        }
    }
    
    public void SaveAllDataAtFirst()
    {
        Debug.Log("신규 데이터 삽입");
        SaveUserData(SaveType.Insert);
        BackendManager.Instance.SendTransaction(TransactionType.Insert, this);
    }
    
    public void SaveUserData(SaveType type)
    {
        userAccountData.LastConnect = LocalTime.ToString("yyyy-MM-dd HH:mm:ss");
        Param paramUserData = new Param();
        paramUserData.Add("AccountLevel", userAccountData.AccountLevel);
        paramUserData.Add("LastConnect", userAccountData.LastConnect);
        if(type == SaveType.Insert)
            BackendManager.Instance.AddTransactionInsert(UserDataType.UserAccount, paramUserData);
        else
            BackendManager.Instance.AddTransactionUpdate(UserDataType.UserAccount, userAccountData.RowIndate, paramUserData);
    }
}

