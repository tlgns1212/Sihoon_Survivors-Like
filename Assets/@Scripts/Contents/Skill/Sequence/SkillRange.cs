using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    SpriteRenderer _sprite;
    ParticleSystem _circle;
    ParticleSystem _circle2;

    private void Awake()
    {
        _sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _circle = Util.FindChild<ParticleSystem>(gameObject, recursive: true);
        _circle2 = _circle.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _sprite.size = Vector2.zero;
    }

    public void SetInfo(Vector2 dir, Vector2 target, float dist)
    {
        float distance = dist;

        _sprite.size = new Vector2(1.3f, distance);

        Vector3 normalDir = dir.normalized;
        float angle = Mathf.Atan2(normalDir.x, normalDir.y) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public float SetCircle(float startSize)
    {
        _circle.Play();
        var main = _circle.main;
        var main2 = _circle2.main;
        main.startSize = startSize;
        main2.startSize = startSize;

        return _circle.main.duration;
    }
}
