CREATE SEQUENCE event_sequence;
CREATE TABLE "Event" (
       "EventID" INT NOT NULL DEFAULT nextval('event_sequence') PRIMARY KEY,
       "EventName" VARCHAR(255) NOT NULL,
       "IsArchived" BOOLEAN NOT NULL DEFAULT false
);

CREATE SEQUENCE priority_sequence;
CREATE TABLE "Priority" (
       "PriorityID" INT NOT NULL DEFAULT nextval('priority_sequence') PRIMARY KEY,
       "Name" VARCHAR(50) NOT NULL
);

INSERT INTO "Priority" ("Name") VALUES
('Elevé'), ('Moyen'), ('Faible'), ('Aucun');

CREATE SEQUENCE approval_sequence;
CREATE TABLE "Approval" (
       "ApprovalID" INT NOT NULL DEFAULT nextval('approval_sequence') PRIMARY KEY,
       "Name" VARCHAR(50) NOT NULL
);

INSERT INTO "Approval" ("Name") VALUES
('Oui'), ('Partiel');

CREATE SEQUENCE message_sequence;
CREATE TABLE "Message" (
       "MessageID" INT NOT NULL DEFAULT nextval('message_sequence') PRIMARY KEY,
       "EventID" INT NOT NULL REFERENCES "Event" ("EventID"),
       "EnteredOn" TIMESTAMP WITHOUT TIME ZONE NOT NULL, -- Wall time
       "Sender" VARCHAR(50) NULL,
       "Recipient" VARCHAR(50) NULL,
       "MessageText" VARCHAR(1000) NULL,
       "PriorityID" INT NULL REFERENCES "Priority" ("PriorityID"),
       "ApprovalID" INT NULL REFERENCES "Approval" ("ApprovalID")
);

-- I am being quite liberal with the null entries - this is to allow
-- partial entry when in a hurry


CREATE sequence user_sequence;
CREATE TABLE "MessagesUser" (
    "MessagesUserID" INT NOT NULL DEFAULT nextval('user_sequence') PRIMARY KEY,
    "UserName" VARCHAR(255) NOT NULL,
    "PasswordHash" VARCHAR(255) NOT NULL
);
