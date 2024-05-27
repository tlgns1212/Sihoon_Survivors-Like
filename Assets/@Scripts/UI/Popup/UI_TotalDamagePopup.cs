using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_TotalDamagePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        TotalDamageContentObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }
    enum Texts
    {
        BackgroundText,
        TotalDamagePopupTitleText,
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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickClosePopup);

        Refresh();
        return true;
    }

    void Refresh()
    {

    }

    public void SetInfo()
    {
        GetObject((int)GameObjects.TotalDamageContentObject).DestroyChilds();
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        foreach (SkillBase skill in skillList.FindAll(skill => skill.IsLearnedSkill))
        {
            UI_SkillDamageItem item = Managers.UI.MakeSubItem<UI_SkillDamageItem>(GetObject((int)GameObjects.TotalDamageContentObject).transform);
            item.SetInfo(skill);
            item.transform.localScale = Vector3.one;
        }
    }

    public void OnClickClosePopup()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
