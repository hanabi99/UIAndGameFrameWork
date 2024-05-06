using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : Node
{
    public string GUID;
    
    public string DialogueText;
    
    public bool EntryPoint = false;
}
