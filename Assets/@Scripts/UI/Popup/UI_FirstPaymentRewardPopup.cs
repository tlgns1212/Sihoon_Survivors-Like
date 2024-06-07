using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_FirstPaymentRewardPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
        GoNowButton,
    }

    enum Texts
    {
        BackgroundText,
        FirstPaymentRewardPopupTitleText,
        FirstPaymentRewardDescriptionText,
        
        GoNowText,
    }

    enum Images
    {
        
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
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.GoNowButton).gameObject.BindEvent(OnClickGoNowButton);
        GetButton((int)Buttons.GoNowButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
        // UI_MaterialItem 다 DEstroyChild하고 새로 만들어야 함.
    }

    void Refresh()
    {
        
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickGoNowButton()
    {
        // 모든 팝업을 닫고 상점 탭으로 이동
    }
    
}
