-- 1. Mostre todo o histórico escolar de um aluno que teve reprovação em uma disciplina, retornando inclusive a reprovação em um semestre e a aprovação no semestre seguinte;
with reprovacoes as (
    select a.id as aluno_id, d.id as disciplina_id, d.codigo, d.nome as disciplina_nome, a.nome as aluno_nome
    from aluno a
    join matricula m on a.id = m.aluno_id
    join oferecimento o on m.oferecimento_id = o.id
    join disciplina d on o.disciplina_id = d.id
    where m.aprovado = false
)
select r.aluno_nome, r.codigo as codigo_disciplina, r.disciplina_nome, o1.ano as ano_reprovacao, o1.semestre as semestre_reprovacao, m1.nota as nota_reprovacao, o2.ano as ano_aprovacao, o2.semestre as semestre_aprovacao, m2.nota as nota_aprovacao
from reprovacoes r
join matricula m1 on r.aluno_id = m1.aluno_id
join oferecimento o1 on m1.oferecimento_id = o1.id
join matricula m2 on r.aluno_id = m2.aluno_id
join oferecimento o2 on m2.oferecimento_id = o2.id
where o1.disciplina_id = r.disciplina_id
    and o2.disciplina_id = r.disciplina_id
    and m1.aprovado = false
    and m2.aprovado = true
    and (o2.ano > o1.ano or (o2.ano = o1.ano and o2.semestre > o1.semestre))
order by r.aluno_nome, r.codigo, o1.ano, o1.semestre
limit 1

-- 2. Mostre todos os TCCs orientados por um professor junto com os nomes dos alunos que fizeram o projeto;

select p.nome as orientador, t.tema, t.ano, t.semestre, string_agg(a.nome, ', ') as alunos
from professor p
join tcc t on p.id = t.professor_id
join aluno_tcc at on t.id = at.tcc_id
join aluno a on at.aluno_id = a.id
where p.id = (select id from professor limit 1)
group by p.nome, t.tema, t.ano, t.semestre
order by t.ano desc, t.semestre desc;

--3. Mostre a matriz curicular de pelo menos 2 cursos diferentes que possuem disciplinas em comum (e.g., Ciência da Computação e Ciência de Dados). Este exercício deve ser dividido em 2 queries sendo uma para cada curso;

select c.nome as curso, d.codigo, d.nome as disciplina, dm.semestre, dep.nome as departamento
from curso c
join matriz_curricular mc on c.id = mc.curso_id
join disciplina_matriz dm on mc.id = dm.matriz_id
join disciplina d on dm.disciplina_id = d.id
join departamento dep on d.departamento_id = dep.id
where c.nome = 'Ciência da Computação'
order by cast(dm.semestre as integer), d.codigo;

select c.nome as curso, d.codigo, d.nome as disciplina, dm.semestre, dep.nome as departamento
from curso c
join matriz_curricular mc on c.id = mc.curso_id
join disciplina_matriz dm on mc.id = dm.matriz_id
join disciplina d on dm.disciplina_id = d.id
join departamento dep on d.departamento_id = dep.id
where c.nome = 'Ciência de Dados'
order by cast(dm.semestre as integer), d.codigo;

select d.codigo, d.nome as disciplina, cc.nome as curso_cc, dm_cc.semestre as semestre_cc, cd.nome as curso_cd, dm_cd.semestre as semestre_cd
from disciplina d
join disciplina_matriz dm_cc on d.id = dm_cc.disciplina_id
join matriz_curricular mc_cc on dm_cc.matriz_id = mc_cc.id
join curso cc on mc_cc.curso_id = cc.id
join disciplina_matriz dm_cd on d.id = dm_cd.disciplina_id
join matriz_curricular mc_cd on dm_cd.matriz_id = mc_cd.id
join curso cd on mc_cd.curso_id = cd.id
where cc.nome = 'Ciência da Computação'
    and cd.nome = 'Ciência de Dados'
order by d.codigo;

-- 4. Para um determinado aluno, mostre os códigos e nomes das diciplinas já cursadas junto com os nomes dos professores que lecionaram a disciplina para o aluno;

select distinct dep.id, dep.nome as departamento
from departamento dep
join disciplina d on dep.id = d.departamento_id
join oferecimento o on d.id = o.disciplina_id
where o.professor_id = (select id from professor limit 1)
order by dep.nome;


