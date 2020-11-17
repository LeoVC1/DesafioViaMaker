using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ViewStudentModal : ModalController
{
    private Aluno currentStudent;

    [SerializeField]
    private TextMeshProUGUI studentName;
    [SerializeField]
    private GameObject defaultView;
    [SerializeField]
    private GameObject editView;
    [SerializeField]
    private GameObject deletView;
    [SerializeField]
    private TMP_InputField newNameField;
    [SerializeField]
    private Button saveNewNameButton;

    public void OpenModal(Aluno aluno)
    {
        base.OpenModal();

        currentStudent = aluno;
        studentName.text = aluno.nome;
    }

    public void EditStudent()
    {
        defaultView.SetActive(false);
        editView.SetActive(true);
    }

    public void CheckNewNameLength(string text)
    {
        if (text.Length > 0)
            saveNewNameButton.interactable = true;
        else
            saveNewNameButton.interactable = false;
    }

    public void SaveStudent()
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();
        databaseAcess.TryUpdateValues("Student", $"StudentName = '{newNameField.text}'", $"StudentID = {currentStudent.id}");
        databaseAcess.Close();
        studentName.text = newNameField.text;
        CancelOperation();
    }

    public void DeleteStudent()
    {
        defaultView.SetActive(false);
        deletView.SetActive(true);
    }

    public void ConfirmDelete()
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();
        databaseAcess.DeleteData("Student", $"StudentID = {currentStudent.id}");
        databaseAcess.Close();
        CancelOperation();
        CloseModal();
    }

    public void CancelOperation()
    {
        newNameField.text = string.Empty;
        SetDefaultView();
    }

    public void SetDefaultView()
    {
        defaultView.SetActive(true);
        editView.SetActive(false);
        deletView.SetActive(false);
    }
}
