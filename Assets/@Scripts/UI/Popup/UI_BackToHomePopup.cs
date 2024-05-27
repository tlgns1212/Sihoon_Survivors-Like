using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_BackToHomePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject
    }
    enum Buttons
    {
        ResumeButton,
        QuitButton,
    }
    enum Texts
    {
        BackToHomeTitleText,
        BackToHomeContentText,
        ResumeText,
        QuitText,
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
        GetButton((int)Buttons.QuitButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }

    void Refresh()
    {

    }

    void OnClickResumeButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickQuitButton()
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = true;
        Managers.Game.Player.StopAllCoroutines();

        StageClearInfo info;
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
        {
            if (Managers.Game.CurrentWaveIndex > info.MaxWaveIndex)
            {
                info.MaxWaveIndex = Managers.Game.CurrentWaveIndex;
                Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.StageIndex] = info;
            }
        }

        Managers.Game.ClearContinueData();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }
}
