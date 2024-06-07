using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ContinuePopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
    }

    enum Texts
    {
        ContinuePopupTitleText,
        CountdownValueText,
        ContinueButtonText,
        ContinueCostValueText,
        AdContinueText,
    }

    enum Buttons
    {
        CloseButton,
        ContinueButton,
        AdContinueButton,
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
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GetButton((int)Buttons.CloseButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        GetButton((int)Buttons.ContinueButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdContinueButton).gameObject.BindEvent(OnClickAdContinueButton);
        GetButton((int)Buttons.AdContinueButton).GetOrAddComponent<UI_ButtonAnimation>();
        Refresh();
        return true;
    }

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int keyCount) == true)
        {
            GetText((int)Texts.ContinueCostValueText).text = $"1/{keyCount}";
        }
        else
        {
            GetText((int)Texts.ContinueCostValueText).text = $"<color=red>0</color>";
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.AdContinueButton).GetOrAddComponent<RectTransform>());
    }

    void OnClickCloseButton()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Game.GameOver();
    }

    IEnumerator CountdownCoroutine()
    {
        int count = 10;
        while (count > 0)
        {
            yield return new WaitForSeconds(1f);
            count--;
            GetText((int)Texts.CountdownValueText).text = count.ToString();
            if (count == 0) break;
        }

        yield return new WaitForSeconds(1f);

        Managers.UI.ClosePopupUI(this);
        Managers.Game.GameOver();
    }

    void OnClickContinueButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int keyCount) == true)
        {
            Managers.Game.RemoveMaterialItem(Define.ID_BRONZE_KEY, 1);
            Managers.Game.Player.Resurrection(1);
            Managers.UI.ClosePopupUI(this);
        }
    }

    void OnClickAdContinueButton()
    {
        Managers.Sound.PlayButtonClick();
        
        Managers.Ads.ShowRewardedAd(() =>
        {
            Managers.Game.Player.Resurrection(1);
            Managers.UI.ClosePopupUI();
        });
    }
}
