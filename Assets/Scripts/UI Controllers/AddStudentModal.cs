using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddStudentModal : ModalController
{
    public DataManager databaseManager;
    public StudentsManager studentsManager;

    public TMP_InputField inputField;

    public Button btnAddNew;

    public void CheckNameLength(string name)
    {
        if (name.Length > 0)
            btnAddNew.interactable = true;
        else
            btnAddNew.interactable = false;
    }

    public void AddNewStudent()
    {
        int newStudentID = databaseManager.AddNewStudent(inputField.text, HeaderController.instance.classID);
        studentsManager.DisplayNewStudent(new Aluno
        {
            turmaId = HeaderController.instance.classID,
            id = newStudentID,
            nome = inputField.text
        });
        CloseModal();
    }
}
