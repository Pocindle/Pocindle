CREATE TABLE Users
(
    UserId             serial8 PRIMARY KEY,
    PocketUsername     varchar(50) NOT NULL,
    KindleEmailAddress varchar(50),
    PocketAccessToken  char(30)
);
