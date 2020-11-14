using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SQLite : MonoBehaviour
{
    void Start()
    {
        /*
        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";

        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS my_table (id INTEGER PRIMARY KEY, val INTEGER )";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        // Insert values in table
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO my_table (id, val) VALUES (0, 5)";
        cmnd.ExecuteNonQuery();

        // Read and print all values in table
        IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query = "SELECT * FROM my_table";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		while (reader.Read())
		{
			Debug.Log("id: " + reader[0].ToString());
			Debug.Log("val: " + reader[1].ToString());
		}

		// Close connection
		dbcon.Close();
        */

        SQLiteHelper SqliteHelper = new SQLiteHelper();

        SqliteHelper.ExecuteCommand("CREATE TABLE IF NOT EXISTS Escola (ID INTEGER PRIMARY KEY NOT NULL, NAME VARCHAR(255) NOT NULL)");
        //SqliteHelper.ExecuteCommand("INSERT INTO Escola(ID, NAME) VALUES(1, 'Escola Viamaker')");
        //SqliteHelper.ExecuteCommand("INSERT INTO Escola(ID, NAME) VALUES(2, 'Escola Ezequiel')");
        //SqliteHelper.ExecuteCommand("INSERT INTO Escola(ID, NAME) VALUES(3, 'Escola Enéas')");
        //SqliteHelper.ExecuteCommand("INSERT INTO Escola(ID, NAME) VALUES(4, 'Escola Uirapuru')");

        IDataReader reader = SqliteHelper.GetAllData("Escola");

        while (reader.Read())
        {
            Debug.Log("ID: " + reader[0].ToString());
            Debug.Log("NAME: " + reader[1].ToString());
        }
    }

	void Update()
    {
        
    }
}
