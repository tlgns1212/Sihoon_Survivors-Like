using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StaminaChargePopup : UI_Popup
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
        StaminaChargePopupTitleText,
        DiaRemainingValueText,
        AdRemainingValueText,
        ChargeInfoText,
        ChargeInfoValueText,
        HaveStaminaValueText,
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        StartCoroutine(CoTimeCheck());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.BuyDiaButton).gameObject.BindEvent(OnClickBuyDiaButton);
        GetButton((int)Buttons.BuyDiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BuyAdButton).gameObject.BindEvent(OnClickBuyAdButton);
        GetButton((int)Buttons.BuyAdButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.HaveStaminaValueText).text = "+1";
        GetText((int)Texts.DiaRemainingValueText).text = $"오늘 남은 횟수 : {Managers.Game.RemainsStaminaByDia}";
        GetText((int)Texts.AdRemainingValueText).text = $"오늘 남은 횟수 : {Managers.Game.StaminaCountAds}";
    }

    IEnumerator CoTimeCheck()
    {
        while (true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Managers.Time.StaminaTime);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            GetText((int)Texts.ChargeInfoValueText).text = formattedTime;
            yield return new WaitForSeconds(1f);
        }
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickBuyDiaButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.RemainsStaminaByDia > 0 && Managers.Game.Dia >= 100)
        {
            string[] spriteNames = new string[1];
            int[] counts = new int[1];

            spriteNames[0] = Managers.Data.MaterialDic[Define.ID_STAMINA].SpriteName;
            counts[0] = 15;

            // UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            // rewardPopup.gameObject.SetActive(true);
            Managers.Game.RemainsStaminaByDia--;
            Managers.Game.Dia -= 100;
            Managers.Game.Stamina += 15;
            // rewardPopup.SetInfo(spriteNames, counts);
        }
    }
    
    void OnClickBuyAdButton()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.StaminaCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                string[] spriteNames = new string[1];
                int[] counts = new int[1];

                spriteNames[0] = Managers.Data.MaterialDic[Define.ID_STAMINA].SpriteName;
                counts[0] = 5;

                // UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                // rewardPopup.gameObject.SetActive(true);
                Managers.Game.StaminaCountAds--;
                Managers.Game.Stamina += 5;
                // rewardPopup.SetInfo(spriteNames, counts);    
            });
        }
    }
}
