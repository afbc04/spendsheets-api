-- Table

CREATE TABLE IF NOT EXISTS Categories (
    id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    name VARCHAR(30) NOT NULL,
    description VARCHAR(100)
);

-- Extensions

CREATE EXTENSION IF NOT EXISTS unaccent;

-- Queries

SELECT id, name, description FROM Categories; -- List
SELECT id, name, description FROM Categories LIMIT 40 OFFSET 40; -- List (Page)
SELECT id, name, description FROM Categories WHERE unaccent(name) ILIKE unaccent('%ae%') LIMIT 40 OFFSET 40; -- List (get by name)
SELECT id, name, description FROM Categories WHERE unaccent(name) ILIKE unaccent('%ae%') ORDER BY name ASC LIMIT 40 OFFSET 40; -- List

SELECT id, name, description FROM Categories WHERE id = 1; -- Get Details
INSERT INTO Categories(name,description) VALUES ('Health','Entry related to health'); -- Insert
UPDATE Categories SET name = 'Hospital' WHERE id = 500102; -- Update
DELETE FROM Categories WHERE id = 700; -- Delete