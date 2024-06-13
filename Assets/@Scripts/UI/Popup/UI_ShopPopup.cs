using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_ShopPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        ShopScrollContent,
        
        StagePackageContentObject,
        
        PickupGuaranteedCountSliderObject,
        AdvancedGuaranteedCountSliderObject,
        
        FreeDiaSoldOutObject,
        FreeDiaRedDotObject,
        
        FreeGoldSoldOutObject,
        FreeGoldRedDotObject,
        
        FirstDiaProductBonusObject,
        SecondDiaProductBonusObject,
        ThirdDiaProductBonusObject,
        FourthDiaProductBonusObject,
        FifthDiaProductBonusObject,
        
        OpenTenCostObject,
        OpenOneCostObject,
        CommonGachaCostObject,
        AdvancedGachaCostObject,
        StagePackageCostObject,
        
        AdKeySoldOutObject,
        AdKeyRedDotObject,
        
        DailyShopTitle,
        DailyShopGroup,
        PickupGachaGroup,
    }

    enum Buttons
    {
        StagePackageButton,
        StagePackagePrevButton,
        StagePackageNextButton,
        BeginnerPackageButton,
        
        PickupGachaInfoButton,
        PickupGachaListButton,
        OpenOneButton,
        OpenTenButton,
        CommonGachaOpenButton,
        AdCommonGachaOpenButton,
        AdAdvancedGachaOpenButton,
        CommonGachaListButton,
        AdvancedGachaOpenButton,
        AdvancedGachaOpenTenButton,
        AdvancedGachaListButton,
        FreeDiaButton,
        FirstDiaProductButton,
        SecondDiaProductButton,
        ThirdDiaProductButton,
        FourthDiaProductButton,
        FifthDiaProductButton,
        
        AdKeyButton,
        SilverKeyProductButton,
        GoldKeyProductButton,
        
        FreeGoldButton,
        FirstGoldProductButton,
        SecondGoldProductButton,
    }
    enum Texts
    {
        PackageTitleText,
        DailyShopTitleText,
        RefreshTimerText,
        RefreshTimerValueText,
        GachaTitleText,
        DiaShopTitleText,
        DiaShopCommentText,
        KeyShopTitleText,
        GoldShopTitleText,
        
        StagePackageTitleText,
        StagePackageItemTitleText,
        PackageFirstProductItemCountValueText,
        PackageSecondProductItemCountValueText,
        PackageThirdProductItemCountValueText,
        PackageFourthProductItemCountValueText,
        StagePackageCostValueText,
        
        BeginnerPackageTitleText,
        BeginnerPackageBuyLimitText,
        BeginnerPackageDiscountText,
        
        PickupGachaTitleText,
        PickupGachaCommentText,
        PickupGachaGuaranteedCountText,
        PickupGachaGuaranteedCountValueText,
        
        CommonGachaTitleText,
        CommonGachaOpenButtonText,
        CommonGachaCostValueText,
        
        AdvancedGachaTitleText,
        AdvancedGachaGuaranteedCountText,
        AdvancedGachaGuaranteedCountValueText,
        AdvancedGachaOpenButtonText,
        AdvancedGachaCostValueText,
        AdvancedGachaTenCostValueText,
        BuyLimitText,
        BuyLimitValueText,
        OpenOneButtonText,
        OpenTenButtonText,
        
        FreeDiaRequestTimeValueText,
        FirstDiaCostValueText,
        SecondDiaCostValueText,
        ThirdDiaCostValueText,
        FourthDiaCostValueText,
        FifthDiaCostValueText,
        
        FreeGoldRequestTimeValueText,
        AdKeyRequestTimeValueText,
        
        FirstGoldProductTitleText,
        FreeGoldTitleText,
        SecondGoldProductTitleText,
    }

    enum Images
    {
        BeginnerPackageFirstItemImage,
    }
    #endregion

    private ScrollRect _scrollRect;
    private UI_GachaListPopup _gachaListPopupUI;
    
    public UI_GachaListPopup GachaListPopupUI
    {
        get
        {
            if (_gachaListPopupUI == null)
            {
                _gachaListPopupUI = Managers.UI.ShowPopupUI<UI_GachaListPopup>();
            }
    
            return _gachaListPopupUI;
        }
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        Refresh();
    }
    
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("ShopPopUp F1");
            Managers.Game.Dia += 300;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("ShopPopUp F2");
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD_KEY], 10);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BRONZE_KEY], 10);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], 10);
            Managers.Game.AddMaterialItem(Define.ID_WEAPON_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_GLOVES_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_RING_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_BELT_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_ARMOR_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_BOOTS_SCROLL, 10);

            Refresh();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("ShopPopUp F3");
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_STAMINA], 10);
            Refresh();
        }
    }
