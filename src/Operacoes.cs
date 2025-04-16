using Npgsql;

public class Operacoes
{
    private readonly NpgsqlDataSource _db;
    private Dictionary<string, List<int>> _ids;
    public Operacoes(NpgsqlDataSource db)
    {
        _db = db;
        _ids = new Dictionary<string, List<int>>();
        
        _ids["professor"] = new List<int>();
        _ids["aluno"] = new List<int>();
        _ids["departamento"] = new List<int>();
        _ids["curso"] = new List<int>();
        _ids["disciplina"] = new List<int>();
        _ids["matriz_curricular"] = new List<int>();
        _ids["disciplina_matriz"] = new List<int>();
        _ids["oferecimento"] = new List<int>();
        _ids["matricula"] = new List<int>();
        _ids["tcc"] = new List<int>();
        _ids["aluno_tcc"] = new List<int>();
    }

    public async Task InserirProfessores()
    {
        string[] nomes = { 
            "Ana Silva", "Carlos Oliveira", "Maria Santos", "João Pereira", 
            "Fernanda Costa", "Roberto Almeida", "Juliana Ferreira", "Pedro Souza",
            "Luciana Martins", "André Rodrigues", "Patrícia Lima", "Ricardo Gomes" 
        };
        
        Console.WriteLine("Inserindo professores");
        
        for (int i = 0; i < nomes.Length; i++)
        {
            string nome = nomes[i];
            int idade = new Random().Next(30, 70);
            bool ativo = true;
            
            await using var cmd = _db.CreateCommand(
                "insert into professor (nome, idade, ativo) values ($1, $2, $3) returning id");
            cmd.Parameters.AddWithValue(nome);
            cmd.Parameters.AddWithValue(idade);
            cmd.Parameters.AddWithValue(ativo);
            
            int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            _ids["professor"].Add(id);
            
            Console.WriteLine($"Professor: {nome}, ID: {id}");
            }
    }

    public async Task InserirAlunos()
    {
        string[] nomes = { 
            "José Oliveira", "Mariana Costa", "Gabriel Santos", "Carolina Pereira", 
            "Lucas Ferreira", "Isabela Silva", "Felipe Souza", "Amanda Rodrigues",
            "Bruno Almeida", "Larissa Lima", "Thiago Martins", "Natália Gomes",
            "Leonardo Campos", "Bianca Ribeiro", "Guilherme Nunes", "Camila Rocha",
            "Rafael Cardoso", "Daniela Moreira", "Marcos Barbosa", "Júlia Castro"
        };
        
        Console.WriteLine("Inserindo alunos");
        
        for (int i = 0; i < nomes.Length; i++)
        {
            string nome = nomes[i];
            string ra = $"RA{100000 + i}";
            int idade = new Random().Next(18, 30);
            bool ativo = true;
            
            await using var cmd = _db.CreateCommand(
                "insert into aluno (ra, nome, idade, ativo) values ($1, $2, $3, $4) returning id");
            cmd.Parameters.AddWithValue(ra);
            cmd.Parameters.AddWithValue(nome);
            cmd.Parameters.AddWithValue(idade);
            cmd.Parameters.AddWithValue(ativo);
            
            int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            _ids["aluno"].Add(id);
            
            Console.WriteLine($"Aluno: {nome}, RA: {ra}, ID: {id}");
        }
    }

    public async Task InserirDepartamentos()
    {
    string[] nomes = { 
        "Departamento de Ciência da Computação", 
        "Departamento de Matemática", 
        "Departamento de Física", 
        "Departamento de Engenharia" 
    };
    
    Random rand = new Random();
    
    Console.WriteLine("Inserindo departamentos");
    
    for (int i = 0; i < nomes.Length; i++)
    {
        string nome = nomes[i];
        int chefeId = _ids["professor"][rand.Next(_ids["professor"].Count)];
        
        await using var cmd = _db.CreateCommand(
            "insert into departamento (nome, chefe_id) values ($1, $2) returning id");
        cmd.Parameters.AddWithValue(nome);
        cmd.Parameters.AddWithValue(chefeId);
        
        int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        _ids["departamento"].Add(id);
        
        Console.WriteLine($"Departamento: {nome}, ID: {id}, Chefe: {chefeId}");
        }
    }

