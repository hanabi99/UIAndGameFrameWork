using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSP : MonoBehaviour
{

    void Start()
    {
        Animal animal = new Dog();
        animal.Run();
    }

}

class Animal
{
    public void Eat()
    {
        Debug.Log("father eat");
    }
    public void Sleep()
    {
        Debug.Log("father Sleep");
    }

    public virtual void Run()
    {
        Debug.Log("father Run");
    }
}

class Dog : Animal
{
    public void Speak()
    {
        Debug.Log("Dog Speak");
    }

    public override void Run()
    {
        Debug.Log("Dog Run");
    }
}
