using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttrType : uint
{
    // 新兵
    Recruit,
    // 中士
    StaffSergeant,
    // 上士
    Sergeant,
    // 上尉
    Captian,
}

public class FlyweightMain : MonoBehaviour
{
    public static readonly Dictionary<AttrType, Dictionary<string, object>> BaseAttrDict = new Dictionary<AttrType, Dictionary<string, object>>
 {
     {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",100},
         {"MoveSpeed",1.0f},
         {"Name","新兵"}
     }},
     {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",200},
         {"MoveSpeed",2.0f},
         {"Name","中士"}
     }},
     {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",500},
         {"MoveSpeed",3.0f},
         {"Name","上士"}
     }},  {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",1000},
         {"MoveSpeed",4.0f},
         {"Name","上尉"}
     }},
 };


    void Start()
    {
        AttrFactory factory = new AttrFactory();
        for (int i = 0; i < 100; i++)
        {
            var values = Enum.GetValues(typeof(AttrFactory.AttrType));
            AttrFactory.AttrType attrType = (AttrFactory.AttrType)values.GetValue(UnityEngine.Random.Range(0, 3));
            SoldierAttr soldierAttr = factory.GetSoldierAttr(attrType, UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(155.0f, 190.0f));
        }
    }




}
public class SoldierAttr
{
    public int hp { get; set; }

    public float height { get; set; }
    public FlyweightAttr flyweightAttr { get; }

    // 构造函数
    public SoldierAttr(FlyweightAttr flyweightAttr, int hp, float height)
    {
        this.flyweightAttr = flyweightAttr;
        this.hp = hp;
        this.height = height;
    }

    public int GetMaxHp()
    {
        return flyweightAttr.maxHp;
    }


    public float GetMoveSpeed()
    {
        return flyweightAttr.moveSpeed;
    }


    public string GetName()
    {
        return flyweightAttr.name;
    }
}


public class FlyweightAttr
{
    public int maxHp { get; set; }
    public float moveSpeed { get; set; }
    public string name { get; set; }
    public FlyweightAttr(string name, int maxHp, float moveSpeed)
    {
        this.name = name;
        this.maxHp = maxHp;
        this.moveSpeed = moveSpeed;
    }
}
public class AttrFactory
{
    /// <summary>
    /// 属性类型枚举
    /// </summary>
    public enum AttrType : uint
    {
        // 新兵
        Recruit = 0,
        // 中士
        StaffSergeant,
        // 上士
        Sergeant,
        // 上尉
        Captian,
    }
    /// <summary>
    /// 基础属性缓存
    /// </summary>
    private Dictionary<AttrType, FlyweightAttr> _flyweightAttrDB = null;
    public AttrFactory()
    {
        _flyweightAttrDB = new Dictionary<AttrType, FlyweightAttr>();
        _flyweightAttrDB.Add(AttrType.Recruit, new FlyweightAttr("士兵", 100, 1.0f));
        _flyweightAttrDB.Add(AttrType.StaffSergeant, new FlyweightAttr("中士", 200, 2.0f));
        _flyweightAttrDB.Add(AttrType.Sergeant, new FlyweightAttr("上士", 500, 3.0f));
        _flyweightAttrDB.Add(AttrType.Captian, new FlyweightAttr("上尉", 1000, 4.0f));
    }
    /// <summary>
    /// 获取角色属性
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="hp">血量</param>
    /// <param name="height">身高</param>
    /// <returns></returns>
    public SoldierAttr GetSoldierAttr(AttrType type, int hp, float height)
    {
        if (!_flyweightAttrDB.ContainsKey(type))
        {
            Debug.LogErrorFormat("{0}属性不存在", type);
            return null;
        }
        FlyweightAttr flyweightAttr = _flyweightAttrDB[type];
        SoldierAttr attr = new SoldierAttr(flyweightAttr, hp, height);
        return attr;
    }
}


