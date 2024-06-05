using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_DiaChargePopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
        BuyDiaButton,
        BuyAdButton,
    }

    enum Texts
    {
        BackgroundText,
        BuyAdText,
        DiaChargePopupTitleText,
        DiaChargeValueText,
        AdChargeValueText,
        AdRemainingValueText,
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
        GetButton((int)Buttons.BuyAdButton).gameObject.BindEvent(OnClickBuyAdButton);
        GetButton((int)Buttons.BuyAdButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BuyDiaButton).gameObject.BindEvent(OnClickBuyDiaButton);
        GetButton((int)Buttons.BuyDiaButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }

    void Refresh()
    {
        GetText((int)Texts.AdRemainingValueText).text = $"오늘 남은 횟수 : {Managers.Game.DiaCountAds}";
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickBuyAdButton()
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.DiaCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                string[] spriteNames = new string[1];
                int[] counts = new int[1];

                spriteNames[0] = Managers.Data.MaterialDic[Define.ID_DIA].SpriteName;
                counts[0] = 200;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.DiaCountAds--;
                Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_DIA], 200);
                Refresh();
                rewardPopup.SetInfo(spriteNames, counts);
            });
        }
    }
    
    void OnClickBuyDiaButton()
    {
        Managers.Sound.PlayButtonClick();
        // TODO : 현금 만들것
    }
}
