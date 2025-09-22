-- ======================
-- SCHEMA: userservices
-- ======================

CREATE SCHEMA IF NOT EXISTS userservices;

CREATE TABLE userservices.memberpackage (
  "MemberTypeId" varchar(20) PRIMARY KEY,
  "NameType" varchar(100),
  "Price" double precision
);

CREATE TABLE userservices."user" (
  "UserId" varchar(50) PRIMARY KEY,
  "UserName" varchar(200),
  "Email" varchar(200),
  "Password" varchar(200),
  "Level" int,
  "ExpPerLevel" int,
  "Coin" int,
  "MemberTypeId" varchar(20),
  "Status" varchar(20),
  CONSTRAINT fk_member FOREIGN KEY ("MemberTypeId") REFERENCES userservices.memberpackage("MemberTypeId")
);

CREATE TABLE userservices.item (
  "ItemId" varchar(20) PRIMARY KEY,
  "Name" varchar(100),
  "Type" varchar(100),
  "Price" double precision,
  "Status" varchar(50)
);

CREATE TABLE userservices.categorydetails (
  "UserId" varchar(50),
  "ItemId" varchar(20),
  "Quantity" int,
  PRIMARY KEY ("UserId", "ItemId"),
  CONSTRAINT fk_user FOREIGN KEY ("UserId") REFERENCES userservices."user"("UserId"),
  CONSTRAINT fk_item FOREIGN KEY ("ItemId") REFERENCES userservices.item("ItemId")
);

CREATE TABLE userservices.plantedlog (
  "UserId" varchar(50),
  "ItemId" varchar(20),
  "Status" varchar(50),
  CONSTRAINT fk_planted_user FOREIGN KEY ("UserId") REFERENCES userservices."user"("UserId"),
  CONSTRAINT fk_planted_item FOREIGN KEY ("ItemId") REFERENCES userservices.item("ItemId")
);

CREATE TABLE userservices.transactionhistory (
  "Id" varchar(20) PRIMARY KEY,
  "UserId" varchar(50),
  "DateTrade" timestamp,
  "Amount" double precision,
  "Status" varchar(100),
  CONSTRAINT fk_tx_user FOREIGN KEY ("UserId") REFERENCES userservices."user"("UserId")
);

-- ======================
-- Dữ liệu mẫu
-- ======================

INSERT INTO userservices.memberpackage VALUES
('M1','Normal',0),
('M2','Premium',50000),
('MAD','Admin',0);

INSERT INTO userservices."user" VALUES
('U001','Adminstrator','khanh@gmail.com','123',NULL,NULL,NULL,'MAD','active'),
('U111','TNK','khanh','123',0,0,10,'M1','active'),
('U222','TNK2','khanh2','123',10,0,100,'M2','active');

INSERT INTO userservices.item VALUES
('DC001','Chair1','Decorate',20,'Active'),
('DC002','Chair2','Decorate',10,'Active'),
('TA0','SeedA0','Seed',1,'Active'),
('TA1','TreeA1','Tree',0,'Active'),
('TA2','TreeA2','Tree',0,'Active'),
('TA3','TreeA3','Tree',0,'Active'),
('TK01','TicketN','Ticket',0,'Active'),
('TK02','TicketG','Ticket',0,'Active');

INSERT INTO userservices.categorydetails VALUES ('U111','TA0',2);
