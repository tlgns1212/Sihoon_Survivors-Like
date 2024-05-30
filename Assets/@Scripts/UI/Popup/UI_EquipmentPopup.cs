using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        WeaponEquipObject,
        GlovesEquipObject,
        RingEquipObject,
        BeltEquipObject,
        ArmorEquipObject,
        BootsEquipObject,
        CharacterRedDotObject,
        MergeButtonRedDotObject,
        EquipInventoryObject,
        ItemInventoryObject,
        EquipInventoryGroupObject,
        ItemInventoryGroupObject,
    }

    enum Buttons
    {
        CharacterButton,
        SortButton,
        MergeButton,
    }

    enum Images
    {
        CharacterImage,
    }

    enum Texts
    {
        AttackValueText,
        HealthValueText,
        SortButtonText,
        MergeButtonText,
        EquipInventoryTitleText,
        ItemInventoryTitleText,
    }
    #endregion

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    [SerializeField] public ScrollRect ScrollRect;
    private Define.EquipmentSortType _equipmentSortType;

    private string sortText_Level = "정렬 : 레벨";
    private string sortText_Grade = "정렬 : 등급";

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetObject((int)GameObjects.CharacterRedDotObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.MergeButtonRedDotObject).gameObject.SetActive(false);
        // GetButton((int)Buttons.CharacterButton).gameObject.BindEvent(OnClickCharacterButton);
        GetButton((int)Buttons.CharacterButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CharacterButton).gameObject.SetActive(false);
        
        // GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
        GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
        // GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();

        _equipmentSortType = Define.EquipmentSortType.Level;
        GetText((int)Texts.SortButtonText).text = sortText_Level;
        
        Refresh();
        
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        
        #region 초기화

        GameObject WeaponContainer = GetObject((int)GameObjects.WeaponEquipObject);
        GameObject GlovesContainer = GetObject((int)GameObjects.GlovesEquipObject);
        GameObject RingContainer = GetObject((int)GameObjects.RingEquipObject);
        GameObject BeltContainer = GetObject((int)GameObjects.BeltEquipObject);
        GameObject ArmorContainer = GetObject((int)GameObjects.ArmorEquipObject);
        GameObject BootsContainer = GetObject((int)GameObjects.BootsEquipObject);
        
        WeaponContainer.DestroyChilds();
        GlovesContainer.DestroyChilds();
        RingContainer.DestroyChilds();
        BeltContainer.DestroyChilds();
        ArmorContainer.DestroyChilds();
        BootsContainer.DestroyChilds();
        #endregion

        #region 장비
        // 1. 장비 리스트를 불러와서 장비 인벤토리에 추가
        foreach (Equipment item in Managers.Game.OwnedEquipments)
        {
            // 착요중인 장비
            if (item.IsEquipped)
            {
                switch (item.EquipmentData.EquipmentType)
                {
                    case Define.EquipmentType.Weapon:
                        // UI_EquipItem
                        break;
                    case Define.EquipmentType.Gloves:
                        break;
                    case Define.EquipmentType.Ring:
                        break;
                    case Define.EquipmentType.Belt:
                        break;
                    case Define.EquipmentType.Armor:
                        break;
                    case Define.EquipmentType.Boots:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

    }
}
