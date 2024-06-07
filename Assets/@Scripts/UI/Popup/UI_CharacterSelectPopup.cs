using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelectPopup : UI_Popup
{
    // TODO : 여기 더 해야 함

    #region Enum

    enum GameObjects
    {
        ContentObject,
        CharacterLevelObject,
        AttackPointObject,
        HealthPointObject,
        CharacterEnhancePanelObject,
        CharacterEnhanceContentObject,
        CharacterUpgradeContentObject,
        EnhanceCostObject,
        UpgradeCostObject,
    }

    enum Images
    {
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        AttackImage,
        CharacterImage,
        HealthImage,
    }

    enum Buttons
    {
        EnhanceButton,
        LevelUpButton,
        EquipButton,
        BackButton,
    }

    enum Texts
    {
        CharacterNameValueText,
        AttackValueText,
        AttackBonusValueText,
        HealthValueText,
        HealthBonusValueText,
        CharacterInventoryTitleText,
        EnhanceButtonText,
        EquipButtonText,
        LevelUpButtonText,
        EnhanceCostGoldValueText,
        EnhanceCostMaterialValueText,
        UpgradeCostMaterialValueText,
    }

    enum Toggles
    {
        EnhanceToggle,
        UpgradeToggle,
    }
    #endregion

    private bool _isCharacterEnhancePanelOpen = false; // 강화 패널 오픈 됐는지

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
        
        GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(false);
        _isCharacterEnhancePanelOpen = false;
        GetImage((int)Images.StarOn_1).gameObject.SetActive(false);
        GetImage((int)Images.StarOn_2).gameObject.SetActive(false);
        GetImage((int)Images.StarOn_3).gameObject.SetActive(false);
        GetImage((int)Images.StarOn_4).gameObject.SetActive(false);
        
        GetButton((int)Buttons.EnhanceButton).gameObject.BindEvent(OnClickEnhanceButton);
        GetButton((int)Buttons.EnhanceButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.LevelUpButton).gameObject.BindEvent(OnClickLevelUpButton);
        GetButton((int)Buttons.LevelUpButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.EnhanceToggle).gameObject.BindEvent(OnClickEnhanceToggle);
        GetToggle((int)Toggles.UpgradeToggle).gameObject.BindEvent(OnClickUpgradeToggle);

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AttackPointObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.HealthPointObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EnhanceCostObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.UpgradeCostObject).GetComponent<RectTransform>());
    }

    void OnClickEquipButton()
    {
        // 현재 선택된 캐릭터 장착
        Managers.Sound.PlayButtonClick();
    }

    void OnClickEnhanceButton()
    {
        Managers.Sound.PlayButtonClick();
        
        GetButton((int)Buttons.EnhanceButton).gameObject.SetActive(false);
        GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(true);
        
        // 탭 리프레시 해결해야 함
        
        GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(true);
        _isCharacterEnhancePanelOpen = true;
        OnClickEnhanceToggle();
        GetButton((int)Buttons.EquipButton).gameObject.SetActive(false);
    }

    void OnClickLevelUpButton()
    {
        // 레벨 업
        Managers.Sound.PlayButtonClick();
    }

    void OnClickBackButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_isCharacterEnhancePanelOpen)
        {
            GetButton((int)Buttons.EnhanceButton).gameObject.SetActive(true);
            GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(false);
            GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(false);
            _isCharacterEnhancePanelOpen = false;
            GetButton((int)Buttons.EquipButton).gameObject.SetActive(true);
        }
        else
        {
            Managers.UI.ClosePopupUI(this);
        }
    }

    void OnClickEnhanceToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(false);
        Refresh();
    }
    void OnClickUpgradeToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(true);
        Refresh();
    }
}
