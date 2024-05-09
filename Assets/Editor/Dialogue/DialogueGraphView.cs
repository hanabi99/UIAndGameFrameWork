using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    //节点默认大小
    public readonly Vector2 defaultNodeSize = new Vector2(150, 200);

    private NodeSearchWindow _nodeSearchWindow;

    public DialogueGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger()); //内容拖动器
        this.AddManipulator(new SelectionDragger()); //选择拖动器
        this.AddManipulator(new RectangleSelector()); //矩形选择

        //添加网格背景
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow();
    }

    private void AddSearchWindow()
    {
        _nodeSearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _nodeSearchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });
        return compatiblePorts;
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode()
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };
        //node.SetPosition(new Rect(100,200,100,150));
        var generatePort = GeneratePort(node, Direction.Output, Port.Capacity.Multi);
        generatePort.portName = "Next";
        node.outputContainer.Add(generatePort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;
        
        node.RefreshExpandedState();
        node.RefreshPorts();


        node.SetPosition(new Rect(100, 200, 100, 150));

        return node;
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    /// <summary>
    /// 创建一个对话窗口
    /// </summary>
    /// <param name="NodeName"></param>
    public DialogueNode CreateDialogueNode(string NodeName)
    {
        var dialogueNode = new DialogueNode()
        {
            title = NodeName,
            DialogueText = NodeName,
            GUID = Guid.NewGuid().ToString()
        };
        //添加输出端口
        var inputNode = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputNode.portName = "Input";
        dialogueNode.inputContainer.Add(inputNode);
        
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        var button = new Button(() => { AddChoicePort(dialogueNode); });
        button.text = "Add Choice";
        dialogueNode.titleContainer.Add(button);

        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);
        
        
        

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string portName = "")
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        // var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        //
        // if (oldLabel != null)
        // {
        //     generatedPort.contentContainer.Remove(oldLabel);
        // }

        int outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(portName) ? $"Choice {outputPortCount + 1}" : portName;

        var textField = new TextField()
        {
            name = string.Empty,
            value = choicePortName,
        };

        textField.style.minWidth = 60;
        textField.style.maxWidth = 100;

        textField.RegisterValueChangedCallback(evt => { generatedPort.portName = evt.newValue; });

        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);

        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };

        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.portName = choicePortName;
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x =>
            x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);
        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}