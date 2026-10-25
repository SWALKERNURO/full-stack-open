#! /bin/bash

if [[ $1 == "test" ]]
then
  PSQL="psql --username=postgres --dbname=worldcuptest -t --no-align -c"
else
  PSQL="psql --username=freecodecamp --dbname=worldcup -t --no-align -c"
fi

# Do not change code above this line. Use the PSQL variable above to query your database.

set -euo pipefail
STATEMENTS="BEGIN; TRUNCATE TABLE games, teams RESTART IDENTITY;"
while IFS=, read -r YEAR ROUND WINNER OPPONENT WINNER_GOALS OPPONENT_GOALS
do
  [[ "$YEAR" == "year" ]] && continue
  OPPONENT_GOALS=${OPPONENT_GOALS%$'\r'}
  [[ "$YEAR" =~ ^[0-9]{4}$ && "$WINNER_GOALS" =~ ^[0-9]+$ && "$OPPONENT_GOALS" =~ ^[0-9]+$ ]] || exit 1
  WINNER_SQL=${WINNER//\'/\'\'}
  OPPONENT_SQL=${OPPONENT//\'/\'\'}
  ROUND_SQL=${ROUND//\'/\'\'}
  STATEMENTS+="INSERT INTO teams(name) VALUES ('$WINNER_SQL'),('$OPPONENT_SQL') ON CONFLICT(name) DO NOTHING;"
  STATEMENTS+="INSERT INTO games(year,round,winner_id,opponent_id,winner_goals,opponent_goals) SELECT $YEAR,'$ROUND_SQL',w.team_id,o.team_id,$WINNER_GOALS,$OPPONENT_GOALS FROM teams w CROSS JOIN teams o WHERE w.name='$WINNER_SQL' AND o.name='$OPPONENT_SQL';"
done < "$(dirname "$0")/games.csv"
STATEMENTS+="COMMIT;"
$PSQL "$STATEMENTS" >/dev/null
