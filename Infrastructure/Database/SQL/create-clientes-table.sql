CREATE  TABLE  IF NOT EXISTS cliente (
    id serial primary key,
    name varchar,
    email varchar,
    cpf varchar,
    cpf_raw bigint
);
