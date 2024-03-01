using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDetection : MonoBehaviour
{
    public bool rightHand;
    public AudioClip[] sfx;
    public AudioSource source;
    public Transform head;

    //Hand is the trigger, and detects specified tags on other colliders
    void OnTriggerEnter(Collider obj)
    {
        switch (obj.tag) {
            case "Hand":
                source.PlayOneShot(sfx[3], 1);
                break;
            case "Wound":
                Destroy(obj.gameObject);
                source.PlayOneShot(sfx[2], 1);
                break;
            case "Respawn":
                source.PlayOneShot(sfx[2], 1);
                break;
            case "Finger R":
                if (Vector3.Distance(transform.position, head.position) < 1) {
                    if (rightHand) source.PlayOneShot(sfx[0], 1);
                    else source.PlayOneShot(sfx[1], 1);
                }
                break;
            case "Finger L":
                if (Vector3.Distance(transform.position, head.position) < 1) {
                    if (!rightHand) source.PlayOneShot(sfx[0], 1);
                    else source.PlayOneShot(sfx[1], 1);
                }
                break;
            default:
                break;
        }
    }
}