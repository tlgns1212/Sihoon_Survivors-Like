using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_UserInfoItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        EvolveToggleRedDotObject,
        EquipmentToggleRedDotObject,
        BattleToggleRedDotObject,
        ShopToggleRedDotObject,
        ChallengeToggleRedDotObject,
        
        MenuToggleGroup,
        CheckEvolveImageObject,
        CheckEquipmentImageObject,
        CheckBattleImageObject,
        CheckShopImageObject,
        CheckChallengeImageObject,
    }
    enum Buttons
    {
        // UserIconButton, // TODO : 유저 정보 팝업 만들어서 호출
    }

    enum Texts
    {
        // UserNameText,
        // UserLevelText
        EvolveToggleText,
        EquipmentToggleText,
        BattleToggleText,
        ShopToggleText,
        ChallengeToggleText,
        StaminaValueText,
        DiaValueText,
        GoldValueText,
    }

    enum Toggles
    {
        EvolveToggle,
        EquipmentToggle,
        BattleToggle,
        ShopToggle,
        ChallengeToggle,
    }

    enum Images
    {
        BackgroundImage,
    }
    #endregion

    // private UI_EvolvePopup _evolvePopupUI;
    // public UI_EvolvePopup EvolvePopupUI
    // {
    //     get { return _evolvePopupUI; }
    // }
    private bool _isSelectedEvolve = false;
    // private UI_EquipmentPopup _equipmentPopupUI;
    // public UI_EquipmentPopup EquipmentPopupUI
    // {
    //     get { return _equipmentPopupUI; }
    // }
    private bool _isSelectedEquipment = false;
    // private UI_BattlePopup _battlePopupUI;
    // public UI_BattlePopup BattlePopupUI
    // {
    //     get { return _battlePopupUI; }
    // }
    private bool _isSelectedBattle = false;
    // private UI_ShopPopup _shopPopupUI;
    // public UI_ShopPopup ShopPopupUI
    // {
    //     get { return _shopPopupUI; }
    // }
    private bool _isSelectedShop = false;
    // private UI_ChallengePopup _challengePopupUI;
    // public UI_ChallengePopup ChallengePopupUI
    // {
    //     get { return _challengePopupUI; }
    // }
    private bool _isSelectedChallenge = false;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        

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

    void OnClickStaminaButton()
    {
        Managers.Sound.PlayButtonClick();
        // Managers.UI.ShowPopupUI<UI_StaminaChargePopup>();
    }

    void OnClickDiaButton()
    {
        Managers.Sound.PlayButtonClick();
        // Managers.UI.ShowPopupUI<UI_DiaChargePopup>();
    }
    
    void OnClickGoldButton()
    {
        Managers.Sound.PlayButtonClick();
        // Managers.UI.ShowPopupUI<UI_GoldChargePopup>();
    }
}
