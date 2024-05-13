
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_LobbyScene : UI_Scene
{
    #region Enum
    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }
    #endregion

    bool isPreload = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        // ������Ʈ ���ε�
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        // �׽�Ʈ��
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            Debug.Log("TestTEst");
            if (isPreload)
                Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
        });
        return true;
    }

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            Managers.UI.ShowToast("test");
        }
    }
#endif
}
