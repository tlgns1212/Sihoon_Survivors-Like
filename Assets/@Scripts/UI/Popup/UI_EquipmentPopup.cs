using System;
using System.Collections;
using System.Collections.Generic;
using Data;
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
        GetButton((int)Buttons.CharacterButton).gameObject.BindEvent(OnClickCharacterButton);
        GetButton((int)Buttons.CharacterButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CharacterButton).gameObject.SetActive(false);
        
        GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
        GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
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
                        UI_EquipItem weapon = Managers.UI.MakeSubItem<UI_EquipItem>(WeaponContainer.transform);
                        weapon.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Gloves:
                        UI_EquipItem gloves = Managers.UI.MakeSubItem<UI_EquipItem>(GlovesContainer.transform);
                        gloves.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Ring:
                        UI_EquipItem ring = Managers.UI.MakeSubItem<UI_EquipItem>(RingContainer.transform);
                        ring.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Belt:
                        UI_EquipItem belt = Managers.UI.MakeSubItem<UI_EquipItem>(BeltContainer.transform);
                        belt.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Armor:
                        UI_EquipItem armor = Managers.UI.MakeSubItem<UI_EquipItem>(ArmorContainer.transform);
                        armor.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Boots:
                        UI_EquipItem boots = Managers.UI.MakeSubItem<UI_EquipItem>(BootsContainer.transform);
                        boots.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    default:
                        break;
                }
            }
        }
        SortEquipments();
        #endregion

        #region 캐릭터
        // 공격력, Hp 설정
        var (hp, attack) = Managers.Game.GetCurrentCharacterStat();
        GetText((int)Texts.AttackValueText).text = (Managers.Game.CurrentCharacter.Atk + attack).ToString();
        GetText((int)Texts.HealthValueText).text = (Managers.Game.CurrentCharacter.MaxHp + hp).ToString();
        #endregion

        SetItem();
        
        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ItemInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipInventoryGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ItemInventoryGroupObject).GetComponent<RectTransform>());

    }

    void OnClickCharacterButton()
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_CharacterSelectPopup>();
    }

    void OnClickSortButton()
    {
        Managers.Sound.PlayButtonClick();
        
        // 레벨로 정렬, 등급으로 정렬 누를때마다 정렬방식 변경
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

    void OnClickMergeButton()
    {
        Managers.Sound.PlayButtonClick();
        UI_MergePopup mergePopupUI = (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI;
        mergePopupUI.SetInfo(null);
        mergePopupUI.gameObject.SetActive(true);
    }

    void SortEquipments()
    {
        Managers.Game.SortEquipment(_equipmentSortType);
        
        GetObject((int)GameObjects.EquipInventoryObject).DestroyChilds();

        foreach (Equipment item in Managers.Game.OwnedEquipments)
        {
            if(item.IsEquipped)
                continue;

            UI_EquipItem equipItem = Managers.Resource.Instantiate("UI_EquipItem", GetObject((int)GameObjects.EquipInventoryObject).transform, true)
                .GetOrAddComponent<UI_EquipItem>();
            
            equipItem.transform.SetParent(GetObject((int)GameObjects.EquipInventoryObject).transform);
            equipItem.SetInfo(item, Define.UI_ItemParentType.EquipInventoryGroup, ScrollRect);
        }
    }

    public void SetItem()
    {
        GameObject container = GetObject((int)GameObjects.ItemInventoryObject);
        container.DestroyChilds();

        foreach (int id in Managers.Game.ItemDictionary.Keys)
        {
            if (Managers.Data.MaterialDic.TryGetValue(id, out MaterialData material) == true)
            {
                UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
                int count = Managers.Game.ItemDictionary[id];
                
                item.SetInfo(material,transform, count, ScrollRect);
            }
        }
    }
}
