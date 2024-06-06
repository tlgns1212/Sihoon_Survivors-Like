using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_MissionItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        ProgressSliderObject,
    }

    enum Buttons
    {
        GetButton,
    }

    enum Texts
    {
        RewardItemValueText,
        ProgressText,
        CompleteText,
        MissionNameValueText,
        MissionProgressValueText,
    }

    enum Images
    {
        RewardItemIconImage,
    }
    enum MissionState
    {
        Progress,
        Complete,
        Rewarded,
    }
    #endregion

    private MissionData _missionData;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
        GetButton((int)Buttons.GetButton).GetOrAddComponent<UI_ButtonAnimation>();

        AchievementContentInit();

        Refresh();
        return true;
    }

    public void SetInfo(MissionData missionData)
    {
        transform.localScale = Vector3.one;
        _missionData = missionData;

        Refresh();
    }

    void Refresh()
    {
        // 미션 클리어 상태에 따라 활성화
        // - ProgressText : 진행중
        // - GetButton : 클리어 시
        // - CompleteText : 보상 지급 완료

        if (_missionData == null)
            return;

        GetText((int)Texts.RewardItemValueText).text = $"{_missionData.RewardValue}";
        GetText((int)Texts.MissionNameValueText).text = $"{_missionData.DescriptionTextId}";
        GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = 0;

        if (Managers.Game.DicMission.TryGetValue(_missionData.MissionTarget, out MissionInfo missionInfo))
        {
            if (missionInfo.Progress > 0)
            {
                GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = (float)missionInfo.Progress / _missionData.MissionTargetValue;
            }

            if (missionInfo.Progress >= _missionData.MissionTargetValue)
            {
                SetButtonUI(MissionState.Complete);
                if (missionInfo.IsRewarded == true)
                {
                    SetButtonUI(MissionState.Rewarded);
                }
            }
            else
            {
                SetButtonUI(MissionState.Progress);
            }

            GetText((int)Texts.MissionProgressValueText).text = $"{missionInfo.Progress}/{_missionData.MissionTargetValue}";
        }

        string sprName = Managers.Data.MaterialDic[_missionData.ClearRewardItemId].SpriteName;
        GetImage((int)Images.RewardItemIconImage).sprite = Managers.Resource.Load<Sprite>(sprName);
    }

    void SetButtonUI(MissionState state)
    {
        GameObject objComplete = GetButton((int)Buttons.GetButton).gameObject;
        GameObject objProgress = GetText((int)Texts.ProgressText).gameObject;
        GameObject objRewarded = GetText((int)Texts.CompleteText).gameObject;

        switch (state)
        {
            case MissionState.Progress:
                objRewarded.SetActive(false);
                objComplete.SetActive(false);
                objProgress.SetActive(true);
                break;
            case MissionState.Complete:
                objRewarded.SetActive(false);
                objComplete.SetActive(true);
                objProgress.SetActive(false);
                break;
            case MissionState.Rewarded:
                objRewarded.SetActive(true);
                objComplete.SetActive(false);
                objProgress.SetActive(false);
                break;
            default:
                break;
        }
    }

    void AchievementContentInit()
    {
        GetButton((int)Buttons.GetButton).gameObject.SetActive(true);
        GetText((int)Texts.ProgressText).gameObject.SetActive(false);
        GetText((int)Texts.CompleteText).gameObject.SetActive(false);
    }

    void OnClickGetButton()
    {
        Managers.Sound.PlayButtonClick();
        string[] spriteNames = new string[1];
        int[] counts = new int[1];

        spriteNames[0] = Managers.Data.MaterialDic[Define.ID_DIA].SpriteName;
        counts[0] = _missionData.RewardValue;

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        Managers.Game.Dia += _missionData.RewardValue;
        if (Managers.Game.DicMission.TryGetValue(_missionData.MissionTarget, out MissionInfo info))
        {
            info.IsRewarded = true;
        }
        Refresh();
        
        rewardPopup.SetInfo(spriteNames, counts);
    }
}
