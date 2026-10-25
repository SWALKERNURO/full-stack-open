\set ON_ERROR_STOP on
CREATE DATABASE salon;
\connect salon
CREATE TABLE customers (customer_id SERIAL PRIMARY KEY, phone VARCHAR(40) UNIQUE NOT NULL, name VARCHAR(100) NOT NULL);
CREATE TABLE services (service_id SERIAL PRIMARY KEY, name VARCHAR(40) UNIQUE NOT NULL);
CREATE TABLE appointments (
  appointment_id SERIAL PRIMARY KEY, customer_id INT NOT NULL REFERENCES customers(customer_id),
  service_id INT NOT NULL REFERENCES services(service_id), time VARCHAR(60) NOT NULL
);
INSERT INTO services(name) VALUES ('cut'),('color'),('perm');
