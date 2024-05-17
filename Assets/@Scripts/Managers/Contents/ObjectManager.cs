using Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<GemController> Gems { get; } = new HashSet<GemController>();
    public HashSet<SoulController> Souls { get; } = new HashSet<SoulController>();
    public HashSet<DropItemController> DropItems { get; } = new HashSet<DropItemController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

    public Transform SkillTransform
    {
        get
        {
            GameObject root = GameObject.Find("@Skill");
            if (root == null)
            {
                root = new GameObject { name = "@Skill" };
            }
            return root.transform;
        }
    }

    public Transform MonsterTransform
    {
        get
        {
            GameObject root = GameObject.Find("@Monster");
            if (root == null)
            {
                root = new GameObject { name = "@Monster" };
            }
            return root.transform;
        }
    }

    public Transform TextFontTransform
    {
        get
        {
            GameObject root = GameObject.Find("@TextFont");
            if (root == null)
            {
                root = new GameObject { name = "@TextFont" };
            }
            return root.transform;
        }
    }

    public ObjectManager()
    {
        Init();
    }

    public void Init()
    {

    }

    public void Clear()
    {
        Monsters.Clear();
        Gems.Clear();
        Souls.Clear();
        Projectiles.Clear();
    }

    public void LoadMap(string mapName)
    {
        GameObject objMap = Managers.Resource.Instantiate(mapName);
        objMap.transform.position = Vector3.zero;
        objMap.name = "@Map";

        objMap.GetComponent<Map>().Init();
    }

    public void ShowDamageFont(Vector2 pos, float damage, float healAmount, Transform parent, bool isCritical = false)
    {
        string prefabName;
        if (isCritical)
        {
            prefabName = "CriticalDamageFont";
        }
        else
        {
            prefabName = "DamageFont";
        }

        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        DamageFont damageText = go.GetOrAddComponent<DamageFont>();
        damageText.SetInfo(pos, damage, healAmount, parent, isCritical);
    }

    public T Spawn<T>(Vector3 position, int templateId = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateId].PrefabLabel);
            go.transform.position = position;
            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            pc.SetInfo(templateId);
            Player = pc;
            Managers.Game.Player = pc;

            return pc as T;
        }
        else if (type == typeof(MonsterController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateId];
            GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
            MonsterController mc = go.GetOrAddComponent<MonsterController>();
            go.transform.position = position;
            mc.SetInfo(templateId);
            go.name = cd.PrefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateId];
            GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
            EliteController ec = go.GetOrAddComponent<EliteController>();
            go.transform.position = position;
            ec.SetInfo(templateId);
            go.name = cd.PrefabLabel;
            Monsters.Add(ec);

            return ec as T;
        }
        else if (type == typeof(BossController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateId];
            GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}");
            BossController bc = go.GetOrAddComponent<BossController>();
            bc.enabled = true;
            go.transform.position = position;
            bc.SetInfo(templateId);
            go.name = cd.PrefabLabel;
            Monsters.Add(bc);

            return bc as T;
        }
        else if (type == typeof(GemController))
        {
            GameObject go = Managers.Resource.Instantiate("ExpGem", pooling: true);
            GemController gc = go.GetOrAddComponent<GemController>();
            go.transform.position = position;
            Gems.Add(gc);
            Managers.Game.CurrentMap.Grid.Add(gc);

            return gc as T;
        }
        else if (type == typeof(SoulController))
        {
            GameObject go = Managers.Resource.Instantiate("Soul", pooling: true);
            SoulController sc = go.GetOrAddComponent<SoulController>();
            go.transform.position = position;
            Souls.Add(sc);
            Managers.Game.CurrentMap.Grid.Add(sc);

            return sc as T;
        }
        else if (type == typeof(PotionController))
        {
            GameObject go = Managers.Resource.Instantiate("Potion", pooling: true);
            PotionController pc = go.GetOrAddComponent<PotionController>();
            go.transform.position = position;
            DropItems.Add(pc);
            Managers.Game.CurrentMap.Grid.Add(pc);

            return pc as T;
        }
        else if (type == typeof(BombController))
        {
            GameObject go = Managers.Resource.Instantiate("Bomb", pooling: true);
            BombController bc = go.GetOrAddComponent<BombController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers.Game.CurrentMap.Grid.Add(bc);

            return bc as T;
        }
        else if (type == typeof(MagnetController))
        {
            GameObject go = Managers.Resource.Instantiate("Magnet", pooling: true);
            MagnetController mc = go.GetOrAddComponent<MagnetController>();
            go.transform.position = position;
            DropItems.Add(mc);
            Managers.Game.CurrentMap.Grid.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteBoxController))
        {
            GameObject go = Managers.Resource.Instantiate("DropBox", pooling: true);
            EliteBoxController ebc = go.GetOrAddComponent<EliteBoxController>();
            go.transform.position = position;
            DropItems.Add(ebc);
            Managers.Game.CurrentMap.Grid.Add(ebc);
            Managers.Sound.Play(Sound.Effect, "Drop_Box");

            return ebc as T;
        }
        else if (type == typeof(ProjectileController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            go.transform.position = position;
            Projectiles.Add(pc);

            return pc as T;
        }
        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            // ????????????????????????
        }
        else if (type == typeof(MonsterController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(EliteController))
        {
            Monsters.Remove(obj as EliteController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(BossController))
        {
            Monsters.Remove(obj as BossController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(GemController))
        {
            Gems.Remove(obj as GemController);
            Managers.Resource.Destroy(obj.gameObject);
            Managers.Game.CurrentMap.Grid.Remove(obj as GemController);
        }
        else if (type == typeof(SoulController))
        {
            Souls.Remove(obj as SoulController);
            Managers.Resource.Destroy(obj.gameObject);
            Managers.Game.CurrentMap.Grid.Remove(obj as SoulController);
        }
        else if (type == typeof(PotionController))
        {
            DropItems.Remove(obj as PotionController);
            Managers.Resource.Destroy(obj.gameObject);
            Managers.Game.CurrentMap.Grid.Remove(obj as PotionController);
        }
        else if (type == typeof(MagnetController))
        {
            DropItems.Remove(obj as MagnetController);
            Managers.Resource.Destroy(obj.gameObject);
            Managers.Game.CurrentMap.Grid.Remove(obj as MagnetController);
        }
        else if (type == typeof(BombController))
        {
            DropItems.Remove(obj as BombController);
            Managers.Resource.Destroy(obj.gameObject);
            Managers.Game.CurrentMap.Grid.Remove(obj as BombController);
        }
        else if (type == typeof(EliteBoxController))
        {
            DropItems.Remove(obj as EliteBoxController);
            Managers.Resource.Destroy(obj.gameObject);
            Managers.Game.CurrentMap.Grid.Remove(obj as EliteBoxController);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }

    public List<MonsterController> GetNearestMonster(int count = 1, int distanceThreshold = 0)
    {
        List<MonsterController> monsterList = Monsters.OrderBy(monster => (Player.CenterPosition - monster.CenterPosition).sqrMagnitude).ToList();

        if (distanceThreshold > 0)
        {
            monsterList = monsterList.Where(monster => (Player.CenterPosition - monster.CenterPosition).magnitude > distanceThreshold).ToList();
        }

        int min = Mathf.Min(count, monsterList.Count);

        List<MonsterController> nearestMonsters = monsterList.Take(min).ToList();

        if (nearestMonsters.Count == 0) return null;

        // 요소 개수가 count와 다른 경우 마지막 요소 반복해서 추가
        while (nearestMonsters.Count < count)
        {
            nearestMonsters.Add(nearestMonsters.Last());
        }
        return nearestMonsters;
    }

    public List<MonsterController> GetMonsters(int count = 1)
    {
        List<MonsterController> monsterList = Monsters.ToList();
        int min = Mathf.Min(count, monsterList.Count);

        List<MonsterController> randomMonsters = monsterList.Take(min).ToList();
        if (randomMonsters.Count == 0) return null;

        // 요소 개수가 count와 다른 경우 마지막 요소 반복해서 추가
        while (randomMonsters.Count < count)
        {
            randomMonsters.Add(randomMonsters.Last());
        }

        randomMonsters.Shuffle();
        return randomMonsters.Take(count).ToList();
    }

    public List<MonsterController> GetMonsterWithinCamera(int count = 1)
    {
        List<MonsterController> monsterList = Monsters.ToList().Where(monster => IsWithInCamera(Camera.main.WorldToViewportPoint(monster.CenterPosition)) == true).ToList();
        monsterList.Shuffle();

        int min = Mathf.Min(count, monsterList.Count);
        List<MonsterController> monsters = monsterList.Take(min).ToList();

        if (monsters.Count == 0) return null;

        while (monsters.Count < count)
        {
            monsters.Add(monsters.Last());
        }

        return monsters.Take(count).ToList();
    }

    public List<Transform> GetFindMonstersInFanShape(Vector3 origin, Vector3 forward, float radius = 2, float angleRange = 80)
    {
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        RaycastHit2D[] targets = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 0, targetLayer);

        // 타겟중에 부채꼴 안에 있는애만 리스트에 넣기
        foreach (RaycastHit2D target in targets)
        {
            // (타겟 - origin)와 (내 정면 벡터)를 내적
            float dot = Vector3.Dot((target.transform.position - origin).normalized, forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;
            // 시야각 판별
            if (degree <= angleRange / 2f)
            {
                listMonster.Add(target.transform);
            }
        }
        return listMonster;
    }

    public void KillAllMonsters()
    {
        UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;

        if (scene != null)
        {
            scene.DoWhiteFlash();
        }
        foreach (MonsterController monster in Monsters.ToList())
        {
            if (monster.ObjectType == ObjectType.Monster)
            {
                monster.OnDead();
            }
        }
        DespawnAllMonsterProjectiles();
    }

    public void DespawnAllMonsterProjectiles()
    {
        foreach (ProjectileController proj in Projectiles.ToList())
        {
            if (proj.Skill.SkillType == SkillType.MonsterSkill_01)
            {
                Despawn(proj);
            }
        }
    }

    public void CollectAllItems()
    {
        foreach (GemController gem in Gems.ToList())
        {
            gem.GetItem();
        }
        foreach (SoulController soul in Souls.ToList())
        {
            soul.GetItem();
        }
    }

    bool IsWithInCamera(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1)
            return true;
        return false;
    }
}
