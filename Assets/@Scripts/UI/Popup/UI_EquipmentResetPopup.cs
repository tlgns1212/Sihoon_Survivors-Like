using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class UI_EquipmentResetPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        ToggleGroupObject,
        TargetEquipment,
        
        ResetInfoGroupObject,
        DowngradeGroupObject,
    }
    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipmentDowngradeButton
    }

    enum Texts
    {
        BackgroundText,
        EquipmentResetPopupTitleText,
        ResetInfoCommentText,
        DowngradeCommentText,
        EquipmentResetButtonText,
        ResultGoldCountValueText,
        ResultMaterialCountValueText,
        TargetEquipmentLevelValueText,
        TargetEnforceValueText,
        ResultEnforceValueText,
        
        ResetTapToggleText,
        DowngradeTapToggleText,
        
        DowngradeTargetEquipmentLevelValueText,
        DowngradeTargetEnforceValueText,
        DowngradeEnchantStoneCountValueText,
        DowngradeResultGoldCountValueText,
        DowngradeResultMaterialCountValueText,
        EquipmentDowngradeButtonText,
    }

    enum Images
    {
        TargetEquipmentGradeBackgroundImage,
        TargetEquipmentImage,
        TargetEquipmentEnforceBackgroundImage,
        ResultEquipmentGradeBackgroundImage,
        ResultEquipmentImage,
        ResultEquipmentEnforceBackgroundImage,
        ResultMaterialImage,
        
        DowngradeTargetEquipmentGradeBackgroundImage,
        DowngradeTargetEquipmentImage,
        DowngradeTargetEquipmentEnforceBackgroundImage,
        DowngradeEquipmentGradeBackgroundImage,
        DowngradeEquipmentImage,
        DowngradeEnchantStoneBackgroundImage,
        DowngradeEnchantStoneImage,
        DowngradeResultMaterialImage,
    }

    enum Toggles
    {
        ResetTapToggle,
        DowngradeTapToggle,
    }
    #endregion

    private bool _isSelectedResetTapTap = false;
    private bool _isSelectedDowngradeTapTap = false;
    public Equipment Equipment;

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
        BindImage(typeof(Images));
        BindToggle(typeof(Toggles));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton);
        GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipmentDowngradeButton).gameObject.BindEvent(OnClickEquipmentDowngradeButton);
        GetButton((int)Buttons.EquipmentDowngradeButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        GetToggle((int)Toggles.ResetTapToggle).gameObject.BindEvent(OnClickResetTapToggle);
        GetToggle((int)Toggles.DowngradeTapToggle).gameObject.BindEvent(OnClickDowngradeTapToggle);

        EquipmentResetPopupContentInit();
        OnClickResetTapToggle();

        return true;
    }

    public void SetInfo(Equipment equipment)
    {
        Equipment = equipment;
        Refresh();
        OnClickResetTapToggle();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (Equipment == null)
        {
            GetObject((int)GameObjects.TargetEquipment).SetActive(false);
        }
        else
        {
            EquipmentResetRefresh();
        }
        
        // 일반 장비 토글 처리 (다운그레이드 불가)
        if (Equipment.EquipmentData.EquipmentGrade == Define.EquipmentGrade.Common)
        {
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        }
        else if (Equipment.EquipmentData.EquipmentGrade == Define.EquipmentGrade.Uncommon)
        {
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        }
        else if (Equipment.EquipmentData.EquipmentGrade == Define.EquipmentGrade.Rare)
        {
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        }
        else if (Equipment.EquipmentData.EquipmentGrade == Define.EquipmentGrade.Epic)
        {
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        }
        else if (Equipment.EquipmentData.EquipmentGrade == Define.EquipmentGrade.Legendary)
        {
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        }
        else
        {
            EquipmentDowngradeRefresh();
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(true);
        }
    }

    // 초기화
    void EquipmentResetRefresh()
    {
        GetImage((int)Images.TargetEquipmentImage).sprite = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetText((int)Texts.TargetEquipmentLevelValueText).text = $"Lv. {Equipment.Level}";
        
        GetImage((int)Images.ResultEquipmentImage).sprite = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetText((int)Texts.ResultGoldCountValueText).text = $"{CalculateResetGold()}";
        
        GetImage((int)Images.ResultMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[Equipment.EquipmentData.LevelupMaterialId].SpriteName);
        GetText((int)Texts.ResultMaterialCountValueText).text = $"{CalculateResetMaterialCount()}";

        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Common:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.EquipmentGrade.Uncommon:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.EquipmentGrade.Rare:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.EquipmentGrade.Epic:
            case Define.EquipmentGrade.Epic1:
            case Define.EquipmentGrade.Epic2:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case Define.EquipmentGrade.Legendary:
            case Define.EquipmentGrade.Legendary1:
            case Define.EquipmentGrade.Legendary2:
            case Define.EquipmentGrade.Legendary3:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case Define.EquipmentGrade.Myth:
            case Define.EquipmentGrade.Myth1:
            case Define.EquipmentGrade.Myth2:
            case Define.EquipmentGrade.Myth3:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Myth;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.MythBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Myth;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = EquipmentUIColors.MythBg;
                break;
            default:
                break;
        }
        
        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.TargetEnforceValueText).text = num.ToString();
            GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
            GetText((int)Texts.ResultEnforceValueText).text = num.ToString();
            GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
    }
    
    // 다운그레이드 리프레시
    void EquipmentDowngradeRefresh()
    {
        GetImage((int)Images.DowngradeTargetEquipmentImage).sprite = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetText((int)Texts.DowngradeTargetEquipmentLevelValueText).text = $"Lv. {Equipment.Level}";
        
        GetImage((int)Images.DowngradeEquipmentImage).sprite = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetText((int)Texts.DowngradeResultGoldCountValueText).text = $"{CalculateResetGold()}";
        
        GetImage((int)Images.DowngradeEnchantStoneImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.EquipDataDic[Equipment.EquipmentData.DowngradeMaterialCode].SpriteName);
        GetText((int)Texts.DowngradeEnchantStoneCountValueText).text = $"{Equipment.EquipmentData.DowngradeMaterialCode}";
        
        GetImage((int)Images.DowngradeResultMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[Equipment.EquipmentData.LevelupMaterialId].SpriteName);
        GetText((int)Texts.DowngradeResultMaterialCountValueText).text = $"{CalculateResetMaterialCount()}";

        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Epic1:
            case Define.EquipmentGrade.Epic2:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.DowngradeEnchantStoneBackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case Define.EquipmentGrade.Legendary1:
            case Define.EquipmentGrade.Legendary2:
            case Define.EquipmentGrade.Legendary3:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.DowngradeEnchantStoneBackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            case Define.EquipmentGrade.Myth1:
            case Define.EquipmentGrade.Myth2:
            case Define.EquipmentGrade.Myth3:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Myth;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.MythBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = EquipmentUIColors.Myth;
                GetImage((int)Images.DowngradeEnchantStoneBackgroundImage).color = EquipmentUIColors.Myth;
                break;
            default:
                break;
        }
        
        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.DowngradeTargetEnforceValueText).text = num.ToString();
            GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
    }

    void EquipmentResetPopupContentInit()
    {
        _isSelectedResetTapTap = false;
        _isSelectedDowngradeTapTap = false;
        
        GetObject((int)GameObjects.ResetInfoGroupObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.DowngradeGroupObject).gameObject.SetActive(false);
    }

    int CalculateResetGold()
    {
        int gold = 0;
        for (int i = 1; i < Equipment.Level; i++)
        {
            gold += Managers.Data.EquipLevelDataDic[i].UpgradeCost;
        }

        return gold;
    }

    int CalculateResetMaterialCount()
    {
        int materialCount = 0;
        for (int i = 1; i < Equipment.Level; i++)
        {
            materialCount += Managers.Data.EquipLevelDataDic[i].UpgradeRequiredItems;
        }

        return materialCount;
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        OnClickResetTapToggle();
        gameObject.SetActive(false);
    }

    void OnClickEquipmentResetButton()
    {
        int gold = CalculateResetGold();
        int materialCount = CalculateResetMaterialCount();
        Equipment.Level = 1;

        string[] spriteNames = new string[2];
        int[] counts = new int[2];

        spriteNames[0] = Define.GOLD_SPRITE_NAME;
        counts[0] = gold;

        int materialCode = Equipment.EquipmentData.LevelupMaterialId;
        spriteNames[1] = Managers.Data.MaterialDic[materialCode].SpriteName;
        counts[1] = materialCount;

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[materialCode], materialCount);
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);
        
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
        gameObject.SetActive(false);
        
        rewardPopup.SetInfo(spriteNames, counts);
    }

    void OnClickEquipmentDowngradeButton()
    {
        if (Equipment.EquipmentData.DowngradeEquipmentCode == null)
            return;

        if (Managers.Data.EquipDataDic.TryGetValue(Equipment.EquipmentData.DowngradeEquipmentCode, out Data.EquipmentData downgradedEquip) == false)
            return;

        int gold = 0, materialCount = 0;
        // 1. 레벨 초기화
        if (Equipment.Level > 1)
        {
            gold = CalculateResetGold();
            materialCount = CalculateResetMaterialCount();
        }
        // 선택된 장비의 아랫단계 장비를 add
        Managers.Game.AddEquipment(downgradedEquip.DataId);
        
        // DowngradeMaterialCode를 갯수만큼 인벤토리에 넣음
        for (int i = 0; i < Equipment.EquipmentData.DowngradeMaterialCount; i++)
        {
            Managers.Game.AddEquipment(Equipment.EquipmentData.DowngradeMaterialCode);
        }
        
        // 선택된 장비를 삭제
        Managers.Game.OwnedEquipments.Remove(Equipment);

        List<string> spriteNameList = new List<string>();
        List<int> count = new List<int>();
        // 골드, 메테리얼, 아랫단계 장비, 강화석
        if (gold > 0)
        {
            spriteNameList.Add(Define.GOLD_SPRITE_NAME);
            count.Add(gold);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);
        }
        if (materialCount > 0)
        {
            int materialCode = Equipment.EquipmentData.LevelupMaterialId;
            spriteNameList.Add(Managers.Data.MaterialDic[materialCode].SpriteName);
            count.Add(materialCount);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[materialCode], materialCount);
        }
        
        spriteNameList.Add(Managers.Data.EquipDataDic[Equipment.EquipmentData.DowngradeMaterialCode].SpriteName);
        count.Add(Equipment.EquipmentData.DowngradeMaterialCount);
        
        spriteNameList.Add(downgradedEquip.SpriteName);
        count.Add(1);

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        rewardPopup.SetInfo(spriteNameList.ToArray(), count.ToArray());
        
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);
        
        // 장비를 다운그레이드하고 리셋 결과의 장비와 아이템을 습득
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);
        // 버튼 누를 때 equipemtnpopup refresh();
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
        gameObject.SetActive(false);
    }

    void OnClickResetTapToggle()
    {
        if (_isSelectedResetTapTap == true)
            return;
        EquipmentResetPopupContentInit();
        _isSelectedResetTapTap = true;
        
        GetObject((int)GameObjects.ResetInfoGroupObject).gameObject.SetActive(true);
        GetToggle((int)Toggles.ResetTapToggle).isOn = true;
    }

    void OnClickDowngradeTapToggle()
    {
        if (_isSelectedDowngradeTapTap == true)
            return;
        EquipmentResetPopupContentInit();
        _isSelectedDowngradeTapTap = true;
        
        GetObject((int)GameObjects.DowngradeGroupObject).gameObject.SetActive(true);
        GetToggle((int)Toggles.DowngradeTapToggle).isOn = true;
    }
}
