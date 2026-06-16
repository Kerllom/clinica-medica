-- 1º deletar o prontuario vinculado
DELETE FROM prontuario
WHERE id_consulta = 1;

-- 2º deletar o pagamento vinculado
DELETE FROM pagamentos
WHERE id_consulta = 1;

-- 3º agora sim deletar a consulta
DELETE FROM consulta
WHERE id_consulta = 1;