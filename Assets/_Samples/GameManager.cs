using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] HandComponents;
    public bool handState;
    public bool toggled;
    public GameObject[] prefabs;

    //Debug spatial mesh enable/disable
    public void WireframeToggle()
    {
        GameObject[] environment = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject piece in environment) piece.GetComponent<MeshRenderer>().enabled = toggled;
        toggled = !toggled;
    }

    //Debug hand blocks 
    public void ToggleHands()
    {
        for (int i = 0; i < HandComponents.Length; i++) HandComponents[i].SetActive(handState);
        handState = !handState;
    }

    //Spawn in desired prefab
    public void SpawnPrefab(int index)
    {
        Instantiate(prefabs[index], transform.position, Quaternion.identity);
    }
}