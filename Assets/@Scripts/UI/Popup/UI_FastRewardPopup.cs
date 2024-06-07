using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_FastRewardPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        ItemContainer,
    }

    enum Buttons
    {
        BackgroundButton,
        AdFreeButton,
        ClaimButton,
    }

    enum Texts
    {
        BackgroundText,
        FastRewardPopupTitleText,
        FastRewardCommentText,
        AdFreeText,
        ClaimCostValueText,
        RemainingCommentText,
        RemainingCountValueText,
    }
    #endregion

    private OfflineRewardData _offlineRewardData;
    private bool _isClaim = false;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.AdFreeButton).gameObject.BindEvent(OnClickAdFreeButton);
        GetButton((int)Buttons.AdFreeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ClaimButton).gameObject.BindEvent(OnClickClaimButton);
        GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Color.white;

        return true;
    }

    public void SetInfo(OfflineRewardData offlineReward)
    {
        _offlineRewardData = offlineReward;
        Refresh();
    }

    void Refresh()
    {
        GameObject container = GetObject((int)GameObjects.ItemContainer);
        container.DestroyChilds();

        if (Managers.Game.Stamina >= 15 && Managers.Game.FastRewardCountStamina > 0)
        {
            GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Util.HexToColor("50D500");
            _isClaim = true;
            GetButton((int)Buttons.ClaimButton).GetOrAddComponent<UI_ButtonAnimation>();
        }
        else
        {
            GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Util.HexToColor("989898");
            _isClaim = false;
        }

        UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        int count = (_offlineRewardData.RewardGold) * 5;
        item.SetInfo(Define.GOLD_SPRITE_NAME, count);

        UI_MaterialItem scroll = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        scroll.SetInfo("Scroll_Random_Icon", _offlineRewardData.FastRewardScroll);

        UI_MaterialItem box = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        box.SetInfo("Key_Silver_Icon", _offlineRewardData.FastRewardBox);

        GetText((int)Texts.RemainingCountValueText).text = Managers.Game.FastRewardCountStamina.ToString();
        
        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.AdFreeButton).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ClaimButton).GetComponent<RectTransform>());
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickAdFreeButton()
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.FastRewardCountAds > 0)
        {
            Managers.Game.FastRewardCountAds--;
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Time.GiveFastOfflineReward(_offlineRewardData);
                Managers.UI.ClosePopupUI(this);
            });
        }
        else
        {
            Managers.UI.ShowToast("더이상 받을 수 없습니다.");
        }
    }

    void OnClickClaimButton()
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.Stamina >= 15 && Managers.Game.FastRewardCountStamina > 0 && _isClaim)
        {
            Managers.Game.Stamina -= 15;
            Managers.Game.FastRewardCountStamina--;
            // 스태미나를 소모하고 팝업 닫기
            Managers.Time.GiveFastOfflineReward(_offlineRewardData);
            Managers.UI.ClosePopupUI(this);
            Refresh();
        }
        else
            return;
    }
}
