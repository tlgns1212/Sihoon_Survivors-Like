using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        SettingButtonRedDotObject,
        // BattlePassButtonRedDotObject,
        // AccountPassButtonRedDotObject,
        MissionButtonRedDotObject,
        AchievementButtonRedDotObject,
        AttendanceCheckButtonRedDotObject,
        OfflineRewardButtonRedDotObject,
        GameStartCostGroupObject,
        SurvivalTimeObject,
        StageRewardProgressFillArea,
        StageRewardProgressSliderObject,
        FirstClearRewardUnlockObject,
        SecondClearRewardUnlockObject,
        ThirdClearRewardUnlockObject,
        FirstClearRedDotObject,
        SecondClearRedDotObject,
        ThirdClearRedDotObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }

    enum Buttons
    {
        SettingButton,
        MissionButton,
        AchievementButton,
        AttendanceCheckButton,
        StageSelectButton,
        OfflineRewardButton,
        GameStartButton,
        
        FirstClearRewardButton,
        SecondClearRewardButton,
        ThirdClearRewardButton,
    }

    enum Texts
    {
        StageNameText,
        SurvivalWaveText,
        SurvivalWaveValueText,
        GameStartButtonText,
        GameStartCostValueText,
        OfflineRewardText,
        
        SettingButtonText,
        MissionButtonText,
        AchievementButtonText,
        AttendanceCheckButtonText,
        
        FirstClearRewardText,
        SecondClearRewardText,
        ThirdClearRewardText,
    }

    enum Images
    {
        StageImage,
        
        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage,
    }

    enum RewardBoxState
    {
        Lock,
        Unlock,
        Complete,
        RedDot,
    }
    #endregion

    private Data.StageData _currentStageData;

    class RewardBox
    {
        public GameObject ItemImage;
        public GameObject UnlockObject;
        public GameObject CompleteObject;
        public GameObject RedDotObject;
        public RewardBoxState State;
    }

    private List<RewardBox> _boxes = new List<RewardBox>();

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        StartCoroutine(CoCheckContinue());
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        // 레드닷 버튼
        GetObject((int)GameObjects.SettingButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.MissionButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.AchievementButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.OfflineRewardButtonRedDotObject).SetActive(false);
        
        // // 버튼 기능
        GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton((int)Buttons.GameStartButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.OfflineRewardButton).gameObject.BindEvent(OnClickOfflineRewardButton);
        GetButton((int)Buttons.OfflineRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MissionButton).gameObject.BindEvent(OnClickMissionButton);
        GetButton((int)Buttons.MissionButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AchievementButton).gameObject.BindEvent(OnClickAchievementButton);
        GetButton((int)Buttons.AchievementButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AttendanceCheckButton).gameObject.BindEvent(OnClickAttendanceCheckButton);
        GetButton((int)Buttons.AttendanceCheckButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        // 생존 웨이브
        GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
        GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(false);
        
        // // 스테이지 보상
        GetButton((int)Buttons.FirstClearRewardButton).gameObject.BindEvent(OnClickFirstClearRewardButton);
        GetButton((int)Buttons.FirstClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondClearRewardButton).gameObject.BindEvent(OnClickSecondClearRewardButton);
        GetButton((int)Buttons.SecondClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ThirdClearRewardButton).gameObject.BindEvent(OnClickThirdClearRewardButton);
        GetButton((int)Buttons.ThirdClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();

        InitBoxes();
        Refresh();
        return true;
    }

    void Refresh()
    {
        if (Managers.Game.CurrentStageData == null)
        {
            Managers.Game.CurrentStageData = Managers.Data.StageDic[1];
        }

        // StageNameText : 마지막 도전한 스테이지 표시
        GetText((int)Texts.StageNameText).text = Managers.Game.CurrentStageData.StageName;

        // SurvivalWaveValueText : 해당 스테이지에서 도달했던 맥수 웨비으 수
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out StageClearInfo info))
        {
            if (info.MaxWaveIndex == 0)
            {
                GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
            }
            else
            {
                GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
            }
        }
        else
        {
            GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
        }

        // StageImage : 마지막 도전한 스테이지의 이미지
        GetImage((int)Images.StageImage).sprite = Managers.Resource.Load<Sprite>(Managers.Game.CurrentStageData.StageImage);

        // 스테이지 보상
        if (info != null)
        {
            _currentStageData = Managers.Game.CurrentStageData;
            int itemCode = _currentStageData.FirstWaveClearRewardItemId;

            // 박스
            InitBoxes();
            SetRewardBoxes(info);
            GetText((int)Texts.FirstClearRewardText).text = $"{_currentStageData.FirstWaveCountValue}";
            GetText((int)Texts.SecondClearRewardText).text = $"{_currentStageData.SecondWaveCountValue}";
            GetText((int)Texts.ThirdClearRewardText).text = $"{_currentStageData.ThirdWaveCountValue}";

            #region 생존 웨이브

            // 슬라이더
            int wave = info.MaxWaveIndex;

            if (info.isClear == true)
            {
                GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.SurvivalWaveValueText).color = Util.HexToColor("60FF08");
                GetText((int)Texts.SurvivalWaveValueText).text = "스테이지 클리어";
                GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
            }
            else
            {
                // 처음 접속
                if (info.MaxWaveIndex == 0)
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Util.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                }
                else // 진행중
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Util.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
                }
            }

            #endregion
        }
        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameStartCostGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.SurvivalTimeObject).GetComponent<RectTransform>());
    }

    IEnumerator CoCheckContinue()
    {
        yield return new WaitForEndOfFrame();

        if (PlayerPrefs.GetInt("ISFIRST") == 1)
        {
            Managers.UI.ShowPopupUI<UI_BeginnerSupportRewardPopup>();
            PlayerPrefs.SetInt("ISFIRST", 0);
        }

        if (Managers.Game.ContinueInfo.isContinue == true)
        {
            Managers.UI.ShowPopupUI<UI_BackToBattlePopup>();
        }
        else
        {
            Managers.Game.ClearContinueData();
        }
    }

    void InitBoxes()
    {
        #region Init

        RewardBox firstBox = new RewardBox
        {
            ItemImage = GetImage((int)Images.FirstClearRewardItemImage).gameObject,
            UnlockObject = GetObject((int)GameObjects.FirstClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.FirstClearRedDotObject).gameObject
        };
        _boxes.Add(firstBox);
        
        RewardBox secondBox = new RewardBox
        {
            ItemImage = GetImage((int)Images.SecondClearRewardItemImage).gameObject,
            UnlockObject = GetObject((int)GameObjects.SecondClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.SecondClearRedDotObject).gameObject
        };
        _boxes.Add(secondBox);
        
        RewardBox thirdBox = new RewardBox
        {
            ItemImage = GetImage((int)Images.ThirdClearRewardItemImage).gameObject,
            UnlockObject = GetObject((int)GameObjects.ThirdClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.ThirdClearRedDotObject).gameObject
        };
        _boxes.Add(thirdBox);
        #endregion

        for (int i = 0; i < _boxes.Count; i++)
        {
            _boxes[i].UnlockObject.SetActive(true);
            _boxes[i].CompleteObject.SetActive(false);
            _boxes[i].RedDotObject.SetActive(false);
        }
    }

    void SetRewardBoxes(StageClearInfo info)
    {
        int wave = info.MaxWaveIndex + 1;

        if (wave < 3)
        {
            InitBoxes();
        }
        else if (wave < 6)
        {
            // 1 상자 세팅
            if (info.isOpenFirstBox == true)
            {
                SetBoxState(0, RewardBoxState.Complete);
            }
            else
            {
                SetBoxState(0, RewardBoxState.RedDot);
            }
        }
        else if (wave < 10)
        {
            // 1,2 상자 세팅
            if (info.isOpenFirstBox == true)
            {
                SetBoxState(0, RewardBoxState.Complete);
            }
            else
            {
                SetBoxState(0, RewardBoxState.RedDot);
            }
            if (info.isOpenSecondBox == true)
            {
                SetBoxState(1, RewardBoxState.Complete);
            }
            else
            {
                SetBoxState(1, RewardBoxState.RedDot);
            }
        }
        else
        {
            // 1,2,3 상자 세팅
            if (info.isOpenFirstBox == true)
            {
                SetBoxState(0, RewardBoxState.Complete);
            }
            else
            {
                SetBoxState(0, RewardBoxState.RedDot);
            }
            if (info.isOpenSecondBox == true)
            {
                SetBoxState(1, RewardBoxState.Complete);
            }
            else
            {
                SetBoxState(1, RewardBoxState.RedDot);
            }
            if (info.isOpenThirdBox == true)
            {
                SetBoxState(2, RewardBoxState.Complete);
            }
            else
            {
                SetBoxState(2, RewardBoxState.RedDot);
            }
        }
    }

    void SetBoxState(int index, RewardBoxState state)
    {
        _boxes[index].UnlockObject.SetActive(false);
        _boxes[index].RedDotObject.SetActive(false);
        _boxes[index].CompleteObject.SetActive(false);
        _boxes[index].State = state;

        switch (state)
        {
            case RewardBoxState.Lock:
                _boxes[index].UnlockObject.SetActive(true);
                break;
            case RewardBoxState.Unlock:
                _boxes[index].UnlockObject.SetActive(false);
                break;
            case RewardBoxState.Complete:
                _boxes[index].CompleteObject.SetActive(true);
                break;
            case RewardBoxState.RedDot:
                _boxes[index].RedDotObject.SetActive(true);
                break;
        }
    }

    void OnClickGameStartButton()
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = false;
        if (Managers.Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Managers.UI.ShowPopupUI<UI_StaminaChargePopup>();
            return;
        }

        Managers.Game.Stamina -= Define.GAME_PER_STAMINA;
        if (Managers.Game.DicMission.TryGetValue(Define.MissionTarget.StageEnter, out MissionInfo mission))
        {
            mission.Progress++;
        }
        Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
    }

    void OnClickStageSelectButton()
    {
        Managers.Sound.PlayButtonClick();

        UI_StageSelectPopup stageSelectPopupUI = Managers.UI.ShowPopupUI<UI_StageSelectPopup>();
        
        stageSelectPopupUI.OnPopupClosed = () =>
        {
            Refresh();
        };
        // TODO : 스테이지 저장 관련해서 처리한 후 최신 스테이지 불러오게 해야 함
        stageSelectPopupUI.SetInfo(Managers.Game.CurrentStageData);
    }

    void OnClickOfflineRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        // Managers.UI.ShowPopupUI<UI_OfflineRewardPopup>();
    }
    
    void OnClickSettingButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }
    
    void OnClickMissionButton()
    {
        Managers.Sound.PlayButtonClick();
        // // Managers.Ads.RequestAndLoadRewardedAd();
        // Managers.UI.ShowPopupUI<UI_MissionPopup>();
    }
    
    void OnClickAchievementButton()
    {
        Managers.Sound.PlayButtonClick();
        // Managers.UI.ShowPopupUI<UI_AchievementPopup>();
    }
    
    void OnClickAttendanceCheckButton()
    {
        Managers.Sound.PlayButtonClick();
        // UI_CheckOutPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutPopup>();
        // popup.SetInfo(Managers.Time.AttendanceDay);
    }

    void OnClickFirstClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_boxes[0].State != RewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenFirstBox = true;
            SetBoxState(0, RewardBoxState.Complete);

            string[] spriteNames = new string[1];
            int[] counts = new int[1];
            int itemId = _currentStageData.FirstWaveClearRewardItemId;

            if (Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData))
            {
                spriteNames[0] = materialData.SpriteName;
                counts[0] = _currentStageData.FirstWaveClearRewardItemValue;
                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                
                Managers.Game.ExchangeMaterial(materialData, counts[0]);
                rewardPopup.SetInfo(spriteNames, counts);
            }
        }
    }

    void OnClickSecondClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_boxes[1].State != RewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenSecondBox = true;
            SetBoxState(1, RewardBoxState.Complete);

            string[] spriteNames = new string[1];
            int[] counts = new int[1];
            int itemId = _currentStageData.SecondWaveClearRewardItemId;

            if (Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData))
            {
                spriteNames[0] = materialData.SpriteName;
                counts[0] = _currentStageData.SecondWaveClearRewardItemValue;
                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                
                Managers.Game.ExchangeMaterial(materialData, counts[0]);
                rewardPopup.SetInfo(spriteNames, counts);
            }
        }
    }
    
    void OnClickThirdClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_boxes[2].State != RewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenThirdBox = true;
            SetBoxState(2, RewardBoxState.Complete);

            string[] spriteNames = new string[1];
            int[] counts = new int[1];
            int itemId = _currentStageData.ThirdWaveClearRewardItemId;

            if (Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData))
            {
                spriteNames[0] = materialData.SpriteName;
                counts[0] = _currentStageData.ThirdWaveClearRewardItemValue;
                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                
                Managers.Game.ExchangeMaterial(materialData, counts[0]);
                rewardPopup.SetInfo(spriteNames, counts);
            }
        }
    }
    
}
