# Sistema de Banco de Dados Universitário

## Sobre o Projeto

Este projeto implementa um sistema de banco de dados para gerenciamento universitário que armazena e manipula informações sobre alunos, professores, departamentos, cursos, disciplinas, históricos escolares e TCCs. O banco de dados utilizado foi o Supabase, baseado no postgres, e a parte da aplicação de população e validação de dados foi feita em dotnet com C#.

### Participantes

Willian Verenka RA 22.124.081-5

## Preparando o projeto
Certifique-se de que tem o [dotnet 9 SDK instalado.](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0)

Caso esteja tudo certo, `dotnet --list-sdks` deve mostrar a versão correta do SDK.

Clone o projeto pelo terminal ou IDE de preferência. Pelo terminal:

`git clone https://github.com/willianverenka/banco-universitario.git`

## Conectando com o seu banco

Navegue até a raíz do projeto:
`cd cd banco-universitario/`

1. Abra o arquivo "appsettings.json" em seu editor de texto de preferência
2. Navegue até o supabase e entre na página da sua database.
3. Clique no botão do topo "Connect"
4. Troque o tipo para .NET e copie o valor mostrado no campo "DefaultConnection"
   ![image](https://github.com/user-attachments/assets/056b00d2-2109-495a-9128-ea75317b29e7)

      


5. Substitua o conteúdo desse campo
![image](https://github.com/user-attachments/assets/d1419b48-c539-46af-8463-6b73a0f0d7eb)

## Preparando seu banco

Antes de executarmos a aplicação, o seu banco de dados deve conter as tabelas do projeto. Você deve executar o conteúdo do arquivo "init.sql" no seu postgres ou supabase para inicializar o schema adequado.

## Executando a aplicação

Com a conexão do banco configurada e com as tabelas prontas, navegue até o source code:

`cd src/`

Execute o projeto:

`dotnet run`

A aplicação abrirá conexão com o seu banco e fará a inserção dos dados fictícios de forma automática. Cada linha de inserção é logada na mesma janela de execução e o programa é parado após a inserção dos dados.

Quando finalizado, você pode visualizar as queries no arquivo "queries.sql", que está uma subpasta acima: `cd ..` e executar na sua plataforma para visualizar os dados. 


Para queries que são arbitrárias, isto é, mostra a relação de um único usuário, sua escolha é feita com base no primeira entrada da tabela em questão.

## Diagramas

```mermaid

erDiagram
    aluno {
        serial id PK
        string ra
        string nome
        smallint idade
        boolean ativo
    }
    
    professor {
        serial id PK
        string nome
        smallint idade
        boolean ativo
    }
    
    curso {
        serial id PK
        string codigo
        string nome
        int coordenador_id FK
    }
    
    departamento {
        serial id PK
        string nome
        int chefe_id FK
    }
    
    disciplina {
        serial id PK
        string codigo
        string nome
        int departamento_id FK
    }
    
    matriz_curricular {
        serial id PK
        int curso_id FK
    }
    
    disciplina_matriz {
        serial id PK
        int matriz_id FK
        int disciplina_id FK
        string semestre
    }
    
    oferecimento {
        serial id PK
        int disciplina_id FK
        int professor_id FK
        string semestre
        int ano
    }
    
    matricula {
        serial id PK
        int aluno_id FK
        int oferecimento_id FK
        decimal nota
        boolean aprovado
    }
    
    tcc {
        serial id PK
        string tema
        int professor_id FK
        string semestre
        int ano
    }
    
    aluno_tcc {
        serial id PK
        int aluno_id FK
        int tcc_id FK
    }
    
    aluno ||--o{ matricula : "cursa"
    oferecimento ||--o{ matricula : "tem"
    professor ||--o{ oferecimento : "leciona"
    disciplina ||--o{ oferecimento : "é oferecida"
    professor ||--o{ tcc : "orienta"
    aluno ||--o{ aluno_tcc : "participa"
    tcc ||--o{ aluno_tcc : "possui"
    matriz_curricular ||--o{ disciplina_matriz : "contém"
    disciplina ||--o{ disciplina_matriz : "participa"
    curso ||--|| matriz_curricular : "possui"
    departamento ||--o{ disciplina : "oferece"
    professor ||--o{ curso : "coordena"
    professor ||--o{ departamento : "chefia"
```

![image](https://github.com/user-attachments/assets/cede7e6d-6574-457e-8d34-104014302bc4)

