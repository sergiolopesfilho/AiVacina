create table Enderecos(
	id int unsigned auto_increment primary key,
	rua varchar(1000) not null,
    bairro varchar(1000) not null,
    cidade varchar(1000) not null
);

create table Pacientes(
	cartaoCidadao varchar(18) not null unique,
    nome varchar(200) not null,
    dataNascimento timestamp,
    senha varchar(1000) not null,
    idEndereco int unsigned not null,
    constraint FK_PacienteEndereco foreign key(idEndereco) references enderecos(id)
);