using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_PickupGachaInfoPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        MythSkillOptionObject,
        EquipmentInfoPopupTitleObject,
        EquipmentGradeSkillScrollContentObject,
        ButtonGroupObject,
        PickupEquipmentButtonGroupObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Toggles
    {
        PickupWeaponToggle,
        PickupArmorToggle,
        PickupGlovesToggle,
        PickupBootsToggle,
        PickupBeltToggle,
        PickupRingToggle,
    }

    enum Texts
    {
        BackgroundText,
        EquipmentGradeSkillText,
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,
        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        LegendarySkillOptionDescriptionValueText,
        MythSkillOptionDescriptionValueText,
        EquipmentDescriptionValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        PickupWeaponImage,
        PickupArmorImage,
        PickupGlovesImage,
        PickupBootsImage,
        PickupBeltImage,
        PickupRingImage,
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
        BindToggle(typeof(Toggles));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetToggle((int)Toggles.PickupWeaponToggle).gameObject.BindEvent(OnClickPickupWeaponToggle);
        GetToggle((int)Toggles.PickupWeaponToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupArmorToggle).gameObject.BindEvent(OnClickPickupArmorToggle);
        GetToggle((int)Toggles.PickupArmorToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupGlovesToggle).gameObject.BindEvent(OnClickPickupGlovesToggle);
        GetToggle((int)Toggles.PickupGlovesToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupBootsToggle).gameObject.BindEvent(OnClickPickupBootsToggle);
        GetToggle((int)Toggles.PickupBootsToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupBeltToggle).gameObject.BindEvent(OnClickPickupBeltToggle);
        GetToggle((int)Toggles.PickupBeltToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupRingToggle).gameObject.BindEvent(OnClickPickupRingToggle);
        GetToggle((int)Toggles.PickupRingToggle).GetOrAddComponent<UI_ButtonAnimation>();
        
        GetObject((int)GameObjects.PickupEquipmentButtonGroupObject).gameObject.SetActive(true);

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        
    }

    void EquipmentInfoInit()
    {
        // 장비 정보 초기화
    }

    void PickupToggleGroupInit()
    {
        // 토글 초기화
    }

    void OnClickPickupWeaponToggle()
    {
        Managers.Sound.PlayButtonClick();
        // 픽업 무기 정보 변경
    }
    void OnClickPickupArmorToggle()
    {
        Managers.Sound.PlayButtonClick();
        // 픽업 갑옷 정보 변경
    }
    void OnClickPickupGlovesToggle()
    {
        Managers.Sound.PlayButtonClick();
        // 픽업 장갑 정보 변경
    }
    void OnClickPickupBootsToggle()
    {
        Managers.Sound.PlayButtonClick();
        // 픽업 신발 정보 변경
    }
    void OnClickPickupBeltToggle()
    {
        Managers.Sound.PlayButtonClick();
        // 픽업 벨트 정보 변경
    }
    void OnClickPickupRingToggle()
    {
        Managers.Sound.PlayButtonClick();
        // 픽업 반지 정보 변경
    }
    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    
}
