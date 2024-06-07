using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_AccountPassItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        FreePassRewardCompleteObject,
        RarePassRewardCompleteObject,
        EpicPassRewardCompleteObject,
        
        FreePassRewardLockObject,
        RarePassRewardLockObject,
        EpicPassRewardLockObject,
    }

    enum Buttons
    {
        FreePassRewardButton,
        RarePassRewardButton,
        EpicPassRewardButton,
    }

    enum Texts
    {
        AccountLevelValueText,
        FreePassRewardItemValueText,
        RarePassRewardItemValueText,
        EpicPassRewardItemValueText,
    }

    enum Images
    {
        FreePassRewardItemImage,
        RarePassRewardItemImage,
        EpicPassRewardItemImage,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.FreePassRewardButton).gameObject.BindEvent(OnClickFreePassRewardButton);
        GetButton((int)Buttons.FreePassRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.RarePassRewardButton).gameObject.BindEvent(OnClickRarePassRewardButton);
        GetButton((int)Buttons.RarePassRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EpicPassRewardButton).gameObject.BindEvent(OnClickEpicPassRewardButton);
        GetButton((int)Buttons.EpicPassRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        
        GetObject((int)GameObjects.FreePassRewardLockObject).SetActive(true);
        GetObject((int)GameObjects.RarePassRewardLockObject).SetActive(true);
        GetObject((int)GameObjects.EpicPassRewardLockObject).SetActive(true);
        
        GetObject((int)GameObjects.FreePassRewardCompleteObject).SetActive(false);
        GetObject((int)GameObjects.RarePassRewardCompleteObject).SetActive(false);
        GetObject((int)GameObjects.EpicPassRewardCompleteObject).SetActive(false);

        Refresh();
        return true;
    }

    void Refresh()
    {
        
    }

    void OnClickFreePassRewardButton()
    {
        // 무료 보상 지급 시 활성화
        // GetObject((int)GameObjects.FreePassRewardLockObject).gameObject.SetActive(true);
    }
    void OnClickRarePassRewardButton()
    {
        // 레어 보상 지급 시 활성화
        // GetObject((int)GameObjects.RarePassRewardLockObject).gameObject.SetActive(true);
    }
    void OnClickEpicPassRewardButton()
    {
        // 에픽 보상 지급 시 활성화
        // GetObject((int)GameObjects.EpicPassRewardLockObject).gameObject.SetActive(true);
    }
    
}
