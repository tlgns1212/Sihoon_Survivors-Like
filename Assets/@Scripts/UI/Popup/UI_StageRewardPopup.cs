using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StageRewardPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        
        StageScrollContentObject,
        StageRewardProgressSliderObject,
        
        FirstClearRewardUnlockObject,
        SecondClearRewardUnlockObject,
        ThirdClearRewardUnlockObject,
        FirstClearOutlineObject,
        SecondClearOutlineObject,
        ThirdClearOutlineObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }

    enum Buttons
    {
        BackButton,
        
        FirstClearRewardButton,
        SecondClearRewardButton,
        ThirdClearRewardButton,
    }

    enum Texts
    {
        StageRewardTitleText,
        StageRewardDescriptionText,
        
        FirstClearRewardText,
        SecondClearRewardText,
        ThirdClearRewardText,
        
        FirstClearRewardItemCountValueText,
        SecondClearRewardItemCountValueText,
        ThirdClearRewardItemCountValueText,
    }

    enum Images
    {
        FirstClearRewardItemBackgroundImage,
        SecondClearRewardItemBackgroundImage,
        ThirdClearRewardItemBackgroundImage,
        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage,
    }
    #endregion

    private int _stageNum = 1;

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
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.FirstClearRewardButton).gameObject.BindEvent(OnClickFirstClearRewardButton);
        GetButton((int)Buttons.FirstClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondClearRewardButton).gameObject.BindEvent(OnClickSecondClearRewardButton);
        GetButton((int)Buttons.SecondClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ThirdClearRewardButton).gameObject.BindEvent(OnClickThirdClearRewardButton);
        GetButton((int)Buttons.ThirdClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();

        ChapterRewardPopupContentInit();

        Refresh();
        return true;
    }

    void ChapterRewardPopupContentInit()
    {
        GetObject((int)GameObjects.FirstClearRewardUnlockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.FirstClearOutlineObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.SecondClearRewardUnlockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.SecondClearOutlineObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.ThirdClearRewardUnlockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.ThirdClearOutlineObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(false);
    }

    public void SetInfo(int stageNum)
    {
        _stageNum = stageNum;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
    }

    void OnClickBackButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickFirstClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true); // 보상 수령 시 활성화
    }
    void OnClickSecondClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(true); // 보상 수령 시 활성화
    }
    void OnClickThirdClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(true); // 보상 수령 시 활성화
    }
}
