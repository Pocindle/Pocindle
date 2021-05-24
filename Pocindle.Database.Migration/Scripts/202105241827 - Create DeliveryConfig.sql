create table deliveryconfig
(
    id bigserial
        constraint deliveryconfig_pk
            primary key,
    "to" text not null,
    "from" int,
    smtpserver text not null,
    password text not null
);
