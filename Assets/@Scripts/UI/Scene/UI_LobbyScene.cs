
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    // private UI_BattlePopup _battlePopupUI;
    private bool _isSelectedBattle = false;
    private UI_ShopPopup _shopPopupUI;
    private bool _isSelectedShop = false;
    private UI_ChallengePopup _challengePopupUI;
    // private UI_MergePopup _mergePopupUI;
    // public UI_MergePopup MergePopupUI
    // {
    //     get { return _mergePopupUI; }
    // }

    // private UI_EquipmentInfoPopup _equipmentInfoPopupUI;
    // public UI_EquipmentInfoPopup EquipmentInfoPopupUI
    // {
    //     get { return _equipmentInfoPopupUI; }
    // }
    //
    // private UI_EquipmentResetPopup _equipmentResetPopupUI;
    // public UI_EquipmentResetPopup EquipmentResetPopupUI
    // {
    //     get { return _equipmentResetPopupUI; }
    // }
    //
    // private UI_RewardPopup _rewardPopupUI;
    // public UI_RewardPopup RewardPopupUI
    // {
    //     get { return _rewardPopupUI; }
    // }
    //
    // private UI_MergeResultPopup _mergeResultPopupUI;
    // public UI_MergeResultPopup MergeResultPopupUI
    // {
    //     get { return _mergeResultPopupUI; }
    // }
    
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
        // GetToggle((int)Toggles.EvolveToggle).gameObject.BindEvent(OnClickEvolveToggle);
        // GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        // GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);
        // GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        // GetToggle((int)Toggles.ChallengeToggle).gameObject.BindEvent(OnClickChallengeToggle);

        // _evolvePopupUI = Managers.UI.ShowPopupUI<UI_EvolvePopup>();
        // _equipmentPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
        // _battlePopupUI = Managers.UI.ShowPopupUI<UI_BattlePopup>();
        // _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
        // _challengePopupUI = Managers.UI.ShowPopupUI<UI_ChallengePopup>();
        // _equipmentInfoPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        // _mergePopupUI = Managers.UI.ShowPopupUI<UI_MergePopup>();
        // _equipmentResetPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentResetPopup>();
        // _rewardPopupUI = Managers.UI.ShowPopupUI<UI_RewardPopup>();
        // _mergeResultPopupUI = Managers.UI.ShowPopupUI<UI_MergeResultPopup>();

        // TogglesInit();
        GetToggle((int)Toggles.BattleToggle).isOn = true;
        // OnClickBattleToggle();

        Managers.Game.OnResourcesChanged += Refresh;
        Refresh();
        
        return true;
    }

    void Refresh()
    {

    }

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnResourcesChanged -= Refresh;
        }
    }

    void OnClickStartButton()
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = false;
        if (Managers.Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Debug.Log("Not Enough Stamina");
            return;
        }

        Managers.Game.Stamina -= Define.GAME_PER_STAMINA;
        if (Managers.Game.DicMission.TryGetValue(Define.MissionTarget.StageEnter, out MissionInfo mission))
        {
            mission.Progress++;
        }

        Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            Managers.UI.ShowToast("test");
        }
    }
#endif
}
