#!/bin/bash
set -euo pipefail
cd "$(dirname "$0")"
[[ "$(./element.sh)" == "Please provide an element as an argument." ]]
[[ "$(./element.sh 1)" == "$(./element.sh H)" ]]
[[ "$(./element.sh H)" == "$(./element.sh Hydrogen)" ]]
[[ "$(./element.sh "H'; SELECT 1; --")" == "I could not find that element in the database." ]]
[[ "$(./element.sh Ne)" == *"20.18 amu."* ]]
printf "Periodic-table CLI checks passed.\n"
