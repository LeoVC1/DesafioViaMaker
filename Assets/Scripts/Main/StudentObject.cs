using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class StudentObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    private int classID;
    private int studentID;
    private string studentName;

    public void Activate(int classID, int studentID, string studentName)
    {
        this.classID = classID;
        this.studentID = studentID;
        this.studentName = studentName;
        nameText.text = studentName;
    }

    //public void ShowAllStudents()
    //{
    //    onStartLoadingStudents.Invoke();
    //    databaseManager.LoadStudents(classID, StudentsCallback);
    //}

    //private void StudentsCallback(Aluno[] students)
    //{
    //    onFinishLoadingStudents.Invoke(students);
    //}
}
