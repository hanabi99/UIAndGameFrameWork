using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    
    private List<Edge> Edges => _targetGraphView.edges.ToList();
    
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
    
    private DialogueContainer _containerCache;
    
    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility()
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveData(string fileName)
    {
        if (!Edges.Any()) return; // If there are no edges, there is no data to save
        
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        
        //将连线得到的数据存储到对话容器中
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = (connectedPorts[i].output.node as DialogueNode);
            
            var inputNode = (connectedPorts[i].input.node as DialogueNode);
            
            dialogueContainer.LinkData.Add(new NodeLinkData()
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        //将节点得到的数据存储到对话容器中
        foreach (var dialogueNode in Nodes.Where(x => !x.EntryPoint))
        {
            dialogueContainer.NodeData.Add(new DialogueNodeData()
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }   
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }
    
    public void LoadData(string fileName)
    {
        _containerCache = Resources.Load<DialogueContainer>(fileName);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }
    
    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.LinkData[0].BaseNodeGuid;
        
        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
    }
    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.NodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);
            
            var nodePorts = _containerCache.LinkData.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ConnectNodes()
    {
    }
}
