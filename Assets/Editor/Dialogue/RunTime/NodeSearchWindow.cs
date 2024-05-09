using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeSearchWindow : ScriptableObject,ISearchWindowProvider
{
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"))
            {
                level = 1,
            },
            new SearchTreeEntry(new GUIContent("Dialogue Node"))
            {
                level = 2,
                userData =  typeof(DialogueNode)
            }
        };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        return true;
    }
}
