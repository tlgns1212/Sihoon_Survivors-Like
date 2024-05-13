using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

using static Define;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; } }
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();
    public List<SupportSkillData> LockedSupportSkills { get; } = new List<SupportSkillData>();
    public List<SupportSkillData> SupportSkills = new List<SupportSkillData>();

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }
    [SerializeField]
    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<Define.SkillType, int>();

    public event Action UpdateSkillUI;
    public ObjectType _ownerType;

    public void Awake()
    {
        _ownerType = GetComponent<CreatureController>().ObjectType;
    }

    public void Init()
    {

    }

    public void LoadSkill(Define.SkillType skillType, int level)
    {
        // 모든 스킬은 0으로 시작함, 레벨 수 만큼 레벨업 하기
        AddSkill(skillType);
        for (int i = 0; i < level; i++)
        {
            LevelUpSkill(skillType);
        }
    }

    public void AddSkill(Define.SkillType skillType, int skillId = 0)
    {
        string className = skillType.ToString();

        if (skillType == SkillType.FrozenHeart || skillType == SkillType.SavageSmash || skillType == SkillType.EletronicField)
        {
            GameObject go = Managers.Resource.Instantiate(skillType.ToString(), gameObject.transform);
            if (go != null)
            {
                SkillBase skill = go.GetOrAddComponent<SkillBase>();
                SkillList.Add(skill);
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = skill.Level;
                }
                else
                {
                    SavedBattleSkill.Add(skillType, skill.Level);
                }
            }
        }
        else
        {
            // AddComponent만 하면 됨
            SequenceSkill skill = gameObject.AddComponent(Type.GetType(className)) as SequenceSkill;
            if (skill != null)
            {
                skill.ActivateSkill();
                skill.Owner = GetComponent<CreatureController>();
                skill.DataId = skillId;
                SkillList.Add(skill);
                SequenceSkills.Add(skill);
            }
            else
            {
                RepeatSkill skillBase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
                SkillList.Add(skillBase);
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = skillBase.Level;
                }
                else
                {
                    SavedBattleSkill.Add(skillType, skillBase.Level);
                }
            }
        }
    }

    public void AddActivatedSkills(SkillBase skill)
    {
        ActivatedSkills.Add(skill);
    }

    int _sequenceIndex = 0;
    bool _stopped = false;

    public void StartNextSequenceSkill()
    {
        if (_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    void OnFinishedSequenceSkill()
    {
        _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    public void StopSkills()
    {
        _stopped = true;

        foreach (var skill in ActivatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }

    public void AddSupportSkill(SupportSkillData skill, bool isLoadSkill = false)
    {
        #region 서포트 스킬 중복 가능
        skill.IsPurchased = true;

        // 1. 스킬 등록 없이 바로 끝내는 것들
        #endregion
    }

    public void LevelUpSkill(Define.SkillType skillType)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillType == skillType)
            {
                SkillList[i].OnLevelUp();
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = SkillList[i].Level;
                }
                UpdateSkillUI?.Invoke();
            }
        }
    }

}
