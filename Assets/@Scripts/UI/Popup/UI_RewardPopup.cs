using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RewardPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        RewardItemScrollContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        RewardPopupTitleText,
        BackgroundText,
    }
    #endregion

    public Action OnClosed;
    private string[] _spriteNames;
    private int[] _counts;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        Refresh();
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_Reward");
        return true;
    }

    public void SetInfo(string[] spriteNames, int[] counts, Action callback = null)
    {
        _spriteNames = spriteNames;
        _counts = counts;
        OnClosed = callback;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        
        GetObject((int)GameObjects.RewardItemScrollContentObject).DestroyChilds();
        for (int i = 0; i < _spriteNames.Length; i++)
        {
            UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(GetObject((int)GameObjects.RewardItemScrollContentObject).transform);
            item.SetInfo(_spriteNames[i], _counts[i]);
        }
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);
        OnClosed?.Invoke();
    }
}
