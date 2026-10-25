function isPrime(number) {
  if (!Number.isSafeInteger(number) || number < 2) return false;
  if (number === 2 || number === 3) return true;
  if (number % 2 === 0 || number % 3 === 0) return false;
  for (let divisor = 5; divisor <= number / divisor; divisor += 6) {
    if (number % divisor === 0 || number % (divisor + 2) === 0) return false;
  }
  return true;
}
module.exports = { isPrime };
