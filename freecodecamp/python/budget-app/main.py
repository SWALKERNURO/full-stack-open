class Category:
    def __init__(self, name):
        self.name = name
        self.ledger = []

    def deposit(self, amount, description=''):
        self.ledger.append({'amount': amount, 'description': description})

    def withdraw(self, amount, description=''):
        if not self.check_funds(amount):
            return False
        self.ledger.append({'amount': -amount, 'description': description})
        return True

    def get_balance(self):
        return sum(entry['amount'] for entry in self.ledger)

    def transfer(self, amount, category):
        if not self.check_funds(amount):
            return False
        self.withdraw(amount, f'Transfer to {category.name}')
        category.deposit(amount, f'Transfer from {self.name}')
        return True

    def check_funds(self, amount):
        return amount <= self.get_balance()

    def __str__(self):
        lines = [self.name.center(30, '*')]
        for entry in self.ledger:
            description = entry['description'][:23]
            lines.append(f"{description:<23}{entry['amount']:>7.2f}")
        lines.append(f'Total: {self.get_balance():.2f}')
        return '\n'.join(lines)


def create_spend_chart(categories):
    spending = [sum(-entry['amount'] for entry in category.ledger
                    if entry['amount'] < 0) for category in categories]
    total = sum(spending)
    percentages = [(amount / total * 100) // 10 * 10 if total else 0
                   for amount in spending]
    lines = ['Percentage spent by category']
    for level in range(100, -1, -10):
        bars = ''.join('o  ' if percentage >= level else '   '
                       for percentage in percentages)
        lines.append(f'{level:>3}| ' + bars)
    lines.append('    ' + '-' * (len(categories) * 3 + 1))
    longest = max((len(category.name) for category in categories), default=0)
    for index in range(longest):
        labels = ''.join((category.name[index] if index < len(category.name)
                          else ' ') + '  ' for category in categories)
        lines.append('     ' + labels)
    return '\n'.join(lines)
