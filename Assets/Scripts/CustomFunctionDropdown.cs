using UnityEditor;
using UnityEngine;
using System.Reflection;
//TODO: Abstract this type to all functions, not just triggerScript
[CustomEditor(typeof(TriggerScript))]
public class CustomFunctionDropdown : Editor
{
    int _selected;
    string[] _options;
    string[] _tags;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TriggerScript script = target as TriggerScript;

        //Tags - Working on getting the Enum multi-select on this
        _tags = UnityEditorInternal.InternalEditorUtility.tags;

        //Find and show the functions if the script is not null
        if (script.script != null) {
            //Get methods from given script
            MethodInfo[] methods = script.script.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            _options = new string[methods.Length];

            //Update the option listing from the script, and if a value has already been set update the _selected value
            for (int i = 0; i < methods.Length; i++) {
                _options[i] = methods[i].Name;
                if (script.scriptName.Equals(methods[i].Name)) this._selected = i;
            }

            //Dropdown listing of functions, passing the chosen value back to the script on change
            EditorGUI.BeginChangeCheck();
            this._selected = EditorGUILayout.Popup("Chosen Function", _selected, _options);
            if (EditorGUI.EndChangeCheck()) script.scriptName = methods[_selected].Name;
        }
    }
}