import pathlib
import queue
import re
import subprocess
import threading

SCRIPT = pathlib.Path(__file__).with_name("number_guess.sh")
USERNAME = "course_user_" + "x" * 10

def play(previous=None):
    process = subprocess.Popen(["bash", str(SCRIPT)], stdin=subprocess.PIPE,
                               stdout=subprocess.PIPE, stderr=subprocess.PIPE,
                               text=True, bufsize=1)
    lines = queue.Queue()
    def read_lines():
        for line in process.stdout:
            lines.put(line.rstrip("\n"))
    threading.Thread(target=read_lines, daemon=True).start()
    low, high, last, sent = 1, 1000, None, 0
    invalid_sent = False
    try:
        while True:
            line = lines.get(timeout=8)
            if line == "Enter your username:":
                process.stdin.write(USERNAME + "\n")
                process.stdin.flush()
            elif line.startswith("Welcome back,"):
                assert previous is not None, line
                assert "played 1 games" in line and f"took {previous} guesses." in line, line
            elif line.startswith("Welcome,"):
                assert previous is None, line
            elif line == "Guess the secret number between 1 and 1000:":
                if previous is None:
                    guess = "not-an-integer"
                    invalid_sent = True
                else:
                    last = (low + high) // 2
                    guess = str(last)
                sent += 1
                process.stdin.write(guess + "\n")
                process.stdin.flush()
            elif "guess again:" in line:
                if line.startswith("That is not an integer"):
                    assert invalid_sent
                elif line.startswith("It's higher"):
                    low = last + 1
                elif line.startswith("It's lower"):
                    high = last - 1
                else:
                    raise AssertionError(line)
                assert low <= high
                last = (low + high) // 2
                sent += 1
                process.stdin.write(str(last) + "\n")
                process.stdin.flush()
            elif line.startswith("You guessed it in"):
                match = re.fullmatch(r"You guessed it in (\d+) tries\. The secret number was (\d+)\. Nice job!", line)
                assert match and int(match[1]) == sent and int(match[2]) == last, line
                assert sent <= 11
                process.stdin.close()
                assert process.wait(timeout=3) == 0, process.stderr.read()
                return sent
    finally:
        if process.poll() is None:
            process.kill()
            process.wait()

first = play()
play(first)
print("Number-guess interactive and persistence checks passed.")