#endif

    public override bool Init()
    {
        if(base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);
        
        // 패키지 상점
        // // GetButton((int)Buttons.StagePackageButton).gameObject.BindEvent(OnClickStagePackageButton);
        // // GetButton((int)Buttons.StagePackageButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.StagePackagePrevButton).gameObject.BindEvent(OnClickStagePackagePrevButton);
        // // GetButton((int)Buttons.StagePackagePrevButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.StagePackageNextButton).gameObject.BindEvent(OnClickStagePackageNextButton);
        // // GetButton((int)Buttons.StagePackageNextButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.BeginnerPackageButton).gameObject.BindEvent(OnClickBeginnerPackageButton);
        // // GetButton((int)Buttons.BeginnerPackageButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        // 가챠 상점
        // // GetButton((int)Buttons.PickupGachaInfoButton).gameObject.BindEvent(OnClickPickupGachaInfoButton);
        // // GetButton((int)Buttons.PickupGachaInfoButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.PickupGachaListsButton).gameObject.BindEvent(OnClickPickupGachaListsButton);
        // // GetButton((int)Buttons.PickupGachaListsButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.OpenOneButton).gameObject.BindEvent(OnClickOpenOneButton);
        // // GetButton((int)Buttons.OpenOneButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.OpenTenButton).gameObject.BindEvent(OnClickOpenTenButton);
        // // GetButton((int)Buttons.OpenTenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CommonGachaOpenButton).gameObject.BindEvent(OnClickCommonGachaOpenButton);
        GetButton((int)Buttons.CommonGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdCommonGachaOpenButton).gameObject.BindEvent(OnClickAdCommonGachaOpenButton);
        GetButton((int)Buttons.AdCommonGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdAdvancedGachaOpenButton).gameObject.BindEvent(OnClickAdAdvancedGachaOpenButton);
        GetButton((int)Buttons.AdAdvancedGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        GetButton((int)Buttons.CommonGachaListButton).gameObject.BindEvent(OnClickCommonGachaListButton);
        GetButton((int)Buttons.CommonGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaOpenButton).gameObject.BindEvent(OnClickAdvancedGachaOpenButton);
        GetButton((int)Buttons.AdvancedGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaOpenTenButton).gameObject.BindEvent(OnClickAdvancedGachaOpenTenButton);
        GetButton((int)Buttons.AdvancedGachaOpenTenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaListButton).gameObject.BindEvent(OnClickAdvancedGachaListButton);
        GetButton((int)Buttons.AdvancedGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        // 출시모델 제외
        // // GetObject((int)GameObjects.DailyShopTitle).gameObject.SetActive(false);
        // // GetObject((int)GameObjects.DailyShopGroup).gameObject.SetActive(false);
        // // GetObject((int)GameObjects.PickupGachaGroup).gameObject.SetActive(false);
        // // GetObject((int)GameObjects.AdvancedGuaranteedCountSliderObject).gameObject.SetActive(false);
        
        // 다이아 상점
        // // GetObject((int)GameObjects.FreeDiaSoldOutObject).gameObject.SetActive(false);
        // // GetObject((int)GameObjects.FreeDiaRedDotObject).gameObject.SetActive(false);
        // // GetButton((int)Buttons.FreeDiaButton).gameObject.BindEvent(OnClickFreeDiaButton);
        // // GetButton((int)Buttons.FreeDiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.FirstDiaProductButton).gameObject.BindEvent(OnClickFirstDiaProductButton);
        // // GetButton((int)Buttons.FirstDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.SecondDiaProductButton).gameObject.BindEvent(OnClickSecondDiaProductButton);
        // // GetButton((int)Buttons.SecondDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.ThirdDiaProductButton).gameObject.BindEvent(OnClickThirdDiaProductButton);
        // // GetButton((int)Buttons.ThirdDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.FourthDiaProductButton).gameObject.BindEvent(OnClickFourthDiaProductButton);
        // // GetButton((int)Buttons.FourthDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        // // GetButton((int)Buttons.FifthDiaProductButton).gameObject.BindEvent(OnClickFifthDiaProductButton);
        // // GetButton((int)Buttons.FifthDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        // 첫구매 보너스
        // // GetObject((int)GameObjects.FirstDiaProductBonusObject).gameObject.SetActive(false);   // 첫구매 이력이 있다면 비활성화
        // // GetObject((int)GameObjects.SecondDiaProductBonusObject).gameObject.SetActive(false);  // 첫구매 이력이 있다면 비활성화
        // // GetObject((int)GameObjects.ThirdDiaProductBonusObject).gameObject.SetActive(false);   // 첫구매 이력이 있다면 비활성화
        // // GetObject((int)GameObjects.FourthDiaProductBonusObject).gameObject.SetActive(false);  // 첫구매 이력이 있다면 비활성화
        // // GetObject((int)GameObjects.FifthDiaProductBonusObject).gameObject.SetActive(false);   // 첫구매 이력이 있다면 비활성화
        
        // 열쇠 상점
        GetObject((int)GameObjects.AdKeySoldOutObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.AdKeyRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.AdKeyButton).gameObject.BindEvent(OnClickAdKeyButton);
        GetButton((int)Buttons.AdKeyButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SilverKeyProductButton).gameObject.BindEvent(OnClickSilverKeyProductButton);
        GetButton((int)Buttons.SilverKeyProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.GoldKeyProductButton).gameObject.BindEvent(OnClickGoldKeyProductButton);
        GetButton((int)Buttons.GoldKeyProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        // 골드 상점
        GetObject((int)GameObjects.FreeGoldSoldOutObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.FreeGoldRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.FreeGoldButton).gameObject.BindEvent(OnClickFreeGoldButton);
        GetButton((int)Buttons.FreeGoldButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.FirstGoldProductButton).gameObject.BindEvent(OnClickFirstGoldProductButton);
        GetButton((int)Buttons.FirstGoldProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondGoldProductButton).gameObject.BindEvent(OnClickSecondGoldProductButton);
        GetButton((int)Buttons.SecondGoldProductButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }

    public void SetInfo(ScrollRect scrollRect)
    {
        _scrollRect = scrollRect;
        Refresh();
    }

    void Refresh()
    {
        Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int goldKeyCount);
        Managers.Game.ItemDictionary.TryGetValue(Define.ID_SILVER_KEY, out int silverKeyCount);
        Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int bronzeKeyCount);

        GetText((int)Texts.CommonGachaOpenButtonText).text = $"{silverKeyCount}/1";
        GetText((int)Texts.AdvancedGachaCostValueText).text = $"{goldKeyCount}/1";
        GetText((int)Texts.AdvancedGachaTenCostValueText).text = $"{goldKeyCount}/10";
        GetButton((int)Buttons.AdAdvancedGachaOpenButton).gameObject.SetActive(Managers.Game.GachaCountAdsAdvanced > 0);
        GetButton((int)Buttons.AdCommonGachaOpenButton).gameObject.SetActive(Managers.Game.GachaCountAdsCommon > 0);
        GetObject((int)GameObjects.FreeGoldSoldOutObject).SetActive(Managers.Game.GoldCountAds == 0); // Soldout 표시
        GetObject((int)GameObjects.AdKeySoldOutObject).SetActive(Managers.Game.BronzeKeyCountAds == 0);

        int goldAmount = 0;
        if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
        {
            goldAmount = offlineReward.RewardGold;
        }

        GetText((int)Texts.FreeGoldTitleText).text = $"{goldAmount}";
        GetText((int)Texts.FirstGoldProductTitleText).text = $"{goldAmount * 3}";
        GetText((int)Texts.SecondGoldProductTitleText).text = $"{goldAmount * 5}";
        
        // 리프레시 버그 대응
        // // LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.StagePackageCostObject).GetComponent<RectTransform>());
        // // LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.OpenTenCostObject).GetComponent<RectTransform>());
        // // LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.OpenOneCostObject).GetComponent<RectTransform>());
        // // LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.OpenTenButton).gameObject.GetComponent<RectTransform>());
        // // LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.OpenOneButton).gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CommonGachaCostObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AdvancedGachaCostObject).GetComponent<RectTransform>());
    }

    #region Buttons

    #region 패키지 버튼(보류)
    // // void OnClickStagePackageButton()
    // // {
    // //     // 상품 구매 모듈 호출
    // // }
    // // void OnClickStagePackagePrevButton()
    // // {
    // //     // 패키지의 구성을 이전 패키지 구성으로 변경
    // // }
    // // void OnClickStagePackageNextButton()
    // // {
    // //     // 패키지의 구성을 다음 패키지 구성으로 변경
    // // }
    // // void OnClickBeginnerPackageButton()
    // // {
    // //     // 상품 구매 모듈 호출
    // // }
    #endregion

    #region 가챠 상점 버튼(보류)
    // // void OnClickGachaListButton()
    // // {
    // //     GachaListPopupUI.gameObject.SetActive(true);
    // //     GachaListPopupUI.SetInfo(Define.GachaType.PickupGacha);
    // // }
    // // void OnClickPickupGachaInfoButton()
    // // {
    // //     Managers.UI.ShowPopupUI<UI_PickupGachaInfoPopup>();
    // // }
    // // void OnClickOpenOneButton()
    // // {
    // //     Managers.Game.Dia -= 300;
    // //     DoGacha(Define.GachaType.PickupGacha, 1);
    // // }
    // // void OnClickOpenTenButton()
    // // {
    // //     Managers.Game.Dia -= 2750;
    // //     DoGacha(Define.GachaType.PickupGacha, 10);
    // // }
    void OnClickCommonGachaOpenButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_SILVER_KEY, out int keyCount))
        {
            if (keyCount > 0)
            {
                Managers.Game.RemoveMaterialItem(Define.ID_SILVER_KEY, 1);
                DoGacha(Define.GachaType.CommonGacha, 1);
                Refresh();
            }
            else
            {
                Managers.UI.ShowToast("열쇠가 부족합니다.");
            }
        }
    }
    void OnClickAdCommonGachaOpenButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.GachaCountAdsCommon > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Game.GachaCountAdsCommon--;
                DoGacha(Define.GachaType.CommonGacha, 1);
                Refresh();
            });
        }
    }
    void OnClickAdAdvancedGachaOpenButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.GachaCountAdsAdvanced > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Game.GachaCountAdsAdvanced--;
                DoGacha(Define.GachaType.AdvancedGacha, 1);
                Refresh();
            });
        }
    }
    void OnClickCommonGachaListButton()
    {
        Managers.Sound.PlayButtonClick();
        // 일반 가챠의 리스트를 가지고 옴
        GachaListPopupUI.SetInfo(Define.GachaType.CommonGacha);
    }
    void OnClickAdvancedGachaOpenButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int keyCount))
        {
            if (keyCount > 0)
            {
                Managers.Game.RemoveMaterialItem(Define.ID_GOLD_KEY, 1);
                DoGacha(Define.GachaType.AdvancedGacha, 1);
                Refresh();
            }
            else
            {
                Managers.UI.ShowToast("열쇠가 부족합니다.");
            }
        }
    }
    void OnClickAdvancedGachaOpenTenButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int keyCount))
        {
            if (keyCount > 10)
            {
                Managers.Game.RemoveMaterialItem(Define.ID_GOLD_KEY, 10);
                DoGacha(Define.GachaType.AdvancedGacha, 10);
                Refresh();
            }
            else
            {
                Managers.UI.ShowToast("열쇠가 부족합니다.");
            }
        }
    }
    void OnClickAdvancedGachaListButton()
    {
        Managers.Sound.PlayButtonClick();
        GachaListPopupUI.SetInfo(Define.GachaType.AdvancedGacha);
    }
    void DoGacha(Define.GachaType gachaType, int count = 1)
    {
        List<Equipment> res = new List<Equipment>();
        res = Managers.Game.DoGacha(gachaType, count).ToList();
        if (Managers.Game.DicMission.TryGetValue(Define.MissionTarget.GachaOpen, out MissionInfo mission))
        {
            mission.Progress++;
        }

        Managers.UI.ShowPopupUI<UI_GachaResultsPopup>().SetInfo(res);
    }
    #endregion

    #region 다이아 상점 버튼(보류)
    // // void OnClickFreeDiaButton()
    // // {
    // //     // 보상을 얻을 수 있을 때 터치 시 무료 다아이 지급
    // // }
    // // void OnClickFirstDiaProductButton()
    // // {
    // //     // 구매 모듈 호출
    // // }
    // // void OnClickSecondDiaProductButton()
    // // {
    // //     // 구매 모듈 호출
    // // }
    // // void OnClickThirdDiaProductButton()
    // // {
    // //     // 구매 모듈 호출
    // // }
    // // void OnClickFourthDiaProductButton()
    // // {
    // //     // 구매 모듈 호출
    // // }
    // // void OnClickFifthDiaProductButton()
    // // {
    // //     // 구매 모듈 호출
    // // }
    #endregion

    #region 열쇠 상점 버튼
    void OnClickAdKeyButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.BronzeKeyCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                string[] spriteNames = new string[1];
                int[] counts = new int[1];

                spriteNames[0] = Managers.Data.MaterialDic[Define.ID_BRONZE_KEY].SpriteName;
                counts[0] = 1;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.BronzeKeyCountAds--;
                Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BRONZE_KEY], 1);
                Refresh();
                rewardPopup.SetInfo(spriteNames, counts);
            });
        }
    }
    void OnClickSilverKeyProductButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia > 150)
        {
            string[] spriteNames = new string[1];
            int[] counts = new int[1];

            spriteNames[0] = Managers.Data.MaterialDic[Define.ID_SILVER_KEY].SpriteName;
            counts[0] = 1;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 150;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], 1);
            Refresh();
            rewardPopup.SetInfo(spriteNames, counts);
        }
        else
        {
            Managers.UI.ShowToast("다이아가 부족합니다.");
        }
    }
    void OnClickGoldKeyProductButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia > 300)
        {
            string[] spriteNames = new string[1];
            int[] counts = new int[1];

            spriteNames[0] = Managers.Data.MaterialDic[Define.ID_GOLD_KEY].SpriteName;
            counts[0] = 1;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 300;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD_KEY], 1);
            Refresh();
            rewardPopup.SetInfo(spriteNames, counts);
        }
        else
        {
            Managers.UI.ShowToast("다이아가 부족합니다.");
        }
    }
    #endregion

    #region 골드 상점 버튼

    void OnClickFreeGoldButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.GoldCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                int goldAmount = 0;
                if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
                {
                    goldAmount = offlineReward.RewardGold;
                }

                string[] spriteNames = new string[1];
                int[] counts = new int[1];

                spriteNames[0] = Define.GOLD_SPRITE_NAME;
                counts[0] = goldAmount;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);

                Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], goldAmount);
                Managers.Game.GoldCountAds--;
                Refresh();
                rewardPopup.SetInfo(spriteNames, counts);
            });
        }
    }

    void OnClickFirstGoldProductButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia >= 300)
        {
            int goldAmount = 0;
            if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
            {
                goldAmount = offlineReward.RewardGold * 3;
            }

            string[] spriteNames = new string[1];
            int[] counts = new int[1];

            spriteNames[0] = Define.GOLD_SPRITE_NAME;
            counts[0] = goldAmount;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 300;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], goldAmount);
            rewardPopup.SetInfo(spriteNames, counts);
        }
        else
        {
            Managers.UI.ShowToast("다이아가 부족합니다.");
        }
    }

    void OnClickSecondGoldProductButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia >= 500)
        {
            int goldAmount = 0;
            if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
            {
                goldAmount = offlineReward.RewardGold * 5;
            }

            string[] spriteNames = new string[1];
            int[] counts = new int[1];

            spriteNames[0] = Define.GOLD_SPRITE_NAME;
            counts[0] = goldAmount;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 500;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], goldAmount);
            rewardPopup.SetInfo(spriteNames, counts);
        }
        else
        {
            Managers.UI.ShowToast("다이아가 부족합니다.");
        }
    }
    #endregion
    #endregion

    #region 버튼 스크롤

    public void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }
    public void OnBeginDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }
    public void OnEndDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
