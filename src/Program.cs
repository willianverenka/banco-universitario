using Npgsql;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).ToString())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("ConnectionString");

await using var db = NpgsqlDataSource.Create(connectionString);

var operacoes = new Operacoes(db);

Console.WriteLine("Iniciando inserção de dados");
        
await operacoes.InserirProfessores();
await operacoes.InserirAlunos();
await operacoes.InserirDepartamentos();
await operacoes.InserirCursos();
await operacoes.InserirDisciplinas();
await operacoes.InserirMatrizesCurriculares();
await operacoes.InserirDisciplinasMatrizes();
await operacoes.InserirOferecimentos();
await operacoes.InserirMatriculas();
await operacoes.InserirTCCs();
await operacoes.InserirAlunosTCCs();
    
Console.WriteLine("Dados inseridos com sucesso");