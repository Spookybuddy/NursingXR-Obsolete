using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScarMaster : MonoBehaviour
{
    public ARMeshManager spatialization;
    public Transform headMarker;
    public GameObject scar;
    public int amount;
    public float width;
    public float height;
    public LineRenderer alignment;
    public float checkRange;
    public LayerMask spatialLayer;

    private bool holding;

    //Removes all wounds
    public void ClearBody()
    {
        holding = true;
        alignment.enabled = true;
        StartCoroutine(DragUpdate());
        GameObject[] scars = GameObject.FindGameObjectsWithTag("Wound");
        for (int i = 0; i < scars.Length; i++) Destroy(scars[i]);
    }

    //Finds the mesh for and places wounds upon the dummy
    public void DecorateBody()
    {
        //Enable the spatial mesh & disable the line
        holding = false;
        alignment.enabled = false;
        spatialization.enabled = true;

        //Attempt to find a place for the body
        if (Physics.Raycast(headMarker.position, Vector3.down, checkRange, spatialLayer)) {
            spatialization.enabled = false;
        } else {
            spatialization.enabled = false;
            return;
        }

        //Spawns scars in the bounds shown, using the normals of the generated spatial mesh
        for (int i = 0; i < amount; i++) {
            //Magnitude relative to head position + rotation
            float X = Random.Range(-width, width);
            float Z = Random.Range(0, height);
            Vector3 GlobalRelativePosition = (headMarker.right * X) + (-headMarker.forward * Z) + headMarker.position;

            //Check for surface in area, until X scars have spawned
            if (Physics.Raycast(GlobalRelativePosition, Vector3.down, out RaycastHit body, checkRange, spatialLayer)) {
                GameObject wound = Instantiate(scar, body.point, Quaternion.LookRotation(-body.normal), transform);
                wound.transform.localScale = new Vector3(Random.Range(0.125f, 0.375f), Random.Range(0.125f, 0.375f), 1);
            } else {
                i--;
            }
        }

        //Moves the head marker out of reach to prevent accidental moving
        headMarker.position += Vector3.up;

        //Disabled the spatial mesh once it has served its purpose
        spatialization.enabled = false;
    }

    //Update the alignment line to aid in placement only when dragging the marker
    private IEnumerator DragUpdate()
    {
        yield return new WaitForEndOfFrame();

        //Set start point to the marker, and the bottom to either the surface hit or the range below the marker
        alignment.SetPosition(0, headMarker.position);
        if (Physics.Raycast(headMarker.position, Vector3.down, out RaycastHit surface, checkRange, spatialLayer)) alignment.SetPosition(1, surface.point);
        else alignment.SetPosition(1, new Vector3(headMarker.position.x, headMarker.position.y - checkRange, headMarker.position.z));

        //Recurse only when still being held
        if (holding) StartCoroutine(DragUpdate());
    }
}