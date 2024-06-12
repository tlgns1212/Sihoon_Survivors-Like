using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectronicField : RepeatSkill
{
    [SerializeField]
    GameObject _normalEffect;
    [SerializeField]
    GameObject _finalEffect;

    private HashSet<CreatureController> _targets = new HashSet<CreatureController>();
    private Coroutine _coDotDamage;

    private void Awake()
    {
        SkillType = Define.SkillType.ElectronicField;
        gameObject.SetActive(false);
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        gameObject.SetActive(true);

        _normalEffect.SetActive(true);
        _finalEffect.SetActive(false);

        transform.localScale = Vector3.one * SkillData.ScaleMultiplier;
    }

    public override void OnLevelUp()
    {
        base.OnLevelUp();

        if (Level == 6)
        {
            _normalEffect.SetActive(false);
            _finalEffect.SetActive(true);
        }
        transform.localScale = Vector3.one * SkillData.ScaleMultiplier;
    }

    public override void OnChangedSkillData()
    {
        transform.localScale = Vector3.one * SkillData.ScaleMultiplier;
    }

    public void Update()
    {
        transform.position = Managers.Game.Player.PlayerCenterPos;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        CreatureController target = other.transform.GetComponent<CreatureController>();

        if (target.IsValid() == false)
            return;
        if (target?.IsMonster() == false)
            return;

        _targets.Add(target);

        target.OnDamaged(Managers.Game.Player, this);
        if (_coDotDamage == null)
        {
            _coDotDamage = StartCoroutine(CoStartDotDamage());
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        CreatureController target = other.transform.GetComponent<CreatureController>();
        if (target.IsValid() == false)
            return;

        _targets.Remove(target);

        if (_targets.Count == 0 && _coDotDamage != null)
        {
            StopCoroutine(_coDotDamage);
            _coDotDamage = null;
        }
    }

    protected IEnumerator CoStartDotDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            List<CreatureController> list = _targets.ToList();

            foreach (CreatureController target in list)
            {
                if (target.IsValid() == false || target.gameObject.IsValid() == false)
                {
                    _targets.Remove(target);
                    continue;
                }
                target.OnDamaged(Managers.Game.Player, this);
            }
        }
    }

    protected override void DoSkillJob()
    {

    }
}
