using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        ResultRewardScrollContentObject,
        ResultGoldObject,
        ResultKillObject,
    }

    enum Texts
    {
        GameResultPopupTitleText,
        ResultStageValueText,
        ResultSurvivalTimeText,
        ResultSurvivalTimeValueText,
        ResultKillValueText,
        ConfirmButtonText,
        ResultGoldValueText,
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
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }
    
    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.ResultStageValueText).text = $"{Managers.Game.CurrentStageData.StageIndex} STAGE";
        
        GetText((int)Texts.ResultKillValueText).text = $"{Managers.Game.Player.KillCount}";
        GetText((int)Texts.ResultGoldValueText).text = $"{Managers.Game.CurrentStageData.ClearRewardGold}";

        Managers.Game.Gold += Managers.Game.CurrentStageData.ClearRewardGold;
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL], Managers.Game.CurrentStageData.ClearRewardGold);

        Transform container = GetObject((int)GameObjects.ResultRewardScrollContentObject).transform;
        container.gameObject.DestroyChilds();

        UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container);
        item.SetInfo(Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL].SpriteName, Managers.Game.CurrentStageData.ClearRewardGold);
        
        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ResultGoldObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ResultKillObject).GetComponent<RectTransform>());
    }

    void OnClickStatisticsButton()
    {
        Managers.Sound.PlayButtonClick();
        // 통계 팝업 호출
    }

    void OnClickConfirmButton()
    {
        Managers.Sound.PlayButtonClick();
        StageClearInfo info;
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
        {
            info.MaxWaveIndex = Managers.Game.CurrentWaveIndex;
            info.isClear = true;
            Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.StageIndex] = info;
        }
        Managers.Game.ClearContinueData();
        Managers.Game.SetNextStage();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }
}
