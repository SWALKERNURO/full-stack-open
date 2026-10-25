#!/bin/bash
set -euo pipefail
PSQL=(psql --username=freecodecamp --dbname=number_guess -X -q -t --no-align --set=ON_ERROR_STOP=1)
printf "Enter your username:\n"
read -r USERNAME || exit 0
if [[ -z "$USERNAME" || ${#USERNAME} -gt 22 ]]
then
  printf "Username must contain between 1 and 22 characters.\n"
  exit 1
fi
USER_DATA=$("${PSQL[@]}" -v username="$USERNAME" <<'SQL'
SELECT user_id,games_played,COALESCE(best_game,0) FROM users WHERE username=:'username';
SQL
)
if [[ -z "$USER_DATA" ]]
then
  USER_ID=$("${PSQL[@]}" -v username="$USERNAME" <<'SQL'
INSERT INTO users(username) VALUES (:'username') RETURNING user_id;
SQL
)
  printf "Welcome, %s! It looks like this is your first time here.\nGuess the secret number between 1 and 1000:\n" "$USERNAME"
else
  IFS='|' read -r USER_ID GAMES_PLAYED BEST_GAME <<< "$USER_DATA"
  printf "Welcome back, %s! You have played %s games, and your best game took %s guesses.\nGuess the secret number between 1 and 1000:\n" "$USERNAME" "$GAMES_PLAYED" "$BEST_GAME"
fi
RANDOM_VALUE=$RANDOM
while (( RANDOM_VALUE >= 32000 )); do RANDOM_VALUE=$RANDOM; done
SECRET_NUMBER=$((RANDOM_VALUE % 1000 + 1))
NUMBER_OF_GUESSES=0
while read -r GUESS
do
  NUMBER_OF_GUESSES=$((NUMBER_OF_GUESSES + 1))
  if [[ ! "$GUESS" =~ ^[+-]?[0-9]+$ ]]
  then
    printf "That is not an integer, guess again:\n"
    continue
  fi
  DIGITS=${GUESS#[+-]}
  while [[ ${#DIGITS} -gt 1 && "$DIGITS" == 0* ]]; do DIGITS=${DIGITS#0}; done
  if [[ ${#DIGITS} -gt 4 ]]
  then
    if [[ "$GUESS" == -* ]]; then printf "It's higher than that, guess again:\n"; else printf "It's lower than that, guess again:\n"; fi
    continue
  fi
  VALUE=$((10#$DIGITS))
  [[ "$GUESS" == -* ]] && VALUE=$((-VALUE))
  if (( VALUE > SECRET_NUMBER ))
  then
    printf "It's lower than that, guess again:\n"
  elif (( VALUE < SECRET_NUMBER ))
  then
    printf "It's higher than that, guess again:\n"
  else
    "${PSQL[@]}" -v user_id="$USER_ID" -v guesses="$NUMBER_OF_GUESSES" <<'SQL'
BEGIN;
INSERT INTO games(user_id,guesses) VALUES (:'user_id'::int,:'guesses'::int);
UPDATE users SET games_played=games_played+1,
  best_game=CASE WHEN best_game IS NULL OR best_game>:'guesses'::int THEN :'guesses'::int ELSE best_game END
WHERE user_id=:'user_id'::int;
COMMIT;
SQL
    printf "You guessed it in %s tries. The secret number was %s. Nice job!\n" "$NUMBER_OF_GUESSES" "$SECRET_NUMBER"
    exit 0
  fi
done
