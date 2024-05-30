using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class UI_GachaEquipmentInfoPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        // CommonSkillOptionObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        EquipmentGradeSkillScrollContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,
        // CommonSkillOptionDescriptionValueText,
        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        LegendarySkillOptionDescriptionValueText,
        EquipmentGradeSkillText,
        BackgroundText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        EquipmentImage,
        GradeBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
        
        // CommonSkillLockImage,
        UncommonSkillLockImage,
        RareSkillLockImage,
        EpicSkillLockImage,
        LegendarySkillLockImage,
    }
    #endregion

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
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

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

        GetImage((int)Images.EquipmentTypeImage).sprite =
            Managers.Resource.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage((int)Images.EquipmentImage).sprite =
            Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);

        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Common:
                // // GetText((int)Texts.EquipmentGradeValueText).text = Equipment.EquipmentData.EquipmentGrade.ToString();
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
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            default:
                break;
        }
        
        // EquipmentNameValueText : 장비의 이름
        GetText((int)Texts.EquipmentNameValueText).text = Equipment.EquipmentData.NameTextId;
        // EquipmentLevelValueText : 장비의 레벨 (현재 레벨/최대 레벨)
        GetText((int)Texts.EquipmentLevelValueText).text = $"{Equipment.Level}/{Equipment.EquipmentData.MaxLevel}";
        // EquipmentOptionImage : 장비 옵션의 아이콘
        string spriteName = Equipment.MaxHpBonus == 0 ? "AttackPoint_Icon.sprite" : "HealthPoint_Icon.sprite";
        GetImage((int)Images.EquipmentOptionImage).sprite = Managers.Resource.Load<Sprite>(spriteName);
        // EquipmentOptionValueText : 장비 옵션 수치
        string bonusValue = Equipment.MaxHpBonus == 0
            ? Equipment.AttackBonus.ToString()
            : Equipment.MaxHpBonus.ToString();
        GetText((int)Texts.EquipmentOptionValueText).text = $"{bonusValue}";
        #endregion

        #region 장비스킬 옵션 설정
        // 만약 장비 데이터 테이블의 각 등급별 옵션(스킬Id)에 스킬이 없다면 등급에 맞는 옵션 오브젝트 비활성화
        // // GetObject((int)GameObjects.CommonSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.RareSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(false);

        // // if (Managers.Data.SupportSkillDic.ContainsKey(Equipment.EquipmentData.common))
        // // {
        // //     SupportSkillData skillData = Managers.Data.SupportSkillDic[Equipment.EquipmentData.CommonGradeSkill];
        // //     GetText((int)Texts.CommonSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
        // //     GetObject((int)GameObjects.CommonSkillOptionObject).SetActive(true);
        // // }
        if (Managers.Data.SupportSkillDic.ContainsKey(Equipment.EquipmentData.UncommonGradeSkill))
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

        #region 장비스킬 옵션 색상 설정

        Define.EquipmentGrade equipmentGrade = Equipment.EquipmentData.EquipmentGrade;
        
        // 공통 색상 변경
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
        // // if (equipmentGrade >= Define.EquipmentGrade.Legendary)
        // // {
        // //     GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = EquipmentUIColors.Legendary;
        // //     GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(false);
        // // }
        #endregion

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
    }

    private void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);

        // Close 할때 EquipmentPopup Refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }
}
