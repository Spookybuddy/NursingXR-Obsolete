using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] HandComponents;
    public bool handState;
    public bool toggled;

    //Debug spatial mesh enable/disable
    public void WireframeToggle()
    {
        GameObject[] environment = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject piece in environment) piece.GetComponent<MeshRenderer>().enabled = toggled;
        toggled = !toggled;
    }

    public void ToggleHands()
    {
        for (int i = 0; i < HandComponents.Length; i++) HandComponents[i].SetActive(handState);
        handState = !handState;
    }
}