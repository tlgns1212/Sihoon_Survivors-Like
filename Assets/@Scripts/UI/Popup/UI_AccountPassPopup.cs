using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_AccountPassPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        AccountPassScrollContentObject,
    }

    enum Buttons
    {
        BackButton,
        
        RarePassButton,
        EpicPassButton,
    }

    enum Texts
    {
        BackgroundText,
        AchievementTitleText,
        AccountPassDescriptionText,
        
        FreePassTitleText,
        RarePassButtonText,
        RarePassPriceText,
        EpicPassButtonText,
        EpicPassPriceText,
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
        
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.RarePassButton).gameObject.BindEvent(OnClickRarePassButton);
        GetButton((int)Buttons.RarePassButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EpicPassButton).gameObject.BindEvent(OnClickEpicPassButton);
        GetButton((int)Buttons.EpicPassButton).GetOrAddComponent<UI_ButtonAnimation>();

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

    void OnClickRarePassButton()
    {
        // 레어 패스 구매, 결제 모듈 호출
    }
    
    void OnClickEpicPassButton()
    {
        // 에픽 패스 구매, 결제 모듈 호출
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
    
}
