using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>刀类(组件类)</para>
/// </summary>
public interface IKnife
{
    //攻击
    void Attack();
    //技能
    void Skill();
}

public class DecoratorMain : MonoBehaviour
{
    void Start()
    {
        //创建组件类
        IKnife knife = new OrdinaryKinfe();
        //添加气波装饰
        knife = new LightWave(knife);

        knife.Attack();

        knife = new BloodPearl(knife);

        knife.Skill();
    }

}

public class LightWave : KinfeDecorator
{
    public LightWave(IKnife knife) : base(knife)
    {

    }

    public override void Attack()
    {
        base.Attack();
        Debug.Log("发出光波");
    }
}

public class BloodPearl : KinfeDecorator
{
    public BloodPearl(IKnife knife) : base(knife)
    {

    }

    public override void Skill()
    {
        base.Skill();
        Debug.Log("回血");
    }
}


public abstract class KinfeDecorator : IKnife
{
    private readonly IKnife _knife;

    protected KinfeDecorator(IKnife knife)
    {
        _knife = knife;
    }

    public virtual void Attack() => _knife.Attack();

    public virtual void Skill() => _knife.Skill();
}


public class OrdinaryKinfe : IKnife
{
    public void Attack()
    {
        Debug.Log("普通挥砍");
    }

    public void Skill()
    {
        Debug.Log("上挑");
    }
}

