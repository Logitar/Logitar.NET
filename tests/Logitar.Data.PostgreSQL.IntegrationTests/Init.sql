DROP TABLE IF EXISTS "Reviews";
DROP TABLE IF EXISTS "Albums";
DROP TABLE IF EXISTS "Artists";

START TRANSACTION;

CREATE TABLE "Artists" (
  "ArtistId" bigint NOT NULL,
  "Name" varchar(255) NOT NULL,
  "Country" varchar(255) NOT NULL,
  CONSTRAINT "PK_Artists" PRIMARY KEY ("ArtistId")
);

CREATE INDEX "IX_Artists_Name" ON "Artists" ("Name");

CREATE INDEX "IX_Artists_Country" ON "Artists" ("Country");

INSERT INTO "Artists" ("ArtistId", "Name", "Country") VALUES
  (37, 'Dream Theater', 'United States'),
  (101594, 'After the Burial', 'United States'),
  (18351, 'Gojira', 'France'),
  (3540326229, 'Archspire', 'Canada'),
  (892, 'Septicflesh', 'Greece');

CREATE TABLE "Albums" (
  "AlbumId" bigint NOT NULL,
  "ArtistId" bigint NOT NULL,
  "ReleaseYear" int NOT NULL,
  "Title" varchar(255) NOT NULL,
  "Genre" varchar(255) NOT NULL,
  CONSTRAINT "PK_Albums" PRIMARY KEY ("AlbumId"),
  CONSTRAINT "FK_Albums_ArtistId" FOREIGN KEY ("ArtistId") REFERENCES "Artists" ("ArtistId") ON DELETE CASCADE
);

CREATE INDEX "IX_Albums_ArtistId" ON "Albums" ("ArtistId");

CREATE INDEX "IX_Albums_ReleaseYear" ON "Albums" ("ReleaseYear");

CREATE INDEX "IX_Albums_Title" ON "Albums" ("Title");

CREATE INDEX "IX_Albums_Genre" ON "Albums" ("Genre");

INSERT INTO "Albums" ("AlbumId", "ArtistId", "ReleaseYear", "Title", "Genre") VALUES
  (457889, 37, 1989, 'When Dream and Day Unite', 'Progressive Metal'),
  (195, 37, 1992, 'Images and Words', 'Progressive Metal'),
  (131, 37, 1994, 'Awake', 'Progressive Metal'),
  (1366, 37, 1997, 'Falling into Infinity', 'Progressive Metal'),
  (1374, 37, 1999, 'Metropolis Pt. 2: Scenes from a Memory', 'Progressive Metal'),
  (127, 37, 2002, 'Six Degrees of Inner Turbulence', 'Progressive Metal'),
  (31521, 37, 2003, 'Train of Thought', 'Progressive Metal'),
  (77078, 37, 2005, 'Octavarium', 'Progressive Metal'),
  (145965, 37, 2007, 'Systematic Chaos', 'Progressive Metal'),
  (229968, 37, 2009, 'Black Clouds & Silver Linings', 'Progressive Metal'),
  (309671, 37, 2011, 'A Dramatic Turn of Events', 'Progressive Metal'),
  (381569, 37, 2013, 'Dream Theater', 'Progressive Metal'),
  (548518, 37, 2016, 'The Astonishing', 'Progressive Metal'),
  (749230, 37, 2019, 'Distance over Time', 'Progressive Metal'),
  (966333, 37, 2021, 'A View from the Top of the World', 'Progressive Metal'),
  (164939, 101594, 2006, 'Forging a Future Self', 'Progressive Deathcore'),
  (524096, 101594, 2009, 'Rareform', 'Progressive Deathcore'),
  (289815, 101594, 2010, 'In Dreams', 'Progressive Deathcore'),
  (393401, 101594, 2013, 'Wolves Within', 'Progressive Deathcore'),
  (554043, 101594, 2016, 'Dig Deep', 'Progressive Deathcore'),
  (764024, 101594, 2019, 'Evergreen', 'Progressive Deathcore'),
  (337200, 18351, 2012, 'L''Enfant Sauvage', 'Progressive Metal'),
  (578377, 18351, 2016, 'Magma', 'Progressive Metal'),
  (925156, 18351, 2021, 'Fortitude', 'Progressive Metal'),
  (303655, 3540326229, 2011, 'All Shall Align', 'Technical Death Metal'),
  (402831, 3540326229, 2014, 'The Lucid Collective', 'Technical Death Metal'),
  (659582, 3540326229, 2017, 'Relentless Mutation', 'Technical Death Metal'),
  (968430, 3540326229, 2021, 'Bleed the Future', 'Technical Death Metal'),
  (298147, 892, 2011, 'The Great Mass', 'Symphonic Death Metal'),
  (1022587, 892, 2022, 'Modern Primitive', 'Symphonic Death Metal');

CREATE TABLE "Reviews" (
  "ReviewId" bigint NOT NULL,
  "AlbumId" bigint NOT NULL,
  "IsPublished" boolean NOT NULL,
  "Note" int NOT NULL,
  "Text" text NULL,
  CONSTRAINT "PK_Reviews" PRIMARY KEY ("ReviewId"),
  CONSTRAINT "FK_Reviews_AlbumId" FOREIGN KEY ("AlbumId") REFERENCES "Albums" ("AlbumId") ON DELETE CASCADE
);

CREATE INDEX "IX_Reviews_AlbumId" ON "Reviews" ("AlbumId");

CREATE INDEX "IX_Reviews_IsPublished" ON "Reviews" ("IsPublished");

CREATE INDEX "IX_Reviews_Note" ON "Reviews" ("Note");

INSERT INTO "Reviews" ("ReviewId", "AlbumId", "IsPublished", "Note", "Text") VALUES
  (17324, 31521, true, 5, 'Never being one to disappoint, DREAM THEATER are back again with yet another colossal disc "..."'),
  (238242, 145965, true, 3, 'After a very smooth and calm record called "Octavarium" Dream Theater tried out something new "..."'),
  (1773050, 968430, true, 4, 'If you think you know tech-death, and whether you like the subgenre or not, you have to check "..."'),
  (1529767, 289815, true, 3, 'I’m not really sure this is djent. I don’t think it is. It could be. No. This screams more of "..."'),
  (173848, 1374, false, 4, 'I think it’s safe to say at this point that when Dream Theater came out, they established "..."'),
  (242832, 131, true, 3, NULL),
  (40130, 127, false, 2, 'Before I go any further, I must add that the concept of doing a few "normal" songs and then "..."'),
  (194956, 966333, true, 1, 'The 2010s were a strange and frustrating time to be a Dream Theater fan. Despite the "..."');

COMMIT;