    public async Task InserirCursos()
    {
    string[][] cursos = { 
        new[] { "CC", "Ciência da Computação" },
        new[] { "CD", "Ciência de Dados" },
        new[] { "SI", "Sistemas de Informação" },
        new[] { "EC", "Engenharia da Computação" }
    };
    
    Random rand = new Random();
    
    Console.WriteLine("Inserindo cursos");
    
    for (int i = 0; i < cursos.Length; i++)
    {
        string codigo = cursos[i][0];
        string nome = cursos[i][1];
        
        
        var professorIds = new List<int>(_ids["professor"]);
        int coordenadorId = professorIds[rand.Next(professorIds.Count)];
        
        await using var cmd = _db.CreateCommand(
            "insert into curso (codigo, nome, coordenador_id) values ($1, $2, $3) returning id");
        cmd.Parameters.AddWithValue(codigo);
        cmd.Parameters.AddWithValue(nome);
        cmd.Parameters.AddWithValue(coordenadorId);
        
        int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        _ids["curso"].Add(id);
        
        Console.WriteLine($"Curso: {nome}, ID: {id}, Coordenador: {coordenadorId}");
        }
    }

    public async Task InserirDisciplinas()
    {
    string[][] disciplinas = {
        
        new[] { "CC001", "Algoritmos e Estruturas de Dados", "0" },
        new[] { "CC002", "Programação Orientada a Objetos", "0" },
        new[] { "CC003", "Banco de Dados", "0" },
        new[] { "CC004", "Sistemas Operacionais", "0" },
        new[] { "MAT001", "Cálculo I", "1" },
        new[] { "MAT002", "Cálculo II", "1" },
        new[] { "MAT003", "Álgebra Linear", "1" },
        new[] { "MAT004", "Estatística", "1" },
        new[] { "FIS001", "Física I", "2" },
        new[] { "FIS002", "Física II", "2" },
        new[] { "ENG001", "Circuitos Elétricos", "3" },
        new[] { "ENG002", "Arquitetura de Computadores", "3" },
        new[] { "CD001", "Mineração de Dados", "0" },
        new[] { "CD002", "Machine Learning", "0" },
        new[] { "SI001", "Gestão de Projetos", "0" }
    };
    
    Console.WriteLine("Inserindo disciplinas");
    
    for (int i = 0; i < disciplinas.Length; i++)
    {
        string codigo = disciplinas[i][0];
        string nome = disciplinas[i][1];
        int departamentoIndex = int.Parse(disciplinas[i][2]);
        int departamentoId = _ids["departamento"][departamentoIndex];
        
        await using var cmd = _db.CreateCommand(
            "insert into disciplina (codigo, nome, departamento_id) values ($1, $2, $3) returning id");
        cmd.Parameters.AddWithValue(codigo);
        cmd.Parameters.AddWithValue(nome);
        cmd.Parameters.AddWithValue(departamentoId);
        
        int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        _ids["disciplina"].Add(id);
        
        Console.WriteLine($"Disciplina: {nome}, ID: {id}, Departamento: {departamentoId}");
        }
    }

    public async Task InserirMatrizesCurriculares()
    {
        Console.WriteLine("Inserindo matrizes curriculares");
        
        for (int i = 0; i < _ids["curso"].Count; i++)
        {
            int cursoId = _ids["curso"][i];
            
            await using var cmd = _db.CreateCommand(
                "insert into matriz_curricular (curso_id) values ($1) returning id");
            cmd.Parameters.AddWithValue(cursoId);
            
            int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            _ids["matriz_curricular"].Add(id);
            
            Console.WriteLine($"Matriz Curricular: ID: {id}, Curso: {cursoId}");
        }
    }

