\set ON_ERROR_STOP on
CREATE DATABASE worldcup;
\connect worldcup
CREATE TABLE teams (team_id SERIAL PRIMARY KEY, name VARCHAR(60) UNIQUE NOT NULL);
CREATE TABLE games (
  game_id SERIAL PRIMARY KEY, year INT NOT NULL, round VARCHAR(40) NOT NULL,
  winner_id INT NOT NULL REFERENCES teams(team_id), opponent_id INT NOT NULL REFERENCES teams(team_id),
  winner_goals INT NOT NULL CHECK(winner_goals>=0), opponent_goals INT NOT NULL CHECK(opponent_goals>=0)
);
