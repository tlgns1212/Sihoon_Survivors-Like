using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UI_GachaRateItem : UI_Base
{
    #region Enum

    enum Texts
    {
        EquipmentNameValueText,
        EquipmentRateValueText,
    }

    enum Images
    {
        BackgroundImage,
    }
    #endregion

    private GachaRateData _gachaRateData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        Refresh();
        return true;
    }

    public void SetInfo(GachaRateData gachaRateData)
    {
        _gachaRateData = gachaRateData;

        Refresh();
        transform.localScale = Vector3.one;
    }

    void Refresh()
    {
        if (_init == false)
            return;

        string weaponName = Managers.Data.EquipDataDic[_gachaRateData.EquipmentId].NameTextId;
        GetText((int)Texts.EquipmentNameValueText).text = weaponName;
        GetText((int)Texts.EquipmentRateValueText).text = _gachaRateData.GachaRate.ToString("P2");
        switch (_gachaRateData.EquipGrade)
        {
            case Define.EquipmentGrade.Common:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.EquipmentGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.EquipmentGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.EquipmentGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
                break;
            default:
                break;
        }
        
    }
}
