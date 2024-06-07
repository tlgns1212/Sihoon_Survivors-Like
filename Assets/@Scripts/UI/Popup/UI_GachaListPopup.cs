using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class UI_GachaListPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        GachaInfoContentObject,
        CommonGachaGradeRateItem,
        CommonGachaRateListObject,
        UncommonGachaGradeRateItem,
        UncommonGachaRateListObject,
        RareGachaGradeRateItem,
        RareGachaRateListObject,
        EpicGachaGradeRateItem,
        EpicGachaRateListObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        GachaListPopupTitleText,
        CommonGradeTitleText,
        CommonGradeRateValueText,
        UncommonGradeTitleText,
        UncommonGradeRateValueText,
        RareGradeTitleText,
        RareGradeRateValueText,
        EpicGradeTitleText,
        EpicGradeRateValueText,
    }
    #endregion

    private Define.GachaType _gachaType;

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
        Refresh();
        return true;
    }

    public void SetInfo(Define.GachaType gachaType)
    {
        _gachaType = gachaType;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_gachaType == Define.GachaType.None)
            return;

        float commonRate = 0f;
        float uncommonRate = 0f;
        float rareRate = 0f;
        float epicRate = 0f;
        
        GetObject((int)GameObjects.CommonGachaRateListObject).DestroyChilds();
        GetObject((int)GameObjects.UncommonGachaRateListObject).DestroyChilds();
        GetObject((int)GameObjects.RareGachaRateListObject).DestroyChilds();
        GetObject((int)GameObjects.EpicGachaRateListObject).DestroyChilds();

        List<GachaRateData> list = Managers.Data.GachaTableDataDic[_gachaType].GachaRateTable.ToList();
        list.Reverse();

        foreach (GachaRateData item in Managers.Data.GachaTableDataDic[_gachaType].GachaRateTable)
        {
            switch (Managers.Data.EquipDataDic[item.EquipmentId].EquipmentGrade)
            {
                case Define.EquipmentGrade.Common:
                    commonRate += item.GachaRate;
                    UI_GachaRateItem commonItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    commonItem.transform.SetParent(GetObject((int)GameObjects.CommonGachaRateListObject).transform);
                    commonItem.SetInfo(item);
                    break;
                case Define.EquipmentGrade.Uncommon:
                    uncommonRate += item.GachaRate;
                    UI_GachaRateItem uncommonItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    uncommonItem.transform.SetParent(GetObject((int)GameObjects.CommonGachaRateListObject).transform);
                    uncommonItem.SetInfo(item);
                    break;
                case Define.EquipmentGrade.Rare:
                    rareRate += item.GachaRate;
                    UI_GachaRateItem rareItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    rareItem.transform.SetParent(GetObject((int)GameObjects.CommonGachaRateListObject).transform);
                    rareItem.SetInfo(item);
                    break;
                case Define.EquipmentGrade.Epic:
                    epicRate += item.GachaRate;
                    UI_GachaRateItem epicItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    epicItem.transform.SetParent(GetObject((int)GameObjects.CommonGachaRateListObject).transform);
                    epicItem.SetInfo(item);
                    break;
                default:
                    break;
            }
        }

        GetText((int)Texts.CommonGradeRateValueText).text = commonRate.ToString("P2");
        GetText((int)Texts.UncommonGradeRateValueText).text = uncommonRate.ToString("P2");
        GetText((int)Texts.RareGradeRateValueText).text = rareRate.ToString("P2");
        GetText((int)Texts.EpicGradeRateValueText).text = epicRate.ToString("P2");
        gameObject.SetActive(true);
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);
    }
}
