using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginSceneUI : MonoBehaviour
{
    public TMP_Text localAppVersionText;
    public TMP_Text serverAppVersionText;
    public TMP_Text loginStatusText;
    public TMP_Text serverDBStatusText;
    public TMP_Text tableDataStatusText;

    public GameObject loginButtonParent;

    private DataManager dataManager;

    private void Start()
    {
        dataManager = DataManager.Instance;
    }

    // 버튼 눌러 신규 가입한 경우
    public void TryLogin(int loginType)
    {
        switch (loginType)
        {
            case 0 :
                dataManager.loginType = LoginType.Guest;
                break;
            case 1 :
                dataManager.loginType = LoginType.Google;
                break;
            case 2 :
                dataManager.loginType = LoginType.Apple;
                break;
        }

        DataManager.Instance.LoginWithType();
    }
}
