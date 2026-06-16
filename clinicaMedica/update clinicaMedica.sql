update pacientes
set cpf = '123.465.789-00'
WHERE id_pacientes = 1;

update consulta 
set status = 'pendente'
where id_consulta = 1;

update pagamentos 
set valor = 200.00
where id_consulta = 1;

