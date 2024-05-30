using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipItem : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        EquipItemRedDotObject,
        NewTextObject,
        EquippedObject,
        SelectObject,
        LockObject,
        SpecialImage,
        GetEffectObject,
    }

    enum Texts
    {
        EquipItemLevelValueText,
        EnforceValueText,
    }

    enum Images
    {
        EquipItemGradeBackgroundImage,
        EquipItemImage,
        EquipItemEnforceBackgroundImage,
        EquipItemTypeBackgroundImage,
        EquipItemTypeImage,
    }
    #endregion

    public Equipment Equipment;
    private ScrollRect _scrollRect;
    private Define.UI_ItemParentType _parentType;
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
        
        GetObject((int)GameObjects.GetEffectObject).SetActive(false);
        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);
        gameObject.BindEvent(OnClickEquipItemButton);

        return true;
    }

    public void SetInfo(Equipment item, Define.UI_ItemParentType parentType, ScrollRect scrollRect = null)
    {
        Equipment = item;
        transform.localScale = Vector3.one;
        _scrollRect = scrollRect;
        _parentType = parentType;

        #region 색상 변경
        // EquipItemGradeBackgroundImage : 합성할 장비 등급의 테두리
        // EquipItemEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 이미지 색깔 변경
        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Common:
                GetImage((int)Images.EquipItemGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.EquipItemTypeBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.EquipmentGrade.Uncommon:
                GetImage((int)Images.EquipItemGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipItemTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.EquipmentGrade.Rare:
                GetImage((int)Images.EquipItemGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.EquipItemTypeBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.EquipmentGrade.Epic:
            case Define.EquipmentGrade.Epic1:
            case Define.EquipmentGrade.Epic2:
                GetImage((int)Images.EquipItemGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipItemEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipItemTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case Define.EquipmentGrade.Legendary:
            case Define.EquipmentGrade.Legendary1:
            case Define.EquipmentGrade.Legendary2:
            case Define.EquipmentGrade.Legendary3:
                GetImage((int)Images.EquipItemGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipItemEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipItemTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case Define.EquipmentGrade.Myth:
            case Define.EquipmentGrade.Myth1:
            case Define.EquipmentGrade.Myth2:
            case Define.EquipmentGrade.Myth3:
                break;
            default:
                break;
        }
        #endregion

        #region 유일 + 1 등의 등급 벨류

        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;
        
        // Epic1 -> 1 리턴, Epic2 -> 2 리턴, Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetText((int)Texts.EnforceValueText).text = "";
            GetImage((int)Images.EquipItemEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.EnforceValueText).text = num.ToString();
            GetImage((int)Images.EquipItemEnforceBackgroundImage).gameObject.SetActive(true);
        }
        #endregion
        
        // EquipItemImage : 장비의 아이콘
        Sprite spr = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipItemImage).sprite = spr;
        // EquipItemTypeImage : 장비 타입 아이콘
        Sprite type = Managers.Resource.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage((int)Images.EquipItemTypeImage).sprite = type;
        // EquipItemLevelValueText : 장비의 현재 레벨
        GetText((int)Texts.EquipItemLevelValueText).text = $"Lv.{Equipment.Level}";
        // EquipItemRedDotObject : 장비가 강화가 가능할 때 출력
        GetObject((int)GameObjects.EquipItemRedDotObject).SetActive(Equipment.IsUpgradeable);
        // NewTextObject : 장비를 처음 습득했을 때 출력
        GetObject((int)GameObjects.NewTextObject).SetActive(!Equipment.IsConfirmed);
        // EquippedObject : 합성 팝업에서 착용 장비 표시용
        GetObject((int)GameObjects.EquippedObject).SetActive(Equipment.IsEquipped);
        // SelectObject : 합성 팝업에서 장비 선택 표시용
        GetObject((int)GameObjects.SelectObject).SetActive(Equipment.IsSelected);
        // LockObject : 합성 팝업에서 선택 불가 표시용
        GetObject((int)GameObjects.LockObject).SetActive(Equipment.IsUnavailable);
        // Special 아이템 일 때
        bool isSpecial = Equipment.EquipmentData.GachaRarity == Define.GachaRarity.Special ? true : false;
        // 캐릭터 장착중인 슬롯에 있으면 "착용중" 오브젝트 끔
        GetObject((int)GameObjects.SpecialImage).SetActive(isSpecial);
        if (parentType == Define.UI_ItemParentType.CharacterEquipmentGroup)
        {
            GetObject((int)GameObjects.EquippedObject).SetActive(false);
        }
        // 가챠 결과 팝업 획득 효과
        if (_parentType == Define.UI_ItemParentType.GachaResultPopup)
        {
            GetObject((int)GameObjects.GetEffectObject).SetActive(true);
        }
    }

    void OnClickEquipItemButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_isDrag) return;
        if (_parentType == Define.UI_ItemParentType.GachaResultPopup)
        {
            Managers.UI.ShowPopupUI<UI_GachaEquipmentInfoPopup>().SetInfo(Equipment);
        }
        else
        {
            Equipment.IsConfirmed = true;
            Managers.Game.SaveGame();
            
            // 해당 장비의 장비 정보 팝업을 호출
            // UI_EquipmentInfoPopup infoPopup = (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI;
            // if (infoPopup != null)
            // {
            //     infoPopup.SetInfo(Equipment);
            //     infoPopup.gameObject.SetActive(true);
            // }
        }
    }

    void OnDrag(BaseEventData baseEventData)
    {
        if (_parentType == Define.UI_ItemParentType.GachaResultPopup) return;
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }
    void OnBeginDrag(BaseEventData baseEventData)
    {
        if (_parentType == Define.UI_ItemParentType.GachaResultPopup) return;
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }
    void OnEndDrag(BaseEventData baseEventData)
    {
        if (_parentType == Define.UI_ItemParentType.GachaResultPopup) return;
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
}
