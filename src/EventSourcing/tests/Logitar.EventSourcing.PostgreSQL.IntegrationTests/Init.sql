DROP TABLE IF EXISTS "Events";

START TRANSACTION;

CREATE TABLE "Events" (
    "EventId" bigint GENERATED BY DEFAULT AS IDENTITY,
    "Id" uuid NOT NULL,
    "ActorId" character varying(255) NOT NULL,
    "IsDeleted" boolean NULL,
    "OccurredOn" timestamp with time zone NOT NULL,
    "Version" bigint NOT NULL,
    "AggregateType" character varying(255) NOT NULL,
    "AggregateId" character varying(255) NOT NULL,
    "EventType" character varying(255) NOT NULL,
    "EventData" text NOT NULL,
    CONSTRAINT "PK_Events" PRIMARY KEY ("EventId")
);

CREATE INDEX "IX_Events_ActorId" ON "Events" ("ActorId");

CREATE INDEX "IX_Events_AggregateId" ON "Events" ("AggregateId");

CREATE INDEX "IX_Events_AggregateType_AggregateId" ON "Events" ("AggregateType", "AggregateId");

CREATE INDEX "IX_Events_EventType" ON "Events" ("EventType");

CREATE UNIQUE INDEX "IX_Events_Id" ON "Events" ("Id");

CREATE INDEX "IX_Events_OccurredOn" ON "Events" ("OccurredOn");

CREATE INDEX "IX_Events_Version" ON "Events" ("Version");

COMMIT;
