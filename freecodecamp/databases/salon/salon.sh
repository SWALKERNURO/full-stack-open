#! /bin/bash
set -euo pipefail
PSQL=(psql --username=freecodecamp --dbname=salon -X -q -t --no-align --set=ON_ERROR_STOP=1)
while true
do
  SERVICES=$("${PSQL[@]}" -c "SELECT service_id || ') ' || name FROM services ORDER BY service_id")
  printf "\nWelcome to the salon. Which service would you like?\n%s\n" "$SERVICES"
  read -r SERVICE_ID_SELECTED || exit 0
  SERVICE_NAME=""
  if [[ "$SERVICE_ID_SELECTED" =~ ^[0-9]{1,9}$ ]]
  then
    SERVICE_NAME=$("${PSQL[@]}" -v service="$SERVICE_ID_SELECTED" <<'SQL'
SELECT name FROM services WHERE service_id=:'service'::int;
SQL
)
  fi
  [[ -n "$SERVICE_NAME" ]] && break
done
printf "\nWhat's your phone number?\n"
read -r CUSTOMER_PHONE || exit 0
CUSTOMER_ID=$("${PSQL[@]}" -v phone="$CUSTOMER_PHONE" <<'SQL'
SELECT customer_id FROM customers WHERE phone=:'phone';
SQL
)
if [[ -z "$CUSTOMER_ID" ]]
then
  printf "\nI don't have a record for that phone number. What's your name?\n"
  read -r CUSTOMER_NAME || exit 0
  CUSTOMER_ID=$("${PSQL[@]}" -v phone="$CUSTOMER_PHONE" -v name="$CUSTOMER_NAME" <<'SQL'
INSERT INTO customers(phone,name) VALUES (:'phone',:'name') RETURNING customer_id;
SQL
)
else
  CUSTOMER_NAME=$("${PSQL[@]}" -v id="$CUSTOMER_ID" <<'SQL'
SELECT name FROM customers WHERE customer_id=:'id'::int;
SQL
)
fi
printf "\nWhat time would you like your %s, %s?\n" "$SERVICE_NAME" "$CUSTOMER_NAME"
read -r SERVICE_TIME || exit 0
"${PSQL[@]}" -v customer="$CUSTOMER_ID" -v service="$SERVICE_ID_SELECTED" -v time="$SERVICE_TIME" <<'SQL'
INSERT INTO appointments(customer_id,service_id,time) VALUES (:'customer'::int,:'service'::int,:'time');
SQL
printf "\nI have put you down for a %s at %s, %s.\n" "$SERVICE_NAME" "$SERVICE_TIME" "$CUSTOMER_NAME"
