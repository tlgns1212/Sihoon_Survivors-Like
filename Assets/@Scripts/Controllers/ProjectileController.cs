using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileController : SkillBase
{
    CreatureController _owner;
    public SkillBase Skill;
    Vector2 _spawnPos;
    Vector3 _dir = Vector3.zero;
    Vector3 _target = Vector3.zero;
    Define.SkillType _skillType;
    Rigidbody2D _rigid;
    int _numPenetrations;
    int _bounceCount = 1;
    GameObject _meteorShadow;

    List<CreatureController> _enteredColliderList = new List<CreatureController>();
    Coroutine _coDotDamage;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void OnChangedSkillData()
    {
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = Define.ObjectType.Projectile;
        return true;
    }

    public void SetInfo(CreatureController owner, Vector2 position, Vector2 dir, Vector2 target, SkillBase skill)
    {
        _owner = owner;
        _spawnPos = position;
        _dir = dir;
        Skill = skill;
        _rigid = GetComponent<Rigidbody2D>();
        _target = target;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;
        _numPenetrations = Skill.SkillData.NumPenetrations;
        _bounceCount = Skill.SkillData.NumBounce;

        switch (skill.SkillType)
        {
            case Define.SkillType.ChainLightning:
                break;
            case Define.SkillType.PhotonStrike:
                break;
            case Define.SkillType.Shuriken:
                break;
            case Define.SkillType.ComboShot:
                break;
            case Define.SkillType.WindCutter:
                break;
            case Define.SkillType.Meteor:
                break;
            case Define.SkillType.PoisonField:
                break;
            case Define.SkillType.EgoSword:
            case Define.SkillType.StormBlade:
                break;
        }
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CoCheckDestroy());
        }
    }

    float _timer = 0;
    private float _rotateAmount = 1000;

    IEnumerator CoChainLightning(Vector3 startPos, Vector3 endPos, bool isFollow = false)
    {
        SetParticleSize(startPos, endPos);
        yield return new WaitForSeconds(0.25f);
        DestroyProjectile();
    }

    void SetParticleSize(Vector3 startPos, Vector3 endPos)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        ParticleSystem childParticle = Util.FindChild<ParticleSystem>(gameObject);
        var main = particle.main;
        var childMain = childParticle.main;

        // Scale
        transform.position = startPos;
        float dist = Vector3.Distance(startPos, endPos);
        main.startSizeX = childMain.startSizeX = dist;
        main.startSizeY = childMain.startSizeY = 8;

        // Rotate
        Vector3 dir = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        main.startRotation = childMain.startRotation = angle * -1f;

        // Cast Box
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        float boxWidth = 1f;
        Vector3 midPos = (startPos + endPos) / 2f;
        Vector2 boxSize = new Vector2(boxWidth, boxWidth);

        RaycastHit2D[] colliders = Physics2D.BoxCastAll(midPos, boxSize, 0, dir, dist * 1.3f, targetLayer);

        foreach (RaycastHit2D hit in colliders)
        {
            MonsterController monster = hit.transform.GetComponent<MonsterController>();
            if (monster != null)
            {
                monster.OnDamaged(_owner, Skill);
            }
        }
    }

    IEnumerator CoWindCutter()
    {
        Vector3 targetPoint = Managers.Game.Player.PlayerCenterPos + _dir * Skill.SkillData.ProjSpeed;
        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;

        Sequence seq = DOTween.Sequence();
        // 1. 목표지점까지 빠르게 도착
        // 2. 도착 후 약간 더 전진
        // 3. 되돌아옴

        float projectileTravelTime = 1f;    // 발사체가 목표지점까지 가는데 걸리는 시간
        float secondSeqStartTime = 0.7f;    // 두 번째 시퀀스 시작시간
        float secondSeqDuringTime = 1.8f;   // 두 법ㄴ째 시퀀스 유지시간

        seq.Append(transform.DOMove(targetPoint, projectileTravelTime).SetEase(Ease.OutExpo))
        .Insert(secondSeqStartTime, transform.DOMove(targetPoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

        yield return new WaitForSeconds(Skill.SkillData.Duration);

        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * Skill.SkillData.ProjSpeed * 4f);
            if (Managers.Game.Player.PlayerCenterPos == transform.position)
            {
                DestroyProjectile();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoPhotonStrike()
    {
        List<MonsterController> target = Managers.Object.GetMonsterWithinCamera(1);
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > 3 || target == null)
            {
                DestroyProjectile();
                _timer = 0;
                break;
            }

            if (target[0].IsValid() == false)
                break;

            Vector2 direction = (Vector2)target[0].CenterPosition - _rigid.position;
            float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
            _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
            _rigid.velocity = transform.up * Skill.SkillData.ProjSpeed;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoMeteor()
    {
        while (true)
        {
            if (_meteorShadow != null)
            {
                Vector2 shadowPosition = _meteorShadow.transform.position;

                float distance = Vector2.Distance(shadowPosition, transform.position);
                float scale = Mathf.Lerp(0f, 2.5f, 1 - distance / 10f);
                _meteorShadow.transform.position = shadowPosition;
                _meteorShadow.transform.localScale = new Vector3(scale, scale, 1f);
            }
            if (Vector2.Distance(_rigid.position, _target) < 0.3f)
            {
                ExplosionMeteor();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoPoisonField(SkillBase skill)
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target, Time.deltaTime * skill.SkillData.ProjSpeed);

            if (transform.position == _target)
            {
                string effectName = skill.Level == 6 ? "PoisonFieldEffect_Final" : "PoisonFieldEffect";

                GameObject fireEffect = Managers.Resource.Instantiate(effectName, pooling: true);
                fireEffect.GetComponent<PoisonFieldEffect>().SetInfo(Managers.Game.Player, skill);
                fireEffect.transform.position = _target;
                DestroyProjectile();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoCheckDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DestroyProjectile();
        }
    }

    IEnumerator CoStartDotDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            foreach (CreatureController target in _enteredColliderList)
            {
                target.OnDamaged(_owner, Skill);
            }
        }
    }

    IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(Skill.SkillData.Duration);
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        Managers.Object.Despawn(this);
    }

    void ExplosionMeteor()
    {
        Managers.Resource.Destroy(_meteorShadow);
        float scanRange = 1.5f;
        string prefabName = Level == 6 ? "MeteorHitEffect_Final" : "MeteorHitEffect";
        GameObject obj = Managers.Resource.Instantiate(prefabName, pooling: true);
        obj.transform.position = transform.position;

        RaycastHit2D[] targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0);

        foreach (RaycastHit2D target in targets)
        {
            CreatureController creature = target.transform.GetComponent<CreatureController>();
            if (creature?.IsMonster() == true)
            {
                creature.OnDamaged(_owner, Skill);
            }
        }
        DestroyProjectile();
    }

    void LaunchComboShot()
    {
        Vector3 targetPoint = _owner.CenterPosition + _dir * Skill.SkillData.ProjRange;
        float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Sequence seq = DOTween.Sequence();
        float duration = Skill.SkillData.Duration;

        seq.Append(transform.DOMove(targetPoint, 0.5f).SetEase(Ease.Linear)).AppendInterval(duration - 0.5f).OnComplete(() =>
        {
            Vector3 targetDir = Managers.Game.Player.CenterPosition - transform.position;
            angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            _rigid.velocity = targetDir.normalized * Skill.SkillData.ProjSpeed;
        });
    }

    void BounceProjectile(CreatureController creature)
    {
        List<Transform> list = new List<Transform>();
        list = Managers.Object.GetFindMonstersInFanShape(creature.CenterPosition, _dir, 5.5f, 240);

        List<Transform> sortedList = (from t in list
                                      orderby Vector3.Distance(t.position, transform.position) descending
                                      select t).ToList();

        if (sortedList.Count == 0)
        {
            DestroyProjectile();
        }
        else
        {
            int index = Random.Range(sortedList.Count / 2, sortedList.Count);
            _dir = (sortedList[index].position - transform.position).normalized;
            _rigid.velocity = _dir * Skill.SkillData.BounceSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        FrozenHeart frozenHeart = other.transform.GetComponent<FrozenHeart>();
        if (frozenHeart != null)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MonsterSkill_01 monsterProj = GetComponent<MonsterSkill_01>();

        if (other.transform.parent != null && monsterProj != null)
        {
            FrozenHeart frozenHeart = other.transform.parent.transform.GetComponent<FrozenHeart>();
            if (frozenHeart != null)
            {
                DestroyProjectile();
            }
        }

        CreatureController creature = other.transform.GetComponent<CreatureController>();
        if (creature.IsValid() == false) return;
        if (this.IsValid() == false) return;

        switch (Skill.SkillType)
        {
            case Define.SkillType.IcycleArrow:
            case Define.SkillType.MonsterSkill_01:
            case Define.SkillType.SpinShot:
            case Define.SkillType.CircleShot:
            case Define.SkillType.PhotonStrike:
                _numPenetrations--;
                if (_numPenetrations < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    DestroyProjectile();
                }
                break;
            case Define.SkillType.Shuriken:
            case Define.SkillType.EnergyBolt:
                _bounceCount--;
                BounceProjectile(creature);
                if (_bounceCount < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    DestroyProjectile();
                }
                break;
            case Define.SkillType.WindCutter:
                _enteredColliderList.Add(creature); ;
                if (_coDotDamage == null)
                {
                    _coDotDamage = StartCoroutine(CoStartDotDamage());
                }
                break;
            default:
                break;
        }
        creature.OnDamaged(_owner, Skill);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CreatureController target = other.transform.GetComponent<CreatureController>();
        if (target.IsValid() == false) return;
        if (this.IsValid() == false) return;

        _enteredColliderList.Remove(target);

        if (_enteredColliderList.Count == 0 && _coDotDamage != null)
        {
            StopCoroutine(_coDotDamage);
            _coDotDamage = null;
        }
    }

}
