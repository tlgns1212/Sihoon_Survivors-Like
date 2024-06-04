using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GachaResultsPopup : UI_Popup
{

    #region Enum

    enum GameObjects
    {
        OpenContentObject,
        ResultsContentObject,
        ResultsContentScrollObject,
        GachaBoxAnim,
    }

    enum Texts
    {
        SkipButtonText,
    }

    enum Buttons
    {
        SkipButton,
        ConfirmButton,
    }
    #endregion

    private List<Equipment> _items = new List<Equipment>();
    private Animator _anim;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ResultsContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        GetObject((int)GameObjects.OpenContentObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.ResultsContentObject).gameObject.SetActive(false);
        
        GetButton((int)Buttons.SkipButton).gameObject.BindEvent(OnClickSkipButton);
        GetButton((int)Buttons.SkipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
        AnimationEventDetector ad = GetObject((int)GameObjects.GachaBoxAnim).GetComponent<AnimationEventDetector>();
        ad.OnEvent -= PlayParticle;
        ad.OnEvent += PlayParticle;
        Refresh();

        var main = _particle.GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_GameResult");

        return true;
    }

    public void SetInfo(List<Equipment> items)
    {
        _items = items;
        Refresh();
    }

    void Refresh()
    {
        
    }

    void OnClickSkipButton()
    {
        Managers.Sound.PlayButtonClick();
        
        GetObject((int)GameObjects.OpenContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.ResultsContentObject).gameObject.SetActive(true);

        GameObject container = GetObject((int)GameObjects.ResultsContentScrollObject);
        container.DestroyChilds();

        foreach (Equipment item in _items)
        {
            UI_EquipItem equipItem = Managers.Resource.Instantiate("UI_EquipItem", pooling: true).GetOrAddComponent<UI_EquipItem>();
            equipItem.transform.SetParent(container.transform);
            equipItem.SetInfo(item, Define.UI_ItemParentType.GachaResultPopup);
        }
    }

    void OnClickConfirmButton()
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);
    }

    [SerializeField] private GameObject _particle;

    public void PlayParticle()
    {
        _particle.SetActive(true);
        StartCoroutine(CoSkill());
    }

    IEnumerator CoSkill()
    {
        yield return new WaitForSeconds(2.5f);
        OnClickSkipButton();
    }
}
