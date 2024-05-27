using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_PausePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ResumeButton,
        HomeButton,
        StatisticsButton,
        SoundButton,
        SettingButton,
    }
    enum Texts
    {
        PauseTitleText,
        ResumeButtonText,
    }
    #endregion


    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
        GetButton((int)Buttons.HomeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoundButton).gameObject.BindEvent(OnClickSoundButton);
        GetButton((int)Buttons.SoundButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();

        return true;
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    void OnClickResumeButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickHomeButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_BackToHomePopup>();
    }

    void OnClickSettingButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    void OnClickSoundButton()
    {
        Managers.Sound.PlayButtonClick();
    }

    void OnClickStatisticsButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
    }
}
