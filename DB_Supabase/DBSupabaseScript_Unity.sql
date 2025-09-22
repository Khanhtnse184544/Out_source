-- ======================
-- SCHEMA: unityservices
-- ======================

CREATE SCHEMA IF NOT EXISTS unityservices;

CREATE TABLE unityservices.scene (
  "UserId" varchar(20) PRIMARY KEY,
  "Status" varchar(20),
  "DateSave" timestamp
);

CREATE TABLE unityservices.scenedetails (
  "UserId" varchar(20),
  "ItemId" varchar(20),
  "Name" varchar(100),
  "Level" int,
  "ExpPerLevel" int,
  "PositionX" double precision,
  "PositionY" double precision,
  CONSTRAINT fk_scene FOREIGN KEY ("UserId") REFERENCES unityservices.scene("UserId")
);

-- (chưa có dữ liệu mẫu cho schema này)
