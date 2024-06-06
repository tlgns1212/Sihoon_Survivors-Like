using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CheckOutPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        CheckOutProgressSliderObject,
        CheckOutBoardObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        CheckOutPopupTitleText,
        
        FirstClearRewardCountText,
        SecondClearRewardCountText,
        ThirdClearRewardCountText,
        DaysCountText,
        CheckOutDescriptionText,
    }

    enum Images
    {
        FirstClearRewardBackgroundImage,
        SecondClearRewardBackgroundImage,
        ThirdClearRewardBackgroundImage,
        
        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage,
    }
    #endregion

    public int UserCheckOutDay;
    private int _monthlyCount;
    private int _dailyCount;
    private Transform _makeSubItemParents;

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
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(false);

        Refresh();
        return true;
    }

    public void SetInfo(int checkOutDay)
    {
        UserCheckOutDay = checkOutDay;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (UserCheckOutDay == 0)
            return;

        _monthlyCount = UserCheckOutDay % 30;
        _dailyCount = _monthlyCount % 10;

        if (_dailyCount == 0)
        {
            _dailyCount = 10;
        }
        
        // 10일 보드판 초기화
        GetObject((int)GameObjects.CheckOutBoardObject).DestroyChilds();
        _makeSubItemParents = GetObject((int)GameObjects.CheckOutBoardObject).transform;
        // _dailyCount 수에 따라 SetInfo에 trueㄱ밧을 넘겨줌
        for (int count = 1; count <= 10; count++)
        {
            UI_CheckOutItem item = Managers.UI.MakeSubItem<UI_CheckOutItem>(_makeSubItemParents);
            item.transform.SetAsLastSibling();
            if (_dailyCount >= count)
            {
                item.SetInfo(count, true);
            }
            else
            {
                item.SetInfo(count, false);
            }
        }
        
        // 갱신 보상 초기화
        if (_monthlyCount >= 10 && _monthlyCount < 20) // 10일
        {
            GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true);
        }
        else if (_monthlyCount >= 20 && _monthlyCount < 30) // 20일
        {
            GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(true);
        }
        else if (_monthlyCount >= 30)
        {
            GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(true);
        }

        GetText((int)Texts.DaysCountText).text = $"{_monthlyCount}일";
        GetObject((int)GameObjects.CheckOutProgressSliderObject).GetComponent<Slider>().value = _monthlyCount;
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayButtonClick();
        UserCheckOutDay = 0;
        Managers.UI.ClosePopupUI(this);
    }
}
