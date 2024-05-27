using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        // LanguageObject,
    }
    enum Buttons
    {
        BackgroundButton,
        SoundEffectOffButton,
        SoundEffectOnButton,
        BackgroundSoundOffButton,
        BackgroundSoundOnButton,
        JoystickFixedOffButton,
        JoystickFixedOnButton,
        // LanguageButton,
        // TermsOfServiceButton,
        // PrivacyPolicyButton,
    }
    enum Texts
    {
        SettingTitleText,
        // UserInfoText,
        // UserIdValueText,
        SoundEffectText,
        BackgroundSoundText,
        JoystickText,
        // LanguageText,
        // LanguageValueText,
        // TermsOfServiceButtonText,
        // PrivacyPolicyButtonText,
        VersionValueText,
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

        GetButton((int)Buttons.SoundEffectOnButton).gameObject.BindEvent(EffectSoundOn);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.BindEvent(EffectSoundOff);

        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.BindEvent(BackgroundSoundOn);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.BindEvent(BackgroundSoundOff);

        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.BindEvent(JoystickFixed);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.BindEvent(JoystickNonFixed);

        GetText((int)Texts.VersionValueText).text = $"버전 : {Application.version}";

        if (Managers.Game.BGMOn == true)
        {
            BackgroundSoundOn();
        }
        else
        {
            BackgroundSoundOff();
        }

        if (Managers.Game.EffectSoundOn == true)
        {
            EffectSoundOn();
        }

        if (Managers.Game.JoystickType == Define.JoystickType.Fixed)
        {
            JoystickFixed();
        }
        else
        {
            JoystickNonFixed();
        }

        Refresh();
        return true;
    }

    void Refresh()
    {

    }

    void EffectSoundOn()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.EffectSoundOn = true;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(false);
    }
    void EffectSoundOff()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.EffectSoundOn = false;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(true);
    }

    void BackgroundSoundOn()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.BGMOn = true;
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(false);
    }
    void BackgroundSoundOff()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.BGMOn = false;
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(true);
    }

    void JoystickFixed()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.JoystickType = Define.JoystickType.Fixed;
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(false);
    }
    void JoystickNonFixed()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.JoystickType = Define.JoystickType.Flexible;
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(true);
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
