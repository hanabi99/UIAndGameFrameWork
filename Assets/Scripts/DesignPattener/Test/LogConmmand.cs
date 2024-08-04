using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGameFrameWork;


public class LogConmmand : CommandBase
{
    public void Excute()
    {
        Debug.Log("Ç°½ø");
    }

    public void Undo()
    {
        Debug.Log("ºóÍË");
    }
}

