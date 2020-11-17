using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// Busca as turmas na Database e as lista atraves do Método DisplayAllClasses
/// </summary>
public class ClassManager : MonoBehaviour
{
    [Header("Object References:")]
    public SchoolClassObject[] classPool; //Object Pool pois métodos como Instantiate e Destroy são mais pesados para o processamento em um mobile
    public RectTransform classesPanel;

    public UnityEvent onDisplayAllClasses;

    public void DisplayAllClasses(int school)
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        IDataReader reader = databaseAcess.GetData("Class", "*", $"SchoolID = {school}");

        foreach(SchoolClassObject schoolClass in classPool)
        {
            schoolClass.gameObject.SetActive(false);
        }

        int i = 0;

        while (reader.Read())
        {
            classPool[i].Activate(int.Parse(reader[1].ToString()), reader[2].ToString());
            classPool[i].gameObject.SetActive(true);
            i++;
        }

        databaseAcess.Close();

        onDisplayAllClasses.Invoke();
    }

    public void ShowClassesPanel(bool show)
    {
        float direction = show ? 0 : -Screen.currentResolution.height;

        classesPanel.DOLocalMoveX(direction, 1).SetEase(Ease.OutSine);
    }
}
