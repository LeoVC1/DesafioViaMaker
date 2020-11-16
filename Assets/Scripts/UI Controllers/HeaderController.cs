using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class HeaderController : MonoBehaviour
{
    public static HeaderController instance;

    public GameObject schoolObject;
    public GameObject classObject;
    public TextMeshProUGUI className;

    public UnityEvent onDisplayClasses;

    public int classID;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayClasses()
    {
        schoolObject.SetActive(true);
        classObject.SetActive(false);

        onDisplayClasses.Invoke();
    }

    public void OnDisplayStudents()
    {
        schoolObject.SetActive(false);
        classObject.SetActive(true);
    }
}
