class HashTable:
    def __init__(self):
        self.collection = {}

    def hash(self, key):
        return sum(ord(character) for character in key)

    def add(self, key, value):
        index = self.hash(key)
        if index not in self.collection:
            self.collection[index] = {}
        self.collection[index][key] = value

    def remove(self, key):
        index = self.hash(key)
        bucket = self.collection.get(index)
        if bucket is not None:
            bucket.pop(key, None)

    def lookup(self, key):
        index = self.hash(key)
        return self.collection.get(index, {}).get(key)
