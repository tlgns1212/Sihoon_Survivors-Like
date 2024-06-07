using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        SelectObject,
        EquippedObject,
        LockObject,
        EquipmentRedDotObject,
    }

    enum Texts
    {
        CharacterLevelValueText,
        EquippedText,
    }

    enum Images
    {
        CharacterImage,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetObject((int)GameObjects.SelectObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.LockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.EquippedObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.EquipmentRedDotObject).gameObject.SetActive(false);
        
        gameObject.BindEvent(OnClickCharacterItem);

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        
    }

    void OnClickCharacterItem()
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.SelectObject).gameObject.SetActive(true);
    }
}
