select
u.login as usuarios,
p.nome as pacientes,
r.nome as recepcionista,
m.nome as medico,
m.crm as crm, 
e.nome as especialidade, 
pr.descricao as prontuario, 
pg.valor as valor_pagamento,
pg.forma_pagamento,
pg.status as status_pagamento
from consulta c
  JOIN pacientes    p  ON c.id_pacientes    = p.id_pacientes
  JOIN usuarios     u  ON p.id_usuarios     = u.id_usuarios
  JOIN medicos      m  ON c.id_medicos      = m.id_medicos
  JOIN especialidade e ON m.id_especialidade = e.id_especialidade
  LEFT JOIN recepcionista r ON r.id_usuarios  = u.id_usuarios
  JOIN pagamentos   pg ON pg.id_consulta    = c.id_consulta
  JOIN prontuario   pr ON pr.id_consulta    = c.id_consulta;

