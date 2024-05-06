using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueNodeData> NodeData = new List<DialogueNodeData>();
    public List<NodeLinkData> LinkData = new List<NodeLinkData>();
}
