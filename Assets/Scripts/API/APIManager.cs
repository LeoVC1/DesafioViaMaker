using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using UnityEngine.Networking;
using UnityEngine.Events;
using System;

public class APIManager : MonoBehaviour
{
	private readonly string basePath = "http://uniescolas.viamaker.com.br/api";

    private void Start()
    {
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer 8mspL8yN09CgSQ3sgMfwQkfNm2bO64NW2789Wo0EodONKcuKeUtu1taZjG3Wu5XQUi61uxIZiDqxlxuaoZW9LJ5Hj992DNp6H0pk1wA6h4CZdtZkV6fv5xv8mKcFmkQe";
        RestClient.DefaultRequestHeaders["content-type"] = "application/json";
    }

    /// <summary>
    /// Retorna as informações da Escola
    /// </summary>
    /// <param name="onSucess">Callback de sucesso</param>
    /// <param name="onError">Callback de erro</param>
    public void GetEscola(Action<EscolaResponse> onSucess = null, Action<Exception> onError = null)
	{
        RestClient.Get(basePath + "/obter/escola").Then(proxy =>
        {
            EscolaResponse response = JsonUtility.FromJson<EscolaResponse>(proxy.Text);

            LogMessage("Response", JsonUtility.ToJson(response, true));

            onSucess.Invoke(response);

        }).Catch(err => {

            LogMessage("Error", err.Message);

            onError.Invoke(err);
        });
    }

    /// <summary>
    /// Retorna uma lista de Alunos de uma Turma especificada pelo parametro turmaId
    /// </summary>
    /// <param name="turmaId">Número da turma</param>
    /// <param name="onSucess">Callback de sucesso</param>
    /// <param name="onError">Callback de erro</param>
    public void GetAlunos(int turmaId, Action<AlunosResponse> onSucess = null, Action<Exception> onError = null)
    {
        RestClient.Post(basePath + "/listar/alunos/turma", new TurmaRequest { turmaId = turmaId }).Then(proxy =>
        {
            AlunosResponse response = JsonUtility.FromJson<AlunosResponse>(proxy.Text);

            LogMessage("Response", JsonUtility.ToJson(response, true));

            onSucess.Invoke(response);

        }).Catch(err => {

            LogMessage("Error", err.Message);

            onError.Invoke(err);

        });
    }

    private void LogMessage(string title, string message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
    }
}
