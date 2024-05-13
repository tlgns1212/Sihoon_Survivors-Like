using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using static Define;

public class UI_TitleScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        Slider,
    }

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }
    #endregion

    bool isPreload = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        // 오브젝트 바인딩
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;
        // 테스트용
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            if (isPreload)
                Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
        });
        GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
        return true;
    }

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count / totalCount;
            if (count == totalCount)
            {
                isPreload = true;
                GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
                Managers.Data.Init();
                // Managers.Game.Init();
                // Managers.Time.Init();
                StartButtonAnimation();
            }
        });
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
    void StartButtonAnimation()
    {
        GetText((int)Texts.StartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
    }
}
