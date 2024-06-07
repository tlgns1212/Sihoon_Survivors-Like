using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterUpgradeEffectItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        LevelEffectUnlock,
    }

    enum Texts
    {
        UpgradeDescriptionValueText,
    }

    enum Images
    {
        StarImage_0,
        StarImage_1,
        StarImage_2,
        StarImage_3,
        StarImage_4,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        

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
}
