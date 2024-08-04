using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGameFrameWork;

public class ConmmandMain : MonoBehaviour
{
   
    void Start()
    {
        LogConmmand logConmmand = new LogConmmand();
        //DebugConmmand dc = new DebugConmmand();
        CommandManager.GetInstance().AddCommands(CommandType.Log, logConmmand);
        //CommandManager.GetInstance().AddCommands(CommandType.Log, dc);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CommandManager.GetInstance().DoStart(CommandType.Log);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //CommandManager.GetInstance().UnDO(CommandType.Log);
        }
    }
}
