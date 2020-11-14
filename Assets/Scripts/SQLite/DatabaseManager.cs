using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    void Start()
    {
        //DatabaseAcess databaseAcess = new DatabaseAcess();

        //databaseAcess.DeleteAllData("School");
        //databaseAcess.DeleteAllData("Class");

        //databaseAcess.TryCreateTable("School", "SchoolID INTEGER PRIMARY KEY NOT NULL, SchoolName VARCHAR(255) NOT NULL");
        //databaseAcess.TryCreateTable("Class", "SchoolID INTEGER, ClassID INTEGER PRIMARY KEY NOT NULL, ClassName VARCHAR(255) NOT NULL, FOREIGN KEY (SchoolID) REFERENCES Escola(SchoolID)");
        //databaseAcess.TryInsertIntoTable("School", "SchoolID, SchoolName", "1, 'Escola Santos'");
        //databaseAcess.TryInsertIntoTable("School", "SchoolID, SchoolName", "2, 'Escola AAA'");
        //databaseAcess.TryUpdateValues("School", "SchoolName = 'Escola Ezquiel'", "SchoolID = 2");

        //databaseAcess.Close();
    }

    /// <summary>
    /// Checa se já existe dados salvos localmente.
    /// </summary>
    /// <returns></returns>
    public bool LocalDataExists()
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        bool localDataExists = databaseAcess.TryCreateTable("School", "SchoolID INTEGER PRIMARY KEY NOT NULL, SchoolName VARCHAR(255) NOT NULL");

        databaseAcess.Close();

        return localDataExists;
    }

    /// <summary>
    /// Cria uma nova tabela com a Escola recebida como parametro.
    /// </summary>
    /// <param name="novaEscola"></param>
    public void CreateNewSchool(Escola novaEscola)
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        databaseAcess.TryCreateTable("School", "SchoolID INTEGER PRIMARY KEY NOT NULL, SchoolName VARCHAR(255) NOT NULL");

        databaseAcess.TryInsertIntoTable("School", "SchoolID, SchoolName", $"{novaEscola.id}, '{novaEscola.nome}'");

        IDataReader reader = databaseAcess.GetAllData("School");

        while (reader.Read())
        {
            Debug.Log("id: " + reader[0]);
            Debug.Log("nome: " + reader[1]);
        }

        databaseAcess.Close();

        CreateScholClasses(novaEscola);
    }

    public void CreateScholClasses(Escola escola)
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        databaseAcess.TryCreateTable("Class", "SchoolID INTEGER, ClassID INTEGER PRIMARY KEY NOT NULL, ClassName VARCHAR(255) NOT NULL, FOREIGN KEY (SchoolID) REFERENCES Escola(SchoolID)");

        foreach(Turma turma in escola.turmas)
        {
            databaseAcess.TryInsertIntoTable("Class", $"SchoolID, ClassID, ClassName", $"{escola.id}, {turma.id}, '{turma.nome}'");
        }

        IDataReader reader = databaseAcess.GetAllData("Class");

        while (reader.Read())
        {
            Debug.Log("escola da turma: " + reader[0]);
            Debug.Log("id da turma: " + reader[1]);
            Debug.Log("nome da turma: " + reader[2]);
        }

        databaseAcess.Close();
    }
}
