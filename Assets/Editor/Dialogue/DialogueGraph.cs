using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    public DialogueGraphView _dialogueGraphView;

    private string _fileName = "New Narrative";

    [MenuItem("Dialogue/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    public void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
    }


    public void ConstructGraphView()
    {
        _dialogueGraphView = new DialogueGraphView()
        {
            name = "Dialogue Graph"
        };
        _dialogueGraphView.StretchToParentSize(); //对其父边框
        rootVisualElement.Add(_dialogueGraphView);
    }

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var fileNameText =  new TextField("File Name");
        fileNameText.SetValueWithoutNotify(_fileName);
        fileNameText.MarkDirtyRepaint();
        fileNameText.RegisterValueChangedCallback((evt) => { _fileName = evt.newValue; });
        toolbar.Add(fileNameText);
       
        toolbar.Add(new Button(() => RequestDataOperation(true)){text = "Save Data"});
        toolbar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});
        var nodeCreateButton = new Button(() =>
        {
           _dialogueGraphView.CreateNode(_fileName);
         });    

        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);
        rootVisualElement.Add(toolbar);
    }

    /// <summary>
    /// 保存或加载节点数据
    /// </summary>
    /// <param name="save"></param>
    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_dialogueGraphView);
        if (save)
        {
            saveUtility.SaveData(_fileName);
        }
        else
        {
            saveUtility.LoadData(_fileName);
        }
    }
    

    public void OnDisable()
    {
        rootVisualElement.Remove(_dialogueGraphView);
    }
}

