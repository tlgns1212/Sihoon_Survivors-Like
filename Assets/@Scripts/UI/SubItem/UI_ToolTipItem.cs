using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTipItem : UI_Base
{
    #region Enum
    enum Images
    {
        TargetImage,
        BackgroundImage,
    }

    enum Buttons
    {
        CloseButton,
    }
    enum Texts
    {
        TargetNameText,
        TargetDescriptionText,
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

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);

        Refresh();
        return true;
    }

    private void OnEnable()
    {
        GetText((int)Texts.TargetNameText).gameObject.SetActive(false);
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(false);
    }

    // 서포트 스킬 툴팁
    public void SetInfo(Data.SupportSkillData skillData, RectTransform targetPos, RectTransform parentCanvas)
    {
        GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(skillData.IconLabel);
        GetText((int)Texts.TargetNameText).gameObject.SetActive(true);
        GetText((int)Texts.TargetNameText).text = skillData.Name;
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
        GetText((int)Texts.TargetDescriptionText).text = skillData.Description;

        // 등급에 따라 배경 색상 변경
        switch (skillData.SupportSkillGrade)
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

        ToolTipPosSet(targetPos, parentCanvas);
        Refresh();
    }

    // 재료 툴팁
    public void SetInfo(Data.MaterialData materialData, RectTransform targetPos, RectTransform parentCanvas)
    {
        GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(materialData.SpriteName);
        GetText((int)Texts.TargetNameText).gameObject.SetActive(true);
        GetText((int)Texts.TargetNameText).text = materialData.NameTextId;
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
        GetText((int)Texts.TargetDescriptionText).text = materialData.DescriptionTextId;

        // 등급에 따라 배경 색상 변경
        switch (materialData.MaterialGrade)
        {
            case Define.MaterialGrade.Common:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
                break;
            case Define.MaterialGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case Define.MaterialGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case Define.MaterialGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case Define.MaterialGrade.Legendary:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            default:
                break;
        }

        ToolTipPosSet(targetPos, parentCanvas);
        Refresh();
    }

    // 몬스터 툴팁
    public void SetInfo(Data.CreatureData creatureData, RectTransform targetPos, RectTransform parentCanvas)
    {
        GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(creatureData.IconLabel);
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
        GetText((int)Texts.TargetDescriptionText).text = creatureData.DescriptionTextId;
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;

        ToolTipPosSet(targetPos, parentCanvas);
        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickCloseButton()
    {
        Managers.Sound.PlayButtonClick();
        // 자신 끄기
        Managers.Resource.Destroy(gameObject);
    }

    // 툴팁 위치 설정
    void ToolTipPosSet(RectTransform targetPos, RectTransform parentCanvas)
    {
        // 기본 정렬
        gameObject.transform.position = targetPos.transform.position;

        // 세로 높이 설정
        float sizeY = targetPos.sizeDelta.y / 2;
        transform.localPosition += new Vector3(0f, sizeY);

        // 가로 높이 설정
        if (targetPos.transform.localPosition.x > 0) // 오른쪽이면
        {
            float canvasMaxX = parentCanvas.sizeDelta.x / 2;
            float targetPosMaxX = transform.localPosition.x + transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            if (canvasMaxX < targetPosMaxX)
            {
                float deltaX = targetPosMaxX - canvasMaxX;
                transform.localPosition = -new Vector3(deltaX + 20, 0f) + transform.localPosition;
            }
        }
        else
        {   // 왼쪽이면
            float canvasMinX = -parentCanvas.sizeDelta.x / 2;
            float targetPosMinX = transform.localPosition.x - transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            if (canvasMinX > targetPosMinX)
            {
                float deltaX = canvasMinX - targetPosMinX;
                transform.localPosition = new Vector3(deltaX + 20, 0f) + transform.localPosition;
            }
        }
    }
}
