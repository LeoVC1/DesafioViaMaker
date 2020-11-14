using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseAcess
{
    private const string database_name = "DatabaseViaMakerApp";
    private string db_connection_string;
    private IDbConnection db_connection;

    public DatabaseAcess()
    {
        db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + database_name;
        db_connection = new SqliteConnection(db_connection_string);
        db_connection.Open();
    }

    ~DatabaseAcess()
    {
        db_connection.Close();
    }

    /// <summary>
    /// Tenta executar um comando NonQuery, retorna false caso o comando não tenha sido executado
    /// </summary>
    /// <param name="commandText">Comando a ser executado</param>
    /// <returns></returns>
    public bool TryExecuteCommand(string commandText)
    {
        IDbCommand dbcmd;

        dbcmd = db_connection.CreateCommand();

        dbcmd.CommandText = commandText;

        try
        {
            dbcmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            Debug.Log("Comando não executado: " + commandText);
            return false;
        }
    }

    /// <summary>
    /// Tenta criar uma tabela
    /// </summary>
    /// <param name="tableName">Nome da tabela</param>
    /// <param name="properties">Propiedades da tabela (Colunas)</param>
    /// <returns></returns>
    public bool TryCreateTable(string tableName, string properties)
    {
        return TryExecuteCommand($"CREATE TABLE {tableName} ({properties})");
    }

    /// <summary>
    /// Tenta inserir valores em uma determinada tabela
    /// </summary>
    /// <param name="tableName">Nome da tabela a ser inserida.</param>
    /// <param name="references">Colunas da tabela a serem inseridas.</param>
    /// <param name="values">Valores de cada coluna a serem inseridos./param>
    /// <returns></returns>
    public bool TryInsertIntoTable(string tableName, string references, string values)
    {
        return TryExecuteCommand($"INSERT INTO {tableName}({references}) VALUES({values})");
    }

    /// <summary>
    /// Tenta atualizar os valores de uma tabela
    /// </summary>
    /// <param name="tableName">Nome da tabela a ser modificada.</param>
    /// <param name="newValues">Novos valores.</param>
    /// <param name="condition">Condiçao para a modificação.</param>
    /// <returns></returns>
    public bool TryUpdateValues(string tableName, string newValues, string condition)
    {
        return TryExecuteCommand($"UPDATE {tableName} SET {newValues} WHERE {condition}");
    }

    public IDataReader GetAllData(string tableName)
    {
        IDbCommand dbcmd = db_connection.CreateCommand();
        dbcmd.CommandText = "SELECT * FROM " + tableName;
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }

    public void DeleteAllData(string tableName)
    {
        bool sucess = TryExecuteCommand("DROP TABLE IF EXISTS " + tableName);

        Debug.Log(sucess ? $"Tabela {tableName} deletada com sucesso." : $"Falha ao deletar a tabela {tableName}");
    }

    public void Close()
    {
        db_connection.Close();
    }
}