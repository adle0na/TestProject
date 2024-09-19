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

    public List<Button> loginButtons;

    public TMP_Text loginStatusText;

    public TMP_Text serverDBLoadingText;

    public TMP_Text tableDataLoadingText;
    
    void Start()
    {
        dataManager = DataManager.Instance;
        
        //AudioManager.Instance.PlayBGM(0);
    }

    void LoginSceneUIInitialize()
    {
        // GameObject getLoginSceneUI = Instantiate(loginSceneUIPrefab, UIManager.Instance.sceneController.transform);
        //
        // getLoginSceneUI.transform.SetSiblingIndex(0);
        //
        // loginSceneUI = getLoginSceneUI.GetComponent<LoginSceneUI>();

        //Button[] buttons = loginSceneUI.loginButtonParent.GetComponentsInChildren<Button>();
        // foreach (Button button in buttons)
        // {
        //     loginButtons.Add(button);
        // }

        //loginSceneUI.localAppVersionText.text = DataManager.Instance.localVersion;
        //loginSceneUI.serverAppVersionText.text = DataManager.Instance.serverVersion;
    }
}
