using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckOutItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        ClearRewardCompleteObject,
    }

    enum Texts
    {
        DayValueText,
        RewardItemCountValueText,
    }

    enum Images
    {
        RewardItemBackgroundImage,
        RewardItemImage,
    }
    #endregion

    private int _dayCount;
    private bool _isCheckOut;

    private void OnEnable()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetObject((int)GameObjects.ClearRewardCompleteObject).gameObject.SetActive(false);

        Refresh();
        return true;
    }

    public void SetInfo(int dayCount, bool isCheckOut)
    {
        transform.localScale = Vector3.one;

        _dayCount = dayCount;
        _isCheckOut = isCheckOut;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (_dayCount == 0)
            return;

        int rewardMaterialId = Managers.Data.CheckOutDataDic[_dayCount].RewardItemId;
        int rewardItemValue = Managers.Data.CheckOutDataDic[_dayCount].MissionTarRewardItemValueGetValue;

        GetText((int)Texts.DayValueText).text = $"{_dayCount}일";
        GetText((int)Texts.RewardItemCountValueText).text = $"{rewardItemValue}";
        GetImage((int)Images.RewardItemImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[rewardMaterialId].SpriteName);
        switch (Managers.Data.MaterialDic[rewardMaterialId].MaterialGrade)
        {
            case Define.MaterialGrade.Common:
                GetImage((int)Images.RewardItemBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.MaterialGrade.Uncommon:
                GetImage((int)Images.RewardItemBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.MaterialGrade.Rare:
                GetImage((int)Images.RewardItemBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.MaterialGrade.Epic:
                GetImage((int)Images.RewardItemBackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case Define.MaterialGrade.Legendary:
                GetImage((int)Images.RewardItemBackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            default:
                break;
        }

        if (_isCheckOut)
        {
            GetObject((int)GameObjects.ClearRewardCompleteObject).gameObject.SetActive(true);

            if (Managers.Game.AttendanceReceived[_dayCount - 1] == false)
            {
                Managers.Game.AttendanceReceived[_dayCount - 1] = true;

                int matId = Managers.Data.CheckOutDataDic[_dayCount].RewardItemId;
                
                string[] spriteNames = new string[1];
                int[] counts = new int[1];

                spriteNames[0] = Managers.Data.MaterialDic[matId].SpriteName;
                counts[0] = Managers.Data.CheckOutDataDic[_dayCount].MissionTarRewardItemValueGetValue;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[matId], Managers.Data.CheckOutDataDic[_dayCount].MissionTarRewardItemValueGetValue);
                rewardPopup.SetInfo(spriteNames, counts);
                Managers.Game.SaveGame();
            }
        }
        else
        {
            GetObject((int)GameObjects.ClearRewardCompleteObject).gameObject.SetActive(false);
        }
    }
}
