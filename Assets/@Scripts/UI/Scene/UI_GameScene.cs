using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        ExpSliderObject,
        WaveObject,


        OnDamaged,
        WhiteFlash,
    }
    enum Buttons
    {
        PauseButton,

    }
    enum Texts
    {
        TimeLimitValueText,
        WaveValueText,
        KillValueText,
        CharacterLevelValueText,
    }
    enum Images
    {

    }
    #endregion

    GameManager _game;
    Coroutine _coWaveAlarm;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindObject(typeof(Buttons));
        BindObject(typeof(Texts));
        BindObject(typeof(Images));

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        GetButton((int)Buttons.PauseButton).GetOrAddComponent<UI_ButtonAnimation>();

        #endregion

        _game = Managers.Game;

        return true;
    }

    private void Awake()
    {
        Init();
        Refresh();
    }

    private void OnDestroy()
    {
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.CharacterLevelValueText).text = "1";
        GetText((int)Texts.KillValueText).text = "1";

        // 왜 하는지는 모르겠지만 웨이브 리프레시 버그 대응이라고 함
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.WaveObject).GetComponent<RectTransform>());
    }

    void OnClickPauseButton()
    {
        Managers.Sound.PlayButtonClick();
    }
}
