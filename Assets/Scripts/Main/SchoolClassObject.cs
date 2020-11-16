using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class SchoolClassObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private DataManager databaseManager;
    [SerializeField]
    private StudentsManager studentsManager;

    public UnityEvent onStartLoadingStudents;

    private int classID;
    private string className;

    public void Activate(int classID, string className)
    {
        this.classID = classID;
        this.className = className;
        nameText.text = className;
    }

    public void ShowAllStudents()
    {
        onStartLoadingStudents.Invoke();
        databaseManager.TryLoadStudents(classID, studentsManager.DisplayAllStudents);
        HeaderController.instance.className.text = nameText.text;
        HeaderController.instance.classID = classID;
    }
}
