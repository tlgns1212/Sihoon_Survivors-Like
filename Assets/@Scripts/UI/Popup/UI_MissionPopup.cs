using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UI_MissionPopup : UI_Popup
{
    // TODO : 미션 엄청 많이 할 게 있음, prefab도 일부만 가져왔으니 더 가져올것

    #region Enum

    enum GameObjects
    {
        ContentObject,
        DailyMissionContentObject,
        DailyMissionScrollObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        DailyMissionTitleText,
        DailyMissionCommentText,
    }

    enum Images
    {
        GradientImage,
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
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        
        GetObject((int)GameObjects.DailyMissionScrollObject).DestroyChilds();
        foreach (KeyValuePair<int,MissionData> data in Managers.Data.MissionDataDic)
        {
            if (data.Value.MissionType == Define.MissionType.Daily)
            {
                UI_MissionItem dailyMission = Managers.UI.MakeSubItem<UI_MissionItem>(GetObject((int)GameObjects.DailyMissionScrollObject).transform);
                dailyMission.SetInfo(data.Value);
            }
        }
    }

    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
