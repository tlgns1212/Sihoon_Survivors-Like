using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentInfoPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        
        EquipmentGradeSkillScrollContentObject,
        ButtonGroupObject,
        CostGoldObject,
        CostMaterialObject,
        LevelupCostGroupObject,
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipButton,
        UnequipButton,
        LevelupButton,
        MergeButton,
    }

    enum Texts
    {
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,
        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        LegendarySkillOptionDescriptionValueText,
        CostGoldValueText,
        CostMaterialValueText,
        EquipButtonText,
        UnequipButtonText,
        MergeButtonText,
        EquipmentGradeSkillText,
        BackgroundText,
        EnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        CostMaterialImage,
        EquipmentImage,
        GradeBackgroundImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
        
        UncommonSkillLockImage,
        RareSkillLockImage,
        EpicSkillLockImage,
        LegendarySkillLockImage,
    }
    #endregion

    public Equipment Equipment;
    private Action _closeAction;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton);
        GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.UnequipButton).gameObject.BindEvent(OnClickUnequipButton);
        GetButton((int)Buttons.UnequipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.LevelupButton).gameObject.BindEvent(OnClickLevelupButton);
        GetButton((int)Buttons.LevelupButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        return true;
    }

    public void SetInfo(Equipment equipment)
    {
        Equipment = equipment;
        Refresh();
    }

    void Refresh()
    {
        #region 정보갱신
        GetButton((int)Buttons.EquipButton).gameObject.SetActive(true);
        GetButton((int)Buttons.UnequipButton).gameObject.SetActive(true);
        // 이미 장착하고 있는 장비라면 EquipButton을 비활성화
        // 장착하고 있지 않는 장비라면 UnequipButton을 비활성화
        if (Equipment.IsEquipped == true)
        {
            GetButton((int)Buttons.UnequipButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.EquipButton).gameObject.SetActive(false);
        }
        
        // 장비 레벨이 1이라면 리셋 버튼 비활성화
        if (Equipment.Level == 1)
        {
            GetButton((int)Buttons.EquipmentResetButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.EquipmentResetButton).gameObject.SetActive(true);
        }

        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage((int)Images.EquipmentImage).sprite = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);

        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Common:
                // GetText((int)Texts.EquipmentGradeValueText).text = Equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "일반";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.CommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.EquipmentGrade.Uncommon:
                GetText((int)Texts.EquipmentGradeValueText).text = "고급";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.UncommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.EquipmentGrade.Rare:
                GetText((int)Texts.EquipmentGradeValueText).text = "희귀";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.RareNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.EquipmentGrade.Epic:
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case Define.EquipmentGrade.Epic1:
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case Define.EquipmentGrade.Epic2:
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case Define.EquipmentGrade.Legendary:
                GetText((int)Texts.EquipmentGradeValueText).text = "전설";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case Define.EquipmentGrade.Legendary1:
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case Define.EquipmentGrade.Legendary2:
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case Define.EquipmentGrade.Legendary3:
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 3";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case Define.EquipmentGrade.Myth:
                break;
            case Define.EquipmentGrade.Myth1:
                break;
            case Define.EquipmentGrade.Myth2:
                break;
            case Define.EquipmentGrade.Myth3:
                break;
            default:
                break;
        }
        
        // EquipmentNameValueText : 대상 장비의 이름
        GetText((int)Texts.EquipmentNameValueText).text = Equipment.EquipmentData.NameTextId;
        // EquipmentLevelValueText : 장비의 레벨 (현재 레벨/최대 레벨)
        GetText((int)Texts.EquipmentLevelValueText).text = $"{Equipment.Level}/{Equipment.EquipmentData.MaxLevel}";
        // EquipmentOptionImage : 장비 옵션의 아이콘
        string spriteName = Equipment.MaxHpBonus == 0 ? "AttackPoint_Icon.sprite" : "HealthPoint_Icon.sprite";
        GetImage((int)Images.EquipmentOptionImage).sprite = Managers.Resource.Load<Sprite>(spriteName);
        // EquipmentOptionValueText : 장비 옵션 수치
        string bonusValue = Equipment.MaxHpBonus == 0 ? Equipment.AttackBonus.ToString() : Equipment.MaxHpBonus.ToString();
        GetText((int)Texts.EquipmentOptionValueText).text = $"+{bonusValue}";
        
        // CostGoldValueText : 레벨업 비용 (보유 / 필요) 만약 코스트가 부족하다면 보유량을 빨간색(#F3614D)으로 보여준다. 부족하지 않다면 흰색(#FFFFFF)
        if (Managers.Data.EquipLevelDataDic.ContainsKey(Equipment.Level))
        {
            EquipmentLevelData levelData = Managers.Data.EquipLevelDataDic[Equipment.Level];
            GetText((int)Texts.CostGoldValueText).text = $"{levelData.UpgradeCost}";
            if (Managers.Game.Gold < levelData.UpgradeCost)
            {
                GetText((int)Texts.CostGoldValueText).color = Util.HexToColor("F3614D");
            }

            GetText((int)Texts.CostMaterialValueText).text = $"{levelData.UpgradeRequiredItems}";
        }
        
        // 레벨업 재료 아이콘
        GetImage((int)Images.CostMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[Equipment.EquipmentData.LevelupMaterialId].SpriteName);
        #endregion

        #region 유일 +1 등의 등급 벨류

        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;
        
        // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetText((int)Texts.EnforceValueText).text = "";
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.EnforceValueText).text = num.ToString();
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
        #endregion

        #region 장비스킬 옵션 설정
        // 만약 장비 데이터 테이블의 각 등급별 옵션(스킬Id)에 해당하는 스킬이 없다면 등급에 맞는 옵션 오브젝트 비활성화
        GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.RareSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(false);

        if (Managers.Data.SupportSkillDic.ContainsKey(Equipment.EquipmentData.UncommonGradeSkill)) // 스킬타입에서 서포트스킬 타입 데이터로 교체 #Neo
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[Equipment.EquipmentData.UncommonGradeSkill];
            GetText((int)Texts.UncommonSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(Equipment.EquipmentData.RareGradeSkill)) 
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[Equipment.EquipmentData.RareGradeSkill];
            GetText((int)Texts.RareSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.RareSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(Equipment.EquipmentData.EpicGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[Equipment.EquipmentData.EpicGradeSkill];
            GetText((int)Texts.EpicSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(Equipment.EquipmentData.LegendaryGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[Equipment.EquipmentData.LegendaryGradeSkill];
            GetText((int)Texts.LegendarySkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(true);
        }
        #endregion

        #region 장비스킬 옵션 색상 변경

        Define.EquipmentGrade equipmentGrade = Equipment.EquipmentData.EquipmentGrade;
        
        // 공통색상 변경
        GetText((int)Texts.UncommonSkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        GetText((int)Texts.RareSkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        GetText((int)Texts.EpicSkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        
        GetImage((int)Images.UncommonSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.RareSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.EpicSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(true);
        
        // 등급별 색상 추가 및 변경
        if (equipmentGrade >= Define.EquipmentGrade.Uncommon)
        {
            GetText((int)Texts.UncommonSkillOptionDescriptionValueText).color = EquipmentUIColors.Uncommon;
            GetImage((int)Images.UncommonSkillLockImage).gameObject.SetActive(false);
        }
        if (equipmentGrade >= Define.EquipmentGrade.Rare)
        {
            GetText((int)Texts.RareSkillOptionDescriptionValueText).color = EquipmentUIColors.Rare;
            GetImage((int)Images.RareSkillLockImage).gameObject.SetActive(false);
        }
        if (equipmentGrade >= Define.EquipmentGrade.Epic)
        {
            GetText((int)Texts.EpicSkillOptionDescriptionValueText).color = EquipmentUIColors.Epic;
            GetImage((int)Images.EpicSkillLockImage).gameObject.SetActive(false);
        }
        if (equipmentGrade >= Define.EquipmentGrade.Legendary)
        {
            GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = EquipmentUIColors.Legendary;
            GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(false);
        }
        #endregion

        #region 리프레시 버그 대응

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ButtonGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CostGoldObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CostMaterialObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.LevelupCostGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetText((int)Texts.CostGoldValueText).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetText((int)Texts.CostMaterialValueText).GetComponent<RectTransform>());

        #endregion
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);

        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }
    void OnClickEquipmentResetButton()
    {
        Managers.Sound.PlayButtonClick();
        UI_EquipmentResetPopup resetPopup = (Managers.UI.SceneUI as UI_LobbyScene).EquipmentResetPopupUI;
        resetPopup.SetInfo(Equipment);
        resetPopup.gameObject.SetActive(true);
    }
    void OnClickEquipButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Equip_Equipment");
        
        // 장비를 장착
        Managers.Game.EquipItem(Equipment.EquipmentData.EquipmentType, Equipment);
        Refresh();
        
        // 창닫기
        gameObject.SetActive(false);
        
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }
    void OnClickUnequipButton()
    {
        Managers.Sound.PlayButtonClick();
        
        // 장비를 장착해제
        Managers.Game.UnEquipItem(Equipment);
        Refresh();
        
        // 창닫기
        gameObject.SetActive(false);
        
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickLevelupButton()
    {
        Managers.Sound.PlayButtonClick();
        
        // 장비레벨이 최대레벨보다 작아야 함
        if (Equipment.Level >= Equipment.EquipmentData.MaxLevel)
            return;
        
        // Cost_Gold, Cost_Material 값 가져오기
        int upgradeCost = Managers.Data.EquipLevelDataDic[Equipment.Level].UpgradeCost;
        int upgradeRequiredItems = Managers.Data.EquipLevelDataDic[Equipment.Level].UpgradeRequiredItems;

        int numMaterial = 0;
        Managers.Game.ItemDictionary.TryGetValue(Equipment.EquipmentData.LevelupMaterialId, out numMaterial);
        if (Managers.Game.Gold >= upgradeCost && numMaterial >= upgradeRequiredItems)
        {
            Equipment.LevelUp();

            Managers.Game.Gold -= upgradeCost;
            Managers.Game.RemoveMaterialItem(Equipment.EquipmentData.LevelupMaterialId, upgradeRequiredItems);
            Managers.Sound.Play(Define.Sound.Effect, "Levelup_Equipment");
            
            Refresh();
        }
        else
        {
            Managers.UI.ShowToast("재화가 부족합니다.");
        }
        
        // 버튼 누를 때 EquipmentPopup Refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickMergeButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Equipment.IsEquipped) return;
        UI_MergePopup mergePopupUI = (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI;
        mergePopupUI.SetInfo(Equipment);
        mergePopupUI.gameObject.SetActive(true);
    }
}
