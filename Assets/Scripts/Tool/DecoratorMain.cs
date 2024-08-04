using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>����(�����)</para>
/// </summary>
public interface IKnife
{
    //����
    void Attack();
    //����
    void Skill();
}

public class DecoratorMain : MonoBehaviour
{
    void Start()
    {
        //���������
        IKnife knife = new OrdinaryKinfe();
        //�������װ��
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
        Debug.Log("�����Ⲩ");
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
        Debug.Log("��Ѫ");
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
        Debug.Log("��ͨ�ӿ�");
    }

    public void Skill()
    {
        Debug.Log("����");
    }
}

