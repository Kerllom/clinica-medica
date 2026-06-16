use clinicaMedica;

insert into usuarios(login , senha , tipo)
values('dalisberto222' , 'senha698' , 'admin'),
      ('mateus765' , 'senha765' , 'moderador'),
      ('karla709' , 'senha145' , 'usuario');
      
insert into especialidade(nome)
values('clinico-geral'),
	  ('enfermeiro'),
      ('pediatra');
      
insert into pacientes(nome , cpf , endereco , telefone , email , data_nascimento , id_usuarios)
values('lucas' , '123.465.789-00' , 'av.afonso pena 262 centro belo horizonte' , '31987654312' , 'lucas56@gmail.com' , '2000-08-09' , 1),
      ('fernando' , '234.444.665-98' , 'rua itamarati 370 sao benedito santa luzia' , '31976543678' , 'fernando44@gmail.com' , '1998-04-06' , 2),
      ('marcos' , '114.764.768-88' , 'av.senhor do bonfim 1052 sao benedito santa luzia' , '31977335798' , 'marcos11@gmail.com' , '1990-07-12' , 3);
      
insert into recepcionista(nome , cpf , id_usuarios)
values('samara' , '888.865.456-86' , 1),
	  ('sabrina' , '414.777.876-98' , 2),
      ('agatha' , '345.754.133-55' , 3 );
      
insert into medicos (nome, crm, id_usuarios, id_especialidade)
values('dr.gustavo',    'CRM/MG 45231', 1, 1),
      ('dr.sebozo',     'CRM/MG 67890', 2, 2),
      ('dra.marinalva', 'CRM/MG 32178', 3, 3);
      
insert into consulta(data_hora , status , id_pacientes , id_medicos)
values('2026-07-11 10:00:00' , 'pendente', 1 , 1),
      ('2026-06-25 16:00:00' , 'confirmado' , 2 , 2),
      ('2026-09-24 13:00:00' , 'confirmado' , 3 , 3);
      
insert into pagamentos(valor , forma_pagamento , data_pagamento , status , id_consulta)
values(200.00 , 'pix' , '2026-07-11' , 'pendente' , 1),
      (370.00 , 'cartaocredito' , '2026-06-10' , 'pago' , 2),
      (450.00 , 'dinheiro' , '2026-09-24' , 'pago' , 3);
      
insert into prontuario(descricao , data_registro , id_pacientes , id_consulta)
values('paciente com dor de cabeca regular , prescrito paracetamol 500mg' , '2026-07-11' , 1 , 1),
      ('paciente com garganta inflamada , prescito ibuprofeno 500mg' , '2026-06-25', 2, 2),
      ('paciente com sintomas de gripe , prescito dorflex 500mg e benegripe 500mg' , '2026-09-24', 3, 3);