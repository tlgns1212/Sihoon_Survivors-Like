using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        GameOverKillObject,
    }
    enum Texts
    {
        GameOverPopupTitleText,
        GameOverStageValueText,
        LastWaveText,
        GameOverLastWaveValueText,
        GameOverKillValueText,
        ConfirmButtonText,
    }
    enum Buttons
    {
        StatisticsButton,
        ConfirmButton,
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
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_Gameover");

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        // 해당 스테이지 수
        GetText((int)Texts.GameOverStageValueText).text = $"{Managers.Game.CurrentStageData.StageIndex} STAGE";
        // 죽기 전 마지막 웨이브 수
        GetText((int)Texts.GameOverLastWaveValueText).text = $"{Managers.Game.CurrentWaveIndex + 1}";
        // 죽기 전 마지막 킬 수
        GetText((int)Texts.GameOverKillValueText).text = $"{Managers.Game.Player.KillCount}";

        Refresh();
    }

    void Refresh()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameOverKillObject).GetComponent<RectTransform>());
    }

    void OnClickStatisticsButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
    }

    void OnClickConfirmButton()
    {
        Managers.Sound.PlayButtonClick();

        StageClearInfo info;
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
        {
            // 기록 갱신
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
