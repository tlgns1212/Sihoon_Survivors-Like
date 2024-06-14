using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MergeEquipItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        EquipmentRedDotObject,
        NewTextObject,
        EquippedObject,
        SelectObject,
        LockObject,
        SpecialImage,
    }

    enum Texts
    {
        EquipmentLevelValueText,
        EnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
    }
    #endregion

    public Equipment Equipment;
    public Action OnClickEquipItem;
    private ScrollRect _scrollRect;
    private bool _isDrag = false;

    private void Awake()
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
        
        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);
        
        gameObject.BindEvent(OnClickEquipItemButton);
        return true;
    }

    public void SetInfo(Equipment item, Define.UI_ItemParentType parentType, ScrollRect scrollRect, bool isSelected, bool isLock)
    {
        Equipment = item;
        transform.localScale = Vector3.one;
        _scrollRect = scrollRect;

        #region 색상 변경

        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Common:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.EquipmentGrade.Uncommon:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.EquipmentGrade.Rare:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.EquipmentGrade.Epic:
            case Define.EquipmentGrade.Epic1:
            case Define.EquipmentGrade.Epic2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case Define.EquipmentGrade.Legendary:
            case Define.EquipmentGrade.Legendary1:
            case Define.EquipmentGrade.Legendary2:
            case Define.EquipmentGrade.Legendary3:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            default:
                break;
        }
        #endregion

        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        Match match = Regex.Match(gradeName, @"\+$");
        if (match.Success)
        {
            num = int.Parse(match.Value);
        }

        if (num == 0)
        {
            GetText((int)Texts.EnforceValueText).text = "";
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.EnforceValueText).text = num.ToString();
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }


        Sprite spr = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipmentImage).sprite = spr;
        Sprite type = Managers.Resource.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage((int)Images.EquipmentTypeImage).sprite = type;
        GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{Equipment.Level}";
        GetObject((int)GameObjects.EquipmentRedDotObject).SetActive(Equipment.IsUpgradeable);
        GetObject((int)GameObjects.NewTextObject).SetActive(!Equipment.IsConfirmed);
        GetObject((int)GameObjects.EquippedObject).SetActive(Equipment.IsEquipped);
        GetObject((int)GameObjects.SelectObject).SetActive(Equipment.IsSelected);
        bool isSpecial = Equipment.EquipmentData.GachaRarity == Define.GachaRarity.Special ? true : false;
        GetObject((int)GameObjects.SpecialImage).SetActive(isSpecial);
        if (parentType == Define.UI_ItemParentType.CharacterEquipmentGroup)
        {
            GetObject((int)GameObjects.EquippedObject).SetActive(false);
        }
        
        GetObject((int)GameObjects.SelectObject).gameObject.SetActive(isSelected);
        GetObject((int)GameObjects.LockObject).gameObject.SetActive(isLock);
    }

    void Refresh()
    {
        
    }

    void OnClickEquipItemButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_isDrag) return;
        if (!Equipment.IsConfirmed)
        {
            OnClickEquipItem?.Invoke();
        }

        (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI.SetMergeItem(Equipment);
        // // 해당 장비의 장비 정보 팝업을 호출
        // // UI_EquipmentInfoPopup infoPopup = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        // // if (infoPopup != null)
        // // {
        // //     infoPopup.SetInfo(Equipment);
        // // }
    }

    void OnDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }
    void OnBeginDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }
    void OnEndDrag(BaseEventData baseEventData)
    {
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
}
