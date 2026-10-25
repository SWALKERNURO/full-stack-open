#!/bin/bash
set -euo pipefail
if [[ $# -eq 0 ]]
then
  printf "Please provide an element as an argument.\n"
  exit 0
fi
PSQL=(psql --username=freecodecamp --dbname=periodic_table -X -q -t --no-align --set=ON_ERROR_STOP=1)
ELEMENT=$("${PSQL[@]}" -v input="$1" <<'SQL'
SELECT e.atomic_number,e.name,e.symbol,t.type,p.atomic_mass,p.melting_point_celsius,p.boiling_point_celsius
FROM elements e JOIN properties p USING(atomic_number) JOIN types t USING(type_id)
WHERE e.atomic_number::text=:'input' OR e.symbol=:'input' OR e.name=:'input';
SQL
)
if [[ -z "$ELEMENT" ]]
then
  printf "I could not find that element in the database.\n"
  exit 0
fi
IFS='|' read -r NUMBER NAME SYMBOL TYPE MASS MELTING BOILING <<< "$ELEMENT"
printf "The element with atomic number %s is %s (%s). It's a %s, with a mass of %s amu. %s has a melting point of %s celsius and a boiling point of %s celsius.\n" "$NUMBER" "$NAME" "$SYMBOL" "$TYPE" "$MASS" "$NAME" "$MELTING" "$BOILING"
