using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;
using System;

public class SchoolManager : MonoBehaviour
{
    [SerializeField]
    private ClassManager classManager; 
    [SerializeField]
    private TMP_Dropdown schoolSelector;

    public int currentSchool;

    private List<Escola> schools = new List<Escola>();

    public void DisplaySchools()
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();
        
        IDataReader reader = databaseAcess.GetAllData("School");

        schools.Clear();

        while (reader.Read())
        {
            schools.Add(new Escola
            {
                id = int.Parse(reader[0].ToString()),
                nome = reader[1].ToString()
            });
        }

        databaseAcess.Close();

        List<string> schoolNames = new List<string>();

        foreach(Escola school in schools)
        {
            schoolNames.Add(school.nome);
        }

        schoolSelector.ClearOptions();

        schoolSelector.AddOptions(schoolNames);

        ChangeSchool(0);
    }

    public void ChangeSchool(int newSchool)
    {
        classManager.DisplayAllClasses(schools[newSchool].id);
    }
}
