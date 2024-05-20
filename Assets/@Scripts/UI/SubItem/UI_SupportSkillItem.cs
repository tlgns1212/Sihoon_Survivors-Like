using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SupportSkillItem : UI_Base
{
    #region Enum
    enum Buttons
    {
        SupportSkillButton,
    }
    enum Images
    {
        BackgroundImage,
        SupportSkillImage,
    }
    #endregion

    Data.SupportSkillData _supportSkillData;
    Transform _makeSubItemParents;
    ScrollRect _scrollRect;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnClickSupportSkillItem);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        return true;
    }

    public void SetInfo(Data.SupportSkillData skill, Transform makeSubItemParents, ScrollRect scrollRect)
    {
        transform.localScale = Vector3.one;
        GetImage((int)Images.SupportSkillImage).sprite = Managers.Resource.Load<Sprite>(skill.IconLabel);
        _supportSkillData = skill;
        _makeSubItemParents = makeSubItemParents;
        _scrollRect = scrollRect;

        // 등급에 따른 배경 색상 변경
        switch (skill.SupportSkillGrade)
        {
            case Define.SupportSkillGrade.Common:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.SupportSkillGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.SupportSkillGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.SupportSkillGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case Define.SupportSkillGrade.Legend:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            default:
                break;
        }
    }

    void OnClickSupportSkillItem()
    {
        Managers.Sound.PlayButtonClick();

        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = gameObject.GetComponent<RectTransform>();
        RectTransform parentCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
        item.SetInfo(_supportSkillData, targetPos, parentCanvas);
        item.transform.SetAsLastSibling();
    }

    #region 버튼 스크롤 대응
    public void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }
    public void OnBeginDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }
    public void OnEndDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
