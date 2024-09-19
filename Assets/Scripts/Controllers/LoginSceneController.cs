using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSceneController : MonoBehaviour
{
    private DataManager dataManager;

    public LoginSceneUI loginSceneUI;
    
    public GameObject loginSceneUIPrefab;
    public GameObject selectCharacterUIPrefab;

    public List<Button> loginButtons;

    public TMP_Text loginStatusText;

    public TMP_Text serverDBLoadingText;

    public TMP_Text tableDataLoadingText;

    private void Awake()
    {
        BackendManager.Instance.Initialize();
    }

    void Start()
    {
        dataManager = DataManager.Instance;
        
        //AudioManager.Instance.PlayBGM(0);

        StartCoroutine(nameof(CheckBackendInitialized));

        StartCoroutine(nameof(CheckIsAlreadyLogined));
    }
    void LoginSceneUIInitialize()
    {
        Debug.LogError("로그인 UI 활성화 했음");
        GameObject getLoginSceneUI = Instantiate(loginSceneUIPrefab, UIManager.Instance.sceneController.transform);
        
        getLoginSceneUI.transform.SetSiblingIndex(0);
        
        loginSceneUI = getLoginSceneUI.GetComponent<LoginSceneUI>();
        
        Button[] buttons = loginSceneUI.loginButtonParent.GetComponentsInChildren<Button>();
        
        foreach (Button button in buttons)
        {
            loginButtons.Add(button);
        }
    }

    IEnumerator CheckBackendInitialized()
    {
        yield return new WaitUntil(() => BackendManager.Instance.IsInitialize);

        LoginSceneUIInitialize();
    }

    IEnumerator CheckIsAlreadyLogined()
    {
        yield return new WaitUntil(() => BackendManager.Instance.IsLogined);
        
        loginSceneUI.loginButtonParent.SetActive(false);
        
        SceneManager.LoadScene(1);
    }
}
