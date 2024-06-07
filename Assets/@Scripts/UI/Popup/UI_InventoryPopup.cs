using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        InventoryScrollContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        InventoryPopupTitleText,
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
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

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

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI();
    }
}
