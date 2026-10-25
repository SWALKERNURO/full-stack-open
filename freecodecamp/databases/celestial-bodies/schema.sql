\set ON_ERROR_STOP on
CREATE DATABASE universe;
\connect universe
-- Fictional observatory catalog for relational database practice.
CREATE TABLE galaxy_type (galaxy_type_id SERIAL PRIMARY KEY, name VARCHAR(40) UNIQUE NOT NULL, description TEXT NOT NULL);
CREATE TABLE galaxy (
  galaxy_id SERIAL PRIMARY KEY, name VARCHAR(60) UNIQUE NOT NULL,
  galaxy_type_id INT NOT NULL REFERENCES galaxy_type(galaxy_type_id),
  description TEXT NOT NULL, catalog_number INT NOT NULL, is_visible BOOLEAN NOT NULL
);
CREATE TABLE star (
  star_id SERIAL PRIMARY KEY, name VARCHAR(60) UNIQUE NOT NULL,
  galaxy_id INT NOT NULL REFERENCES galaxy(galaxy_id),
  age_in_millions_of_years INT NOT NULL, relative_mass NUMERIC NOT NULL,
  is_spherical BOOLEAN NOT NULL, description TEXT NOT NULL
);
CREATE TABLE planet (
  planet_id SERIAL PRIMARY KEY, name VARCHAR(60) UNIQUE NOT NULL,
  star_id INT NOT NULL REFERENCES star(star_id), orbital_order INT NOT NULL,
  has_life BOOLEAN NOT NULL, is_spherical BOOLEAN NOT NULL
);
CREATE TABLE moon (
  moon_id SERIAL PRIMARY KEY, name VARCHAR(60) UNIQUE NOT NULL,
  planet_id INT NOT NULL REFERENCES planet(planet_id),
  description TEXT NOT NULL, is_spherical BOOLEAN NOT NULL
);
INSERT INTO galaxy_type(name, description) VALUES
('Spiral','Illustrative spiral catalog type'),('Elliptical','Illustrative elliptical catalog type'),('Irregular','Illustrative irregular catalog type');
INSERT INTO galaxy(name,galaxy_type_id,description,catalog_number,is_visible) VALUES
('Atlas',1,'Fictional coursework galaxy',101,true),('Beacon',2,'Fictional coursework galaxy',102,false),
('Cedar',3,'Fictional coursework galaxy',103,true),('Dawn',1,'Fictional coursework galaxy',104,true),
('Ember',2,'Fictional coursework galaxy',105,false),('Fern',3,'Fictional coursework galaxy',106,false);
INSERT INTO star(name,galaxy_id,age_in_millions_of_years,relative_mass,is_spherical,description) VALUES
('Atlas Light',1,4000,1.0,true,'Illustrative star'),('Beacon Light',2,2500,1.2,true,'Illustrative star'),
('Cedar Light',3,6000,0.7,true,'Illustrative star'),('Dawn Light',4,3000,1.5,true,'Illustrative star'),
('Ember Light',5,7000,0.6,true,'Illustrative star'),('Fern Light',6,1000,2.0,true,'Illustrative star');
INSERT INTO planet(name,star_id,orbital_order,has_life,is_spherical)
SELECT 'Practice World ' || value, ((value-1)%6)+1, CASE WHEN value<=6 THEN 1 ELSE 2 END, false, true
FROM generate_series(1,12) AS value;
INSERT INTO moon(name,planet_id,description,is_spherical)
SELECT 'Practice Moon ' || value, ((value-1)%12)+1, 'Fictional satellite for database practice', value%2=0
FROM generate_series(1,20) AS value;
