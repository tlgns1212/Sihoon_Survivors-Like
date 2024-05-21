using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelectPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        SkillCardSelectListObject,
        ExpSliderObject,
        DisabledObject,
        CharacterLevelObject,
    }
    enum Buttons
    {
        CardRefreshButton,
        ADRefreshButton,
    }
    enum Texts
    {
        SkillSelectCommentText,
        SkillSelectTitleText,
        CardRefreshText,
        CardRefreshCountValueText,
        ADRefreshText,

        CharacterLevelUpTitleText,
        CharacterLevelValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
    }
    enum Images
    {
        BattleSkillIcon_0,
        BattleSkillIcon_1,
        BattleSkillIcon_2,
        BattleSkillIcon_3,
        BattleSkillIcon_4,
        BattleSkillIcon_5
    }
    #endregion

    GameManager _game;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);
        GetButton((int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickADRefreshButton);

        GetObject((int)GameObjects.DisabledObject).gameObject.SetActive(false);

        _game = Managers.Game;

        Refresh();

        SetRecommendSkills();
        List<SkillBase> activeSkills = Managers.Game.Player.Skills.SkillList.Where(skill => skill.IsLearnedSkill).ToList();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            SetCurrentSkill(i, activeSkills[i]);
        }
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_SkillSelect");
        return true;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SetRecommendSkills();
        }
    }
#endif

    void Refresh()
    {

        GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
        GetText((int)Texts.BeforeLevelValueText).text = $"{_game.Player.Level - 1}";
        GetText((int)Texts.AfterLevelValueText).text = $"{_game.Player.Level}";

        if (_game.Player.SkillRefreshCount > 0)
        {
            GetText((int)Texts.CardRefreshCountValueText).text = $"남은 횟수 : {_game.Player.SkillRefreshCount}";
        }
        else
        {
            GetText((int)Texts.CardRefreshCountValueText).text = $"<color=red>남은 횟수 : 0</color>";
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
    }

    void SetRecommendSkills()
    {
        GameObject container = GetObject((int)GameObjects.SkillCardSelectListObject);

        container.DestroyChilds();
        List<SkillBase> list = _game.Player.Skills.RecommendSkills();

        foreach (SkillBase skill in list)
        {
            // UI_SkillCardItem item = Managers.UI.MakeSubItem<UI_SkillCardItem>(container.transform);
            // item.SetInfo();
        }
    }

    void SetCurrentSkill(int index, SkillBase skill)
    {
        GetImage(index).sprite = Managers.Resource.Load<Sprite>(skill.SkillData.IconLabel);
        GetImage(index).enabled = true;
    }

    void OnClickCardRefreshButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_game.Player.SkillRefreshCount > 0)
        {
            SetRecommendSkills();
            _game.Player.SkillRefreshCount--;
        }
        Refresh();
    }

    void OnClickADRefreshButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_game.SkillRefreshCountAds > 0)
        {
            // Managers.Ads.ShowRewardedAd(() =>
            // {
            //     _game.SkillRefreshCountAds--;
            //     SetRecommendSkills();
            // });
        }
        else
        {
            Managers.UI.ShowToast("더이상 사용할 수 없습니다.");
        }
    }
}
