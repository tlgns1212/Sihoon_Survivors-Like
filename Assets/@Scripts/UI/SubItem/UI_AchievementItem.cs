using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_AchievementItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        ProgressSlider,
    }

    enum Buttons
    {
        GetButton,
        // // GoNowButton,
    }

    enum Texts
    {
        RewardItemValueText,
        CompleteText,
        AchievementNameValueText,
        AchievementValueText,
        ProgressText,
    }

    enum Images
    {
        RewardItemIcon,
    }
    enum MissionState
    {
        Progress,
        Complete,
        Rewarded,
    }
    #endregion
    
    
    private AchievementData _achievementData;

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
        // // GetButton((int)Buttons.GoNowButton).gameObject.BindEvent(OnClickGoNowButton);
        // // GetButton((int)Buttons.GoNowButton).GetOrAddComponent<UI_ButtonAnimation>();
        AchievementContentInit();

        Refresh();
        return true;
    }
    
    public void SetInfo(AchievementData achievementData)
    {
        transform.localScale = Vector3.one;
        _achievementData = achievementData;

        Refresh();
    }

    void Refresh()
    {
        // 미션 클리어 상태에 따라 활성화
        // - GoNowButton : 진행중
        // - GetButton : 클리어 시
        // - CompleteText : 보상 지급 완료

        if (_achievementData == null)
            return;

        GetText((int)Texts.RewardItemValueText).text = $"{_achievementData.RewardValue}";
        GetText((int)Texts.AchievementNameValueText).text = $"{_achievementData.DescriptionTextId}";
        GetObject((int)GameObjects.ProgressSlider).GetComponent<Slider>().value = 0;

        int progress = Managers.Achievement.GetProgressValue(_achievementData.MissionTarget);
            if (progress > 0)
            {
                GetObject((int)GameObjects.ProgressSlider).GetComponent<Slider>().value = (float)progress / _achievementData.MissionTargetValue;
            }

            if (progress >= _achievementData.MissionTargetValue)
            {
                SetButtonUI(MissionState.Complete);
                if (_achievementData.IsRewarded == true)
                {
                    SetButtonUI(MissionState.Rewarded);
                }
            }
            else
            {
                SetButtonUI(MissionState.Progress);
            }

            GetText((int)Texts.AchievementValueText).text = $"{progress}/{_achievementData.MissionTargetValue}";

        string sprName = Managers.Data.MaterialDic[_achievementData.ClearRewardItemId].SpriteName;
        GetImage((int)Images.RewardItemIcon).sprite = Managers.Resource.Load<Sprite>(sprName);
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
        counts[0] = _achievementData.RewardValue;

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        Managers.Game.Dia += _achievementData.RewardValue;
        Managers.Achievement.RewardedAchievement(_achievementData.AchievementId);
        _achievementData = Managers.Achievement.GetNextAchievement(_achievementData.AchievementId);
        if (_achievementData != null)
        {
            Refresh();    
        }
        rewardPopup.SetInfo(spriteNames, counts);
    }
}
