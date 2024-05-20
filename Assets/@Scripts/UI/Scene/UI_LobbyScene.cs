
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
        // 오브젝트 바인딩
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        // 테스트용
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        return true;
    }

    void Refresh()
    {

    }

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnResourcesChanged -= Refresh;
        }
    }

    void OnClickStartButton()
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = false;
        if (Managers.Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Debug.Log("Not Enough Stamina");
            return;
        }

        Managers.Game.Stamina -= Define.GAME_PER_STAMINA;
        if (Managers.Game.DicMission.TryGetValue(Define.MissionTarget.StageEnter, out MissionInfo mission))
        {
            mission.Progress++;
        }

        Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
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
