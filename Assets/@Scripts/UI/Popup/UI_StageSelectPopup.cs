using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_StageSelectPopup : UI_Popup
{
    #region Enum

    enum GameObjects
    {
        ContentObject,
        StageScrollContentObject,
        AppearingMonsterContentObject,
        StageSelectScrollView,
    }

    enum Buttons
    {
        StageSelectButton,
        BackButton,
    }

    enum Texts
    {
        StageSelectTitleText,
        AppearingMonsterText,
        StageSelectButtonText,
    }

    enum Images
    {
        LArrowImage,
        RArrowImage,
    }
    #endregion

    private StageData _stageData;
    private HorizontalScrollSnap _scrollSnap;

    public Action OnPopupClosed;

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
        
        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();

        _scrollSnap = Util.FindChild<HorizontalScrollSnap>(gameObject, recursive: true);
        _scrollSnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
        _scrollSnap.StartingScreen = Managers.Game.CurrentStageData.StageIndex - 1;

        Refresh();
        return true;
    }

    public void SetInfo(StageData stageData)
    {
        _stageData = stageData;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (_stageData == null)
            return;

        GameObject stageContainer = GetObject((int)GameObjects.StageScrollContentObject);
        stageContainer.DestroyChilds();

        _scrollSnap.ChildObjects = new GameObject[Managers.Data.StageDic.Count];

        foreach (StageData stageData in Managers.Data.StageDic.Values)
        {
            UI_StageInfoItem item = Managers.UI.MakeSubItem<UI_StageInfoItem>(stageContainer.transform);
            item.SetInfo(stageData);
            _scrollSnap.ChildObjects[stageData.StageIndex - 1] = item.gameObject;
        }

        StageInfoRefresh();
    }

    void StageInfoRefresh()
    {
        UIRefresh();

        List<int> monsterList = _stageData.AppearingMonsters.ToList();

        GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
        container.DestroyChilds();
        for (int i = 0; i < monsterList.Count; i++)
        {
            UI_MonsterInfoItem monsterInfoItemUI = Managers.UI.MakeSubItem<UI_MonsterInfoItem>(container.transform);
            monsterInfoItemUI.SetInfo(monsterList[i], _stageData.StageLevel, transform);
        }
    }

    void UIRefresh()
    {
        // 기본 상태
        GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
        GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);

        #region 스테이지 화살표
        if (_stageData.StageIndex == 1)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(false);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (_stageData.StageIndex >= 2 && _stageData.StageIndex < 50)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (_stageData.StageIndex == 50)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(false);
        }
        #endregion

        #region 스테이지 선택 버튼

        if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex, out StageClearInfo info) == false)
            return;
        
        // 게임 처음 시작하고 스테이지 창을 오픈 한 경우
        if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
        {
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        }
        // 스테이지 진행중
        if (info.StageIndex <= _stageData.StageIndex)
        {
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        }
        // 새로운 스테이지
        if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex - 1, out StageClearInfo prevInfo) == false)
            return;
        if (prevInfo.isClear == true)
        {
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        }
        else
        {
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        }
        #endregion
    }

    void OnClickStageSelectButton()
    {
        Managers.Game.CurrentStageData = _stageData;
        OnPopupClosed?.Invoke();
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickBackButton()
    {
        OnPopupClosed?.Invoke();
        Managers.UI.ClosePopupUI(this);
    }

    void OnChangeStage(int index)
    {
        // 현재 스테이지 설정
        _stageData = Managers.Data.StageDic[index + 1];

        int[] monsterData = _stageData.AppearingMonsters.ToArray();

        GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
        container.DestroyChilds();

        for (int i = 0; i < monsterData.Length; i++)
        {
            UI_MonsterInfoItem item = Managers.UI.MakeSubItem<UI_MonsterInfoItem>(GetObject((int)GameObjects.AppearingMonsterContentObject).transform);
            item.SetInfo(monsterData[i], _stageData.StageLevel, transform);
        }
        
        UIRefresh();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AppearingMonsterContentObject).GetComponent<RectTransform>());
    }
}
