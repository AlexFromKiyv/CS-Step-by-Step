# Модіфікатори доступу

При роботі з типами треба завдти враховувати які члени видимі а які невидиме для різних частин програми. Типи (класи, інтерфейси, структури, перерахування та делегати), а також їхні члени (властивості, методи, конструктори та поля) визначаються за допомогою певного ключового слова, щоб контролювати, наскільки «видимим» елемент є для інших частин програми.

### public 

Public єлементи не мають обмежень доступу. Доступ можна отримати з об'єкта, похідного класу а також з іншої зовнішньої збірці.

### private

Private єлементи доступні тільки в класі або структурі в якій вони визначені.

### protected

Protected єлементи доступні в класі в якому вони визначені а також в усіх похідних класах. Похідний клас може бути в іншій збірці.
(Корисний при створенні ієрархії класів)

### internal

Internal єлементи доступні в рамках збірки.

### protected internal 

Protected internal єлементи доступні в рамках збірки та в межах похідного класу іншої збірки.  
(Корсиний при створенні бібліотеки та модульних тестів) 

### private protected 

 Можна отримати доступ за типами, похідними від класу, які оголошені в його збірці, що містить.


## Доступ за замовчуванням 

За замовчуванням типи є internal а члени типу private. Додамо рішеня AccessModifiers

```cs
    // is internal 
    class LivingOrganism
    {
        // is private
        LivingOrganism()
        {
        }
    }
```
```cs
LivingOrganism organism = new(); // don't work .LivingOrganism() inaccesible

```
При такому визначенні конструктор за замовченням перевизначенний як приватний і створити об'ект в іншій частині програми не буде змоги.

```cs
    internal class Animal
    {
        double weight;
        public Animal()
        {                       
        }
    }
```
```cs
void UsingAnimal()
{
    Animal animal = new();

    // animal.weight //no access
}
```
Зазвичай для створення об'єктів класу конcтруктор робиться public.

Можна зробити доступ до класу і до його членів.
```cs
    public class Dog
    {
        public string Breed;

        public Dog(string breed)
        {
            Breed = breed;
        }
    }
```
```
void UsingDog()
{

    Dog snupy = new("mutt");
    Console.WriteLine(snupy.Breed);

}

```
Такий тип і члени будуть доступні з завнішних збірок.


## Вкладені типи та структури

Типи та структури можуть бути об'явлені в межах типу private. Але тип в namaspase ні.

```cs
    //private class Horse // Element defined in namespace cannot be
    //                    // explixitly declared as private
    public class Hourse
    {
        private bool _sick;
    }
```

## Модіфікатори в рамках проекту.

AllKindModifiers.cs
```cs
    class AllKindModifiers
    {
        //private only for this class
        string VarWithEpmtyModifier = "VarWithEpmtyModifier";

        //private only for this class
        private string VarWithPrivate = "VarWithPrivate";
        
        // for this and inheritance classes from this project
        protected private string VarWithProtectedPrivate = "VarWithProtectedPrivate";

        // for this and inheritance classes 
        protected string VarWithProtected = "VarWithProtected";

        // for this project
        internal string VarWithInternal = "VarWithInternal";

        // for this project and inheritance classes other project
        protected internal string VarWithProtectedInternal = "VarWithProtectedInternal";

        //for all
        public string VarWithPublic = "VarWithPublic";

        //this field is private only for this class
        void MethodWithEpmtyModifier() => Console.WriteLine("MethodWithEpmtyModifier");

        //private only for this class
        private void MethodWithPrivate() => Console.WriteLine("MethodWithPrivate");

        //for this and inheritance classes from this project
        protected private void MethodWithProtectedPrivate() => Console.WriteLine("MethodWithProtectedPrivate");

        // for this and inheritance classes 
        protected void MethodWithProtected() => Console.WriteLine("MethodWithProtected");

        // for this project
        internal void MethodWithInternal() => Console.WriteLine("MethodWithInternal");
        
        // for this project and inheritance classes other project
        protected internal void MethodWithProtectedInternal() => Console.WriteLine("MethodWithProtectedInternal");

        // for all
        public void MethodWithPublic() => Console.WriteLine("MethodWithPublic");
    }
```
Класс вказано без модіфікатора, тому він буде internal. Тобто він буде доступний тільки в збірці.
Спробуємо використати в іншому класі збірки.

```cs
    internal class CallerAllKindModifiers
    {
        public void AttemptingToCall()
        {
            AllKindModifiers allDataAndMemeber = new();

            // inaccessible
            // allDataAndMemeber.VarWithEpmtyModifier 
            // allDataAndMemeber.VarWithPrivate
            // allDataAndMemeber.VarWithProtectedPrivate
            // allDataAndMemeber.VarWithProtected
            Console.WriteLine(allDataAndMemeber.VarWithInternal);
            Console.WriteLine(allDataAndMemeber.VarWithProtectedInternal);
            Console.WriteLine(allDataAndMemeber.VarWithPublic);

            // inaccessible
            //allDataAndMemeber.MethodWithEpmtyModifier();
            //allDataAndMemeber.MethodWithPrivate();
            //allDataAndMemeber.MethodWithProtected();
            allDataAndMemeber.MethodWithInternal();
            allDataAndMemeber.MethodWithProtectedInternal();
            allDataAndMemeber.MethodWithPublic();
        }
    }
```
Тут явно видно як обмежується доступ.

