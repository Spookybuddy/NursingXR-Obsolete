using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//using UnityEngine.Events;

public enum TagList
{
    Untagged = 1,
    Respawn = 1 << 1,
    Finish = 1 << 2,
    EditorOnly = 1 << 3,
    MainCamera = 1 << 4,
    Player = 1 << 5,
    GameController = 1 << 6,
    Environment = 1 << 7,
    Hand = 1 << 8,
    FingerL = 1 << 9,
    FingerR = 1 << 10,
    Wound = 1 << 11
}

public enum CallOn
{
    Enter = 1,
    Stay = 1 << 1,
    Exit = 1 << 2
}

public enum Functions
{
    Duplicate = 1,
    Destroy = 1 << 1
}

public class TriggerScript : MonoBehaviour
{
    //Enumerator dropdown multi selections
    [EnumFlags]
    public TagList tags;
    [EnumFlags]
    public CallOn calledWhen;
    [EnumFlags]
    public Functions function;

    //Outside script object to call function of
    [Header("Outside Script")]
    public MonoBehaviour script;
    [HideInInspector]
    public string scriptName;

    //Using Unity Events might be the smarter choice, but I feel cooler using the custom Editor
    //public UnityEvent events;

    //Function to perform on entry
    void OnTriggerEnter(Collider collide)
    {
        if (CorrectTrigger(1)) TagCheck(collide);
    }

    //Function to perform while still inside
    void OnTriggerStay(Collider collide)
    {
        if (CorrectTrigger(2)) TagCheck(collide);
    }

    //Function to perform on exit
    void OnTriggerExit(Collider collide)
    {
        if (CorrectTrigger(4)) TagCheck(collide);
    }

    //Checks the listed valid tags
    private void TagCheck(Collider collide)
    {
        string[] tagged = CorrectTags();
        for (int i = 0; i < tagged.Length; i++) {
            if (collide.CompareTag(tagged[i])) CorrectFunction();
        }
    }

    //Returns whether the trigger function should be executed
    private bool CorrectTrigger(int bit)
    {
        int binary = (int)calledWhen;
        switch (binary) {
            //Everything
            case -1:
                return true;
            //Nothing
            case 0:
                return false;
            //Specifics
            default:
                //Checks binary against functions to determine if the bits match
                if (binary % 2 == bit) return true;
                if (Mathf.Min(binary % 4) == bit) return true;
                if (binary >= bit && bit > 3) return true;
                return false;
        }
    }

    //Returns tag list to check on objects
    private string[] CorrectTags()
    {
        //Nothing check first, returns nothing
        if ((int)tags == 0) return null;

        //Everything check, returns full listing
        string[] fullList = TagList.GetNames(typeof(TagList));
        if ((int)tags == -1) return fullList;

        //Other checks, recording all 1 bits to a list to convert to array
        List<string> append = new List<string>();
        for (int i = 0; i < fullList.Length; i++) {
            string binary = (System.Convert.ToString((int)tags >> i, 2));
            if (binary[binary.Length - 1] == '1') append.Add(fullList[i]);
        }
        return append.ToArray();
    }

    //Calls correct function
    private void CorrectFunction()
    {
        string[] fullList = Functions.GetNames(typeof(Functions));
        switch ((int)function) {
            //Everything
            case -1:
                for (int i = 0; i < fullList.Length; i++) Invoke(fullList[i], 0);
                return;
            //Nothing (Why would you need this?)
            case 0:
                return;
            //Specifics
            default:
                if (script != null) {
                    //Check all the function names, and if the scriptName matches any return. Failure to match results in resorting to this.function
                    MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    for (int i = 0; i < methods.Length; i++) {
                        if (scriptName.Equals(methods[i].Name)) {
                            script.Invoke(scriptName, 0);
                            return;
                        }
                    }
                }

                //Convert the Enum to the functions
                string binary = (System.Convert.ToString((int)function, 2));
                int length = binary.Length - 1;
                for (int i = length; i > -1; i--) {
                    if (binary[i] == '1') Invoke(fullList[length - i], 0);
                }
                return;
        }
    }

    //Functions to call -----------------------------------------------------------------------------------------------------------------------------
    public void Destroy()
    {

    }

    public void Duplicate()
    {

    }
}