    public async Task InserirDisciplinasMatrizes()
    {
        
        Dictionary<int, List<(int disciplinaId, string semestre)>> disciplinasPorCurso = new();
        
        
        disciplinasPorCurso[0] = new List<(int, string)>
        {
            (0, "1"), 
            (4, "1"), 
            (6, "1"), 
            (8, "1"), 
            (1, "2"), 
            (5, "2"), 
            (9, "2"), 
            (2, "3"), 
            (11, "3"), 
            (3, "4"), 
            (7, "4"), 
            (12, "5"), 
            (13, "6"), 
            (14, "7")  
        };
    
    
        disciplinasPorCurso[1] = new List<(int, string)>
        {
            (0, "1"), 
            (4, "1"), 
            (6, "1"), 
            (1, "2"), 
            (5, "2"), 
            (7, "2"), 
            (2, "3"), 
            (12, "3"), 
            (13, "4"), 
            (14, "5")  
        };
        
        
        disciplinasPorCurso[2] = new List<(int, string)>
        {
            (0, "1"), 
            (4, "1"), 
            (1, "2"), 
            (2, "3"), 
            (14, "3"), 
            (3, "4")  
        };
        
        
        disciplinasPorCurso[3] = new List<(int, string)>
        {
            (0, "1"), 
            (4, "1"), 
            (8, "1"), 
            (1, "2"), 
            (5, "2"), 
            (9, "2"), 
            (10, "3"), 
            (11, "3"), 
            (3, "4"), 
            (2, "5")  
        };
        
        Console.WriteLine("Inserindo disciplinas nas matrizes");
        
        for (int cursoIndex = 0; cursoIndex < disciplinasPorCurso.Count; cursoIndex++)
        {
            int matrizId = _ids["matriz_curricular"][cursoIndex];
            var disciplinas = disciplinasPorCurso[cursoIndex];
            
            foreach (var (disciplinaIndex, semestre) in disciplinas)
            {
                int disciplinaId = _ids["disciplina"][disciplinaIndex];
                
                await using var cmd = _db.CreateCommand(
                    "insert into disciplina_matriz (matriz_id, disciplina_id, semestre) values ($1, $2, $3) returning id");
                cmd.Parameters.AddWithValue(matrizId);
                cmd.Parameters.AddWithValue(disciplinaId);
                cmd.Parameters.AddWithValue(semestre);
                
                int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                _ids["disciplina_matriz"].Add(id);
                
                Console.WriteLine($"Disciplina-Matriz: ID: {id}, Matriz: {matrizId}, Disciplina: {disciplinaId}, Semestre: {semestre}");
            }
        }
    }   

    public async Task InserirOferecimentos()
    {
        Console.WriteLine("Inserindo oferecimentos");
        Random rand = new Random();
        
        
        int[] anos = { 2023, 2024 };
        string[] semestres = { "1", "2" };
        
        foreach (int disciplinaId in _ids["disciplina"])
        {
            foreach (int ano in anos)
            {
                foreach (string semestre in semestres)
                {
                    
                    int professorId = _ids["professor"][rand.Next(_ids["professor"].Count)];
                    
                    await using var cmd = _db.CreateCommand(
                        "insert into oferecimento (disciplina_id, professor_id, semestre, ano) values ($1, $2, $3, $4) returning id");
                    cmd.Parameters.AddWithValue(disciplinaId);
                    cmd.Parameters.AddWithValue(professorId);
                    cmd.Parameters.AddWithValue(semestre);
                    cmd.Parameters.AddWithValue(ano);
                    
                    int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    _ids["oferecimento"].Add(id);
                    
                    Console.WriteLine($"Oferecimento: ID: {id}, Disciplina: {disciplinaId}, Professor: {professorId}, {semestre}/{ano}");
                }
            }
        }
    }

