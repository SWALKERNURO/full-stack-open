/* Return the changed value and assign it to the caller's variable. */
int x = 5;
x = ChangeValue(x);
Console.WriteLine(x);
int ChangeValue(int value)
{
    value = 10;
    return value;
}