using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : UI_Scene
{
    private GameObject _handler;
    private GameObject _joystickBG;
    private Vector2 _moveDir { get; set; }
    private Vector2 _joystickTouchPos;
    private Vector2 _joystickOriginalPos;
    private float _joystickRadius;

    enum GameObjects
    {
        JoystickBG,
        Handler,
    }

    private void OnDestroy()
    {
        Managers.UI.OnTimeScaleChanged -= OnTimeScaleChanged;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.OnTimeScaleChanged += OnTimeScaleChanged;

        BindObject(typeof(GameObjects));
        _handler = GetObject((int)GameObjects.Handler);
        _joystickBG = GetObject((int)GameObjects.JoystickBG);
        _joystickOriginalPos = _joystickBG.transform.position;
        _joystickRadius = _joystickBG.GetComponent<RectTransform>().sizeDelta.y / 5;

        gameObject.BindEvent(OnPointerDown, null, type: Define.UIEvent.PointerDown);
        gameObject.BindEvent(OnPointerUp, null, type: Define.UIEvent.PointerUp);
        gameObject.BindEvent(null, OnDrag, type: Define.UIEvent.Drag);

        SetActiveJoystick(false);

        return true;
    }

    #region Event
    public void OnPointerDown()
    {
        SetActiveJoystick(true);

        _joystickTouchPos = Input.mousePosition;

        if (Managers.Game.JoystickType == Define.JoystickType.Flexible)
        {
            _handler.transform.position = Input.mousePosition;
            _joystickBG.transform.position = Input.mousePosition;
        }
    }

    public void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;

        _moveDir = Managers.Game.JoystickType == Define.JoystickType.Fixed ? (dragPos - _joystickOriginalPos).normalized : (dragPos - _joystickTouchPos).normalized;

        float joystickDist = (dragPos - _joystickOriginalPos).sqrMagnitude;

        Vector3 newPos;
        // 조이스틱이 반지름 안에 있는 경우
        if (joystickDist < _joystickRadius)
        {
            newPos = _joystickTouchPos + _moveDir * joystickDist;
        }
        else    // 조이스틱이 반지름 안에 있는 경우
        {
            newPos = Managers.Game.JoystickType == Define.JoystickType.Fixed ? _joystickOriginalPos + _moveDir * _joystickRadius : _joystickTouchPos + _moveDir * _joystickRadius;
        }

        _handler.transform.position = newPos;
        Managers.Game.MoveDir = _moveDir;
    }

    void SetActiveJoystick(bool isActive)
    {
        if (isActive)
        {
            _handler.GetComponent<Image>().DOFade(1, 0.5f);
            _joystickBG.GetComponent<Image>().DOFade(1, 0.5f);
        }
        else
        {
            _handler.GetComponent<Image>().DOFade(0, 0.5f);
            _joystickBG.GetComponent<Image>().DOFade(0, 0.5f);
        }
    }

    public void OnPointerUp()
    {
        _moveDir = Vector2.zero;
        _handler.transform.position = _joystickOriginalPos;
        _joystickBG.transform.position = _joystickOriginalPos;
        Managers.Game.MoveDir = _moveDir;
        SetActiveJoystick(false);
    }

    public void OnTimeScaleChanged(int timeScale)
    {
        if (timeScale == 1)
        {
            gameObject.SetActive(true);
            OnPointerUp();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}