    public async Task InserirMatriculas()
    {
        Console.WriteLine("Inserindo matrículas");
        Random rand = new Random();
        
        
        foreach (int alunoId in _ids["aluno"])
        {
            
            var oferecimentosAluno = new HashSet<int>();
            int totalOferecimentos = rand.Next(10, 16);
            
            while (oferecimentosAluno.Count < totalOferecimentos && oferecimentosAluno.Count < _ids["oferecimento"].Count)
            {
                int oferecimentoIndex = rand.Next(_ids["oferecimento"].Count);
                oferecimentosAluno.Add(_ids["oferecimento"][oferecimentoIndex]);
            }
            
            
            foreach (int oferecimentoId in oferecimentosAluno)
            {
                decimal nota = Math.Round((decimal)(rand.NextDouble() * 10), 2);
                bool aprovado = nota >= 6.0m;
                
                //a cada quinto estudante e setimo oferecimento, o aluno é reprovado
                if (alunoId % 5 == 0 && oferecimentoId % 7 == 0)
                {
                    
                    nota = Math.Round((decimal)(rand.NextDouble() * 5), 2);
                    aprovado = false;
                    
                    await using var cmd = _db.CreateCommand(
                        "insert into matricula (aluno_id, oferecimento_id, nota, aprovado) values ($1, $2, $3, $4) returning id");
                    cmd.Parameters.AddWithValue(alunoId);
                    cmd.Parameters.AddWithValue(oferecimentoId);
                    cmd.Parameters.AddWithValue(nota);
                    cmd.Parameters.AddWithValue(aprovado);
                    
                    int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    _ids["matricula"].Add(id);
                    
                    Console.WriteLine($"Matrícula (Reprovação): ID: {id}, Aluno: {alunoId}, Oferecimento: {oferecimentoId}, Nota: {nota}");
                    
                    
                    await using var cmdGetInfo = _db.CreateCommand(
                        "SELECT disciplina_id, semestre, ano FROM oferecimento WHERE id = $1");
                    cmdGetInfo.Parameters.AddWithValue(oferecimentoId);
                    
                    await using var reader = await cmdGetInfo.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int disciplinaId = reader.GetInt32(0);
                        string semestre = reader.GetString(1);
                        int ano = reader.GetInt32(2);
                        
                        await reader.CloseAsync();
                        
                        
                        await using var cmdNextOfer = _db.CreateCommand(
                            "SELECT id FROM oferecimento WHERE disciplina_id = $1 AND (ano > $2 OR (ano = $2 AND semestre > $3)) ORDER BY ano, semestre LIMIT 1");
                        cmdNextOfer.Parameters.AddWithValue(disciplinaId);
                        cmdNextOfer.Parameters.AddWithValue(ano);
                        cmdNextOfer.Parameters.AddWithValue(semestre);
                        
                        object nextOferObj = await cmdNextOfer.ExecuteScalarAsync();
                        // se encontrar um oferecimento apos a reprovacao do aluno, registra o aluno neste oferecimento e é aprovado nessa segunda tentativa
                        if (nextOferObj != null)
                        {
                            int nextOferId = Convert.ToInt32(nextOferObj);
                            nota = Math.Round((decimal)(rand.NextDouble() * 4 + 6), 2); 
                            aprovado = true;
                            
                            await using var cmdNextMatricula = _db.CreateCommand(
                                "insert into matricula (aluno_id, oferecimento_id, nota, aprovado) values ($1, $2, $3, $4) returning id");
                            cmdNextMatricula.Parameters.AddWithValue(alunoId);
                            cmdNextMatricula.Parameters.AddWithValue(nextOferId);
                            cmdNextMatricula.Parameters.AddWithValue(nota);
                            cmdNextMatricula.Parameters.AddWithValue(aprovado);
                            
                            int nextMatriculaId = Convert.ToInt32(await cmdNextMatricula.ExecuteScalarAsync());
                            _ids["matricula"].Add(nextMatriculaId);
                            
                            Console.WriteLine($"Matrícula (Aprovação Posterior): ID: {nextMatriculaId}, Aluno: {alunoId}, Oferecimento: {nextOferId}, Nota: {nota}");
                        }
                    }
                }
                else
                {
                    // verifica se a matrícula já existe
                    await using var checkCmd = _db.CreateCommand(
                        "select count(*) from matricula where aluno_id = $1 and oferecimento_id = $2");
                    checkCmd.Parameters.AddWithValue(alunoId);
                    checkCmd.Parameters.AddWithValue(oferecimentoId);
                    long count = (long)await checkCmd.ExecuteScalarAsync();

                    // insere se nao houver matricula para aquele aluno no oferecimento
                    if (count == 0)
                    {
                        await using var cmd = _db.CreateCommand(
                            "insert into matricula (aluno_id, oferecimento_id, nota, aprovado) values ($1, $2, $3, $4) returning id");
                        cmd.Parameters.AddWithValue(alunoId);
                        cmd.Parameters.AddWithValue(oferecimentoId);
                        cmd.Parameters.AddWithValue(nota);
                        cmd.Parameters.AddWithValue(aprovado);
                        int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        _ids["matricula"].Add(id);
                        Console.WriteLine($"Matrícula: ID: {id}, Aluno: {alunoId}, Oferecimento: {oferecimentoId}, Nota: {nota}, Aprovado: {aprovado}");
                    }
                    else
                    {
                        Console.WriteLine($"Matrícula já existe para Aluno: {alunoId}, Oferecimento: {oferecimentoId} - Ignorando");
                    }
                }
            }
        }
    }

    public async Task InserirTCCs()
    {
        Console.WriteLine("Inserindo TCCs");
        Random rand = new Random();
        
        string[] temas = {
            "Aplicação de Machine Learning em Saúde",
            "Sistema de Gestão Acadêmica",
            "Análise de Redes Sociais",
            "Realidade Virtual em Educação",
            "Segurança em IoT",
            "Blockchain em Serviços Financeiros",
            "Processamento de Linguagem Natural",
            "Aplicações de Visão Computacional",
            "Desenvolvimento de Jogos Educativos",
            "Sistemas Embarcados para Smart Cities"
        };
        
        int[] anos = { 2023, 2024 };
        string[] semestres = { "1", "2" };
        
        for (int i = 0; i < temas.Length; i++)
        {
            string tema = temas[i];
            int professorId = _ids["professor"][rand.Next(_ids["professor"].Count)];
            int ano = anos[rand.Next(anos.Length)];
            string semestre = semestres[rand.Next(semestres.Length)];
            
            await using var cmd = _db.CreateCommand(
                "insert into tcc (tema, professor_id, semestre, ano) values ($1, $2, $3, $4) returning id");
            cmd.Parameters.AddWithValue(tema);
            cmd.Parameters.AddWithValue(professorId);
            cmd.Parameters.AddWithValue(semestre);
            cmd.Parameters.AddWithValue(ano);
            
            int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            _ids["tcc"].Add(id);
            
            Console.WriteLine($"TCC: ID: {id}, Tema: {tema}, Professor: {professorId}, {semestre}/{ano}");
        }
    }

    public async Task InserirAlunosTCCs()
    {
        Console.WriteLine("Inserindo alunos nos TCCs");
        Random rand = new Random();
        
        foreach (int tccId in _ids["tcc"])
        {
            
            int quantidadeAlunos = rand.Next(1, 4);
            var alunosTcc = new HashSet<int>();
            
            while (alunosTcc.Count < quantidadeAlunos)
            {
                int alunoIndex = rand.Next(_ids["aluno"].Count);
                alunosTcc.Add(_ids["aluno"][alunoIndex]);
            }
            
            foreach (int alunoId in alunosTcc)
            {
                await using var cmd = _db.CreateCommand(
                    "insert into aluno_tcc (aluno_id, tcc_id) values ($1, $2) returning id");
                cmd.Parameters.AddWithValue(alunoId);
                cmd.Parameters.AddWithValue(tccId);
                
                int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                _ids["aluno_tcc"].Add(id);
                
                Console.WriteLine($"Aluno-TCC: ID: {id}, Aluno: {alunoId}, TCC: {tccId}");
            }
        }
    }
}