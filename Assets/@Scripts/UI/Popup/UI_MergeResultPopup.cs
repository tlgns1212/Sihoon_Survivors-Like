using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UI_MergeResultPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        ImproveAtk,
        ImproveHp,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        MergeResultCommentText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EnforceValueText,
        ImproveLevelText,
        BeforeLevelValueText,
        AfterLevelValueText,
        ImproveAtkText,
        BeforeAtkValueText,
        AfterAtkValueText,
        ImproveHpText,
        BeforeHpValueText,
        AfterHpValueText,
        ImproveOptionValueText,
        EquipmentGradeValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
    }
    #endregion
    
    private Equipment _beforeEquipment;
    private Equipment _afterEquipment;
    private Action _closeAction;
    
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        Managers.Sound.Play(Define.Sound.Effect, "Result_CommonMerge");
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        Refresh();
        return true;
    }

    public void SetInfo(Equipment beforeEquipment, Equipment afterEquipment, Action callback = null)
    {
        _beforeEquipment = beforeEquipment;
        _afterEquipment = afterEquipment;
        _closeAction = callback;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (_beforeEquipment == null)
            return;
        if (_afterEquipment == null)
            return;

        #region 기본 정보

        Sprite spr = Managers.Resource.Load<Sprite>(_afterEquipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipmentImage).sprite = spr;
        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_afterEquipment.EquipmentData.EquipmentType}_Icon.sprite");

        GetText((int)Texts.EquipmentNameValueText).text = $"{_afterEquipment.EquipmentData.NameTextId}";
        GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{_beforeEquipment.Level}";
        #endregion

        #region 등급 대응 + 추가 옵션

        switch (_afterEquipment.EquipmentData.EquipmentGrade)
        {
            case Define.EquipmentGrade.Uncommon:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetText((int)Texts.EquipmentGradeValueText).text = "고급";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.UncommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                int uncommonGradeSkillId = _afterEquipment.EquipmentData.UncommonGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[uncommonGradeSkillId].Description}";
                break;
            case Define.EquipmentGrade.Rare:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetText((int)Texts.EquipmentGradeValueText).text = "희귀";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.RareNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                int rareGradeSkillId = _afterEquipment.EquipmentData.RareGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[rareGradeSkillId].Description}";
                break;
            case Define.EquipmentGrade.Epic:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.EpicBg;
                int epicGradeSkillId = _afterEquipment.EquipmentData.EpicGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[epicGradeSkillId].Description}";
                break;
            case Define.EquipmentGrade.Epic1:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                break;
            case Define.EquipmentGrade.Epic2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                break;
            case Define.EquipmentGrade.Legendary:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetText((int)Texts.EquipmentGradeValueText).text = "전설";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                int legendaryGradeSkillId = _afterEquipment.EquipmentData.LegendaryGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[legendaryGradeSkillId].Description}";
                break;
            case Define.EquipmentGrade.Legendary1:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "전설 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                break;
            case Define.EquipmentGrade.Legendary2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "전설 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                break;
            case Define.EquipmentGrade.Legendary3:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "전설 3";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                break;
            default:
                break;
        }
        #endregion

        string gradeName = _afterEquipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;
        
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetText((int)Texts.EnforceValueText).text = "";
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.EnforceValueText).text = num.ToString();
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }

        #region 옵션

        GetText((int)Texts.BeforeLevelValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}";
        GetText((int)Texts.AfterLevelValueText).text = $"{Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel}";

        if (_beforeEquipment.EquipmentData.AtkDmgBonus != 0)
        {
            GetObject((int)GameObjects.ImproveAtk).SetActive(true);
            GetObject((int)GameObjects.ImproveHp).SetActive(false);
            
            GetText((int)Texts.BeforeAtkValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}";
            GetText((int)Texts.AfterAtkValueText).text = $"{Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel}";
        }
        else
        {
            GetObject((int)GameObjects.ImproveAtk).SetActive(false);
            GetObject((int)GameObjects.ImproveHp).SetActive(true);
            
            GetText((int)Texts.BeforeHpValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}";
            GetText((int)Texts.AfterHpValueText).text = $"{Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel}";
        }
        #endregion
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        _closeAction?.Invoke();
        gameObject.SetActive(false);
    }
}
