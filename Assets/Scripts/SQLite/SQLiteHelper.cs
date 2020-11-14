using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SQLiteHelper
{
    private const string database_name = "DatabaseViaMakerApp";

    public string db_connection_string;

    public IDbConnection db_connection;

    public SQLiteHelper()
    {
        db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + database_name;
        Debug.Log("db_connection_string" + db_connection_string);
        db_connection = new SqliteConnection(db_connection_string);
        db_connection.Open();
    }

    ~SQLiteHelper()
    {
        db_connection.Close();
    }

    public void ExecuteCommand(string commandText)
    {
        // Create table
        IDbCommand dbcmd;

        dbcmd = GetDbCommand();

        dbcmd.CommandText = commandText;

        try
        {
            dbcmd.ExecuteReader();
        }
        catch
        {
            Debug.Log("Falha ao executar o comando: " + commandText);
        }
    }

    //helper functions
    public IDbCommand GetDbCommand()
    {
        return db_connection.CreateCommand();
    }

    public IDataReader GetAllData(string table_name)
    {
        IDbCommand dbcmd = db_connection.CreateCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + table_name;
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }

    public void DeleteAllData(string table_name)
    {
        IDbCommand dbcmd = db_connection.CreateCommand();
        dbcmd.CommandText = "DROP TABLE IF EXISTS " + table_name;
        dbcmd.ExecuteNonQuery();
    }

    public IDataReader GetNumOfRows(string table_name)
    {
        IDbCommand dbcmd = db_connection.CreateCommand();
        dbcmd.CommandText =
            "SELECT COALESCE(MAX(id)+1, 0) FROM " + table_name;
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }

    public void Close()
    {
        db_connection.Close();
    }
}