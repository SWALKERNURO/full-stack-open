def hanoi_solver(disks):
    rods = [list(range(disks, 0, -1)), [], []]
    states = [' '.join(str(rod) for rod in rods)]

    def move(count, source, destination, spare):
        if count <= 0:
            return
        move(count - 1, source, spare, destination)
        rods[destination].append(rods[source].pop())
        states.append(' '.join(str(rod) for rod in rods))
        move(count - 1, spare, destination, source)

    move(disks, 0, 2, 1)
    return '\n'.join(states)
