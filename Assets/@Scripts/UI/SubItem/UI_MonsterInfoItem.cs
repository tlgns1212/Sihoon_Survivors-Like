using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UI_MonsterInfoItem : UI_Base
{
    #region Enum

    enum Buttons
    {
        MonsterInfoButton,
    }

    enum Texts
    {
        MonsterLevelValueText,
    }

    enum Images
    {
        MonsterImage,
    }
    #endregion

    private CreatureData _creature;
    private Transform _makeSubItemParents;
    private int _level;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.MonsterInfoButton).gameObject.BindEvent(OnClickMonsterInfoButton);

        return true;
    }

    public void SetInfo(int monsterId, int level, Transform makeSubItemParents)
    {
        _makeSubItemParents = makeSubItemParents;
        transform.localScale = Vector3.one;

        if (Managers.Data.CreatureDic.TryGetValue(monsterId, out _creature))
        {
            _level = level;
        }

        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (_creature == null)
            return;

        GetText((int)Texts.MonsterLevelValueText).text = $"Lv.{_level}";
        GetImage((int)Images.MonsterImage).sprite = Managers.Resource.Load<Sprite>(_creature.IconLabel);
    }

    void OnClickMonsterInfoButton()
    {
        Managers.Sound.PlayButtonClick();

        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = gameObject.GetComponent<RectTransform>();
        RectTransform parentCanvas = _makeSubItemParents.GetComponent<RectTransform>();
        item.SetInfo(_creature, targetPos, parentCanvas);
        item.transform.SetAsLastSibling();
    }
}
