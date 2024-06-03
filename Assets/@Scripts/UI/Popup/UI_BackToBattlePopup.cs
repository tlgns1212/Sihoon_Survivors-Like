using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_BackToBattlePopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
    }

    enum Buttons
    {
        ConfirmButton,
        CancelButton,
    }

    enum Texts
    {
        BackToBattleTitleText,
        BackToBattleContentText,
        ConfirmText,
        CancelText,
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

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton);
        GetButton((int)Buttons.CancelButton).GetOrAddComponent<UI_ButtonAnimation>();

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickConfirmButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
    }

    void OnClickCancelButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.ClearContinueData();
        Managers.UI.ClosePopupUI(this);
    }

}
