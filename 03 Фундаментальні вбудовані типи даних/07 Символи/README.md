# Символи

System.Char скорочено char допомогає працювати з символами. Додамо проект Chars

```cs
static void ExplorationOfChar()
{
    char myChar = '1';
    Console.WriteLine(myChar);
    Console.WriteLine($"Is {myChar}  digit :{char.IsDigit(myChar)}");
    Console.WriteLine($"Is {myChar}  letter :{char.IsLetter(myChar)}");
    
    myChar = 'H';
    Console.WriteLine($"Is {myChar}  letter :{char.IsLetter(myChar)}");
    Console.WriteLine($"Is {myChar}  whitespace :{char.IsWhiteSpace(myChar)}");
    
    myChar = ' ';
    Console.WriteLine($"Is {myChar}  whitespace :{char.IsWhiteSpace(myChar)}");
    Console.WriteLine($"Is third whitespace :{char.IsWhiteSpace("Hi girl",2)}");

    myChar = '!';
    Console.WriteLine($"Is {myChar}  punctuation :{char.IsPunctuation(myChar)}");

    myChar = '>';
    Console.WriteLine($"Is {myChar}  punctuation :{char.IsPunctuation(myChar)}");
}
ExplorationOfChar();
```
```
1
Is 1  digit :True
Is 1  letter :False
Is H  letter :True
Is H  whitespace :False
Is    whitespace :True
Is third whitespace :True
Is !  punctuation :True
Is >  punctuation :False
```