-- 5. Liste todos os chefes de departamento e coordenadores de curso em apenas uma query de forma que a primeira coluna seja o nome do professor, a segunda o nome do departamento coordena e a terceira o nome do curso que coordena. Substitua os campos em branco do resultado da query pelo texto "nenhum"

select p.nome as nome_professor, coalesce(dep.nome, 'nenhum') as departamento_chefia, coalesce(c.nome, 'nenhum') as curso_coordenacao
from professor p
left join departamento dep on p.id = dep.chefe_id
left join curso c on p.id = c.coordenador_id
-- o professor necessariamente deve chefear algo
where dep.id is not null or c.id is not null
order by p.nome;

-- 6. Encontre os nomes de todos os estudantes
select nome 
from alunos 
where ativo = true
order by nome;

-- 7. Liste os IDs e nomes de todos os professores
select id, nome 
from professor 
where ativo = true
order by nome;

-- 8. Encontre os nomes de todos os estudantes que cursaram "Banco de Dados"
select a.nome
from aluno a
join matricula m on a.id = m.aluno_id
join oferecimento o on m.oferecimento_id = o.id
join disciplina d on o.disciplina_id = d.id
where d.nome like '%Banco de Dados%'
order by a.nome;

-- 9. Liste os IDs dos professores que ensinam mais de um curso
select p.id, p.nome
from professor p
join oferecimento o on p.id = o.professor_id
join disciplina d on o.disciplina_id = d.id
join disciplina_matriz dm on d.id = dm.disciplina_id
join matriz_curricular mc on dm.matriz_id = mc.id
group by p.id, p.nome
having count(distinct mc.curso_id) > 1
order by p.nome;

-- 10. Encontre os departamentos que oferecem cursos ministrados por um professor específico
select distinct dep.id, dep.nome as departamento
from departamento dep
join disciplina d on dep.id = d.departamento_id
join oferecimento o on d.id = o.disciplina_id
where o.professor_id = 1  -- Substitua pelo ID do professor desejado
order by dep.nome;

-- 11. Liste os cursos que foram ministrados por mais de um professor em semestres diferentes
select c.codigo, c.nome as curso
from curso c
join matriz_curricular mc on c.id = mc.curso_id
join disciplina_matriz dm on mc.id = dm.matriz_id
join disciplina d on dm.disciplina_id = d.id
join oferecimento o on d.id = o.disciplina_id
group by c.id, c.codigo, c.nome
having count(distinct o.professor_id) > 1
and count(distinct o.semestre) > 1
order by c.nome;

-- 12. Recupere os nomes dos estudantes que cursaram disciplinas em mais de 2 departamentos
select a.nome
from aluno a
join matricula m on a.id = m.aluno_id
join oferecimento o on m.oferecimento_id = o.id
join disciplina d on o.disciplina_id = d.id
group by a.id, a.nome
having count(distinct d.departamento_id) > 2
order by a.nome;

-- 13. Encontre os estudantes que cursaram tanto uma disciplina específica quanto outra
select a.nome
from aluno a
where a.id in (
    select m1.aluno_id
    from matricula m1
    join oferecimento o1 on m1.oferecimento_id = o1.id
    join disciplina d1 on o1.disciplina_id = d1.id
    where d1.codigo = 'CC001'
) 
and a.id in (
    select m2.aluno_id
    from matricula m2
    join oferecimento o2 on m2.oferecimento_id = o2.id
    join disciplina d2 on o2.disciplina_id = d2.id
    where d2.codigo = 'CC003'
)
order by a.nome;

-- 14. Liste as disciplinas que têm a maior quantidade de alunos matriculados
select d.codigo, d.nome, count(m.aluno_id) as total_alunos
from disciplina d
join oferecimento o on d.id = o.disciplina_id
join matricula m on o.id = m.oferecimento_id
group by d.id, d.codigo, d.nome
order by total_alunos desc
limit 5;

-- 15. Encontre os alunos que não participaram de nenhum TCC
select a.id, a.nome
from aluno a
where a.id not in (
    select at.id
    from aluno_tcc at
    where at.aluno_id is not null
)
and a.ativo = true
order by a.nome;