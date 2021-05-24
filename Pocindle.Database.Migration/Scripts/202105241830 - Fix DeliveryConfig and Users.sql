alter table deliveryconfig alter column "from" type text using "from"::text;

alter table deliveryconfig alter column "from" set not null;

alter table users drop column kindleemailaddress;

alter table users
    add deliveryconfigid bigint;

alter table users drop column column_5;

alter table users
    add constraint users_deliveryconfig_id_fk
        foreign key (deliveryconfigid) references deliveryconfig;

