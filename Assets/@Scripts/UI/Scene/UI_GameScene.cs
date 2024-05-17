using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    public void DoWhiteFlash()
    {
        StartCoroutine(CoWhiteScreen());
    }

    IEnumerator CoWhiteScreen()
    {
        Color targetColor = new Color(1, 1, 1, 1f);

        yield return null;
        DG.Tweening.Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(targetColor, 0.3f))
            .Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(Color.clear, 0.3f)).OnComplete(() => { });
    }

    void OnClickPauseButton()
    {
        Managers.Sound.PlayButtonClick();
    }
}
