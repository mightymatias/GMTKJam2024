using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbColliderSound : MonoBehaviour
{
    public AudioSource CrumbSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CrumbSound.Play();
    }
}
