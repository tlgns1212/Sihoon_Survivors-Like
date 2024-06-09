using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MaterialItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        GetEffectObject,
    }

    enum Buttons
    {
        MaterialInfoButton,
    }

    enum Texts
    {
        ItemCountValueText,
    }

    enum Images
    {
        MaterialItemImage,
        MaterialItemBackgroundImage,
    }
    #endregion

    private Data.MaterialData _materialData;
    private Transform _makeSubItemParents;
    private ScrollRect _scrollRect;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetObject((int)GameObjects.GetEffectObject).SetActive(false);
        
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);
        gameObject.BindEvent(OnClickMaterialInfoButton);
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnClickMaterialInfoButton);

        return true;
    }

    public void SetInfo(string spriteName, int count)
    {
        transform.localScale = Vector3.one;
        GetImage((int)Images.MaterialItemImage).sprite = Managers.Resource.Load<Sprite>(spriteName);
        GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Epic;
        GetText((int)Texts.ItemCountValueText).text = $"{count}";
        GetObject((int)GameObjects.GetEffectObject).SetActive(true);
    }

    public void SetInfo(MaterialData data, Transform makeSubItemParents, int count, ScrollRect scrollRect = null)
    {
        transform.localScale = Vector3.one;
        _scrollRect = scrollRect;
        _makeSubItemParents = makeSubItemParents;
        _materialData = data;

        GetImage((int)Images.MaterialItemImage).sprite = Managers.Resource.Load<Sprite>(_materialData.SpriteName);
        GetText((int)Texts.ItemCountValueText).text = $"{count}";

        switch (_materialData.MaterialGrade)
        {
            case Define.MaterialGrade.Common:
                GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.MaterialGrade.Uncommon:
                GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.MaterialGrade.Rare:
                GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.MaterialGrade.Epic:
            case Define.MaterialGrade.Epic1:
            case Define.MaterialGrade.Epic2:
                GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case Define.MaterialGrade.Legendary:
            case Define.MaterialGrade.Legendary1:
            case Define.MaterialGrade.Legendary2:
            case Define.MaterialGrade.Legendary3:
                GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            default:
                break;
        }
    }

    void OnClickMaterialInfoButton()
    {
        if (_makeSubItemParents == null)
            return;
        Managers.Sound.PlayButtonClick();
        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = gameObject.GetComponent<RectTransform>();
        RectTransform parentCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
        item.SetInfo(_materialData, targetPos, parentCanvas);
        item.transform.SetAsLastSibling();
    }

    #region Scroll

    private void OnDrag(BaseEventData baseEventData)
    {
        if (_scrollRect == null) return;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }
    private void OnBeginDrag(BaseEventData baseEventData)
    {
        if (_scrollRect == null) return;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }
    private void OnEndDrag(BaseEventData baseEventData)
    {
        if (_scrollRect == null) return;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
