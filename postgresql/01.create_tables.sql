CREATE SEQUENCE event_sequence;
CREATE TABLE "Event" (
       "EventID" INT NOT NULL DEFAULT nextval('event_sequence') PRIMARY KEY,
       "EventName" VARCHAR(255) NOT NULL
);

CREATE SEQUENCE message_sequence;
CREATE TABLE "Message" (
       "MessageID" INT NOT NULL DEFAULT nextval('message_sequence') PRIMARY KEY,
       "EventID" INT NOT NULL REFERENCES "Event" ("EventID"),
       "EnteredOn" TIMESTAMP WITHOUT TIME ZONE NOT NULL, -- Wall time
       "Sender" VARCHAR(50) NULL,
       "Recipient" VARCHAR(50) NULL,
       "MessageText" VARCHAR(1000) NULL
);

-- I am being quite liberal with the null entries - this is to allow
-- partial entry when in a hurry

