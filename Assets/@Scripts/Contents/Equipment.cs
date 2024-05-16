using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public string key = "";

    public Data.EquipmentData EquipmentData;
    public int Level { get; set; } = 1;
    public int AttackBonus { get; set; } = 0;
    public int MaxHpBonus { get; set; } = 0;
    bool _isEquipped = false;
    public bool IsEquipped
    {
        get { return _isEquipped; }
        set { IsEquipped = value; }
    }
    public bool IsOwned { get; set; } = false;
    public bool IsUpgradeable { get; set; } = false;
    public bool IsConfirmed { get; set; } = false;  // 장비 획들을 했는지
    public bool IsEquipmentSynthesizable { get; set; } = false; // 장비가 합성 가능한지
    public bool IsSelected { get; set; } = false;   // 합성 팝업에서 선택이 되어 있는지
    public bool IsUnavailable { get; set; } = false;    // 합성 팝업에서 선택이 불가능한지

    public Equipment(string key)
    {
        this.key = key;

        EquipmentData = Managers.Data.EquipDataDic[key];

        SetInfo(Level);
        IsOwned = true;
    }

    public void SetInfo(int level)
    {
        Level = level;

        AttackBonus = EquipmentData.AtkDmgBonus + (Level - 1) * EquipmentData.AtkDmgBonusPerUpgrade;
        MaxHpBonus = EquipmentData.MaxHpBonus + (Level - 1) * EquipmentData.MaxHpBonusPerUpgrade;
    }

    public void LevelUp()
    {
        Level++;
        EquipmentData = Managers.Data.EquipDataDic[key];
        AttackBonus = EquipmentData.AtkDmgBonus + (Level - 1) * EquipmentData.AtkDmgBonusPerUpgrade;
        MaxHpBonus = EquipmentData.MaxHpBonus + (Level - 1) * EquipmentData.MaxHpBonusPerUpgrade;
    }

    public void ResetEquipLevel()
    {
        int receiveGold = 0;
        int receiveMaterial = 0;
        while (Level > 1)
        {
            Level--;
            receiveGold += Managers.Data.EquipLevelDataDic[Level].UpgradeCost;
            receiveMaterial += Managers.Data.EquipLevelDataDic[Level].UpgradeRequiredItems;
        }

        // Managers.Game.Gold += receiveGold;
        // // Managers.Game.Material += receiveMaterial;

        // 장비의 초기값 가져오기
        AttackBonus = EquipmentData.AtkDmgBonus + Level * EquipmentData.AtkDmgBonusPerUpgrade;
        MaxHpBonus = EquipmentData.MaxHpBonus + Level * EquipmentData.MaxHpBonusPerUpgrade;
    }
}
