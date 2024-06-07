using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterLevelEffectItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        LevelEffectUnlock
    }

    enum Texts
    {
        LevelEffectDescriptionValueText,
        ConstraintValueText,
    }

    enum Images
    {
        ConstraintImage,
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
