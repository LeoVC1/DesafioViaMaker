using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class StudentObject : MonoBehaviour
{
    public ViewStudentModal viewStudentModal;

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

    public void ViewStudent()
    {
        viewStudentModal.OpenModal(new Aluno
        {
            turmaId = classID,
            id = studentID,
            nome = studentName
        });
    }
}
