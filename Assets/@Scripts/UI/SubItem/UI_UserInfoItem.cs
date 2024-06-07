using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_UserInfoItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        // UserExpSliderObject,
    }
    enum Buttons
    {
        StaminaButton,
        DiaButton,
        GoldButton,
    }

    enum Texts
    {
        // UserNameText,
        // UserLevelText,
        StaminaValueText,
        DiaValueText,
        GoldValueText,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.StaminaButton).gameObject.BindEvent(OnClickStaminaButton);
        GetButton((int)Buttons.StaminaButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.DiaButton).gameObject.BindEvent(OnClickDiaButton);
        GetButton((int)Buttons.DiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.GoldButton).gameObject.BindEvent(OnClickGoldButton);
        GetButton((int)Buttons.GoldButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        

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

    void OnClickStaminaButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_StaminaChargePopup>();
    }

    void OnClickDiaButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_DiaChargePopup>();
    }
    
    void OnClickGoldButton()
    {
        Managers.Sound.PlayButtonClick();
        // TODO : Managers.UI.ShowPopupUI<UI_GoldChargePopup>();
    }
}
