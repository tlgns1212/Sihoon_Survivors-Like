using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageInfoItem : UI_Base
{
    #region Enum

    enum GameObjects
    {
        MaxWaveGroupObject,
        
        FirstClearRewardLockObject,
        SecondClearRewardLockObject,
        ThirdClearRewardLockObject,
        
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }

    enum Texts
    {
        StageValueText,
        MaxWaveText,
        MaxWaveValueText,
    }

    enum Images
    {
        StageImage,
        StageLockImage,
    }
    #endregion

    private StageData _stageData;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        ClearRewardCompleteInit();
        
        return true;
    }

    public void SetInfo(StageData data)
    {
        _stageData = data;
        transform.localScale = Vector3.one;

        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.StageValueText).text = $"{_stageData.StageIndex} 스테이지";
        GetImage((int)Images.StageImage).sprite = Managers.Resource.Load<Sprite>(_stageData.StageImage);
        if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex, out StageClearInfo info) == false)
            return;
        
        // 1. 최대 클리어 스테이지
        if (info.MaxWaveIndex > 0)
        {
            GetImage((int)Images.StageLockImage).gameObject.SetActive(false);
            GetImage((int)Images.StageImage).color = Color.white;

            if (info.isClear == true) // 스테이지 완료
            {
                GetText((int)Texts.MaxWaveText).gameObject.SetActive(false);
                GetText((int)Texts.MaxWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.MaxWaveValueText).color = Util.HexToColor("60FF08");
                GetText((int)Texts.MaxWaveValueText).text = "스테이지 클리어";
            }
            else    // 스테이지 진행중
            {
                GetText((int)Texts.MaxWaveText).gameObject.SetActive(true);
                GetText((int)Texts.MaxWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.MaxWaveValueText).color = Util.HexToColor("FFDB08");
                GetText((int)Texts.MaxWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
            }
            
            GetObject((int)GameObjects.FirstClearRewardLockObject).SetActive(false);
            GetObject((int)GameObjects.SecondClearRewardLockObject).SetActive(false);
            GetObject((int)GameObjects.ThirdClearRewardLockObject).SetActive(false);
            
            GetObject((int)GameObjects.FirstClearRewardCompleteObject).SetActive(info.isOpenFirstBox);
            GetObject((int)GameObjects.SecondClearRewardCompleteObject).SetActive(info.isOpenSecondBox);
            GetObject((int)GameObjects.ThirdClearRewardCompleteObject).SetActive(info.isOpenThirdBox);
        }
        else
        {
            // 게임 처음 시작하고 스테이지창을 오픈
            if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
            {
                GetImage((int)Images.StageLockImage).gameObject.SetActive(false);
                GetImage((int)Images.StageImage).color = Color.white;
                
                GetText((int)Texts.MaxWaveText).gameObject.SetActive(false);
                GetText((int)Texts.MaxWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.MaxWaveValueText).color = Util.HexToColor("FFDB08");
                GetText((int)Texts.MaxWaveValueText).text = "기록 없음";
                
                GetObject((int)GameObjects.FirstClearRewardLockObject).SetActive(false);
                GetObject((int)GameObjects.SecondClearRewardLockObject).SetActive(false);
                GetObject((int)GameObjects.ThirdClearRewardLockObject).SetActive(false);
            
                GetObject((int)GameObjects.FirstClearRewardCompleteObject).SetActive(info.isOpenFirstBox);
                GetObject((int)GameObjects.SecondClearRewardCompleteObject).SetActive(info.isOpenSecondBox);
                GetObject((int)GameObjects.ThirdClearRewardCompleteObject).SetActive(info.isOpenThirdBox);
            }

            if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex - 1, out StageClearInfo prevInfo) == false)
                return;
            if (prevInfo.isClear == true)
            {
                GetImage((int)Images.StageLockImage).gameObject.SetActive(false);
                GetImage((int)Images.StageImage).color = Color.white;
                
                GetText((int)Texts.MaxWaveText).gameObject.SetActive(false);
                GetText((int)Texts.MaxWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.MaxWaveValueText).color = Util.HexToColor("FFDB08");
                GetText((int)Texts.MaxWaveValueText).text = "기록 없음";
                
                GetObject((int)GameObjects.FirstClearRewardLockObject).SetActive(false);
                GetObject((int)GameObjects.SecondClearRewardLockObject).SetActive(false);
                GetObject((int)GameObjects.ThirdClearRewardLockObject).SetActive(false);
            
                GetObject((int)GameObjects.FirstClearRewardCompleteObject).SetActive(info.isOpenFirstBox);
                GetObject((int)GameObjects.SecondClearRewardCompleteObject).SetActive(info.isOpenSecondBox);
                GetObject((int)GameObjects.ThirdClearRewardCompleteObject).SetActive(info.isOpenThirdBox);
            }
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MaxWaveGroupObject).GetComponent<RectTransform>());
    }

    void ClearRewardCompleteInit()
    {
        GetObject((int)GameObjects.FirstClearRewardLockObject).SetActive(false);
        GetObject((int)GameObjects.SecondClearRewardLockObject).SetActive(false);
        GetObject((int)GameObjects.ThirdClearRewardLockObject).SetActive(false);
            
        GetObject((int)GameObjects.FirstClearRewardCompleteObject).SetActive(true);
        GetObject((int)GameObjects.SecondClearRewardCompleteObject).SetActive(true);
        GetObject((int)GameObjects.ThirdClearRewardCompleteObject).SetActive(true);
        
        GetImage((int)Images.StageLockImage).gameObject.SetActive(true);
        GetImage((int)Images.StageImage).color = Util.HexToColor("3A3A3A");
        GetText((int)Texts.MaxWaveText).gameObject.SetActive(false);
        GetText((int)Texts.MaxWaveValueText).gameObject.SetActive(false);
    }
}
