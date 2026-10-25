// Demonstration poll: voters may select each option once.
const poll = new Map();

function addOption(option) {
  if (typeof option !== 'string' || option.trim() === '') {
    return 'Option cannot be empty.';
  }
  if (poll.has(option)) return 'Option "' + option + '" already exists.';
  poll.set(option, new Set());
  return 'Option "' + option + '" added to the poll.';
}

function vote(option, voterId) {
  if (!poll.has(option)) return 'Option "' + option + '" does not exist.';
  const voters = poll.get(option);
  if (voters.has(voterId)) {
    return 'Voter ' + voterId + ' has already voted for "' + option + '".';
  }
  voters.add(voterId);
  return 'Voter ' + voterId + ' voted for "' + option + '".';
}

function displayResults() {
  const results = ['Poll Results:'];
  for (const [option, voters] of poll) {
    results.push(option + ': ' + voters.size + ' votes');
  }
  return results.join('\n');
}

addOption('Turkey');
addOption('Morocco');
addOption('Spain');
vote('Turkey', 'demo-voter-1');
vote('Turkey', 'demo-voter-2');
vote('Morocco', 'demo-voter-3');
console.log(displayResults());
