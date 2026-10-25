const { test } = require('node:test');
const assert = require('node:assert/strict');
const { isPrime } = require('./index');
test('prime classification agrees with trial division across a range', () => {
  for (let number = -10; number <= 5000; number++) {
    let expected = number >= 2;
    for (let divisor = 2; divisor * divisor <= number; divisor++) {
      if (number % divisor === 0) { expected = false; break; }
    }
    assert.equal(isPrime(number), expected, 'number ' + number);
  }
});
test('invalid numbers and known large values', () => {
  for (const value of [NaN, Infinity, -Infinity, 1.5, '7', null, undefined, Number.MAX_SAFE_INTEGER + 1]) assert.equal(isPrime(value), false);
  assert.equal(isPrime(999983), true);
  assert.equal(isPrime(999983 * 7), false);
});
