using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSlotItem : UI_Base
{
    #region Enums
    enum SkillLevelObjects
    {
        SkillLevel_0,
        SkillLevel_1,
        SkillLevel_2,
        SkillLevel_3,
        SkillLevel_4,
        SkillLevel_5,
    }
    enum Images
    {
        BattleSkillImage
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(SkillLevelObjects));
        BindImage(typeof(Images));
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

        return true;
    }

    public void SetInfo(string iconLabel, int skillLevel = 1)
    {
        GetImage((int)Images.BattleSkillImage).sprite = Managers.Resource.Load<Sprite>(iconLabel);

        // 별 모두 끄기
        for (int i = 0; i < 6; i++)
        {
            GetObject(i).SetActive(false);
        }

        // 스킬 레벨만큼 별 켜기
        for (int i = 0; i < skillLevel; i++)
        {
            GetObject(i).SetActive(true);
        }
    }
}
