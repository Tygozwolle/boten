create table boats
(
    id                     int auto_increment
        primary key,
    captain_seat_available tinyint(1)                     not null,
    seats                  int                            not null,
    level                  tinyint                        not null,
    description            varchar(600) default ' '       not null,
    name                   varchar(25)  default 'Titanic' not null
);

create table boat_images
(
    id    int  not null
        primary key,
    image blob not null,
    constraint boat_images_ibfk_1
        foreign key (id) references boats (id)
            on delete cascade
);

create table events
(
    id          int auto_increment
        primary key,
    title       varchar(50)                                  not null,
    description text                                         not null,
    start_time  datetime                                     not null,
    end_time    datetime                                     not null,
    category    enum ('wedstrijd', 'les', 'uitje', 'overig') not null
);

create table event_reserved_boats
(
    event_id int not null,
    boat_id  int not null,
    primary key (event_id, boat_id),
    constraint event_reserved_boats_ibfk_1
        foreign key (event_id) references events (id)
            on delete cascade,
    constraint event_reserved_boats_ibfk_2
        foreign key (boat_id) references boats (id)
            on delete cascade
);

create index boat_id
    on event_reserved_boats (boat_id);

create table members
(
    member_id  int auto_increment
        primary key,
    first_name varchar(50)       not null,
    infix      varchar(10)       null,
    last_name  varchar(50)       not null,
    level      tinyint default 1 not null,
    email      varchar(50)       not null,
    password   varchar(5000)     not null,
    constraint email
        unique (email)
);

create table damage_reports
(
    id          int auto_increment
        primary key,
    boat_id     int                                    not null,
    description text                                   not null,
    member_id   int                                    not null,
    report_time timestamp  default current_timestamp() not null,
    fixed       tinyint(1) default 0                   not null,
    usable      tinyint(1) default 0                   not null,
    constraint damage_reports_ibfk_1
        foreign key (member_id) references members (member_id)
            on delete cascade
);

create table damage_report_fotos
(
    id               int auto_increment
        primary key,
    damage_report_id int        not null,
    image            mediumblob not null,
    constraint damage_report_fotos_ibfk_1
        foreign key (damage_report_id) references damage_reports (id)
);

create index damage_report_id
    on damage_report_fotos (damage_report_id);

create index member_id
    on damage_reports (member_id);

create table event_participant
(
    event_id    int  not null,
    member_id   int  not null,
    result_time time null,
    result      text null,
    primary key (event_id, member_id),
    constraint event_participant_ibfk_1
        foreign key (event_id) references events (id)
            on delete cascade,
    constraint event_participant_ibfk_2
        foreign key (member_id) references members (member_id)
            on delete cascade
);

create index member_id
    on event_participant (member_id);

create table member_roles
(
    member_id int                                                                        not null,
    role      enum ('materiaal_commissaris', 'evenementen_commissaris', 'beheerder', '') not null,
    primary key (member_id, role),
    constraint member_roles_ibfk_1
        foreign key (member_id) references members (member_id)
            on delete cascade
);

create table reservation
(
    reservation_id int auto_increment
        primary key,
    boat_id        int                                   not null,
    member_id      int                                   not null,
    creation_date  timestamp default current_timestamp() not null,
    start_time     datetime                              not null,
    end_time       datetime                              not null,
    constraint reservation_ibfk_1
        foreign key (boat_id) references boats (id)
            on delete cascade,
    constraint reservation_ibfk_2
        foreign key (member_id) references members (member_id)
            on delete cascade
);

create index boat_id
    on reservation (boat_id);

create index member_id
    on reservation (member_id);

INSERT INTO boten_reservering.members (member_id, first_name, infix, last_name, level, email, password)
VALUES (0, 'System', null, 'System', DEFAULT, 'System', 'System');

INSERT INTO `members`(`member_id`, `first_name`, `infix`, `last_name`, `level`, `email`, `password`)
VALUES (1, 'Admin', ' ', ' ', 1, 'admin', 'ChangeMe');

INSERT INTO `member_roles`(`member_id`, `role`)
VALUES (1, 'beheerder');