using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BackEnd;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackendManager : Singleton<BackendManager>
{
    public static BackendManager instance;
    
    public ServerType serverStatus = ServerType.Live; 
    
    [SerializeField]
    List<TransactionValue> transactionList = new List<TransactionValue>();
    
    public int checkLoginWayData = -1;

    public bool IsInitialize;
    public bool IsLogined;

    public int SuccessLoadDataCount = 0;
    
    Thread serverCheckThread;
    
    private Queue<int> checkEvent = new Queue<int>();
    
    private int initTimeCount = 0;

    public string UserIndate = string.Empty;
    public string Nickname   = string.Empty;
    public string UID        = string.Empty;
    
    //시즌패스 사용시 구독정보
    //public string SubscriptionType = string.Empty;

    // 방치형 보상 처리
    public bool LoadServerTime = false;

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
        BackendCustomSetting settings = new BackendCustomSetting();
        
        switch (serverStatus)
        {
            case ServerType.Dev:
                settings.clientAppID = "6d4666a0-74f3-11ef-9dc4-4195e354b2e68395";
                settings.signatureKey = "6d468db0-74f3-11ef-9dc4-4195e354b2e68395";
                settings.functionAuthKey = "";
                settings.timeOutSec = 100;
                settings.useAsyncPoll = true;
                break;
            case ServerType.Live:
                settings.clientAppID = "fc115650-7684-11ef-9995-1d5167c08fef8407";
                settings.signatureKey = "fc117d60-7684-11ef-9995-1d5167c08fef8407";
                settings.functionAuthKey = "";
                settings.timeOutSec = 100;
                settings.useAsyncPoll = true;
                break;
        }

        Debug.Log("일반 초기화 실행");
        var bro = Backend.Initialize(settings);
        
        if (bro.IsSuccess())
        {
            Debug.Log("Backend Initialize Success : " + bro);
            
            Backend.ErrorHandler.OnMaintenanceError = () => {
                Debug.LogError("Server Inspection Error");
            };
            Backend.ErrorHandler.OnTooManyRequestError = () => {
                Debug.LogError("403 Error");
            };
            Backend.ErrorHandler.OnTooManyRequestByLocalError = () => {
                Debug.LogError("403 Local Error");
            };
            Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
                Debug.LogError("Cannot Refresh");
            };
            Backend.ErrorHandler.OnDeviceBlockError = () => {
                Debug.LogError("Device Access Denied");
            };

            /* 버전체크 나중에 추가 
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //CheckLastVersion();
            }
            */
            
            CheckLoginWayData();

            if (checkLoginWayData >= 0)
            {
                StartTokenLogin();
            }

            IsInitialize = true;
        }
        else
        {
            Debug.Log("Initialize Failed : " + bro);
        }
    }

    #region DebugFunctions

    public static void BackEndDebug(string str, BackendReturnObject bro)
    {
        Debug.Log(str);
        Debug.LogFormat("Status : {0}\nErrorCode : {1}\nMessage : {2}", bro.GetStatusCode(), bro.GetErrorCode(), bro.GetMessage());
    }
    
    public void InsertLog(GameLogType type, string str)
    {
        Param param = new();
        param.Add("Key", str);

        SendLog(type, param);
    }

    public void SendLog(GameLogType type, Param param)
    {
        string paramToString = JsonConvert.SerializeObject(param.GetValue());
        Debug.Log($"{type} / {paramToString}");
        Backend.GameLog.InsertLog($"{type}", param, 30, (callback) => { });
    }

    #endregion
    
    #region ### LoginFunctions ###
    public void GuestLoginSequense()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin("GuestLoginTry2 Sequence");
        Debug.LogError($"{bro.IsSuccess()} {bro.GetStatusCode()} {bro.GetErrorCode()} {bro.GetMessage()}");
        StartCoroutine((LoginProcess(bro)));
        PlayerPrefs.SetInt("LoginWay", 0);
    }
    
    public void GuestIdDelete()
    {
        if (Backend.BMember.GetGuestID().Length > 0)
        {
            Debug.LogFormat("GuestID {0} Delete", Backend.BMember.GetGuestID());
            Backend.BMember.DeleteGuestInfo();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("LoginWay", -1);
        }
        else
        {
            Debug.LogFormat("Server Not Connected");
        }
    }
    
    /* GPGS 나중에 추가
    public void GoogleLoginSetting()
    {
        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
            .RequestIdToken()
            .Build();
        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 바꿔주세요.
        //GPGS 시작.
        PlayGamesPlatform.Activate();
        
        GoogleLoginSequense();
    } */

    // 로그인 코루틴
    private IEnumerator LoginProcess(BackendReturnObject bro)
    {
        if (!bro.IsSuccess())
        {
            PlayerPrefs.SetInt("LoginWay", -1);
            Debug.LogError($"{bro.IsSuccess()} / {bro.GetStatusCode()} / {bro.GetErrorCode()} / {bro.GetMessage()}");
            switch (bro.GetStatusCode())
            {
                case "401": //서버점검
                    switch (bro.GetErrorCode())
                    {
                        case "BadUnauthorizedException":
                            if (bro.GetMessage().Contains("accessToken") || bro.GetMessage().Contains("refreshToken"))
                            {
                                Debug.LogError("accessToken or refreshToken 만료");
                            }
                            else if (bro.GetMessage().Contains("maintenance"))
                            {
                                Debug.LogError("서버 점검중");
                                
                            }
                            else if (bro.GetMessage().Contains("customId"))
                            {
                                Debug.LogError($"Guest Data Damaged");
                                GuestIdDelete();
                                UIManager.Instance.OpenRecyclePopupOneButton("시스템 에러", "삭제된 계정에 접근 하였습니다. \n 로그인을 다시 시도해주세요.", null, "");
                            }
                            break;
                    }
                    break;
                case "403": //에러
                    if(bro.GetMessage().Contains("blocked "))
                    {
                        Debug.LogError($"{bro.GetErrorCode()}");
                    }
                    break;
            }
            yield break;
        }
        
        // UID 불러오기
        Backend.BMember.GetUserInfo((callback) =>
        {
            Debug.LogError(callback.GetReturnValue());

            if (Backend.UID != null)
            {
                // UID 확인 성공 로그인 완료 처리
                UID = Backend.UID;
                Nickname = Backend.UserNickName;
                CheckNickName();
            }
        });

        UserIndate = Backend.UserInDate;
        
        // 방치형 보상 체크 함수
        GetServerTime();
        
        while (!LoadServerTime)
            yield return null;
        Debug.LogError($"LoadServerTime {LoadServerTime}");
        
        // 일단은 그냥 데이터 불러오기, 나중엔 로컬에 저장하고 불러오는 방식으로 변경 필요
        GoogleSheetManager.Instance.LoadAllData();
        
        DataManager.Instance.SetDefaultData();

        switch (bro.GetStatusCode())
        {
            case "201": //신규 회원가입
                Debug.Log("신규 회원으로 시작합니다");
                SetNewUserDataSaveToServer();
                InsertLog(GameLogType.SignIn, $"{Application.version}");
                break;
            case "200": //로그인
                Debug.Log("일반 로그인");
                GetUserDataFromServer();
                InsertLog(GameLogType.LogIn, $"{Application.version}");
                break;
        }
        
        yield return new WaitUntil(() => SuccessLoadDataCount == typeof(UserDataType).GetEnumNames().Length);

        StartCoroutine(nameof(RefreshToken));
    }
    
    public void StartTokenLogin()
    {
        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            StartCoroutine(LoginProcess(callback));
        });
    }
    
    public void SetNewUserDataSaveToServer()
    {
        DataManager.Instance.SaveAllDataAtFirst();
    }
    
    // 기기에 저장된 로그인 방식 확인
    public void CheckLoginWayData()
    {
        if (PlayerPrefs.HasKey("LoginWay"))
        {
            checkLoginWayData = PlayerPrefs.GetInt("LoginWay");
        }
        Debug.LogError(PlayerPrefs.HasKey("LoginWay") + checkLoginWayData.ToString());
    }

    public void CheckNickName()
    {
        if (Nickname.Length <= 0)
        {
            Debug.LogError("닉네임이 생성되지 않은 계정");

            UIManager.Instance.OpenNickNameChangePopup(false);
        }
        else
        {
            IsLogined = true;
        }
    }
    
    #endregion
    
    public void GetServerTime()
    {
        //InitTime();
        InvokeRepeating("GetServerTimeFor5minutes", 300f, 300f);
    }
    
    public void GetUserDataFromServer()
    {
        transactionList.Clear();
        for(int i = 0; i < Enum.GetValues(typeof(UserDataType)).Length; i++)
            AddTransactionSetGet((UserDataType)i);
        SendTransaction(TransactionType.SetGet, this);
    }
    
    public void AddTransactionSetGet(UserDataType table)
    {
        for (int i = 0; i < transactionList.Count; i++)
            if (transactionList[i].table.ToString() == table.ToString())
                transactionList.RemoveAt(i);
        transactionList.Add(TransactionValue.SetGet(table.ToString(), new Where()));
        if (transactionList.Count > 9)
            SendTransaction(TransactionType.SetGet, this);
    }
    
    public void AddTransactionInsert(UserDataType table, Param param)
    {
        for (int i = 0; i < transactionList.Count; i++)
            if (transactionList[i].table.ToString() == table.ToString())
                transactionList.RemoveAt(i);
        transactionList.Add(TransactionValue.SetInsert(table.ToString(), param));
        if (transactionList.Count > 9)
            SendTransaction(TransactionType.Insert, this);
    }
    
    public void AddTransactionUpdate(UserDataType table, string indate, Param param)
    {
        for (int i = 0; i < transactionList.Count; i++)
            if (transactionList[i].table.ToString() == table.ToString())
                transactionList.RemoveAt(i);
        transactionList.Add(TransactionValue.SetUpdateV2(table.ToString(), indate, Backend.UserInDate,  param));
        if (transactionList.Count > 9)
            SendTransaction(TransactionType.Update, this);
    }
    
    public BackendReturnObject SendTransaction(TransactionType type, object obj)
    {

        if (transactionList.Count <= 0) {
            return null;
        }

        BackendReturnObject bro = null;
        switch (type)
        {
            case TransactionType.Insert:
                BackendReturnObject broInsert = Backend.GameData.TransactionWriteV2(transactionList);
                if (broInsert.IsSuccess())
                {
                    LitJson.JsonData json = broInsert.GetReturnValuetoJSON()["putItem"];
                    for (int i = 0; i < json.Count; i++)
                        DataManager.Instance.SetRowInDate((UserDataType)Enum.Parse(typeof(UserDataType), json[i]["table"].ToString()), json[i]["inDate"].ToString());
                    SuccessLoadDataCount += json.Count;
                }
                else
                {
                    Debug.LogError($"Send Failed {broInsert.IsSuccess()} {broInsert.GetStatusCode()} {broInsert.GetErrorCode()} {broInsert.GetMessage()}");
                }
                break;
            case TransactionType.Update:
                for (int i = 0; i < transactionList.Count; i++)
                {
                    Param param = new Param();
                    param = transactionList[i].param;
                    string paramToString = JsonConvert.SerializeObject(param.GetValue());
                    int count = System.Text.Encoding.Default.GetByteCount(paramToString);
                    Debug.Log($"{transactionList[i].table} / byte : {count}");
                }
                Backend.GameData.TransactionWriteV2(transactionList, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        Debug.Log($"Data Update Success");
                    }
                    else
                    {
                        if (callback.GetStatusCode().Contains("400"))
                        {
                            if (callback.GetErrorCode().Contains("HttpRequestException"))
                            {
                                //네트워크 연결 끊어짐
                            }
                        }

                        if (callback.GetStatusCode().Contains("401"))
                        {
                            if (callback.GetErrorCode().Contains("BadUnauthorizedException"))
                            {
                                //업데이트 에러
                            }
                        }
                        Debug.LogError($"Send Failed {callback.IsSuccess()} {callback.GetStatusCode()} {callback.GetErrorCode()} {callback.GetMessage()}");
                    }
                });
                break;
            case TransactionType.SetGet:
                List<TransactionValue> actions = new List<TransactionValue>();
                actions = new(transactionList);
                Backend.GameData.TransactionReadV2(actions, (callback) =>
                {
                    // 이후 처리
                    if (callback.IsSuccess())
                    {
                        LitJson.JsonData gameDataListJson = callback.GetFlattenJSON()["Responses"];
                        for (int j = 0; j < gameDataListJson.Count; j++)
                            DataManager.Instance.SetUserData(Enum.Parse<UserDataType>(actions[j].table), gameDataListJson[j]);
                        SuccessLoadDataCount += gameDataListJson.Count;
                    }
                    else
                    {
                        switch (callback.GetStatusCode())
                        {
                            case "404":
                                if (callback.GetErrorCode() == "NotFoundException")
                                {
                                    transactionList.Clear();
                                    for (int i = 0; i < actions.Count; i++)
                                    {
                                        BackendReturnObject bro = Backend.GameData.GetMyData(actions[i].table, new Where());
                                        if (bro.IsSuccess())
                                        {
                                            LitJson.JsonData data = bro.FlattenRows();
                                            if (data.Count <= 0)
                                            {
                                                //insert 필요
                                                switch (actions[i].table)
                                                {
                                                    case "UserData":
                                                        //DataManager.Instance.SaveUserData(SaveType.Insert);
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                DataManager.Instance.SetUserData(Enum.Parse<UserDataType>(actions[i].table), data[0]);
                                                SuccessLoadDataCount++;
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError($"GetData {bro.IsSuccess()} {bro.GetStatusCode()} {bro.GetErrorCode()} {bro.GetMessage()}");
                                        }
                                    }
                                    SendTransaction(TransactionType.Insert, this);
                                }
                                break;
                        }
                    }
                });
                break;
        }
        transactionList.Clear();
        return bro;
    }
    
    private IEnumerator RefreshToken()
    {
        int count = 0;
        WaitForSeconds waitForRefreshTokenCycle = new WaitForSeconds(60 * 60 * 6); // 60초 x 60분 x 8시간
        WaitForSeconds waitForRetryCycle = new WaitForSeconds(15f);
        // 첫 호출시에는 리프레시 토큰하지 않도록 8시간을 기다리게 해준다.
        bool isStart = false;
        if (!isStart)
        {
            isStart = true;
            yield return waitForRefreshTokenCycle; // 8시간 기다린 후 반복문 시작
        }
        BackendReturnObject bro = null;
        bool isRefreshSuccess = false;
        // 이후부터는 반복문을 돌면서 8시간마다 최대 3번의 리프레시 토큰을 수행하게 된다.
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                bro = Backend.BMember.RefreshTheBackendToken();
                Debug.Log("리프레시 토큰 성공 여부 : " + bro);
                if (bro.IsSuccess())
                {
                    Debug.Log("토큰 재발급 완료");
                    isRefreshSuccess = true;
                    break;
                }
                else
                {
                    if (bro.GetMessage().Contains("bad refreshToken"))
                    {
                        Debug.LogError("중복 로그인 발생");
                        isRefreshSuccess = false;
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("15초 뒤에 토큰 재발급 다시 시도");
                    }
                }
                yield return waitForRetryCycle; // 15초 뒤에 다시시도
            }
            if (bro == null)
            {
                Debug.LogError("토큰 재발급에 문제가 발생했습니다. 수동 로그인을 시도해주세요");
                //팝업 띄울것
                StopCoroutine(nameof(RefreshToken));
            }
            if (!bro.IsSuccess())
            {
                Debug.LogError("토큰 재발급에 문제가 발생하였습니다 : " + bro);
                //팝업 띄울것
                StopCoroutine(nameof(RefreshToken));
            }
            // 성공할 경우 값 초기화 후 8시간을 wait합니다.
            if (isRefreshSuccess)
            {
                Debug.Log("8시간 토큰 재 호출");
                isRefreshSuccess = false;
                count = 0;
                yield return waitForRefreshTokenCycle;
            }
        }
    }

    public void ChangeNickName(string nickName, bool isChange)
    {
        Backend.BMember.UpdateNickname( nickName, ( callback ) =>
        {
            switch (callback.StatusCode)
            {
                case 204 :
                    if (isChange)
                    {
                        UIManager.Instance.OpenRecyclePopupOneButton("시스템 메세지", $"닉네임을 변경을 {nickName}으로 완료 했습니다.", null, "");
                    }
                    else
                    {
                        Nickname = nickName;
                        IsLogined = true;
                    }
                    break;
                case 400 :
                    UIManager.Instance.OpenRecyclePopupOneButton("시스템 에러", $"잘못된 닉네임 입력입니다.", null, "");
                    break;
                case 409 :
                    UIManager.Instance.OpenRecyclePopupOneButton("시스템 에러", $"이미 중복된 닉네임이 있습니다.", null, "");
                    break;
            }
        });
    }
    
    /* 방치형 보상때 사용
    public void InitTime()
    {
        if(initTimeCount > 3)
        {
            Debug.LogError(initTimeCount);
            //GameManager.Instance.UiManager.AlertManager.Initialize(AlertType.DataLoadError);
            return;
        }

        Backend.Utils.GetServerTime((callback) =>
        {
            if (callback.IsSuccess())
            {
                string time = callback.GetReturnValuetoJSON()["utcTime"].ToString();
                DateTime parsedDate = DateTime.Parse(time);
                DataManager.Instance.SetLocalTime(parsedDate);
                LoadServerTime = true;
            }
            else
            {
                initTimeCount++;
                InitTime();
            }
        });
    }
    */
}
