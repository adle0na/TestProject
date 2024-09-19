using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private static UIManager instance;
    
    // SceneController (Canvas)
    public Canvas sceneController;

    // 팝업 생성 위치
    public Transform popupParent;

    // Canvas에 존재하는 PopupList
    public List<GameObject> PopupList;

    // 가장 최근에 생성된 팝업
    public GameObject CurrentPopup;

    // 가장 최근에 생성된 팝업의 정보
    public CurrentUIStatus CurrentUIStatus;

    // ABB 액션
    private Action ABBAction;

    [Header("PopupPrefabs")]
    public GameObject popupParentPrefab;

    public GameObject NickNameChangePopupPrefab;
    
    public GameObject RecyclePopup_1ButtonPrefab;
    public GameObject RecyclePopup_2ButtonPrefab;
    public GameObject SettingPopupPrefab;

    public GameObject LoadingIndicatorPrefab;
    public GameObject InGameReadyPrefab;

    private GameObject indicator;
    private GameObject inGamePopup;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCanvasAndUIController();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentUIStatus == CurrentUIStatus.ABBPopup)
            {
                if (ABBAction == null)
                {
                    PopupListPop();
                }
                else
                {
                    ABBAction();
                }
            }
                
        }
    }
    
    // 씬 이동시 마다 컨트롤러 세팅
    public void FindCanvasAndUIController()
    {
        sceneController = null;
        
        sceneController = FindObjectOfType<Canvas>();

        if (sceneController != null)
        {
            GameObject popupTransformObj = Instantiate(popupParentPrefab, sceneController.transform);

            popupTransformObj.transform.SetSiblingIndex(sceneController.transform.childCount - 1);

            popupParent = popupTransformObj.transform;
        }
        else
        {
            Debug.LogError("UI 컨트롤러 탐색 실패");
        }
    }

    #region UIFunctions

    #endregion
    
    #region PopupFunctions

    // ABB 입력 액션을 가진 팝업 생성
    public void PopupListAddABB(GameObject gameObject, Action action)
    {
        PopupList.Add(gameObject);

        gameObject.SetActive(true);
        
        CurrentPopup = gameObject;

        ABBAction = action;

        CurrentUIStatus = CurrentUIStatus.ABBPopup;
        
        DarkBGCheck();
    }
    
    // ABB 입력 액션이 없는 팝업 생성
    public void PopupListAddNoneABB(GameObject gameObject)
    {
        PopupList.Add(gameObject);

        gameObject.SetActive(true);
        
        CurrentPopup = gameObject;
        
        CurrentUIStatus = CurrentUIStatus.NoneABBPopup;

        DarkBGCheck();
    }
    
    // 맨위의 팝업 종료
    public void PopupListPop()
    {
        if (PopupList != null && PopupList.Count > 0)
        {
            if (CurrentPopup != null && CurrentPopup.activeSelf)
            {
                Destroy(CurrentPopup);
                    
                PopupList.RemoveAt(PopupList.Count - 1);

                if (PopupList.Count > 0)
                {
                    CurrentPopup = PopupList[0];
                }
            }
        }

        DarkBGCheck();
    }

    // UI 전환시 모든 팝업 종료
    public void AllPopupClear()
    {
        foreach (GameObject popup in PopupList)
        {
            if (popup != null && popup.activeSelf)
            {
                Destroy(popup);
            }
        }
        PopupList.Clear();
        CurrentPopup = null;

        DarkBGCheck();
    }
    
    // 팝업이 한개라도 있는 경우 DargBG를 통해 UI 터치를 막음
    private void DarkBGCheck()
    {
        popupParent.gameObject.SetActive(PopupList.Count > 0);
    }

    public void OpenPopup(GameObject popupObj)
    {
        GameObject Popup = Instantiate(popupObj, popupParent);
        
        PopupListAddABB(Popup, null);
    }

    public void OpenNickNameChangePopup(bool isChange)
    {
        GameObject nickNamePopup = Instantiate(NickNameChangePopupPrefab, popupParent);

        PopupList.Add(nickNamePopup);

        CurrentPopup = nickNamePopup;
        
        NickNameCreatePopup getPopup = nickNamePopup.GetComponent<NickNameCreatePopup>();

        getPopup.isChange = false;
        
        DarkBGCheck();
    }
    
    // 공용 1버튼 팝업 생성 (제목, 내용, ABB액션 )
    public void OpenRecyclePopupOneButton(String title, String descript, Action action, string buttonText)
    {
        GameObject Popup = Instantiate(RecyclePopup_1ButtonPrefab, popupParent);

        RecyclePopupABB getPopup = Popup.GetComponent<RecyclePopupABB>();

        getPopup.titleText.text       = title;
        getPopup.descriptionText.text = descript;

        getPopup.agreeBtnText.text = buttonText == "" ? "확인" : buttonText;
        
        getPopup.ButtonClicked = (action != null) ? action : PopupListPop;

        getPopup.SetButtonAction();
        
        PopupListAddABB(Popup, action);
    }
    
    public void OpenRecyclePopupTwoButton(String title, String descript, Action YButtonAction, Action NButtonAction)
    {
        GameObject Popup = Instantiate(RecyclePopup_2ButtonPrefab, popupParent);

        RecyclePopupNoneABB target = Popup.GetComponent<RecyclePopupNoneABB>();

        target.titleText.text       = title;
        target.descriptionText.text = descript;
        target.YAction = (YButtonAction != null) ? YButtonAction : PopupListPop;
        target.NAction = (NButtonAction != null) ? NButtonAction : PopupListPop;
        
        target.SetYButtonAction();
        target.SetNButtonAction();

        PopupListAddNoneABB(Popup);
    }

    public void OpenIndicator()
    {
        indicator = Instantiate(LoadingIndicatorPrefab, sceneController.transform);
    }

    public void CloseIndicator()
    {
        Destroy(indicator);
    }

    public void OpenReadyMSG()
    {
        inGamePopup = Instantiate(InGameReadyPrefab, sceneController.transform);
    }

    IEnumerator CloseGameStartMSG()
    {
        yield return new WaitForSeconds(1);

        Destroy(inGamePopup);
    }

    #endregion
}
