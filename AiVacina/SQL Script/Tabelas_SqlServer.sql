create table Enderecos(
	id int IDENTITY(1,1) primary key,
	rua varchar(150) not null,
    bairro varchar(100) not null,
    cidade varchar(100) not null
);

create table Pacientes(
	cartaoCidadao varchar(18) not null unique,
    nome varchar(200) not null,
    dataNascimento datetime,
    senha varchar(150) not null,
    idEndereco int not null,
    cpfAdmPosto varchar(14) null,
	perfil varchar(100),
	email varchar(50) null
    constraint FK_PacienteEndereco foreign key(idEndereco) references enderecos(id)
);

create table Postos(
	idEstabelecimento int IDENTITY(1,1) primary key,
    nomeEstabelecimento varchar(200) not null,
    admPosto varchar(200) not null,
    cpfAdmPosto varchar(14) not null,
    cnpj varchar(18) not null,
    idEndereco int not null,
    constraint FK_PostoEndereco foreign key(idEndereco) references enderecos(id)
);

--Atualizar, o codVacina não pode ser PK pois pode ser cadastrado a mesma vacina pra lotes distintos
--Aline sugeriu mudar a PK pra lote (dferente vacinas no mesmo lote)
create table Vacinas(
	codVacina int primary key,
    loteVacina varchar(100) not null,
    nomeVacina varchar(200) not null,
    quantidade int not null,
    dataValidade datetime2 not null,
    grupoalvo varchar(200) not null,
	postoCNPJ varchar(18)
);


create table AgendamentoVacinas(
	id int IDENTITY(1,1) primary key,
    idPosto int not null,
    idVacina int not null,
    cartaocidadao  varchar(18) not null,
    dataAgendamento datetime not null,
    constraint FK_AgendamentoPosto foreign key(idPosto) references postos(idEstabelecimento),
    constraint FK_AgendamentoVacina foreign key(idVacina) references vacinas(codVacina),
    constraint FK_AgendamentoUsuario foreign key(cartaocidadao) references pacientes(cartaoCidadao)
);

create table HorariosCancelados(
	id int IDENTITY(1,1) primary key,
    dia varchar(50),
    horarios varchar(max) not null
);


create table CarteiraVacinacao(
	id int IDENTITY(1,1) primary key,  
    cartaoCidadao varchar(18) not null unique, 
	nomeCompleto varchar(max) not null,
	dataNascimento datetime2 not null,
	dataCadastro datetime2 null
    --idPosto int,
    --constraint FK_CarteiraVacinacaoPosto foreign key(idPosto) references postos(idEstabelecimento)
);

create table VacinasAplicadas(
	idVacinaAplicada int IDENTITY(1,1) not null,
	vacina varchar(200) not null,
	dataVacinação datetime2 not null,
	dataReforco datetime2 not null,
	idCarteira int not null,
	CONSTRAINT Pk_VacinasAplicadas PRIMARY KEY (id),
    CONSTRAINT FK_VacinaAplicadaCarteira FOREIGN KEY (idCarteira) REFERENCES CarteiraVacinacao(id)
);

insert into Vacinas(loteVacina,nomeVacina,quantidade,dataValidade,grupoalvo,postoCNPJ)
values(001,'Gripe A',30,'2020/05/03','Bebês e Idosos','22.323.458/0001-79');

insert into Vacinas(loteVacina,nomeVacina,quantidade,dataValidade,grupoalvo,postoCNPJ)
values(002,'Gripe B',30,'2020/10/03','Bebês e Idosos','22.323.458/0001-79');

insert into Vacinas(codVacina,loteVacina,nomeVacina,quantidade,dataValidade,grupoalvo,postoCNPJ)
values('564862',002,'Gripe B',30,'2020/10/03','Bebês e Idosos','11.564.858/0001-98');

insert into Vacinas(codVacina,loteVacina,nomeVacina,quantidade,dataValidade,grupoalvo,postoCNPJ)
values('8564',004,'Gripe C',30,'2020/10/03','Bebês e Idosos','22.333.444/0001-53');

insert into Enderecos(rua,bairro,cidade)
values('Rua Dona Firmina, 865','Bairro Santana','Pouso Alegre');

--Checar se, na tabela endereços, existe o id utilizado nessa tabela 
insert into postos(nomeEstabelecimento, admPosto, cpfAdmPosto,cnpj,idEndereco)
values ('Posto São José','Antonio dos Santos','123.255.656-85','22.323.458/0001-79',1);

--Checar se, na tabela endereços, existe o id utilizado nessa tabela 
insert into postos(nomeEstabelecimento, admPosto, cpfAdmPosto,cnpj,idEndereco)
values ('Posto São Vincente','José Bonifacio','555.666.777-88','22.333.444/0001-53',1);

--Checar se, na tabela endereços, existe o id utilizado nessa tabela 
insert into postos(nomeEstabelecimento, admPosto, cpfAdmPosto,cnpj,idEndereco)
values ('Posto Vila Clarice','Amaro da Silva','564.678.525-99','11.564.858/0001-98',1);