using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public DatabaseManager databaseManager;
    public APIManager apiManager;

    void Start()
    {
        if (databaseManager.LocalDataExists())
        {
            Debug.Log("Já existe dados salvos localmente.");
            return;
        }

        apiManager.GetEscola(OnGetEscola, OnError);
    }

    public void OnGetEscola(EscolaResponse escolaResponse)
    {
        if (escolaResponse.sucesso == "false")
        {
            Debug.Log("API retornou um falso sucesso.");
            return;
        }

        if(escolaResponse.retorno == null)
        {
            Debug.Log("API retornou um retorno nulo.");
            return;
        }

        databaseManager.CreateNewSchool(escolaResponse.retorno);
    }

    public void OnError(Exception exception)
    {
        Debug.Log("Falha ao se conectar com a API.");
    }
}
