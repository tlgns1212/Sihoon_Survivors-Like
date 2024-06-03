
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_LobbyScene : UI_Scene
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
        // UserIconButton, // 추후 유저 정보 팝업 만들어야 함
    }

    enum Texts
    {
        EvolveToggleText,
        EquipmentToggleText,
        BattleToggleText,
        ShopToggleText,
        ChallengeToggleText,
        // UserNameText,
        // UserLevelText,
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

    private UI_EvolvePopup _evolvePopupUI;
    private UI_EquipmentPopup _equipmentPopupUI;
    private bool _isSelectedEquipment = false;

    public UI_EquipmentPopup EquipmentPopupUI { get { return _equipmentPopupUI; } }

    private UI_BattlePopup _battlePopupUI;
    private bool _isSelectedBattle = false;
    private UI_ShopPopup _shopPopupUI;
    private bool _isSelectedShop = false;
    private UI_ChallengePopup _challengePopupUI;
    private UI_MergePopup _mergePopupUI;
    public UI_MergePopup MergePopupUI
    {
        get { return _mergePopupUI; }
    }

    private UI_EquipmentInfoPopup _equipmentInfoPopupUI;
    public UI_EquipmentInfoPopup EquipmentInfoPopupUI
    {
        get { return _equipmentInfoPopupUI; }
    }
    private UI_EquipmentResetPopup _equipmentResetPopupUI;
    public UI_EquipmentResetPopup EquipmentResetPopupUI
    {
        get { return _equipmentResetPopupUI; }
    }
    
    private UI_RewardPopup _rewardPopupUI;
    public UI_RewardPopup RewardPopupUI
    {
        get { return _rewardPopupUI; }
    }
    
    private UI_MergeResultPopup _mergeResultPopupUI;
    public UI_MergeResultPopup MergeResultPopupUI
    {
        get { return _mergeResultPopupUI; }
    }
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        // 오브젝트 바인딩
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Toggles));
        BindImage(typeof(Images));

        // 토글 클릭 시 행동
        GetToggle((int)Toggles.EvolveToggle).gameObject.BindEvent(OnClickEvolveToggle);
        GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);
        GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        GetToggle((int)Toggles.ChallengeToggle).gameObject.BindEvent(OnClickChallengeToggle);

        _evolvePopupUI = Managers.UI.ShowPopupUI<UI_EvolvePopup>();
        _equipmentPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
        _battlePopupUI = Managers.UI.ShowPopupUI<UI_BattlePopup>();
        _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
        _challengePopupUI = Managers.UI.ShowPopupUI<UI_ChallengePopup>();
        _equipmentInfoPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        _mergePopupUI = Managers.UI.ShowPopupUI<UI_MergePopup>();
        _equipmentResetPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentResetPopup>();
        _rewardPopupUI = Managers.UI.ShowPopupUI<UI_RewardPopup>();
        _mergeResultPopupUI = Managers.UI.ShowPopupUI<UI_MergeResultPopup>();

        TogglesInit();
        GetToggle((int)Toggles.BattleToggle).isOn = true;
        OnClickBattleToggle();

        Managers.Game.OnResourcesChanged += Refresh;
        Refresh();
        
        return true;
    }

    void Refresh()
    {
        GetText((int)Texts.StaminaValueText).text = $"{Managers.Game.Stamina}/{Define.MAX_STAMINA}";
        GetText((int)Texts.DiaValueText).text = Managers.Game.Dia.ToString();
        GetText((int)Texts.GoldValueText).text = Managers.Game.Gold.ToString();
        
        // 토글 선택 시 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
    }

    private void Awake()
    {
        Init();
    }
    
    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnResourcesChanged -= Refresh;
        }
    }

    void TogglesInit()
    {
        #region 팝업 초기화
        _shopPopupUI.gameObject.SetActive(false);
        _equipmentPopupUI.gameObject.SetActive(false);
        _battlePopupUI.gameObject.SetActive(false);
        _challengePopupUI.gameObject.SetActive(false);
        _evolvePopupUI.gameObject.SetActive(false);
        _equipmentInfoPopupUI.gameObject.SetActive(false);
        _mergePopupUI.gameObject.SetActive(false);
        _equipmentResetPopupUI.gameObject.SetActive(false);
        _rewardPopupUI.gameObject.SetActive(false);
        _mergeResultPopupUI.gameObject.SetActive(false);
        #endregion

        #region 토글 버튼 초기화
        // 재클릭 방지 트리거 초기화
        _isSelectedEquipment = false;
        _isSelectedBattle = false;
        _isSelectedShop = false;
        
        // 레드닷 버튼 초기화
        GetObject((int)GameObjects.EvolveToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.BattleToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.ChallengeToggleRedDotObject).SetActive(false);

        // 선택 토글 아이콘 초기화
        GetObject((int)GameObjects.CheckEvolveImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckEquipmentImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckBattleImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckShopImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckChallengeImageObject).SetActive(false);
        
        GetObject((int)GameObjects.CheckEvolveImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckChallengeImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        
        GetText((int)Texts.EvolveToggleText).gameObject.SetActive(false);
        GetText((int)Texts.EquipmentToggleText).gameObject.SetActive(false);
        GetText((int)Texts.BattleToggleText).gameObject.SetActive(false);
        GetText((int)Texts.ShopToggleText).gameObject.SetActive(false);
        GetText((int)Texts.ChallengeToggleText).gameObject.SetActive(false);
        
        GetToggle((int)Toggles.EvolveToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.EquipmentToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.BattleToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.ShopToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.ChallengeToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        #endregion
    }

    void ShowUI(GameObject contentPopup, Toggle toggle, TMP_Text text, GameObject obj2, float duration = 0.1f)
    {
        TogglesInit();
        
        contentPopup.SetActive(true);
        toggle.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 150);
        text.gameObject.SetActive(true);
        obj2.SetActive(true);
        obj2.GetComponent<RectTransform>().DOSizeDelta(new Vector2(200, 180), duration).SetEase(Ease.InOutQuad);

        Refresh();
    }

    void OnClickShopToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.BackgroundImage).color = Util.HexToColor("525DAD");
        if (_isSelectedShop == true)
            return;
        ShowUI(_shopPopupUI.gameObject, GetToggle((int)Toggles.ShopToggle), GetText((int)Texts.ShopToggleText), GetObject((int)GameObjects.CheckShopImageObject));
        _isSelectedShop = true;
    }
    void OnClickEquipmentToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.BackgroundImage).color = Util.HexToColor("5C254B");
        if (_isSelectedEquipment == true)
            return;
        ShowUI(_equipmentPopupUI.gameObject, GetToggle((int)Toggles.EquipmentToggle), GetText((int)Texts.EquipmentToggleText), GetObject((int)GameObjects.CheckEquipmentImageObject));
        _isSelectedEquipment = true;
        
        _equipmentPopupUI.SetInfo();
    }
    void OnClickBattleToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.BackgroundImage).color = Util.HexToColor("1F5FA0");
        if (_isSelectedBattle == true)
            return;
        ShowUI(_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        _isSelectedBattle = true;
    }
    void OnClickEvolveToggle()
    {
        Managers.Sound.PlayButtonClick();
        // GetImage((int)Images.BackgroundImage).color = Util.HexToColor("1F5FA0");
        // if (_isSelectedBattle == true)
        //     return;
        // ShowUI(_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        // _isSelectedBattle = true;
    }
    
    void OnClickChallengeToggle()
    {
        Managers.Sound.PlayButtonClick();
        // GetImage((int)Images.BackgroundImage).color = Util.HexToColor("1F5FA0");
        // if (_isSelectedBattle == true)
        //     return;
        // ShowUI(_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        // _isSelectedBattle = true;
    }
}
