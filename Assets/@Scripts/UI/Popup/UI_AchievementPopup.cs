using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UI_AchievementPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        AchievementScrollObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        AchievementTitleText,
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
        if (_init == false)
            return;
        
        GetObject((int)GameObjects.AchievementScrollObject).DestroyChilds();

        foreach (AchievementData data in Managers.Achievement.GetProceedingAchievements())
        {
            UI_AchievementItem item = Managers.UI.MakeSubItem<UI_AchievementItem>(GetObject((int)GameObjects.AchievementScrollObject).transform);
            item.SetInfo(data);
        }
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
