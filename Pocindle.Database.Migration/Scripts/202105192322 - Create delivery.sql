CREATE TABLE Delivery
(
    DeliveryId            serial8 PRIMARY KEY,
    UserId                serial8  NOT NULL,
    ArticleUrl            text     NOT NULL,
    EpubFile              char(41) NOT NULL,
    MobiFile              char(41) NOT NULL,
    DeliveryStatus        int4     NOT NULL,
    DeliveryFailedMessage text,

    CONSTRAINT fk_User FOREIGN KEY (UserId) REFERENCES public.users
);
