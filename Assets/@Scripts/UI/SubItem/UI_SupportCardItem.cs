using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SupportCardItem : UI_Base
{
    #region UI 기능 리스트
    // 정보 갱신
    // CardNameText : 서포트 스킬 이름
    // SupportSkillImage : 서포트 스킬 아이콘
    // TargetDescriptionText : 서포트 스킬 설명
    // SoulValueText : 서포트 스킬 코스트
    // SoldOutObject : 서포트 카드 구매 완료시 활성화 
    // LockToggle : 토글이 활성화 되었다면 서포트 스킬이 변경되지 않음(잠금을 해제 할때까지 유지)

    // 로컬라이징
    // LockToggleText : 잠금
    #endregion

    #region Enum
    enum GameObjects
    {
        SoldOutObject,
    }
    enum Toggles
    {
        LockToggle,
    }
    enum Images
    {
        SupportSkillImage,
        SupportSkillCardBackgroundImage,
        SupportCardTitleImage,
    }
    enum Texts
    {
        CardNameText,
        SoulValueText,
        LockTargetText,
        SkillDescriptionText,
    }
    #endregion

    SupportSkillData _supportSkillData;
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
        BindToggle(typeof(Toggles));

        GetToggle((int)Toggles.LockToggle).gameObject.BindEvent(OnClickLockToggle);
        gameObject.BindEvent(OnClickBuy);
        GetToggle((int)Toggles.LockToggle).GetOrAddComponent<UI_ButtonAnimation>();

        return true;
    }

    public void SetInfo(Data.SupportSkillData supportSkill)
    {
        transform.localScale = Vector3.one;
        _supportSkillData = supportSkill;
        GetObject((int)GameObjects.SoldOutObject).SetActive(false);

        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.CardNameText).text = _supportSkillData.Name;
        GetText((int)Texts.SkillDescriptionText).text = _supportSkillData.Description;
        GetText((int)Texts.SoulValueText).text = _supportSkillData.Price.ToString();
        GetImage((int)Images.SupportSkillImage).sprite = Managers.Resource.Load<Sprite>(_supportSkillData.IconLabel);
        GetObject((int)GameObjects.SoldOutObject).SetActive(_supportSkillData.IsPurchased);
        GetToggle((int)Toggles.LockToggle).isOn = _supportSkillData.IsLocked;

        switch (_supportSkillData.SupportSkillGrade)
        {
            case Define.SupportSkillGrade.Common:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.CommonNameColor;
                break;
            case Define.SupportSkillGrade.Uncommon:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.UncommonNameColor;
                break;
            case Define.SupportSkillGrade.Rare:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.RareNameColor;
                break;
            case Define.SupportSkillGrade.Epic:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.EpicNameColor;
                break;
            case Define.SupportSkillGrade.Legend:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.LegendaryNameColor;
                break;
            default:
                break;
        }
    }

    void OnClickLockToggle()
    {
        Managers.Sound.PlayButtonClick();
        if (_supportSkillData.IsPurchased)
            return;
        if (GetToggle((int)Toggles.LockToggle).isOn == true)
        {
            _supportSkillData.IsLocked = true;
            Managers.Game.Player.Skills.LockedSupportSkills.Add(_supportSkillData);
        }
        else
        {
            _supportSkillData.IsLocked = false;
            Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkillData);
        }
    }

    void OnClickBuy()
    {
        if (GetObject((int)GameObjects.SoldOutObject).activeInHierarchy == true)
            return;
        if (Managers.Game.Player.SoulCount >= _supportSkillData.Price)
        {
            Managers.Game.Player.SoulCount -= _supportSkillData.Price;

            if (Managers.Game.Player.Skills.LockedSupportSkills.Contains(_supportSkillData))
            {
                Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkillData);
            }

            Managers.Game.Player.Skills.AddSupportSkill(_supportSkillData);
            GetObject((int)GameObjects.SoldOutObject).SetActive(true);
        }
    }
}
