using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LearnSkillPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        LearnSkillCommentText,
        SkillDescriptionText,
        CardNameText
    }
    enum Images
    {
        SkillCardBackgroundImage,
        SkillImage,
        StarOn_0,
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        StarOn_5

    }
    #endregion

    SkillBase _skill;

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
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        #endregion
        return true;
    }

    public void SetInfo()
    {
        //배우고있는 스킬 중 하나 레벨업 시켜준다.
        int index = UnityEngine.Random.Range(0, Managers.Game.Player.Skills.ActivatedSkills.Count);
        _skill = Managers.Game.Player.Skills.RecommendDropSkill();
        
        if (_skill != null)
            Managers.Game.Player.Skills.LevelUpSkill(_skill.SkillType);
        else
        {
            //TODO 배울 스킬이 없을땐 고기나 금화 주기?
            Managers.UI.ClosePopupUI(this);
        }
        
        GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(_skill.SkillData.IconLabel);
        GetText((int)Texts.CardNameText).text = _skill.SkillData.Name;
        GetText((int)Texts.SkillDescriptionText).text = _skill.SkillData.Description;
        GetImage((int)Images.StarOn_1).gameObject.SetActive(_skill.Level >= 2);
        GetImage((int)Images.StarOn_2).gameObject.SetActive(_skill.Level >= 3);
        GetImage((int)Images.StarOn_3).gameObject.SetActive(_skill.Level >= 4);
        GetImage((int)Images.StarOn_4).gameObject.SetActive(_skill.Level >= 5);
        GetImage((int)Images.StarOn_5).gameObject.SetActive(_skill.Level >= 6);

        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickBackgroundButton() // 터치하여 닫기 버튼
    {
        Managers.UI.ClosePopupUI(this);
    }
}
