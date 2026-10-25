// Educational simulation with fictional transactions only.
class BankAccount {
  constructor() {
    this.balance = 0;
    this.transactions = [];
  }

  deposit(amount) {
    if (!Number.isFinite(amount) || amount <= 0) {
      return 'Deposit amount must be greater than zero.';
    }
    this.transactions.push({ type: 'deposit', amount });
    this.balance += amount;
    return 'Successfully deposited $' + amount + '. New balance: $' + this.balance;
  }

  withdraw(amount) {
    if (!Number.isFinite(amount) || amount <= 0 || amount > this.balance) {
      return 'Insufficient balance or invalid amount.';
    }
    this.transactions.push({ type: 'withdraw', amount });
    this.balance -= amount;
    return 'Successfully withdrew $' + amount + '. New balance: $' + this.balance;
  }

  checkBalance() {
    return 'Current balance: $' + this.balance;
  }

  listAllDeposits() {
    return 'Deposits: ' + this.transactions
      .filter(transaction => transaction.type === 'deposit')
      .map(transaction => transaction.amount).join(',');
  }

  listAllWithdrawals() {
    return 'Withdrawals: ' + this.transactions
      .filter(transaction => transaction.type === 'withdraw')
      .map(transaction => transaction.amount).join(',');
  }
}

const myAccount = new BankAccount();
myAccount.deposit(300);
myAccount.deposit(150);
myAccount.withdraw(40);
myAccount.deposit(75);
myAccount.withdraw(25);
console.log(myAccount.checkBalance());
console.log(myAccount.listAllDeposits());
console.log(myAccount.listAllWithdrawals());
