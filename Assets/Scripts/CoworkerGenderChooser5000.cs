using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoworkerGenderChooser5000 : MonoBehaviour
{
    public GameObject male, female;
    private void Start ()
    {
        bool isMale = Random.value < 0.5f;

        male.SetActive (isMale);
        female.SetActive (!isMale);
    }
}
