using System;

[Serializable]
public partial class EscolaResponse
{
    public string sucesso;

    public Escola retorno;
}

[Serializable]
public partial class Escola
{
    public int id;

    public string nome;

    public Turma[] turmas;
}

[Serializable]
public partial class Turma
{
    public int id;

    public string nome;
}

public partial class AlunosResponse
{
    public string sucesso;

    public Aluno[] retorno;
}

[Serializable]
public partial class Aluno
{
    public int id;

    public string nome;
}