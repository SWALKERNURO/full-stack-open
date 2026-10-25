\set ON_ERROR_STOP on
BEGIN;
DELETE FROM properties WHERE atomic_number=1000;
DELETE FROM elements WHERE atomic_number=1000;
ALTER TABLE properties RENAME COLUMN weight TO atomic_mass;
ALTER TABLE properties RENAME COLUMN melting_point TO melting_point_celsius;
ALTER TABLE properties RENAME COLUMN boiling_point TO boiling_point_celsius;
ALTER TABLE properties ALTER COLUMN melting_point_celsius SET NOT NULL;
ALTER TABLE properties ALTER COLUMN boiling_point_celsius SET NOT NULL;
ALTER TABLE elements ALTER COLUMN symbol SET NOT NULL;
ALTER TABLE elements ALTER COLUMN name SET NOT NULL;
ALTER TABLE elements ADD UNIQUE(symbol);
ALTER TABLE elements ADD UNIQUE(name);
ALTER TABLE properties ADD FOREIGN KEY(atomic_number) REFERENCES elements(atomic_number);
CREATE TABLE types (type_id INT PRIMARY KEY, type VARCHAR(30) UNIQUE NOT NULL);
INSERT INTO types VALUES (1,'metal'),(2,'nonmetal'),(3,'metalloid');
ALTER TABLE properties ADD COLUMN type_id INT REFERENCES types(type_id);
UPDATE properties SET type_id=types.type_id FROM types WHERE properties.type=types.type;
ALTER TABLE properties ALTER COLUMN type_id SET NOT NULL;
UPDATE elements SET symbol=upper(left(symbol,1)) || substring(symbol FROM 2);
ALTER TABLE properties ALTER COLUMN atomic_mass TYPE DECIMAL;
UPDATE properties SET atomic_mass=CASE WHEN atomic_mass::text LIKE '%.%' THEN rtrim(rtrim(atomic_mass::text,'0'),'.')::numeric ELSE atomic_mass END;
INSERT INTO elements(atomic_number,symbol,name) VALUES (9,'F','Fluorine'),(10,'Ne','Neon');
INSERT INTO properties(atomic_number,atomic_mass,melting_point_celsius,boiling_point_celsius,type_id)
VALUES (9,18.998,-220,-188.1,2),(10,20.18,-248.6,-246.1,2);
ALTER TABLE properties DROP COLUMN type;
COMMIT;
