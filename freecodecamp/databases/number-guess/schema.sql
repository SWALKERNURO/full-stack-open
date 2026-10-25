\set ON_ERROR_STOP on
CREATE DATABASE number_guess;
\connect number_guess
CREATE TABLE users (
  user_id SERIAL PRIMARY KEY, username VARCHAR(22) UNIQUE NOT NULL,
  games_played INT NOT NULL DEFAULT 0, best_game INT CHECK(best_game>0)
);
CREATE TABLE games (
  game_id SERIAL PRIMARY KEY, user_id INT NOT NULL REFERENCES users(user_id),
  guesses INT NOT NULL CHECK(guesses>0)
);
