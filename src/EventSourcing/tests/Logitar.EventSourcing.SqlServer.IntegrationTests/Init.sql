IF OBJECT_ID(N'[Events]') IS NOT NULL
BEGIN
    DROP TABLE [Events];
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Events] (
    [EventId] bigint NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [ActorId] nvarchar(255) NOT NULL,
    [OccurredOn] datetime2 NOT NULL,
    [Version] bigint NOT NULL,
    [DeleteAction] nvarchar(255) NOT NULL,
    [AggregateType] nvarchar(255) NOT NULL,
    [AggregateId] nvarchar(255) NOT NULL,
    [EventType] nvarchar(255) NOT NULL,
    [EventData] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([EventId])
);
GO

CREATE INDEX [IX_Events_ActorId] ON [Events] ([ActorId]);
GO

CREATE INDEX [IX_Events_AggregateId] ON [Events] ([AggregateId]);
GO

CREATE INDEX [IX_Events_AggregateType_AggregateId] ON [Events] ([AggregateType], [AggregateId]);
GO

CREATE INDEX [IX_Events_EventType] ON [Events] ([EventType]);
GO

CREATE UNIQUE INDEX [IX_Events_Id] ON [Events] ([Id]);
GO

CREATE INDEX [IX_Events_OccurredOn] ON [Events] ([OccurredOn]);
GO

CREATE INDEX [IX_Events_Version] ON [Events] ([Version]);
GO

COMMIT;
GO
