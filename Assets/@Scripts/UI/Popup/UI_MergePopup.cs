using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_MergePopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        SelectedEquipObject,
        OptionResultObject,
        ImproveAtk,
        ImproveHp,
        FirstCostEquipNeedObject,
        FirstCostEquipSelectObject,
        SecondCostEquipNeedObject,
        SecondCostEquipSelectObject,
        MergeAllButtonRedDotObject,
        EquipInventoryScrollContentObject,
        
        MergeStartEffect,
        MergeFinishEffect,
    }

    enum Buttons
    {
        EquipResultButton,
        FirstCostButton,
        SecondCostButton,
        SortButton,
        MergeAllButton,
        MergeButton,
        BackButton,
    }

    enum Texts
    {
        SelectedEquipLevelValueText,
        SelectedEquipEnforceValueText,
        EquipmentNameText,
        BeforeGradeValueText,
        AfterGradeValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
        ImproveLevelText,
        BeforeAtkValueText,
        AfterAtkValueText,
        ImproveHpText,
        BeforeHpValueText,
        AfterHpValueText,
        ImproveOptionValueText,
        FirstCostEquipEnforceValueText,
        FirstSelectEquipLevelValueText,
        FirstSelectEquipEnforceValueText,
        SecondSelectEquipLevelValueText,
        SecondSelectEquipEnforceValueText,
        EquipmentTitleText,
        SortButtonText,
        MergeAllButtonText,
        SelectEquipmentCommentText,
        SelectMergeCommentText,
    }

    enum Images
    {
        MergePossibleOutlineImage,
        SelectedEquipGradeBackgroundImage,
        SelectedEquipImage,
        SelectedEquipEnforceBackgroundImage,
        SelectedEquipTypeBackgroundImage,
        SelectedEquipTypeImage,
        LevelArrowImage,
        AtkArrowImage,
        HpArrowImage,
        FirstCostEquipGradeBackgroundImage,
        FirstCostEquipImage,
        FirstCostEquipBackgroundImage,
        FirstSelectEquipGradeBackgroundImage,
        FirstSelectEquipImage,
        FirstSelectEquipEnforceBackgroundImage,
        FirstSelectEquipTypeBackgroundImage,
        FirstSelectEquipTypeImage,
        SecondCostEquipGradeBackgroundImage,
        SecondCostEquipImage,
        SecondSelectEquipGradeBackgroundImage,
        SecondSelectEquipImage,
        SecondSelectEquipEnforceBackgroundImage,
        SecondSelectEquipTypeBackgroundImage,
        SecondSelectEquipTypeImage
    }
    #endregion
    
    [SerializeField] public ScrollRect ScrollRect;
    private Equipment _equipment;

    private Equipment _mergeEquipment1;
    private Equipment _mergeEquipment2;
    private Define.EquipmentSortType _equipmentSortType;

    private string sortText_Level = "정렬 : 레벨";
    private string sortText_Grade = "정렬 : 등급";

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
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.EquipResultButton).gameObject.BindEvent(OnClickEquipResultButton);
        GetObject((int)GameObjects.SelectedEquipObject).SetActive(false);
        GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false);
        GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false);
        GetObject((int)GameObjects.OptionResultObject).SetActive(false);
        
        GetObject((int)GameObjects.MergeStartEffect).SetActive(false);
        GetObject((int)GameObjects.MergeFinishEffect).SetActive(false);
        
        GetButton((int)Buttons.FirstCostButton).gameObject.BindEvent(OnClickFirstCostButton);
        GetObject((int)GameObjects.FirstCostEquipNeedObject).SetActive(false);
        GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(false);
        
        GetButton((int)Buttons.SecondCostButton).gameObject.BindEvent(OnClickSecondCostButton);
        GetObject((int)GameObjects.SecondCostEquipNeedObject).SetActive(false);
        GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(false);
        
        GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
        GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeAllButton).gameObject.BindEvent(OnClickMergeAllButton);
        GetButton((int)Buttons.MergeAllButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        // 정렬 기준 (레벨 default)
        _equipmentSortType = Define.EquipmentSortType.Level;
        GetText((int)Texts.SortButtonText).text = sortText_Level;
        
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
        GetObject((int)GameObjects.MergeAllButtonRedDotObject).SetActive(false);
        
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();
        return true;
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;
        _mergeEquipment1 = null;
        _mergeEquipment2 = null;
        Refresh();
    }

    public void SetMergeItem(Equipment equipment, bool showUI = true)
    {
        if (equipment.IsEquipped == true) return;
        if (equipment.Level > 1) return;
        if (_equipment == null)
        {
            _equipment = equipment;
            if (showUI)
            {
                Refresh_SelectedEquipObject();
                SortEquipments();
            }

            return;
        }

        if (_equipment == equipment) return;
        if (_equipment.EquipmentData.EquipmentType != equipment.EquipmentData.EquipmentType) return;
        if (equipment.Equals(_mergeEquipment1)) return;
        if (equipment.Equals(_mergeEquipment2)) return;

        if (_mergeEquipment1 == null)
        {
            if (_equipment.EquipmentData.MergeEquipmentType1 == Define.MergeEquipmentType.ItemCode)
            {
                if (equipment.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment1) return;
            }
            else if (_equipment.EquipmentData.MergeEquipmentType1 == Define.MergeEquipmentType.Grade)
            {
                if (equipment.EquipmentData.EquipmentGrade != (Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), _equipment.EquipmentData.MergeEquipment1))
                    return;
            }
            else return;

            _mergeEquipment1 = equipment;
            if (showUI)
            {
                Refresh_MergeEquip1();
            }
        }
        else if (_mergeEquipment2 == null)
        {
            if (_equipment.EquipmentData.MergeEquipmentType2 == Define.MergeEquipmentType.ItemCode)
            {
                if (equipment.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment2) return;
            }
            else if (_equipment.EquipmentData.MergeEquipmentType2 == Define.MergeEquipmentType.Grade)
            {
                if (equipment.EquipmentData.EquipmentGrade != (Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), _equipment.EquipmentData.MergeEquipment2))
                    return;
            }
            else return;

            _mergeEquipment2 = equipment;
            if (showUI)
            {
                Refresh_MergeEquip2();
            }
        }
        else return;

        if (showUI)
        {
            CheckEnableMergeButton();
        }

        SortEquipments();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        
        Refresh_SelectedEquipObject();
        Refresh_MergeEquip1();
        Refresh_MergeEquip2();
        CheckEnableMergeButton();
        SortEquipments();
    }

    void Refresh_SelectedEquipObject()
    {
        if (_equipment == null)
        {
            // 합성할 아이템 비었을 때
            GetObject((int)GameObjects.SelectedEquipObject).SetActive(false);
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(true);
            GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(true);
            GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false);
            GetObject((int)GameObjects.OptionResultObject).SetActive(false);
            GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
            GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
            return;
        }
        else
        {
            GetImage((int)Images.SelectedEquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
            GetImage((int)Images.SelectedEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_equipment.EquipmentData.EquipmentType}_Icon.sprite");

            switch (_equipment.EquipmentData.EquipmentGrade)
            {
                case Define.EquipmentGrade.Common:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
                    break;
                case Define.EquipmentGrade.Uncommon:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    break;
                case Define.EquipmentGrade.Rare:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
                    break;
                case Define.EquipmentGrade.Epic:
                case Define.EquipmentGrade.Epic1:
                case Define.EquipmentGrade.Epic2:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                    GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                    break;
                case Define.EquipmentGrade.Legendary:
                case Define.EquipmentGrade.Legendary1:
                case Define.EquipmentGrade.Legendary2:
                case Define.EquipmentGrade.Legendary3:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    break;
                default:
                    break;
            }
            string gradeName = _equipment.EquipmentData.EquipmentGrade.ToString();
            int num = 0;

            // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
            Match match = Regex.Match(gradeName, @"\d+$");
            if (match.Success)
                num = int.Parse(match.Value);

            if (num == 0)
            {
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "";
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText((int)Texts.SelectedEquipEnforceValueText).text = num.ToString();
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
            }

            GetText((int)Texts.SelectedEquipLevelValueText).text = $"Lv.{_equipment.Level}";
            GetObject((int)GameObjects.SelectedEquipObject).SetActive(true);
            
            GetObject((int)GameObjects.OptionResultObject).SetActive(false);
            GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false);
            GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(true);
        }

        if (_equipment.EquipmentData.MergeEquipmentType1 == Define.MergeEquipmentType.None)
        {
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(false);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(false);
        }
        else if (_equipment.EquipmentData.MergeEquipmentType2 == Define.MergeEquipmentType.None)
        {
            // 강화석 한개
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(false);
        }
        else
        {
            // 두개
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(true);
        }
    }

    void Refresh_MergeEquip1()
    {
        if (_mergeEquipment1 == null)
        {
            GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(false);
        }
        else
        {
            GetImage((int)Images.FirstSelectEquipImage).sprite = Managers.Resource.Load<Sprite>(_mergeEquipment1.EquipmentData.SpriteName);
            GetImage((int)Images.FirstSelectEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_mergeEquipment1.EquipmentData.EquipmentType}_Icon.sprite");
            switch (_mergeEquipment1.EquipmentData.EquipmentGrade)
            {
                case Define.EquipmentGrade.Common:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
                    break;
                case Define.EquipmentGrade.Uncommon:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    break;
                case Define.EquipmentGrade.Rare:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
                    break;
                case Define.EquipmentGrade.Epic:
                case Define.EquipmentGrade.Epic1:
                case Define.EquipmentGrade.Epic2:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                    GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                    break;
                case Define.EquipmentGrade.Legendary:
                case Define.EquipmentGrade.Legendary1:
                case Define.EquipmentGrade.Legendary2:
                case Define.EquipmentGrade.Legendary3:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    break;
                default:
                    break;
            }

            string gradeName = _mergeEquipment1.EquipmentData.EquipmentGrade.ToString();
            int num = 0;

            Match match = Regex.Match(gradeName, @"\d+$");
            if (match.Success)
            {
                num = int.Parse((match.Value));
            }

            if (num == 0)
            {
                GetText((int)Texts.FirstSelectEquipEnforceValueText).text = "";
                GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText((int)Texts.FirstSelectEquipEnforceValueText).text = num.ToString();
                GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
            }

            GetText((int)Texts.FirstSelectEquipLevelValueText).text = $"Lv.{_mergeEquipment1.Level}";
            GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(true);
        }
    }
    
    void Refresh_MergeEquip2()
    {
        if (_mergeEquipment2 == null)
        {
            GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(false);
        }
        else
        {
            GetImage((int)Images.SecondSelectEquipImage).sprite = Managers.Resource.Load<Sprite>(_mergeEquipment2.EquipmentData.SpriteName);
            GetImage((int)Images.SecondSelectEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_mergeEquipment2.EquipmentData.EquipmentType}_Icon.sprite");
            switch (_mergeEquipment2.EquipmentData.EquipmentGrade)
            {
                case Define.EquipmentGrade.Common:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
                    break;
                case Define.EquipmentGrade.Uncommon:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    break;
                case Define.EquipmentGrade.Rare:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
                    break;
                case Define.EquipmentGrade.Epic:
                case Define.EquipmentGrade.Epic1:
                case Define.EquipmentGrade.Epic2:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                    GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                    break;
                case Define.EquipmentGrade.Legendary:
                case Define.EquipmentGrade.Legendary1:
                case Define.EquipmentGrade.Legendary2:
                case Define.EquipmentGrade.Legendary3:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    break;
                default:
                    break;
            }

            string gradeName = _mergeEquipment2.EquipmentData.EquipmentGrade.ToString();
            int num = 0;

            Match match = Regex.Match(gradeName, @"\d+$");
            if (match.Success)
            {
                num = int.Parse((match.Value));
            }

            if (num == 0)
            {
                GetText((int)Texts.SecondSelectEquipEnforceValueText).text = "";
                GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText((int)Texts.SecondSelectEquipEnforceValueText).text = num.ToString();
                GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
            }

            GetText((int)Texts.SecondSelectEquipLevelValueText).text = $"Lv.{_mergeEquipment2.Level}";
            GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(true);
        }
    }

    bool CheckEnableMergeButton()
    {
        #region 합성 가능 유무 판단

        if (_equipment == null)
        {
            GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
            GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false);
            GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false);
            return false;
        }

        if (_mergeEquipment2 == null && GetButton((int)Buttons.SecondCostButton).gameObject.activeSelf)
        {
            GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
            GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
            GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false);
            GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false);
            return false;
        }

        if (_mergeEquipment1 == null)
        {
            GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
            GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
            GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false);
            GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false);
            return false;
        }
        
        GetObject((int)GameObjects.OptionResultObject).SetActive(true);
        GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false);
        GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false);
        GetObject((int)GameObjects.MergeStartEffect).SetActive(true);
        GetObject((int)GameObjects.MergeFinishEffect).SetActive(false);
        #endregion

        #region 옵션

        GetImage((int)Images.SelectedEquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        string mergedItemId = _equipment.EquipmentData.MergedItemCode;
        GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
        GetObject((int)GameObjects.OptionResultObject).SetActive(true);
        GetText((int)Texts.EquipmentNameText).text = $"{Managers.Data.EquipDataDic[mergedItemId].NameTextId}";
        GetText((int)Texts.BeforeLevelValueText).text = $"{Managers.Data.EquipDataDic[_equipment.EquipmentData.DataId].MaxLevel}";
        GetText((int)Texts.AfterLevelValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].MaxLevel}";

        if (Managers.Data.EquipDataDic[mergedItemId].AtkDmgBonus != 0)  // 장비의 공격력이 0이 아니면 무기임
        {
            GetObject((int)GameObjects.ImproveAtk).SetActive(true);
            GetObject((int)GameObjects.ImproveHp).SetActive(false);

            GetText((int)Texts.BeforeAtkValueText).text = $"{Managers.Data.EquipDataDic[_equipment.EquipmentData.DataId].MaxLevel}";
            GetText((int)Texts.AfterAtkValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].MaxLevel}";
        }
        else
        {
            // 체력
            GetObject((int)GameObjects.ImproveAtk).SetActive(false);
            GetObject((int)GameObjects.ImproveHp).SetActive(true);

            GetText((int)Texts.BeforeHpValueText).text = $"{Managers.Data.EquipDataDic[_equipment.EquipmentData.DataId].MaxLevel}";
            GetText((int)Texts.AfterHpValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].MaxLevel}";
        }
        
        // 등급별
        switch (Managers.Data.EquipDataDic[mergedItemId].EquipmentGrade)
        {
            case Define.EquipmentGrade.Uncommon:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;

                int uncommonGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].UncommonGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[uncommonGradeSkillId].Description}";

                GetText((int)Texts.BeforeGradeValueText).text = "일반";
                GetText((int)Texts.AfterGradeValueText).text = "고급";
                break;
            case Define.EquipmentGrade.Rare:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;

                int rareGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].UncommonGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[rareGradeSkillId].Description}";

                GetText((int)Texts.BeforeGradeValueText).text = "고급";
                GetText((int)Texts.AfterGradeValueText).text = "희귀";
                break;
            case Define.EquipmentGrade.Epic:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                int epicGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].UncommonGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[epicGradeSkillId].Description}";

                GetText((int)Texts.BeforeGradeValueText).text = "희귀";
                GetText((int)Texts.AfterGradeValueText).text = "에픽";
                break;
            case Define.EquipmentGrade.Epic1:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "1";

                GetText((int)Texts.BeforeGradeValueText).text = "에픽";
                GetText((int)Texts.AfterGradeValueText).text = "에픽 1";
                break;
            case Define.EquipmentGrade.Epic2:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "2";

                GetText((int)Texts.BeforeGradeValueText).text = "에픽 1";
                GetText((int)Texts.AfterGradeValueText).text = "에픽 2";
                break;
            case Define.EquipmentGrade.Legendary:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                int legendaryGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].UncommonGradeSkill;
                GetText((int)Texts.ImproveOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[legendaryGradeSkillId].Description}";

                GetText((int)Texts.BeforeGradeValueText).text = "에픽 2";
                GetText((int)Texts.AfterGradeValueText).text = "전설";
                break;
            case Define.EquipmentGrade.Legendary1:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "1";

                GetText((int)Texts.BeforeGradeValueText).text = "전설";
                GetText((int)Texts.AfterGradeValueText).text = "전설 1";
                break;
            case Define.EquipmentGrade.Legendary2:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "2";

                GetText((int)Texts.BeforeGradeValueText).text = "전설 1";
                GetText((int)Texts.AfterGradeValueText).text = "전설 2";
                break;
            case Define.EquipmentGrade.Legendary3:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "3";

                GetText((int)Texts.BeforeGradeValueText).text = "전설 2";
                GetText((int)Texts.AfterGradeValueText).text = "전설 3";
                break;
            default:
                break;
        }
        #endregion
        
        GetButton((int)Buttons.MergeButton).gameObject.SetActive(true);
        return true;
    }

    void SortEquipments()
    {
        Managers.Game.SortEquipment(_equipmentSortType);
        
        GetObject((int)GameObjects.EquipInventoryScrollContentObject).DestroyChilds();
        
        // 장비 리스트 작성
        foreach (Equipment inventoryEquipmentItem in Managers.Game.OwnedEquipments)
        {
            bool isSelected = false;
            bool isLock = false;
            // 장비 상태 결정 (선택됨 or 선택불가 or 선택가능)
            if (_equipment != null)
            {
                if (_equipment == inventoryEquipmentItem || _mergeEquipment1 == inventoryEquipmentItem || _mergeEquipment2 == inventoryEquipmentItem)
                {
                    isSelected = true;
                    continue;   // 선택된 장비는 리스트에서 제외
                }
                else if (_mergeEquipment1 != null)  // 2개 재료가 모두 있거나 합성 불가능하면 잠금
                {
                    if (_equipment.EquipmentData.MergeEquipmentType2 == Define.MergeEquipmentType.None || _mergeEquipment2 != null)
                    {
                        isLock = true;
                    }
                }

                if (_equipment.EquipmentData.EquipmentType != inventoryEquipmentItem.EquipmentData.EquipmentType)
                {
                    isLock = true;
                }
                else
                {
                    if (_equipment.EquipmentData.MergeEquipmentType1 != Define.MergeEquipmentType.None && _mergeEquipment1 == null) // 1번 재료 판단
                    {
                        if (_equipment.EquipmentData.MergeEquipmentType1 == Define.MergeEquipmentType.ItemCode) // 합성 재료가 동일 아이템 일때
                        {
                            if (inventoryEquipmentItem.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment1)
                            {
                                isLock = true;
                            }
                        }
                        else if (_equipment.EquipmentData.MergeEquipmentType1 == Define.MergeEquipmentType.Grade)   // 합성 재료가 동일 등급일때
                        {
                            if (inventoryEquipmentItem.EquipmentData.EquipmentGrade !=
                                (Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), _equipment.EquipmentData.MergeEquipment1))
                            {
                                isLock = true;
                            }
                        }
                    }
                    
                    if (_equipment.EquipmentData.MergeEquipmentType2 != Define.MergeEquipmentType.None && _mergeEquipment2 == null) // 2번 재료 판단
                    {
                        if (_equipment.EquipmentData.MergeEquipmentType2 == Define.MergeEquipmentType.ItemCode) // 합성 재료가 동일 아이템 일때
                        {
                            if (inventoryEquipmentItem.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment2)
                            {
                                isLock = true;
                            }
                        }
                        else if (_equipment.EquipmentData.MergeEquipmentType2 == Define.MergeEquipmentType.Grade)   // 합성 재료가 동일 등급일때
                        {
                            if (inventoryEquipmentItem.EquipmentData.EquipmentGrade !=
                                (Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), _equipment.EquipmentData.MergeEquipment2))
                            {
                                isLock = true;
                            }
                        }

                        if (inventoryEquipmentItem.Level > 1)
                        {
                            isLock = true;
                        }

                        if (inventoryEquipmentItem.IsEquipped)
                        {
                            isLock = true;
                        }
                    }
                }
            }

            UI_MergeEquipItem item = Managers.UI.MakeSubItem<UI_MergeEquipItem>(GetObject((int)GameObjects.EquipInventoryScrollContentObject).transform);
            item.OnClickEquipItem = () =>
            {
                inventoryEquipmentItem.IsConfirmed = true;
            };
            item.SetInfo(inventoryEquipmentItem, Define.UI_ItemParentType.EquipInventoryGroup, ScrollRect, isSelected, isLock);
        }
    }

    void OnClickBackButton()
    {
        Managers.Sound.PlayPopupClose();
        
        gameObject.SetActive(false);
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickEquipResultButton()
    {
        Managers.Sound.PlayButtonClick();

        _equipment = null;
        _mergeEquipment1 = null;
        _mergeEquipment2 = null;

        Refresh();
    }

    void OnClickFirstCostButton()
    {
        Managers.Sound.PlayButtonClick();
        _mergeEquipment1 = null;
        Refresh();
    }

    void OnClickSecondCostButton()
    {
        Managers.Sound.PlayButtonClick();
        _mergeEquipment2 = null;
        Refresh();
    }

    void OnClickSortButton()
    {
        Managers.Sound.PlayButtonClick();

        if (_equipmentSortType == Define.EquipmentSortType.Level)
        {
            _equipmentSortType = Define.EquipmentSortType.Grade;
            GetText((int)Texts.SortButtonText).text = sortText_Grade;
        }
        else if (_equipmentSortType == Define.EquipmentSortType.Grade)
        {
            _equipmentSortType = Define.EquipmentSortType.Level;
            GetText((int)Texts.SortButtonText).text = sortText_Level;
        }
        
        SortEquipments();
    }

    void OnClickMergeAllButton()
    {
        Managers.Sound.PlayButtonClick();
        StartCoroutine(CoMergeAll());
    }

    IEnumerator CoMergeAll()
    {
        // 자동 합성 버튼
        Managers.Game.SortEquipment(_equipmentSortType);

        Equipment[] equipments = Managers.Game.OwnedEquipments.ToArray();
        List<Equipment> newEquipments = new List<Equipment>();
        for (int i = 0; i < equipments.Length; i++)
        {
            if(equipments[i].EquipmentData.EquipmentGrade > Define.EquipmentGrade.Epic)
                continue;
            if (equipments[i].IsEquipped)
                continue;
            if (equipments[i] != null)
            {
                SetMergeItem(equipments[i],false);
            }
            if(_equipment == null)
                continue;

            for (int j = i; j < equipments.Length; j++)
            {
                if(equipments[j] != null)
                {
                    SetMergeItem(equipments[j],false);
                    if (CheckEnableMergeButton())
                    {
                        Equipment newItem = Managers.Game.MergeEquipment(_equipment, _mergeEquipment1, _mergeEquipment2, true);
                        if (newItem != null)
                        {
                            newEquipments.Add(newItem);
                        }
                    }
                }
            }

            if (i % 5 == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            _equipment = null;
            _mergeEquipment1 = null;
            _mergeEquipment2 = null;
        }
        SortEquipments();
        if (newEquipments.Count > 0)
        {
            Managers.UI.ShowPopupUI<UI_MergeAllResultPopup>().SetInfo(newEquipments);
        }
        Managers.Game.SaveGame();
    }

    void OnClickMergeButton()
    {
        Managers.Sound.PlayButtonClick();

        Equipment beforeEquipment = _equipment;

        Equipment newItem = Managers.Game.MergeEquipment(_equipment, _mergeEquipment1, _mergeEquipment2);

        UI_MergeResultPopup resultPopup = (Managers.UI.SceneUI as UI_LobbyScene).MergeResultPopupUI;
        resultPopup.SetInfo(beforeEquipment, newItem, OnClosedMergeResultPopup);
        resultPopup.gameObject.SetActive(true);
        
        SortEquipments();
    }

    void OnClosedMergeResultPopup()
    {
        OnClickEquipResultButton();
    }
}
