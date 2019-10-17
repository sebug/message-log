do $$
BEGIN
IF NOT EXISTS (
   SELECT *
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_NAME = 'MessagesUser') THEN

CREATE sequence user_sequence;
CREATE TABLE "MessagesUser" (
    "MessagesUserID" INT NOT NULL DEFAULT nextval('user_sequence') PRIMARY KEY,
    "UserName" VARCHAR(255) NOT NULL,
    "PasswordHash" VARCHAR(255) NOT NULL
);
END IF;
END;
$$;
