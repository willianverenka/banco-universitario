-- Tabelas independentes
CREATE TABLE professor (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    idade SMALLINT CHECK (idade > 0),
    ativo BOOLEAN DEFAULT TRUE
);

CREATE TABLE aluno (
    id SERIAL PRIMARY KEY,
    ra VARCHAR(20) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL,
    idade SMALLINT CHECK (idade > 0),
    ativo BOOLEAN DEFAULT TRUE
);

-- Tabelas com dependências
CREATE TABLE departamento (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL UNIQUE,
    chefe_id INTEGER REFERENCES professor(id)
);

CREATE TABLE curso (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL UNIQUE,
    coordenador_id INTEGER REFERENCES professor(id)
);

CREATE TABLE disciplina (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL,
    departamento_id INTEGER NOT NULL REFERENCES departamento(id)
);

CREATE TABLE matriz_curricular (
    id SERIAL PRIMARY KEY,
    curso_id INTEGER NOT NULL REFERENCES curso(id),
    UNIQUE(curso_id)
);

CREATE TABLE disciplina_matriz (
    id SERIAL PRIMARY KEY,
    matriz_id INTEGER NOT NULL REFERENCES matriz_curricular(id),
    disciplina_id INTEGER NOT NULL REFERENCES disciplina(id),
    semestre VARCHAR(10) NOT NULL,
    UNIQUE(matriz_id, disciplina_id)
);

CREATE TABLE oferecimento (
    id SERIAL PRIMARY KEY,
    disciplina_id INTEGER NOT NULL REFERENCES disciplina(id),
    professor_id INTEGER NOT NULL REFERENCES professor(id),
    semestre VARCHAR(10) NOT NULL,
    ano INTEGER NOT NULL,
    UNIQUE(disciplina_id, professor_id, semestre, ano)
);

CREATE TABLE matricula (
    id SERIAL PRIMARY KEY,
    aluno_id INTEGER NOT NULL REFERENCES aluno(id),
    oferecimento_id INTEGER NOT NULL REFERENCES oferecimento(id),
    nota DECIMAL(4,2),
    aprovado BOOLEAN,
    UNIQUE(aluno_id, oferecimento_id)
);

CREATE TABLE tcc (
    id SERIAL PRIMARY KEY,
    tema VARCHAR(200) NOT NULL,
    professor_id INTEGER NOT NULL REFERENCES professor(id),
    semestre VARCHAR(10) NOT NULL,
    ano INTEGER NOT NULL
);

CREATE TABLE aluno_tcc (
    id SERIAL PRIMARY KEY,
    aluno_id INTEGER NOT NULL REFERENCES aluno(id),
    tcc_id INTEGER NOT NULL REFERENCES tcc(id),
    UNIQUE(aluno_id, tcc_id)
);
