using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MA_Exit_Window : EditorWindow
{
    string myString = "Hello World";
    float radius = 0.5f;
    float offset = 0.5f;
    MA_Exit[] allExits;
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Dracon/Modular Additions/Mass Exit Update")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MA_Exit_Window));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        radius = EditorGUILayout.Slider("Radius", radius, 0, 1);
        offset = EditorGUILayout.Slider("Offset", offset, 0, 1);

        if (GUILayout.Button("Get Exits"))
        {
            List<MA_Exit> tempExits = new List<MA_Exit>();
            foreach (MA_Exit exit in Resources.FindObjectsOfTypeAll<MA_Exit>() as MA_Exit[])
            {
                if (!EditorUtility.IsPersistent(exit.transform.root.gameObject) && !(exit.hideFlags == HideFlags.NotEditable || exit.hideFlags == HideFlags.HideAndDontSave))
                    tempExits.Add(exit);
            }
            allExits = tempExits.ToArray();
        }

        GUILayout.Label("Current Exit Count: " + (allExits != null ? allExits.Length.ToString() : "0"));

        if (GUILayout.Button("Set Exit Statistics"))
        {
            if (allExits == null)
            {
                Debug.Log("Please refresh exits before attempting update");
            }
            foreach (MA_Exit exit in allExits)
            {
                exit.checkRadius = radius;
                exit.forwardOffset = offset;
            }
        }
        if (GUILayout.Button("Update Attached Status"))
        {
            if (allExits == null)
            {
                Debug.Log("Please refresh exits before attempting update");
            }
            foreach (MA_Exit exit in allExits)
            {
                exit.UpdateAttached(radius, offset);
            }
        }
    }
}
