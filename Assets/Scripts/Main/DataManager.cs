using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Classe que gerencia os principais métodos que buscam dados na API e os salvam na Database
/// </summary>
public class DataManager : MonoBehaviour
{
    public APIManager apiManager;

    public UnityEvent onFinishDataInitialization;

    private Action<Aluno[]> onLoadStudents; //Temporary allocation of the action

    #region Initialization
    void Start()
    {
        if (LocalDataExists())
        {
            Debug.Log("Já existe dados salvos localmente.");
            onFinishDataInitialization.Invoke();
            return;
        }

        apiManager.GetEscola(OnGetEscola, OnError);
    }

    /// <summary>
    /// Força um carregamento de dados da API.
    /// </summary>
    public void ForceLoadData()
    {
        apiManager.GetEscola(OnGetEscola, OnError);
    }

    public void OnGetEscola(EscolaResponse escolaResponse)
    {
        if (escolaResponse.sucesso == "false")
        {
            Debug.Log("API retornou um falso sucesso.");
            return;
        }

        if (escolaResponse.retorno == null)
        {
            Debug.Log("API retornou um retorno nulo.");
            return;
        }

        CreateNewSchool(escolaResponse.retorno);
    }

    public void OnError(Exception exception)
    {
        Debug.Log("Falha ao se conectar com a API.");
    }

    private void OnFinishDataInitialization()
    {
        onFinishDataInitialization.Invoke();
    }

    /// <summary>
    /// Checa se já existe dados salvos localmente.
    /// </summary>
    /// <returns></returns>
    public bool LocalDataExists()
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        IDataReader reader;

        int i = 0;

        try
        {
            reader = databaseAcess.GetData("School", "SchoolID");

            while (reader.Read())
            {
                i += int.Parse(reader[0].ToString());
            }

            databaseAcess.Close();
        }
        catch
        {
            return false;
        }

        return i == 0 ? false : true;
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

        //IDataReader reader = databaseAcess.GetAllData("School");

        //while (reader.Read())
        //{
        //    Debug.Log("id: " + reader[0]);
        //    Debug.Log("nome: " + reader[1]);
        //}

        databaseAcess.Close();

        CreateSchoolClasses(novaEscola);
    }

    /// <summary>
    /// Cria as turmas da Escola recebida como parametro
    /// </summary>
    /// <param name="escola"></param>
    /// <param name="onFinishDatabaseCreation"></param>
    public void CreateSchoolClasses(Escola escola)
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        databaseAcess.TryCreateTable("Class", "SchoolID INTEGER, ClassID INTEGER PRIMARY KEY NOT NULL, ClassName VARCHAR(255) NOT NULL, FOREIGN KEY (SchoolID) REFERENCES School(SchoolID)");

        foreach (Turma turma in escola.turmas)
        {
            databaseAcess.TryInsertIntoTable("Class", $"SchoolID, ClassID, ClassName", $"{escola.id}, {turma.id}, '{turma.nome}'");
            StartCoroutine(LoadStudentsOnServer(turma));
        }

        databaseAcess.Close();

        onFinishDataInitialization.Invoke();
    }

    /// <summary>
    /// Coroutine para possibilitar carregar varias turmas ao mesmo tempo
    /// </summary>
    /// <param name="turma"></param>
    /// <returns></returns>
    private IEnumerator LoadStudentsOnServer(Turma turma, bool forceLoad = false)
    {
        CreateStudents(turma.id, null);
        yield return null;
    }

    /// <summary>
    /// Carrega os alunos de uma determinada classe da API
    /// </summary>
    /// <param name="classID"></param>
    /// <param name="onLoadStudents"></param>
    public void CreateStudents(int classID, Action<Aluno[]> onLoadStudents = null)
    {
        apiManager.GetAlunos(classID, SaveStudents);
        this.onLoadStudents = onLoadStudents;
    }

    /// <summary>
    /// Cria os alunos de uma determinada turma atraves do retorno da API
    /// </summary>
    /// <param name="alunosResponse"></param>
    public void SaveStudents(AlunosResponse alunosResponse)
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        databaseAcess.TryCreateTable("Student", "SchoolID INTEGER, ClassID INTEGER, StudentID PRIMARY KEY NOT NULL, StudentName VARCHAR(255) NOT NULL, FOREIGN KEY (SchoolID) REFERENCES Escola(SchoolID), FOREIGN KEY (ClassID) REFERENCES Class(ClassID)");

        foreach (Aluno aluno in alunosResponse.retorno)
        {
            databaseAcess.TryInsertIntoTable("Student", $"SchoolID, ClassID, StudentID, StudentName", $"{1}, {alunosResponse.turmaId}, {aluno.id}, '{aluno.nome}'");
        }

        databaseAcess.Close();

        onLoadStudents?.Invoke(alunosResponse.retorno);
    }
    #endregion

    /// <summary>
    /// Adiciona um novo estudante em um turma especifica
    /// </summary>
    /// <param name="studentName"></param>
    /// <param name="studentClassID"></param>
    public int AddNewStudent(string studentName, int studentClassID)
    {
        DatabaseAcess databaseAcess = new DatabaseAcess();

        IDataReader reader = databaseAcess.GetData("Student", "StudentID");

        int newStudentID = -1;

        while (reader.Read())
        {
            newStudentID = int.Parse(reader[0].ToString()) + 1;
        }

        databaseAcess.TryInsertIntoTable("Student", $"SchoolID, ClassID, StudentID, StudentName", $"{1}, {studentClassID}, {newStudentID}, '{studentName}'");

        return newStudentID;
    }

    /// <summary>
    /// Carrega os alunos de uma determinada classe da Database
    /// </summary>
    /// <param name="classID"></param>
    /// <param name="onLoadStudents"></param>
    /// <param name="forceLoad"></param>
    public void LoadStudentsFromDatabase(int classID, Action<Aluno[]> onLoadStudents = null)
    {
        List<Aluno> students = new List<Aluno>();

        DatabaseAcess databaseAcess = new DatabaseAcess();

        IDataReader reader;

        try
        {
            reader = databaseAcess.GetData("Student", "StudentID, StudentName", $"ClassID = {classID}");

            while (reader.Read())
            {
                students.Add(new Aluno
                {
                    id = int.Parse(reader[0].ToString()),
                    nome = reader[1].ToString(),
                    turmaId = classID
                });
            }
        }
        catch
        {
            Debug.Log("Tabela de estudantes não encontrada.");
        }

        onLoadStudents?.Invoke(students.ToArray());
    }

}
