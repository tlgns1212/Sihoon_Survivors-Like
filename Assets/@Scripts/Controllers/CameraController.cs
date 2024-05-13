using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region 변수
    public Transform _playerTransform;
    public float Height { get; set; } = 0;
    public float Width { get; set; } = 0;

    [SerializeField]
    float _tickValue = 5;
    [SerializeField]
    float _adjust = 0.5f;
    bool _isShake = false;

    #endregion

    void Start()
    {
        SetCameraSize();
    }

    void SetCameraSize()
    {
        Camera.main.orthographicSize = 21f;
        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;
    }

    void LateUpdate()
    {
        // TODO : if statement
        if (_playerTransform != null && _isShake == false)
        {
            LimitCameraArea();
        }
    }

    void LimitCameraArea()
    {
        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10f);

        // TODO : clamp
    }

    public void Shake()
    {
        if (_isShake == false)
        {
            StartCoroutine(CoShake(0.25f));
        }
    }

    IEnumerator CoShake(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);
        _isShake = true;

        while (elapsed < duration)
        {
            if (Managers.UI.GetPopupCount() > 0)
                break;

            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * _tickValue;
            transform.position += new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f,
                0f) * _adjust * Mathf.PingPong(elapsed, halfDuration);
            yield return null;
        }
        _isShake = false;
    }
}

