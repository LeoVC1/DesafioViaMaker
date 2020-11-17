using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class StudentsManager : MonoBehaviour
{
    [Header("Object References:")]
    public DataManager dataManager;
    public StudentObject[] studentPool; //Object Pool pois métodos como Instantiate e Destroy são mais pesados para o processamento em um mobile
    public RectTransform studentsPanel;
    private Aluno[] currentStudents;

    public UnityEvent onDisplayAllStudents;

    public void RefreshStudents()
    {
        dataManager.LoadStudentsFromDatabase(HeaderController.instance.classID, DisplayAllStudents);
    }

    public void DisplayAllStudents(Aluno[] students)
    {
        currentStudents = students;

        foreach (StudentObject studentsObject in studentPool)
        {
            studentsObject.gameObject.SetActive(false);
        }

        for(int i = 0; i < students.Length; i++)
        {
            studentPool[i].Activate(students[i].turmaId, students[i].id, students[i].nome);
            studentPool[i].gameObject.SetActive(true);
        }

        HeaderController.instance.OnDisplayStudents();

        onDisplayAllStudents.Invoke();
    }

    public void DisplayNewStudent(Aluno student)
    {
        foreach (StudentObject studentsObject in studentPool)
        {
            if (!studentsObject.gameObject.activeInHierarchy)
            {
                studentsObject.Activate(student.turmaId, student.id, student.nome);
                studentsObject.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void ShowStudentsPanel(bool show)
    {
        float direction = show ? 0 : Screen.currentResolution.height;

        studentsPanel.DOLocalMoveX(direction, 1).SetEase(Ease.OutSine);
    }
}
