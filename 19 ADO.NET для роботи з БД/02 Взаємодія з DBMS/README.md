# Взаємодія з DBMS

Для розгляду питання створемо тестову базу даних SQL Server AutoLot. Ця база даних міститиме п’ять взаємопов’язаних таблиць (Inventory, Makes, Orders, Customers, and CreditRisks), які містять різні фрагменти даних, що представляють інформацію для вигаданої компанії з продажу автомобілів. Перш ніж ознайомитися з деталями бази даних, ви повинні налаштувати SQL Server і IDE SQL Server.  

# Налаштування SQL Server і Azure Data Studio

Якщо ви користуєтеся розробником на базі Windows і встановили Visual Studio 2022, у вас також встановлено спеціальний екземпляр SQL Server Express (називається LocalDb), який можна використовувати для всіх наступних прикладах. Якщо ви готові використовувати цю версію, перейдіть до розділу «Встановлення IDE SQL Server».

## Встановлення SQL Server

Для цього розділу та багатьох інших розділів вам знадобиться доступ до екземпляра SQL Server. Якщо ви використовуєте машину розробки, відмінну від Windows, і не маєте доступного зовнішнього екземпляра SQL Server або не хочете використовувати зовнішній екземпляр SQL Server, ви можете запустити SQL Server у контейнері Docker на вашому Mac або Linux робоча станція на базі. Docker також працює на машинах Windows, тому ви можете запускати приклади в цій книзі за допомогою Docker незалежно від обраної вами операційної системи.

### Встановлення SQL Server у контейнер Docker

Docker Desktop можна завантажити з www.docker.com/get-started. Завантажте та встановіть відповідну версію (Windows, Mac, Linux) для вашої робочої станції (вам знадобиться безкоштовний обліковий запис користувача DockerHub). Переконайтеся, що ви вибрали контейнери Linux, коли з’явиться запит. Вибір контейнера (Windows або Linux) — це операційна система, яка працює всередині контейнера, а не операційна система вашої робочої станції. 

Далі тренба витягнути образу та запустити SQL Server 2019. Контейнери засновані на образах, і кожний образ є багатошаровим набором, який створює кінцевий продукт. 
Щоб отримати образ, необхідний для запуску SQL Server 2019 у контейнері, відкрийте командне вікно та введіть таку команду:
```console 
docker pull mcr.microsoft.com/mssql/server:2019-latest
```
Після завантаження образу на комп’ютер потрібно запустити SQL Server. Для цього введіть таку команду (всі в одному рядку):

```console
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd' -p 5433:1433 --name AutoLot -h AutoLotHost -d mcr.microsoft.com/mssql/server:2019-latest
```

Попередня команда приймає ліцензійну угоду кінцевого користувача, встановлює пароль (у реальному житті потрібно використовувати надійний пароль), встановлює зіставлення портів (порт 5433 на вашому комп’ютері зіставляється з портом за замовчуванням для SQL Server, яким є 1433) у контейнері, називає контейнер (AutoLot), називає хост (AutoLotHost) і, нарешті, інформує Docker використовувати попередньо завантажений образ. Це не налаштування, які ви хочете використовувати для реального розвитку. Щоб отримати інформацію про зміну пароля SA та переглянути навчальний посібник, перейдіть на сторінку https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1 -cmd.

Щоб переконатися, що він працює, введіть команду docker ps -a у командному рядку.

```console
C:\Users\japik>docker ps -a
CONTAINER ID IMAGE                                       PORTS                    NAMES
347475cfb823 mcr.microsoft.com/mssql/server:2019-latest  0.0.0.0:5433->1433/tcp   AutoLot
```
Щоб зупинити контейнер, введіть docker stop 34747, де цифри 34747 є першими п’ятьма символами ідентифікатора контейнера. Щоб перезапустити контейнер, введіть docker start 34747, знову оновивши команду з початком ідентифікатора вашого контейнера.

Ви також можете використовувати назву контейнера (у цьому прикладі AutoLot) із командами Docker CLI, наприклад, docker start AutoLot.

Якщо ви хочете використовувати  Docker Dashboard, клацніть правою кнопкою миші корабель Docker (у системному треї) і виберіть  Docker Dashboard, і ви побачите образ, запущене на порту 5433.

Щоб підключитися до SQL Server за допомогою зашифрованого з’єднання, на хості повинен бути встановлений сертифікат. Дотримуйтеся вказівок у документах, щоб установити сертифікат у вашому контейнері Docker і ввімкнути безпечні з’єднання: https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-docker-container-security?view =sql-server-ver15. Зашифроване підключення до SQL Server слід використовувати в реальній розробці.

### Встановлення SQL Server 2019

Спеціальний екземпляр SQL Server під назвою LocalDb інсталюється разом із Visual Studio 2022. Якщо ви вирішите не використовувати SQL Server Express LocalDB або Docker і користуєтеся комп’ютером Windows, вам потрібно інсталювати SQL Server 2019 Developer Edition. SQL Server 2019 Developer Edition є безкоштовним і його можна завантажити тут:

https://www.microsoft.com/en-us/sql-server/sql-server-downloads




### Встановлення IDE SQL Server

Azure Data Studio — це нова IDE для використання з SQL Server. Вонон безкоштовне і кросплатформний, тому працюватиме на Windows, Mac або Linux. Його можна завантажити звідси:

https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio

Якщо ви користуєтеся комп’ютером Windows і надаєте перевагу SQL Server Management Studio (SSMS), ви можете завантажити останню копію звідси:

https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms.

Після встановлення Azure Data Studio або SSMS настав час підключитися до екземпляра бази даних.


### Підключення до SQL Server у контейнері Docker

Щоб підключитися до екземпляра SQL Server, який працює в контейнері Docker, спочатку переконайтеся, що він запущений і працює. Потім натисніть «Create a connection» в Azure Data Studio.

У діалоговому вікні «Connection Details» введіть .,5433 для значення «Server». Крапка вказує на поточний host, а 5433 — це порт, який ви вказали під час створення екземпляра SQL Server у контейнері Docker. Хост і порт повинні бути розділені комою. Введіть sa для імені користувача; пароль той самий, який ви ввели під час створення екземпляра SQL Server. Ім’я необов’язкове, але дозволяє швидко вибрати це підключення в наступних сеансах Azure Data Studio.

### Підключення до SQL Server LocalDb

В Azure Data Studio натисніть «Create a connection».

Щоб підключитися до інстальованої у Visual Studio версії SQL Server Express LocalDb, оновіть інформацію про підключення вказавши 

    Server : (localdb)\mssqllocaldb

    Name(optional) : AutoLotLocalDb 

Підключаючись до LocalDb, ви можете використовувати Windows Authentication, оскільки екземпляр працює на тому самому комп’ютері, що й Azure Data Studio, і має той самий контекст безпеки, що й користувач, який зараз увійшов у систему.

Якщо ви підключаєтесь до будь-якого іншого екземпляра SQL Server, відповідно оновіть властивості підключення.

# Робота з Базою даних

Стосовно бази даних є два варіанти. Перший варіант відновити існуючу БД з резервонї комії. Другий створити нову базу за допомогою запитів до сервера БД.

## Відновлення резервної копії бази даних AutoLot

Замість створення бази даних з нуля ви можете використовувати SSMS або Azure Data Studio, щоб відновити одну з наданих резервних копій, які містяться файла. Є дві резервні копії: одна з назвою AutoLotWindows.ba_ призначена для використання на машині Windows (LocalDb, Windows Server тощо), а друга з назвою AutoLotDocker.ba_ призначена для використання в контейнері Docker. Скопіюйте необхіду вам копію в відоме вам місце. Змініть розширення на з ba_ на bak.

### Копіювання файлу резервної копії у ваш контейнер

Якщо ви використовуєте SQL Server у контейнері Docker, спочатку потрібно скопіювати файл резервної копії в контейнер. Docker CLI надає механізм для роботи з файловою системою контейнера. Спочатку створіть новий каталог для резервного копіювання за допомогою такої команди у вікні команд на вашому хості:

```console 
docker exec -it AutoLot mkdir var/opt/mssql/backup
```
Структура шляху має відповідати операційній системі контейнера (у цьому випадку Ubuntu), навіть якщо ваш хост-машина базується на Windows. Далі скопіюйте резервну копію в новий каталог за допомогою такої команди (оновивши розташування AutoLotDocker.bak до відносного або абсолютного шляху вашої локальної машини):

```console
[Windows]
docker cp .\AutoLotDocker.bak AutoLot:var/opt/mssql/backup
[Non-Windows]
docker cp ./AutoLotDocker.bak AutoLot:var/opt/mssql/backup
```
Зауважте, що структура вихідного каталогу відповідає хост-машині (у моєму прикладі Windows), а ціль — це ім’я контейнера, а потім шлях до каталогу (у форматі цільової ОС).

### Відновлення бази даних на SQL Server в SSMS (Docker Windows)

Оскілкі в контейнері знаходиться SSMS то процес відбуваеться незалежно від середовиша де працює Server SQL. Щоб відновити базу даних за допомогою SSMS, клацніть правою кнопкою миші вузол Databases у Object Explorer. Виберіть Restore Database. Виберіть Device і клацніть три крапки. Відкриється діалогове вікно «Select Backup Device». Залиште для параметра «Backup media type» значення «Файл», а потім натисніть «Додати», перейдіть до файлу AutoLotDocker.bak у контейнері та натисніть «ОК». Коли ви повернетеся на головний екран відновлення, натисніть OK


### Відновлення бази даних за допомогою Azure Data Studio

Для відновлення бази в Azure Data Studio треба 

1. Вибрати раніше створенне з'єдннання
2. View > Command Palette (Ctrl+Shift+P) > Restore
3. Restore from > Backup file
4. Вказати Buckup file path
5. Restore

## Створення бази даних з нуля.

Створення бази даних з нуля може дати більше розуміння з чого вона складається і як створити свою власну. Можна використати або Azure Data Studio або SQL Server Management Studio. 


### Строрення БД.

Щоб створити базу даних AutoLot, підключіться до сервера бази даних за допомогою Azure Data Studio. Відкрийте новий запит, вибравши  File ➤ New Query (або натиснувши Ctrl+N) і ввівши такий текст команди:

```sql
USE [master]
GO
CREATE DATABASE [AutoLot]
GO
ALTER DATABASE [AutoLot] SET RECOVERY SIMPLE
GO
```
Окрім зміни режиму відновлення на простий, це створює базу даних AutoLot із використанням стандартних параметрів SQL Server. Натисніть «Run» (або натисніть F5), щоб створити базу даних.

### Створення таблиць

База даних AutoLot містить п’ять таблиць: Inventory, Makes, Customers, Orders і CreditRisks.


### Створення таблиці Inventory

Таблиця для обліку наявних авто.

```sql
USE [AutoLot]
GO
CREATE TABLE [dbo].[Inventory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [MakeId] [int] NOT NULL,
    [Color] [nvarchar](50) NOT NULL,
    [PetName] [nvarchar](50) NOT NULL,
    [TimeStamp] [timestamp] NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED
(
  [Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
```
У таблиці Inventory зберігається зовнішній ключ до (ще не створеної) таблиці Makes.

Якщо ви не знайомі з типом даних SQL Server TimeStamp (який відповідає byte[] у C#), не хвилюйтеся про це зараз. Просто знайте, що він використовується для перевірки паралельності на рівні рядків.

### Створення таблиці Makes

Таблиця для виробників авто.

```sql
USE [AutoLot]
GO
CREATE TABLE [dbo].[Makes](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [TimeStamp] [timestamp] NULL,
 CONSTRAINT [PK_Makes] PRIMARY KEY CLUSTERED
(
  [Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
```

### Створення таблиці Customers

Таблиця для кліентів.

```sql
USE [AutoLot]
GO
CREATE TABLE [dbo].[Customers](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [FirstName] [nvarchar](50) NOT NULL,
  [LastName] [nvarchar](50) NOT NULL,
  [TimeStamp] [timestamp] NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED
(
  [Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
```

### Створення таблиці Orders
Ви будете використовувати наступну таблицю для представлення автомобіля, який замовив даний клієнт.

```sql
USE [AutoLot]
GO
CREATE TABLE [dbo].[Orders](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [CustomerId] [int] NOT NULL,
  [CarId] [int] NOT NULL,
  [TimeStamp] [timestamp] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED
(
  [Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
```

### Створення таблиці CreditRisks

Представляти клієнтів, які вважаються кредитними ризиками.

```sql
USE [AutoLot]
GO
CREATE TABLE [dbo].[CreditRisks](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [FirstName] [nvarchar](50) NOT NULL,
  [LastName] [nvarchar](50) NOT NULL,
  [CustomerId] [int] NOT NULL,
  [TimeStamp] [timestamp] NULL,
 CONSTRAINT [PK_CreditRisks] PRIMARY KEY CLUSTERED
(
    [Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
```

### Створення зв’язків таблиць

Давайте додамо зв’язки зовнішніх ключів між взаємопов’язаними таблицями.

### Створення зв’язку Inventory з Makes 

Кожний автомобіль має виробника.

```sql
USE [AutoLot]
GO
CREATE NONCLUSTERED INDEX [IX_Inventory_MakeId] ON [dbo].[Inventory]
(
  [MakeId] ASC
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Make_Inventory] FOREIGN KEY([MakeId])
REFERENCES [dbo].[Makes] ([Id])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Make_Inventory]
GO
```

### Створення зв’язку Orders з Inventory

Замовлення відносится до автомобіля.

```sql 
USE [AutoLot]
GO
CREATE NONCLUSTERED INDEX [IX_Orders_CarId] ON [dbo].[Orders]
(
  [CarId] ASC
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Inventory] FOREIGN KEY([CarId])
REFERENCES [dbo].[Inventory] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Inventory]
GO
```

### Створення зв’язку Orders з Customers


```sql 
USE [AutoLot]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Orders_CustomerId_CarId] ON [dbo].[Orders]
(
  [CustomerId] ASC,
  [CarId] ASC
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
GO
```

### Створення зв’язку Customers з CreditRisks 

```sql 
USE [AutoLot]
GO
CREATE NONCLUSTERED INDEX [IX_CreditRisks_CustomerId] ON [dbo].[CreditRisks]
(
  [CustomerId] ASC
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CreditRisks]  WITH CHECK ADD  CONSTRAINT [FK_CreditRisks_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CreditRisks] CHECK CONSTRAINT [FK_CreditRisks_Customers]
GO
```

### Створення Stored Procedure

 ADO.NET дозволяє викликати Stored Procedure. Це підпрограми коду, що зберігаються в базі даних, які виконують певні дії. Stored Procedures можуть повертати дані або просто оперувати даними, нічого не повертаючи. Створимо процедуру.

```sql
USE [AutoLot]
GO
CREATE PROCEDURE [dbo].[GetPetName]
@carID int,
@petName nvarchar(50) output
AS
SELECT @petName = PetName from dbo.Inventory where Id = @carID
GO
```
Повертає ім’я улюбленця автомобіля на основі наданого carId.

### Додавання даних в таблицю Makes

```sql
USE [AutoLot]
GO
SET IDENTITY_INSERT [dbo].[Makes] ON
INSERT INTO [dbo].[Makes] ([Id], [Name]) VALUES (1, N'VW')
INSERT INTO [dbo].[Makes] ([Id], [Name]) VALUES (2, N'Ford')
INSERT INTO [dbo].[Makes] ([Id], [Name]) VALUES (3, N'Saab')
INSERT INTO [dbo].[Makes] ([Id], [Name]) VALUES (4, N'Yugo')
INSERT INTO [dbo].[Makes] ([Id], [Name]) VALUES (5, N'BMW')
INSERT INTO [dbo].[Makes] ([Id], [Name]) VALUES (6, N'Pinto')
SET IDENTITY_INSERT [dbo].[Makes] OFF
```

### Додавання даних в таблицю Inventory 

```sql
USE [AutoLot]
GO
SET IDENTITY_INSERT [dbo].[Inventory] ON
GO
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (1, 1, N'Black', N'Zippy')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (2, 2, N'Rust', N'Rusty')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (3, 3, N'Black', N'Mel')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (4, 4, N'Yellow', N'Clunker')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (5, 5, N'Black', N'Bimmer')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (6, 5, N'Green', N'Hank')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (7, 5, N'Pink', N'Pinky')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (8, 6, N'Black', N'Pete')
INSERT INTO [dbo].[Inventory] ([Id], [MakeId], [Color], [PetName]) VALUES (9, 4, N'Brown', N'Brownie')SET IDENTITY_INSERT [dbo].[Inventory] OFF
GO
```

### Додавання даних в таблицю Customers

```sql
USE [AutoLot]
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([Id], [FirstName], [LastName]) VALUES (1, N'Dave', N'Brenner')
INSERT INTO [dbo].[Customers] ([Id], [FirstName], [LastName]) VALUES (2, N'Matt', N'Walton')
INSERT INTO [dbo].[Customers] ([Id], [FirstName], [LastName]) VALUES (3, N'Steve', N'Hagen')
INSERT INTO [dbo].[Customers] ([Id], [FirstName], [LastName]) VALUES (4, N'Pat', N'Walton')
INSERT INTO [dbo].[Customers] ([Id], [FirstName], [LastName]) VALUES (5, N'Bad', N'Customer')
SET IDENTITY_INSERT [dbo].[Customers] OFF
```

### Додавання даних в таблицю Orders

```sql
USE [AutoLot]
GO
SET IDENTITY_INSERT [dbo].[Orders] ON
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [CarId]) VALUES (1, 1, 5)
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [CarId]) VALUES (2, 2, 1)
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [CarId]) VALUES (3, 3, 4)
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [CarId]) VALUES (4, 4, 7)
SET IDENTITY_INSERT [dbo].[Orders] OFF
```

### Додавання даних в таблицю CreditRisks

```sql
USE [AutoLot]
GO
SET IDENTITY_INSERT [dbo].[CreditRisks] ON
INSERT INTO [dbo].[CreditRisks] ([Id], [FirstName], [LastName], [CustomerId]) VALUES (1, N'Bad', N'Customer', 5)
SET IDENTITY_INSERT [dbo].[CreditRisks] OFF
```
На цьому база даних AutoLot завершена. Звичайно, це далека від реальної бази даних додатків, але вона задовольнить ваші потреби в цій главі. Тепер, коли у вас є база даних для тестування, можна заглибитися в деталі моделі постачальника даних ADO.NET.


# Шаблон Factory постачальника даних ADO.NET

Шаблон Factory для постачальника даних .NET дозволяє створювати єдину кодову базу, використовуючи узагальнені типи доступу до даних. Всі класи в постачальнику даних походять від тих самих базових класів, визначених у просторі імен System.Data.Common.

    DbCommand: абстрактний базовий клас для всіх класів команд

    DbConnection: абстрактний базовий клас для всіх класів підключення

    DbDataAdapter: абстрактний базовий клас для всіх класів адаптерів даних

    DbDataReader: абстрактний базовий клас для всіх класів читачів даних

    DbParameter: абстрактний базовий клас для всіх класів параметрів

    DbTransaction: абстрактний базовий клас для всіх класів транзакцій

Кожен із сумісних із .NET постачальників даних містить тип класу, який походить від System.Data.Common.DbProviderFactory. Цей базовий клас визначає кілька методів для отримання спеціфічних для провайдера об'єктів роботи з даними. Ось члени DbProviderFactory:

```cs
public abstract class DbProviderFactory
{
..public virtual bool CanCreateDataAdapter { get;};
..public virtual bool CanCreateCommandBuilder { get;};
  public virtual DbCommand CreateCommand();
  public virtual DbCommandBuilder CreateCommandBuilder();
  public virtual DbConnection CreateConnection();
  public virtual DbConnectionStringBuilder
    CreateConnectionStringBuilder();
  public virtual DbDataAdapter CreateDataAdapter();
  public virtual DbParameter CreateParameter();
  public virtual DbDataSourceEnumerator
    CreateDataSourceEnumerator();
}
```

Щоб отримати тип, похідний від DbProviderFactory, для вашого постачальника даних, кожен постачальник надає статичну властивість, яка використовується для повернення правильного типу. Щоб повернути версію SQL Server DbProviderFactory, використовуйте такий код:

```cs
// Get the factory for the SQL data provider.
DbProviderFactory sqlFactory =
  Microsoft.Data.SqlClient.SqlClientFactory.Instance;
```
Щоб зробити програму більш універсальною, ви можете створити фабрику DbProviderFactory, яка повертає певний варіант DbProviderFactory на основі налаштування у файлі appsettings.json для програми. Коли ви отримаєте фабрику для вашого провайдера даних можна отримати асоційовані об’єкти даних провайдера (наприклад, з’єднання, команди та зчитувачі даних).

## Використаня DbProviderFactory

Створіть новий проект C# Console Application (під назвою DataProviderFactory), який роздрукує залишки автомобілів з бази даних AutoLot. Для цього початкового прикладу ви жорстко закодуєте логіку доступу до даних безпосередньо в консольній програмі (для спрощення), але далі буде розглянуто більш універсальний варіант.

Додайте в проект пакети:
Microsoft.Extensions.Configuration.Json
System.Data.Common
System.Data.Odbc
System.Data.OleDb
Microsoft.Data.SqlClient

Далі визначте константу компілятора PC (якщо ви використовуєте ОС Windows)

```xml
<PropertyGroup>
  <DefineConstants>PC</DefineConstants>
</PropertyGroup>
```

Додайте DataProviderEnum.cs

DataProviderFactory\DataProviderEnum.cs
```cs
namespace DataProviderFactory;

//OleDb is Windows only
enum DataProviderEnum
{
    SqlServer,
#if PC
    OleDb,
#endif
    Odbc
}
```

Додайте новий файл JSON під назвою appsettings.json. Оскільки БД може бути в контейнері Docker або в окремому екземплярі SQL Sever варіанти підключень можуть бути різними. Ост файл з різними варіантам.

```json
{
  "ProviderName": "SqlServer",
  //"ProviderName": "OleDb",
  //"ProviderName": "Odbc",
  "SqlServer": {
    // for localdb use @"Data Source=(localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot"
    "ConnectionString": "Data Source=.,5433;User Id=sa;Password=P@ssw0rd;Initial Catalog=AutoLot;Encrypt=False"
  },
  "Odbc": {
    // for localdb use @"Driver={ODBC Driver 17 for SQL Server};Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=Yes";
    "ConnectionString": "Driver={ODBC Driver 17 for SQL Server};Server=localhost,5433;Database=AutoLot;UId=sa;Pwd=P@ssw0rd;Encrypt=False;"
  },
  "OleDb": {
    // if localdb use @"Provider=SQLNCLI11;Data Source=(localdb)\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=SSPI"),
    "ConnectionString": "Provider=SQLNCLI11;Data Source=.,5433;User Id=sa;Password=P@ssw0rd;Initial Catalog=AutoLot;Encrypt=False;"
  }
}
```
Якшо використавується LocalDb.

```json
{
  "ProviderName": "SqlServer",
  //"ProviderName": "OleDb",
  //"ProviderName": "Odbc",
  "SqlServer": {
    "ConnectionString": "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot"
  },
  "Odbc": {
    "ConnectionString": "Driver={ODBC Driver 17 for SQL Server};Server=(localdb)\\mssqllocaldb;Database=AutoLot;Trusted_Connection=Yes"
  },
  "OleDb": {
    "ConnectionString": "Provider=SQLNCLI11;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=SSPI"
  }
}
```

Під час використання SQL Server у контейнері Docker, у якому не встановлено сертифікат, рядок підключення має бути незашифрованим, тому ми маємо Encrypt=False; у рядках підключення Docker. Для реальних програм не використовуйте цей параметр; замість цього переконайтеся, що контейнер (або ваш екземпляр SQL Server) має сертифікат, і використовуйте Encrypt=True; замість цього.

При кожному виконанні файл конфігурації повинен бути в каталозі виконання. У файлі проекту налаштуемо MSBuild для копіювання файлу налаштувань JSON у вихідний каталог під час кожної збірки.

```xml
<ItemGroup>
  <None Update='appsettings.json'>
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
</ItemGroup>
```
CopyToOutputDirectory чутливий до пробілів. Переконайтеся, що все в одному рядку без пробілів навколо слова Always. 

Тепер, коли у вас є належний файл appsettings.json, ви можете читати значення постачальника та ConnectionString за допомогою конфігурації .NET.

Створимо функцію що повертає дані з файла конфігурації

```cs
void Run()
{
    Console.WriteLine(GetProviderFromConfiguration());
}
Run();

static (DataProviderEnum Provider, string? ConnectionString) GetProviderFromConfiguration()
{
    IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();

    string? providerName = config["ProviderName"];

    if (Enum.TryParse<DataProviderEnum>(providerName, out DataProviderEnum provider))
    {
        return (provider, config[$"{providerName}:ConnectionString"]);
    }
    throw new Exception("Invalid data provider value supplied.");
}
```
```console
(SqlServer, Data Source=(localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot)
```
Ця функція зчитує конфігурацію, встановлє правильне значення DataProviderEnum, отримують рядок підключення.

Створемо функцію шо повертає екземпляр DbProviderFactory відповідно до значення DataProviderEnum.

```cs
void Run()
{
    Console.WriteLine(GetDbProviderFactory(DataProviderEnum.OleDb));
}
Run();

//

static DbProviderFactory GetDbProviderFactory(DataProviderEnum provider)
	=> provider switch
	{
		DataProviderEnum.SqlServer => SqlClientFactory.Instance,
		DataProviderEnum.Odbc => OdbcFactory.Instance,
#if PC
        DataProviderEnum.OleDb => OleDbFactory.Instance,
#endif
		_ => SqlClientFactory.Instance

    };

```
```
System.Data.OleDb.OleDbFactory
```
Використаємо ці функції для отримання даних з БД.

```cs
static void Run()
{
    PrintOutSimpleList();
}
Run();

static void PrintOutSimpleList()
{
    var (provider, connectionString) = GetProviderFromConfiguration();

    DbProviderFactory factory = GetDbProviderFactory(provider);

    // Get the connection object
    using DbConnection? connection = factory.CreateConnection();
    Console.WriteLine($"Your connection object is : {connection?.GetType().Name}");

    if (connection == null) return;

    // Opening a connection to the database
    connection.ConnectionString = connectionString;
    connection.Open();

    // Make command object
    DbCommand? command = factory.CreateCommand();
    Console.WriteLine($"Your command object is a : {command?.GetType().Name}");

    if (command == null) return;

    command.Connection = connection;
    command.CommandText =
        "Select i.Id, m.Name From Inventory " +
        "i inner join Makes m on m.Id = i.MakeId";

    // Make data reader
    using DbDataReader reader = command.ExecuteReader();
    Console.WriteLine($"Your data reader object is a : {reader.GetType().Name}");

    Console.WriteLine("\n\t\tInventory\n");
    //Print out data
    while (reader.Read())
    {
        Console.WriteLine($"\t{reader["Id"]}\t{reader["Name"]} ");
    }
}
```
```console
Your connection object is : SqlConnection
Your command object is a : SqlCommand
Your data reader object is a : SqlDataReader

                Inventory

        1       VW
        2       Ford
        3       Saab
        4       Yugo
        9       Yugo
        5       BMW
        6       BMW
        7       BMW
        8       Pinto
```
Зауважте, що для діагностичних цілей ви використовуєте служби відображення, щоб надрукувати ім’я базового з’єднання, команди та пристрою читання даних.

Тепер змінемо файл налаштовування вказавши іншого провайдера. 

```json
{
  //"ProviderName": "SqlServer",
  //"ProviderName": "OleDb",
  "ProviderName": "Odbc",
  "SqlServer": {
    "ConnectionString": "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot"
  },
  "Odbc": {
    "ConnectionString": "Driver={ODBC Driver 17 for SQL Server};Server=(localdb)\\mssqllocaldb;Database=AutoLot;Trusted_Connection=Yes"
  },
  "OleDb": {
    "ConnectionString": "Provider=SQLNCLI11;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=SSPI"
  }
}
```
```console
Your connection object is : OdbcConnection
Your command object is a : OdbcCommand
Your data reader object is a : OdbcDataReader

                Inventory

        1       VW
        2       Ford
        3       Saab
        4       Yugo
        9       Yugo
        5       BMW
        6       BMW
        7       BMW
        8       Pinto
```

На цьому етапі достатньо знати, що ви можете використовувати модель фабрики постачальників даних ADO.NET для створення єдиної кодової бази, яка може використовувати різні постачальники даних декларативним способом. Змінюється об'ект провайдера та рядок підключення весь інший код залишається однаковим для вирішування завдання.

Хоча підхід з використанням фабрики досить потужний він не без доліків. Ви повинні переконатися, що база коду використовує лише типи та методи, спільні для всіх постачальників через членів абстрактних базових класів. Таким чином, під час створення бази коду ви обмежені членами, доступними DbConnection, DbCommand та іншими типами простору імен System.Data.Common. Враховуючи це, ви можете виявити, що цей узагальнений підхід не дозволяє вам отримати прямий доступ до деяких наворотів конкретної СУБД. Якщо ви повинні мати можливість викликати певних членів основного постачальника (наприклад, SqlConnection), ви можете зробити це за допомогою явного приведення, як у цьому прикладі:

```cs
if (connection is SqlConnection sqlConnection)
{
  // Print out which version of SQL Server is used.
  WriteLine(sqlConnection.ServerVersion);
}
```
Однак, роблячи це, вашу кодову базу стає дещо важче підтримувати (і менш гнучкою), оскільки ви повинні додати кілька перевірок під час виконання. Тим не менш, якщо вам потрібно побудувати бібліотеки доступу до даних ADO.NET найбільш гнучким можливим способом, використання шаблона factory постачальника даних забезпечує чудовий механізм для цього.

Entity Framework Core і його підтримка впровадження залежностей(dependency injection) значно спрощує створення бібліотек доступу до даних, яким потрібен доступ до різних джерел даних.

# Типи Connection, Command, and DataReader детально.

Як показано в попередньому прикладі, ADO.NET дозволяє вам взаємодіяти з базою даних за допомогою об’єктів підключення, команди та зчитувача даних вашого постачальника даних.Створимо розширений приклад, щоб глибше зрозуміти ці об’єкти. 
У наведеному попередньому прикладі вам потрібно виконати наступні кроки, якщо ви хочете підключитися до бази даних і прочитати записи за допомогою об’єкта читача даних:

1. Створіть, налаштуйте та відкрийте свій об’єкт підключення.
2. Створіть і налаштуйте об’єкт команди, вказавши об’єкт підключення як аргумент конструктора або за допомогою властивості Connection.
3. Викличте ExecuteReader() у налаштованому об’єкті команд.
4. Обробляйте кожен запис за допомогою методу Read() засобу читання даних.

Щоб почати, створіть новий проект консольної програми під назвою AutoLot.DataReader і додайте пакет Microsoft.Data.SqlClient.

```cs
using Microsoft.Data.SqlClient;

static void UseDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    Console.WriteLine($"Connection state: {connection.State}\n");

    string sql = @"Select i.id, m.Name as Make, i.Color, i.Petname
                   FROM Inventory i
                   INNER JOIN Makes m on m.Id = i.MakeId";

    SqlCommand myCommand = new(sql, connection);

    using SqlDataReader myDataReader = myCommand.ExecuteReader();

    while (myDataReader.Read())
    {
        Console.WriteLine($"{myDataReader["Make"]} {myDataReader["Color"]} {myDataReader["PetName"]}");
    }

}
UseDataReader();
```
```console
Connection state: Open

VW Black Zippy
Ford Rust Rusty
Saab Black Mel
Yugo Yellow Clunker
BMW Black Bimmer
BMW Green Hank
BMW Pink Pinky
Pinto Black Pete
Yugo Brown Brownie
```

## Робота з об’єктами Connection

Перший крок, який потрібно зробити під час роботи з постачальником даних, — це встановити сеанс із джерелом даних за допомогою об’єкта підключення (який, як ви пам’ятаєте, походить від DbConnection). Об’єкти підключення .NET надаються з відформатованим рядком підключення; цей рядок містить кілька пар ім'я-значення, розділених крапкою з комою. Ви використовуєте цю інформацію, щоб визначити ім’я комп’ютера, до якого ви хочете підключитися, необхідні налаштування безпеки, ім’я бази даних на цьому комп’ютері та іншу інформацію про постачальника даних.

При використані контейнера Docker рядок підключення може виглядати таким чином:

  Data Source=.,5433;User Id=sa;Password=P@ssw0rd;Initial Catalog=AutoLot;Encrypt=False


Як ви можете зробити висновок ,Initial Catalog стосується бази даних, з якою ви хочете встановити сеанс. Data Source визначає ім'я машини, яка підтримує базу даних.Використовуеться .,5433, який стосується хост-комп’ютера (крапка такий самий, як і використання «localhost»), і порт 5433, який є портом, який контейнер Docker зіставив із портом SQL Server. Якби ви використовували інший екземпляр, ви б визначили властивість як machinename,port\instance. Наприклад, MYSERVER\SQLSERVER2019 означає, що MYSERVER — це ім’я сервера, на якому працює SQL Server, використовується порт за замовчуванням, а SQLSERVER2019 — це ім’я екземпляра. Якщо машина локальна для розробки, ви можете використовувати крапку (.) або маркер (localhost) для імені сервера. Якщо екземпляр SQL Server є екземпляром за замовчуванням, ім’я екземпляра не вказано. Наприклад, якщо ви створили AutoLot на інсталяції Microsoft SQL Server, налаштованій як екземпляр за замовчуванням на вашому локальному комп’ютері, ви повинні використовувати «Data Source=localhost».
Окрім цього, ви можете надати будь-яку кількість токенів, які представляють облікові дані безпеки. Якщо для Integrated Security встановлено значення true, для автентифікації та авторизації використовуються поточні облікові дані Windows.
Після того, як ви встановите рядок з’єднання, ви можете використовувати виклик Open(), щоб встановити з’єднання з СУБД. На додаток до членів ConnectionString, Open() і Close(), об’єкт підключення надає ряд членів, які дозволяють налаштувати додаткові параметри щодо вашого з’єднання, наприклад параметри часу очікування та інформацію про транзакції.

Члени типу DbConnection

  BeginTransaction() : Цей метод використовується для початку транзакції бази даних.

  ChangeDatabase() : Цей метод використовується для зміни бази даних під час відкритого підключення.

  ConnectionTimeout : Ця властивість лише для читання повертає кількість часу очікування під час встановлення з’єднання перед припиненням і створенням помилки (значення за замовчуванням залежить від постачальника). Якщо ви бажаєте змінити значення за замовчуванням, укажіть сегмент часу очікування з’єднання в рядку підключення (наприклад, час очікування з’єднання=30).

  Database : Ця read-only властивість отримує назву бази даних, яку підтримує об’єкт підключення.

  DataSource : Ця read-only властивість отримує розташування бази даних, яку підтримує об’єкт підключення.

  GetSchema() : Цей метод повертає об’єкт DataTable, який містить інформацію про схему з джерела даних.

  State : Ця read-only властивість отримує поточний стан підключення, який представлено переліком ConnectionState.

Властивості типу DbConnection зазвичай доступні лише для читання і корисні лише тоді, коли потрібно отримати характеристики з’єднання під час виконання. Якщо вам потрібно змінити параметри за замовчуванням, ви повинні змінити сам рядок підключення. Наприклад, наступний рядок з’єднання встановлює параметр часу очікування з’єднання зі стандартного (15 секунд для SQL Server) на 30 секунд:

  Data Source=.,5433;User Id=sa;Password=P@ssw0rd;Initial Catalog=AutoLot;Encrypt=False;Connect Timeout=30

Подивимось дані нашого з'єднання.

```cs
static void UseDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    ShowConnectionStatus(connection);
  //...
}
```
```cs
static void ShowConnectionStatus(SqlConnection connection)
{
    Console.WriteLine("\n\tInfo about your connection\n");
    Console.WriteLine($"Database location : {connection.DataSource}");
    Console.WriteLine($"Database : {connection.Database}");
    Console.WriteLine($"Timeout : {connection.ConnectionTimeout}");
    Console.WriteLine($"State : {connection.State}");
    Console.WriteLine();
}
```

```

        Info about your connection

Database location : (localdb)\mssqllocaldb
Database : AutoLot
Timeout : 15
State : Open

```
Хоча більшість із цих властивостей не потребують пояснень, властивість State заслуговує окремої згадки. Ви можете призначити цій властивості будь-яке значення переліку ConnectionState, як показано тут:

```cs
public enum ConnectionState
{
  Broken,
  Closed,
  Connecting,
  Executing,
  Fetching,
  Open
}
```
Однак єдиними дійсними значеннями ConnectionState є ConnectionState.Open, ConnectionState.Connecting і ConnectionState.Closed (решту членів цього переліку зарезервовано для використання в майбутньому). Крім того, завжди безпечніше закрити з’єднання, навіть якщо стан з’єднання наразі ConnectionState.Closed.

## Робота з об’єктами ConnectionStringBuilder

Програмна робота з рядками підключення може бути громіздкою, оскільки вони часто представлені у вигляді рядкових літералів, які важко підтримувати та, у кращому випадку, схильні до помилок. Постачальники даних, сумісні з .NET, підтримують об’єкти конструктора рядка з’єднання, які дозволяють установлювати пари ім’я-значення за допомогою строго типізованих властивостей.

```cs
static void UsingSqlConnectionStringBuilder()
{
    using SqlConnection connection = new();

    var connectionStringBuilder = new SqlConnectionStringBuilder()
    {
        InitialCatalog = "AutoLot",
        DataSource = "(localdb)\\mssqllocaldb",
        IntegratedSecurity = true,
        ConnectTimeout = 30,
        Encrypt = false,
    };

    Console.WriteLine(connectionStringBuilder.ConnectionString);

    connection.ConnectionString = connectionStringBuilder.ConnectionString;
    connection.Open();

    ShowConnectionStatus(connection);
}
UsingSqlConnectionStringBuilder();
```

```
Data Source=(localdb)\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=True;Connect Timeout=30;Encrypt=False

        Info about your connection

Database location : (localdb)\mssqllocaldb
Database : AutoLot
Timeout : 30
State : Open
```

Створюєте екземпляр SqlConnectionStringBuilder, встановлюєте відповідні властивості та отримуєте внутрішній рядок за допомогою властивості ConnectionString. Також зауважте, що ви використовуєте типовий конструктор типу. Якщо ви так захочете, ви також можете створити екземпляр об’єкта конструктора рядка з’єднання вашого постачальника даних, передавши наявний рядок з’єднання як початкову точку (це може бути корисним, коли ви динамічно читаєте ці значення із зовнішнього джерела). Після того, як ви гідратували об’єкт початковими рядковими даними, ви можете змінити конкретні пари ім’я-значення за допомогою відповідних властивостей.

## Робота з об’єктами Command

Тепер, коли ви краще розумієте роль об’єкта підключення, наступний порядок роботи — перевірити, як надсилати SQL-запити до відповідної бази даних. Тип SqlCommand (який походить від DbCommand) є OO представленням SQL-запиту, імені таблиці або збереженої процедури. Ви вказуєте тип команди за допомогою властивості CommandType, яка може приймати будь-яке значення з enum CommandType , як показано тут:

```cs
public enum CommandType
{
  StoredProcedure,
  TableDirect,
  Text // Default value.
}
```
Створимо комануди з власними запитами.

```cs

static void CreatingCommandObjects()
{
 
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    // Create command object via ctor args.
    string sql1 = @"Select i.id, m.Name as Make, i.Color, i.Petname
                   FROM Inventory i
                   INNER JOIN Makes m on m.Id = i.MakeId";
    SqlCommand myCommand1 = new(sql1, connection);

    // Create another command object via properties.
    SqlCommand myCommand2 = new();
    myCommand2.Connection = connection;
    myCommand2.CommandText = "Select m.id, m.Name from Makes m";


}
CreatingCommandObjects();

```
Коли ви створюєте командний об’єкт, ви можете встановити SQL-запит як параметр конструктора або безпосередньо за допомогою властивості CommandText. Крім того, коли ви створюєте командний об’єкт, вам потрібно вказати підключення, яке ви хочете використовувати. Знову ж таки, ви можете зробити це як параметр конструктора або за допомогою властивості Connection.
На даний момент ви фактично не надіслали SQL-запит до бази даних AutoLot, а підготували стан об’єкта команди для майбутнього використання.

Члени типу DbCommand

  CommandTimeout : Отримує або встановлює час очікування під час виконання команди перед припиненням спроби та генеруванням помилки. За замовчуванням 30 секунд.

  Connection : Отримує або встановлює DbConnection, який використовується цим екземпляром DbCommand.

  Parameters : Gets the collection of DbParameter objects used for a parameterized query.

  Cancel() : Cancels the execution of a command.

  ExecuteReader() : Виконує SQL-запит і повертає об’єкт DbDataReader постачальника даних, який надає доступ лише для пересилання та лише для читання для результату запиту.

  ExecuteNonQuery() : Виконує SQL-завдання (наприклад, insert, update, delete або create table).

  ExecuteScalar() : Полегшена версія методу ExecuteReader(), яка була розроблена спеціально для однотонних запитів (наприклад, отримання кількості записів).

  Prepare() : Створює підготовлену (або скомпільовану) версію команди на джерелі даних. Як ви, напевно, знаєте, підготовлений запит виконується трохи швидше та корисний, коли вам потрібно виконати той самий запит кілька разів (зазвичай із різними параметрами кожного разу).


## Робота з Data Readers

Після встановлення активного з’єднання та команди SQL наступним кроком є ​​надсилання запиту до джерела даних. Як ви могли здогадатися, у вас є кілька способів зробити це.
Тип DbDataReader (який реалізує IDataReader) є найпростішим і найшвидшим способом отримання інформації зі сховища даних.

Зчитувачі даних представляють потік даних, призначений лише для читання, що повертається по одному запису за раз ідучи в перед послідоності. Враховуючи це, засоби читання даних корисні лише тоді, коли надсилаються оператори вибору SQL до основного сховища даних. Зчитувачі даних корисні, коли вам потрібно швидко переглянути великі обсяги даних і вам не потрібно підтримувати представлення в пам’яті. Наприклад, якщо ви запитуєте 20 000 записів із таблиці для зберігання в текстовому файлі, зберігати цю інформацію в DataSet буде досить інтенсивно з використанням пам’яті (оскільки DataSet зберігає весь результат запиту в пам’яті одночасно).Кращий підхід полягає у створенні зчитувача даних, який обертається над кожним записом якомога швидше. Однак майте на увазі, що об’єкти читання даних (на відміну від об’єктів адаптера даних, які ви розглянете пізніше) зберігають відкрите з’єднання зі своїм джерелом даних, доки ви явно не закриєте з’єднання.

Ви отримуєте об’єкти читання даних з об’єкта команди за допомогою виклику ExecuteReader(). Зчитувач даних представляє поточний запис, який він прочитав із бази даних. Зчитувач даних має метод індексатора (наприклад, синтаксис [] у C#), який дозволяє отримати доступ до стовпця в поточному записі. Ви можете отримати доступ до стовпця або за назвою, або за допомогою цілого числа від нуля.

```cs
static void ObtainDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    SqlCommand myCommand = new();
    myCommand.Connection = connection;
    myCommand.CommandText = "Select m.id, m.Name from Makes m";

    using SqlDataReader dataReader = myCommand.ExecuteReader();

    dataReader.Read();
    Console.WriteLine($"{dataReader["id"]} {dataReader["Name"]}");

    Console.WriteLine();
    while (dataReader.Read())
    {
        Console.WriteLine($"{dataReader["id"]} {dataReader["Name"]}");
    }
    dataReader.Close();

    Console.WriteLine();

    string sql1 = @"Select i.id, m.Name as Make, i.Color, i.Petname
                   FROM Inventory i
                   INNER JOIN Makes m on m.Id = i.MakeId";
    SqlCommand myCommand1 = new(sql1, connection);

    using SqlDataReader dataReader1 = myCommand1.ExecuteReader();

    while (dataReader1.Read())
    {
        for (int i = 0; i < dataReader1.FieldCount; i++)
        {
            Console.Write($"{dataReader1.GetName(i)} = {dataReader1.GetValue(i)}\t");
        }
        Console.WriteLine();
    }
}
ObtainDataReader();
```
```console
1 VW

2 Ford
3 Saab
4 Yugo
5 BMW
6 Pinto

id = 1  Make = VW       Color = Black   Petname = Zippy
id = 2  Make = Ford     Color = Rust    Petname = Rusty
id = 3  Make = Saab     Color = Black   Petname = Mel
id = 4  Make = Yugo     Color = Yellow  Petname = Clunker
id = 5  Make = BMW      Color = Black   Petname = Bimmer
id = 6  Make = BMW      Color = Green   Petname = Hank
id = 7  Make = BMW      Color = Pink    Petname = Pinky
id = 8  Make = Pinto    Color = Black   Petname = Pete
id = 9  Make = Yugo     Color = Brown   Petname = Brownie
```

В прикладі використовується метод Read(), який зчитує перший або наступний запис. Коли наступного запису нема повертається false. Для кожного вхідного запису, який ви читаєте з бази даних, ви використовуєте індексатор типів, щоб роздрукувати дані запису. Також аби не кодувати жорстко назви стовбців можна використати методи GetName(i) та GetValue(i) та властивість кількості стовбців FieldCount.

### Отримання кількох наборів результатів за допомогою зчитувача даних

Об’єкти зчитування даних можуть отримувати кілька наборів результатів за допомогою одного об’єкта команди. Наприклад, якщо ви хочете отримати всі рядки з таблиці Inventory, а також усі рядки з таблиці Customers, ви можете вказати обидва оператори SQL SELECT за допомогою розділювача крапки з комою.

```cs
static void MultipleResultSetsWithDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    SqlCommand myCommand = new();
    myCommand.Connection = connection;
    myCommand.CommandText = "Select m.id, m.Name from Makes m; Select * from Customers";

    using SqlDataReader dataReader = myCommand.ExecuteReader();
    do
    {
        while (dataReader.Read())
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Console.Write($"{dataReader.GetName(i)} = {dataReader.GetValue(i)}\t");
            }
            Console.WriteLine();
        }
    } while (dataReader.NextResult());
}
MultipleResultSetsWithDataReader();
```
```console
id = 1  Name = VW
id = 2  Name = Ford
id = 3  Name = Saab
id = 4  Name = Yugo
id = 5  Name = BMW
id = 6  Name = Pinto
Id = 1  FirstName = Dave        LastName = Brenner      TimeStamp = System.Byte[]
Id = 2  FirstName = Matt        LastName = Walton       TimeStamp = System.Byte[]
Id = 3  FirstName = Steve       LastName = Hagen        TimeStamp = System.Byte[]
Id = 4  FirstName = Pat LastName = Walton       TimeStamp = System.Byte[]
Id = 5  FirstName = Bad LastName = Customer     TimeStamp = System.Byte[]

```
Завжди автоматично повертається перший набір результатів. Аби отримати наступні результат визмваються метод NextResult. Зчитувач даних може обробляти лише оператори SQL Select; ви не можете використовувати їх для зміни існуючої таблиці бази даних за допомогою запитів Insert, Update або Delete.

# Робота із запитами Create, Update, та Delete

Метод ExecuteReader() витягує об’єкт читача даних, який дозволяє перевіряти результати оператора SQL Select, використовуючи потік інформації лише для перегляду в перед та лише для читання. Однак, якщо ви хочете надіслати оператори SQL, які призводять до модифікації даної таблиці (або будь-якого іншого оператора SQL без запиту, наприклад створення таблиць або надання дозволів), ви викликаєте метод ExecuteNonQuery() свого об’єкта команди. Цей єдиний метод виконує вставки, оновлення та видалення на основі формату тексту команди.

Термін NonQuery — це оператор SQL, який не повертає набір результатів. Таким чином, оператори Select є запитами, тоді як оператори Insert, Update та Delete – ні. Враховуючи це, ExecuteNonQuery() повертає int, який представляє кількість рядків, які зазнали впливу, а не новий набір записів.

Попередні приклади лише відкривали з’єднання та використовували їх для отримання даних. Це лише одна частина роботи з базою даних; фреймворк доступу до даних не принесе великої користі, якщо він також повністю не підтримує функції Create, Read, Update та Delete (CRUD). Дізнаємось як це зробити за допомогою викликів ExecuteNonQuery().

Почніть із створення нового проекту бібліотеки класів C# під назвою AutoLot.DataAccessLayer та додайте пакет Microsoft.Data.SqlClient. Додайте новий файл класу під назвою GlobalUsings.cs до проекту та оновіть файл до таких глобальних операторів using :

AutoLot.DataAccessLaye\GlobalUsings.cs
```cs
global using System.Data;
global using System.Reflection;
global using Microsoft.Data.SqlClient;
```
Перш ніж створювати клас, який виконуватиме операції з даними, ми спочатку створимо клас C#, який представлятиме запис із таблиці Inventory із пов’язаною інформацією Make.

## Класи Car і CarViewModel

Сучасні бібліотеки доступу до даних використовують класи (які зазвичай називають models або entities), які використовуються для представлення та транспортування даних із бази даних. Крім того, класи можна використовувати для представлення даних, які поєднують дві або більше таблиць, щоб зробити дані більш значущими. Entity класи використовуються для роботи з каталогом бази даних (для операторів оновлення), а view model класи використовуються для відображення даних у змістовний спосіб. Ці концепції є основою object relational mappers (ORMs), таких як Entity Framework Core. Наразі просто створимо одну модель (для необробленого рядка Inventory) та одну модель перегляду (об’єднавши рядок Inventory з пов’язаними даними у таблиці Makes).

Додайте нову папку до свого проекту під назвою Models і додайте до неї два нових файли під назвою Car.cs і CarViewModel.cs.

Car.cs
```cs
namespace AutoLot.DataAccessLayer.Models;

public class Car
{
    public int Id { get; set; }
    public int MakeId { get; set; }
    public string Color { get; set; }
    public string PetName { get; set; }
    public byte[] TimeStamp { get; set; }
}
```
CarViewModel.cs
```cs
namespace AutoLot.DataAccessLayer.Models;

public class CarViewModel :Car
{
    public string Make { get; set; }
}

```
Додамо папку до GlobalUsings.cs

GlobalUsings.cs
```cs
global using System.Data;
global using System.Reflection;
global using Microsoft.Data.SqlClient;

global using AutoLot.DataAccessLayer.Models;
```

# Клас InventoryDal

Далі додайте нову папку під назвою DataOperations. У цій новій папці додайте новий клас під назвою InventoryDal.cs і змініть клас на public. Цей клас визначатиме різні члени для взаємодії з таблицею Inventory бази даних AutoLot.

## Додавання конструкторів

```cs
public class InventoryDal
{
    // Variables
    private readonly string _connectionString;

    // Constructors
    public InventoryDal(string connectionString)
    {
        _connectionString = connectionString;
    }
    public InventoryDal() : this("Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot")
    {
    }
}
```
Створюеться конструктор, який приймає рядковий параметр (connectionString) і присвоює значення змінній рівня класу. Далі створюеться конструктор без параметрів, який передає стандартний рядок з’єднання іншому конструктору. Це дає змогу змінювати конфігурацію підключення (зрозуміор шо в реальних проетах не має такого жорстко закодованих рядків).

## Відкриття та закриття підключення

```cs
public class InventoryDal
{
    // Variables
    private readonly string _connectionString;
    private SqlConnection? _sqlConnection = null;

    // Constructors

    //...

    // Connection
    private void OpenConnection()
    {
        _sqlConnection = new SqlConnection { ConnectionString = _connectionString };
        _sqlConnection.Open();
    }

    private void CloseConnection() 
    { 
        if (_sqlConnection?.State != ConnectionState.Closed)
        {
            _sqlConnection?.Close();
        } 
    }    
}

```
Додана змінна рівня класу для утримання з’єднання, яке використовуватиметься кодом доступу до даних. Також додано два методи: один для відкриття з’єднання (OpenConnection()), а інший – для закриття з’єднання (CloseConnection()). У методі CloseConnection() перевіряеться стан з’єднання. 

Для стислості більшість методів у класі InventoryDal не використовуватимуть блоки try/catch для обробки можливих винятків, а також не створюватимуть спеціальні винятки, щоб повідомити про різні проблеми з виконанням (наприклад, неправильний рядок підключення). Якщо ви збираєтеся створити індустріальну бібліотеку доступу до даних, вам обов’язково захочеться використовувати методи обробки структурованих винятків, щоб врахувати будь-які аномалії виконання.

## Додавання IDisposable

```cs

public class InventoryDal : IDisposable
{
    // ...

    // Implementation the disposable pattern
    bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) 
        {
            return;
        }
        if (disposing) 
        { 
            _sqlConnection?.Dispose();
        }
        _disposed = true;  
    }

    public void Dispose() 
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

```
Додано інтерфейс IDisposable до визначення класу. Далі реалізовано disposable патерн, викликавши Dispose в об’єкті SqlConnection.

## Методи виботки
В класі операції з данними почнемо з того що ми вже знаемо про об’єкти Command, DataReaders і загальні колекції, щоб отримати записи з таблиці Inventory. Як ви бачили раніше, об’єкт DataReader постачальника даних дозволяє вибирати записи за допомогою механізму лише для читання та проходу в перед за допомогою методу Read().У цьому прикладі властивість CommandBehavior на DataReader налаштовано на автоматичне закриття з’єднання, коли закривається зчитувач.

```cs
   // Methods of data selection
   public List<CarViewModel> GetAllInventory()
   {
       List<CarViewModel> inventory = new();

       OpenConnection();

       string sql = 
           @"SELECT i.Id, i.Color, i.PetName,m.Name as Make 
             FROM Inventory i 
             INNER JOIN Makes m on m.Id = i.MakeId";
       using SqlCommand command = new(sql, _sqlConnection);

       SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
       while (dataReader.Read())
       {
           inventory.Add(new CarViewModel
           {
               Id = dataReader.GetInt32("Id"),
               Color = dataReader.GetString("Color"),
               Make = dataReader.GetString("Make"),
               PetName = dataReader.GetString("PetName")
           });
       }
       dataReader.Close();

       return inventory;
   }
```
Метод GetAllInventory() повертає List<CarViewModel> для представлення всіх даних у таблиці Inventory.

Протестуємо в Program.cs

```cs
static void TestGetAllInventory()
{
    InventoryDal inventoryDal = new InventoryDal();

    var inventory = inventoryDal.GetAllInventory();

    foreach (var item in inventory)
    {
        Console.WriteLine($"{item.Id}\t{item.Make}\t{item.Color}\t{item.PetName}");
    }
}
TestGetAllInventory();
```
```console
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
```

Наступний метод вибору отримує одну CarViewModel на основі CarId.

```cs
    public CarViewModel VerySimple_GetCar(int id)
    {
        OpenConnection();
       
        CarViewModel car = new();

        //This should use parameters for security reasons
        string sql =
            $@"SELECT i.Id, i.Color, i.PetName,m.Name as Make 
               FROM Inventory i 
               INNER JOIN Makes m on m.Id = i.MakeId
               WHERE i.Id = {id}";

        using SqlCommand command = new(sql, _sqlConnection);

        SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        dataReader.Read();
        car = new CarViewModel
        {
            Id = dataReader.GetInt32("Id"),
            Color = dataReader.GetString("Color"),
            Make = dataReader.GetString("Make"),
            PetName = dataReader.GetString("PetName")
        };
        dataReader.Close();
        return car;
    }
```

Протестуємо.

```cs

static void Test_VerySimple_GetCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    CarViewModel car = inventoryDal.VerySimple_GetCar(7);
    Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
}
Test_VerySimple_GetCar();
```
```console
7       BMW     Pink    Pinky
```
Загалом це погана практика приймати введення користувача в необроблені оператори SQL, як це робиться тут. Далі код буде оновлено для використання параметрів.

## Методи додавання 

Вставка нового рядка в таблицю проводиться аналогічно зчитуванню. Відбувається відкриття з’єднання, виклик ExecuteNonQuery() за допомогою вашого об’єкта команди та закриття з’єднання. 

```cs
    public void VerySimple_InsertCar(string color, int makeId, string petName)
    {
        OpenConnection();

        string sql = $"Insert Into Inventory (MakeId, Color, PetName) Values ('{makeId}', '{color}', '{petName}')";
        
        using SqlCommand command = new(sql, _sqlConnection);

        command.ExecuteNonQuery();
        
        CloseConnection();
    }
```
Метод приймає три параметри, які зіставляються з неідентичними стовпцями таблиці Inventory (Color, MakeId та PetName). Ці аргументи використовуються для форматування типу рядка для вставки нового запису. Використовується свій об’єкт SqlConnection для виконання оператора SQL. 


Протестуємо.
```cs
static void Test_VerySimple_InsertCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.VerySimple_InsertCar("Green", 1, "Ella");
    
    TestGetAllInventory();
}
Test_VerySimple_InsertCar();
```
```console
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
```

Кращий метод використовує Car для створення строго типізованого методу, гарантуючи, що всі властивості передаються в метод у правильному порядку.

```cs
    public void Simple_InsertCar(Car car)
    {
        OpenConnection();

        string sql = $"Insert Into Inventory (MakeId, Color, PetName) Values ('{car.MakeId}', '{car.Color}', '{car.PetName}')";

        using SqlCommand command = new(sql, _sqlConnection);

        command.ExecuteNonQuery();

        CloseConnection();
    }
```
```cs
static void Test_Simple_InsertCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    Car car = new() { Color = "Gray", MakeId = 1, PetName = "Elektric" };
    inventoryDal.Simple_InsertCar(car);

    TestGetAllInventory();
}
Test_Simple_InsertCar();
```
```console
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
11      VW      Grey    Elektric
12      VW      Gray    Elektric
```

## Метод видалення

Видалити існуючий запис так само просто, як вставити новий запис. Але перед видаленням краще зробити перевірку.

```cs
    // Methods for deletion

    public void Simple_DeleteCar(int id)
    {
        OpenConnection();

        string sql = $"Delete from Inventory where Id = '{id}' ";
        using SqlCommand command = new(sql, _sqlConnection);

        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("Exception in DB:" + sqlEx.Message);
        }
        catch (Exception ex)  {
            Console.WriteLine(ex.Message);
        }
        
        CloseConnection();
    }

```
У цьому коді виклик виконання запиту до БД виконується в області оператора try/catch. Це обробляє ситуації які можуть виникнутив в роботі СУБД. Таблиці повязані між собою за допомогою зовнішніх кючів і видаленя з однієї таблиці може не виконатись якшо є посилання на рядок іншої. Стандартні параметри INSERT і UPDATE для зовнішніх ключів за замовчуванням запобігають видаленню пов’язаних записів у зв’язаних таблицях. Коли це трапляється, виникає виняткова ситуація SqlException. 

```cs
static void Test_Simple_DeleteCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Simple_DeleteCar(11);
    TestGetAllInventory();

    Console.WriteLine();

    inventoryDal.Simple_DeleteCar(5);

    Console.WriteLine();

    TestGetAllInventory();
}
Test_Simple_DeleteCar();

```
```
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
12      VW      Gray    Elektric

Exception in DB:The DELETE statement conflicted with the REFERENCE constraint "FK_Orders_Inventory". The conflict occurred in database "AutoLot", table "dbo.Orders", column 'CarId'.
The statement has been terminated.

1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
12      VW      Gray    Elektric
```

Як бачимо рядок не видалено.

### Метод оновлення

Коли справа доходить до оновлення наявного запису в таблиці інвентаризації, перше, що ви повинні вирішити, це те, що ви хочете дозволити абоненту змінити, будь то колір автомобіля, ім’я домашньої тварини, марка чи все одразу. Один із способів надати абоненту повну гнучкість — це визначити метод, який приймає тип рядка для представлення будь-якого виду оператора SQL, але це в кращому випадку ризиковано. 
В ідеалі ви хочете мати набір методів, які дозволяють абоненту оновлювати запис різними способами. Реалізуєм метод для одного поля.

```cs
    // Methods for update
    public void Simple_Update(int id, string newPetName)
    {
        OpenConnection();

        string sql = $"Update Inventory Set PetName = '{newPetName}' Where Id = '{id}'";

        using SqlCommand command = new(sql, _sqlConnection);

        command.ExecuteNonQuery();

        CloseConnection();
    }

```
```cs
static void Test_Simple_Update()
{
    TestGetAllInventory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Simple_Update(12, "Electra");

    TestGetAllInventory();
}
Test_Simple_Update();
```

```console
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
12      VW      Gray    Elektric

1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
12      VW      Gray    Electra
```

## Робота з параметризованими об’єктами Command

Наразі логіка вставки, оновлення та видалення для типу InventoryDal використовує жорстко закодовані рядкові літерали для кожного запиту SQL. У параметризованих запитах параметри SQL є об’єктами, а не простими блоками тексту. Обробка SQL-запитів у більш об’єктно-орієнтований спосіб допомагає зменшити кількість друкарських помилок (враховуючи строго типізовані властивості). Крім того, параметризовані запити зазвичай виконуються набагато швидше, ніж літеральний рядок SQL, оскільки вони аналізуються точно один раз (а не кожного разу, коли рядок SQL призначається властивості CommandText). Параметризовані запити також допомагають захистити від атак SQL-ін’єкцій (добре відома проблема безпеки доступу до даних).
Для підтримки параметризованих запитів об’єкти Command ADO.NET підтримують колекцію окремих об’єктів параметрів. За замовчуванням ця колекція порожня, але ви можете вставити будь-яку кількість об’єктів параметрів, які зіставляються з параметром-заповнювачем у запиті SQL. Якщо ви хочете пов’язати параметр у запиті SQL з членом колекції параметрів об’єкта команди, ви можете додати до текстового параметра SQL символ @ (принаймні, коли використовується Microsoft SQL Server; не всі СУБД підтримують цю нотацію).

### Вказівка ​​параметрів за допомогою типу DbParameter

Перш ніж створити параметризований запит, вам потрібно ознайомитися з типом DbParameter (який є базовим класом для конкретного об’єкта параметра постачальника).Цей клас підтримує низку властивостей, які дозволяють налаштувати назву, розмір і тип даних параметра, а також інші характеристики, включаючи напрямок руху параметра.

Ключові члени типу DbParameter

  DbType : Отримує або встановлює власний тип даних параметра, представлений як тип даних CLR

  Direction : Отримує або встановлює, чи є параметр лише введенням, лише виведенням, двонаправленим чи параметром значення, що повертається

  IsNullable : Отримує або встановлює, чи параметр приймає нульові значення

  ParameterName : Отримує або встановлює назву DbParameter

  Size : Отримує або встановлює максимальний розмір параметра даних у байтах; це корисно лише для текстових даних

  Value : Отримує або встановлює значення параметра

Тепер давайте розглянемо, як заповнити колекцію об’єкта команди сумісними з DBParameter об’єктами, переробивши методи InventoryDal для використання параметрів.

### Додавання параметру до методу вибору

Створемо метод отримання з таблиці з об'єктом параметра

```cs
    public CarViewModel GetCar(int id)
    {
        OpenConnection();

        CarViewModel car = new();

        SqlParameter sqlParameter = new SqlParameter 
        { 
            ParameterName = "@id",
            Value = id,
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Input,
        };

        string sql =
            $@"SELECT i.Id, i.Color, i.PetName,m.Name as Make 
               FROM Inventory i 
               INNER JOIN Makes m on m.Id = i.MakeId
               WHERE i.Id = @id";

        using SqlCommand command = new(sql, _sqlConnection);
        command.Parameters.Add(sqlParameter);

        SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        dataReader.Read();
        car = new CarViewModel
        {
            Id = dataReader.GetInt32("Id"),
            Color = dataReader.GetString("Color"),
            Make = dataReader.GetString("Make"),
            PetName = dataReader.GetString("PetName")
        };
        dataReader.Close();
        return car;
    }
```
```cs
static void Test_GetCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    CarViewModel car = inventoryDal.GetCar(7);
    Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
}
Test_GetCar();
```
```console
7       BMW     Pink    Pinky
```
Значення ParameterName має відповідати імені, яке використовується в запиті SQL , тип має відповідати типу стовпця бази даних, а напрямок залежить від того, чи використовується параметр для надсилання даних у запит (ParameterDirection.Input) або якщо він призначений для повернення даних із запиту (ParameterDirection.Output). Параметри також можуть бути визначені як input/output або як значення, що повертаються (наприклад, із збереженої процедури). В рядку sql використвується назнв параметра @id.

### Додавання параметру до методу видалення

Аналогічно можна оновити метод видалення.

```cs
    public void DeleteCar(int id)
    {
        OpenConnection();

        SqlParameter sqlParameter = new SqlParameter
        {
            ParameterName = "@id",
            Value = id,
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Input
        };

        string sql = $"Delete from Inventory where Id = @id ";
        using SqlCommand command = new(sql, _sqlConnection);
        command.Parameters.Add(sqlParameter);

        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("Exception in DB:" + sqlEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        CloseConnection();
    }
```
```cs
static void Test_DeleteCar()
{
    TestGetAllInventory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Simple_DeleteCar(12);

    TestGetAllInventory();

}
Test_DeleteCar();
```
```console
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
12      VW      Gray    Electra

1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella
```

### Додавання параметрів до методу оновлення

Для цього методу потрібні два параметри: один для id, а інший для нового PetName.

```cs
    // Methods for update with parameters
    public void Update(int id, string newPetName)
    {
        OpenConnection();

        SqlParameter id_parameter = new SqlParameter
        {
            ParameterName = "@id",
            Value = id,
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Input
        };

        SqlParameter petName_parameter = new SqlParameter
        {
            ParameterName = "@petName",
            Value = newPetName,
            SqlDbType = SqlDbType.NVarChar,
            Size = 50,
            Direction = ParameterDirection.Input
        };

        string sql = $"Update Inventory Set PetName = @petName Where Id = @id";

        using SqlCommand command = new(sql, _sqlConnection);

        command.Parameters.Add(id_parameter);
        command.Parameters.Add(petName_parameter);

        command.ExecuteNonQuery();

        CloseConnection();
    }
```

Перший параметр створюється так само, як у двох попередніх прикладах, а другий створює параметр, який відповідає типу бази даних NVarChar (тип поля PetName з таблиці Inventory).

```cs
static void Test_Update()
{
    TestGetAllInventory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Update(10, "Electra");

    TestGetAllInventory();
}
Test_Update();
```
```
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Ella

1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
```

### Додавання параметру до методу вставки

```cs
    // Method for insert with parameters
    public void InsertCar(Car car)
    {
        OpenConnection();

        string sql = "Insert Into Inventory" +
                     "(MakeId, Color, PetName) Values" +
                     "(@MakeId, @Color, @PetName)";


        using SqlCommand command = new(sql, _sqlConnection);

        SqlParameter parameter = new SqlParameter
        {
            ParameterName = "@MakeId",
            Value = car.MakeId,
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Input
        };
        command.Parameters.Add(parameter);

        parameter = new SqlParameter
        {
            ParameterName = "@Color",
            Value = car.Color,
            SqlDbType = SqlDbType.NVarChar,
            Size = 50,
            Direction = ParameterDirection.Input
        };
        command.Parameters.Add(parameter);

        parameter = new SqlParameter
        {
            ParameterName = "@PetName",
            Value = car.PetName,
            SqlDbType = SqlDbType.NVarChar,
            Size = 50,
            Direction = ParameterDirection.Input
        };
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();

        CloseConnection();
    }
```


```cs
static void Test_InsertCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    Car car = new() { Color = "White", MakeId = 2, PetName = "Lapik" };
    inventoryDal.InsertCar(car);

    TestGetAllInventory();
}
Test_InsertCar();
```
```
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Lapik
```
Хоча для створення параметризованого запиту часто потрібно більше коду, кінцевим результатом є зручніший спосіб програмного налаштування операторів SQL, а також досягнення кращої загальної продуктивності. Вони також надзвичайно корисні, коли ви хочете запустити збережену процедуру.

## Виконання збереженої процедури

Збережена процедура — це іменований блок коду SQL, який зберігається в базі даних. Ви можете створити збережені процедури так, щоб вони повертали набір рядків або скалярних типів даних або виконували будь-що інше, що має сенс (наприклад, вставляли, оновлювали або видаляли записи). Ви також можете змусити їх приймати будь-яку кількість необов’язкових параметрів. Кінцевим результатом є одиниця роботи, яка веде себе як типовий метод, за винятком того, що вона розташована в сховищі даних, а не у двійковому бізнес-об’єкті.  

Створюючи базу даних ми створили одну процедуру.

```sql
USE [AutoLot]
GO
CREATE PROCEDURE [dbo].[GetPetName]
@carID int,
@petName nvarchar(50) output
AS
SELECT @petName = PetName from dbo.Inventory where Id = @carID
GO
```
Назва процедур GetPetName яка має два параметри. Перший для визначення рядка @carID другий для результату @petName.
Створимо метод для її використання.

```cs
    // Executing a Stored Procedure
    public string? LookUpPetName(int id)
    {
        OpenConnection();

        using SqlCommand command = new("GetPetName", _sqlConnection);

        command.CommandType = CommandType.StoredProcedure;

        // Input parameter
        SqlParameter parameter = new SqlParameter
        {
            ParameterName = "@carID",
            Value = id,
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Input
        };
        command.Parameters.Add(parameter);

        // Output parameter
        parameter = new SqlParameter
        {
            ParameterName = "@petName",
            SqlDbType = SqlDbType.NVarChar,
            Size = 50,
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();

        CloseConnection();

        return command.Parameters["@petName"].Value.ToString() ;
    }
```
```cs
static void Test_LookUpPetName()
{
    TestGetAllInventory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();

    Console.WriteLine(inventoryDal.LookUpPetName(5));
    Console.WriteLine(inventoryDal.LookUpPetName(155));

}
Test_LookUpPetName();
```
```console
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Lapik

Bimmer


```
Одним з важливих аспектів виклику збереженої процедури є пам’ятати, що командний об’єкт може представляти оператор SQL (за замовчуванням) або ім’я збереженої процедури. Якщо ви хочете повідомити об’єкт команди, що він буде викликати збережену процедуру, ви передаєте ім’я процедури (як аргумент конструктора або за допомогою властивості CommandText) і повинні встановити для властивості CommandType значення CommandType.StoredProcedure. (Якщо ви цього не зробите, ви отримаєте виняток під час виконання, оскільки об’єкт команди за замовчуванням очікує оператора SQL.).
Далі зверніть увагу, що для властивості Direction параметра @petName встановлено значення ParameterDirection.Output. Як і раніше, ви додаєте кожен об’єкт параметра до колекції параметрів об’єкта команди.
Після того, як збережена процедура завершиться викликом ExecuteNonQuery(), ви можете отримати значення вихідного параметра, звернувшись до колекції параметрів об’єкта команди та відповідне приведення.

На даний момент у вас є надзвичайно проста бібліотека доступу до даних, яку можна використовувати для створення клієнта для відображення та редагування ваших даних.

## Використанна класу клієнським додатком

Додайте нову консольну програму AutoLot.Client до рішення і додайте посилання на проект AutoLot.DataAccessLayer. Очистіть створений код у файлі program.cs та додайте наступні using у верхній частині файлу:


```cs
using AutoLot.DataAccessLayer.DataOperations;
using AutoLot.DataAccessLayer.Models;
```
Далі перевіримо всі операції CRUD.

```cs
static void Run()
{
    InventoryDal inventoryDal = new();
    Console.WriteLine("\t\tAll list");
    List<CarViewModel> cars = inventoryDal.GetAllInventory();
    ViewListOfCar(cars);
    Console.WriteLine("\n\n");

    
    int firstId = cars.OrderBy(c => c.Make).Select(r => r.Id).First();
    CarViewModel car = inventoryDal.GetCar(firstId);
    Console.WriteLine("\t\tFinding first car by Make");
    Console.WriteLine("Id\tMake\tColor\tPet Name");
    Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
    Console.WriteLine("\n\n");

    Console.WriteLine("\t\tInsert");
    Car newCar = new() { Color = "Red", MakeId = 5, PetName = "Cher" };
    inventoryDal.InsertCar(newCar);
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");

    Console.WriteLine("\t\tDelete");
    int lastId = inventoryDal.GetAllInventory().Max(c => c.Id);
    Console.WriteLine($"Last ID {lastId}");
    inventoryDal.DeleteCar(lastId);
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");


    Console.WriteLine("\t\tUpdate");
    inventoryDal.Update(13, "Shmapik");
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");


    Console.WriteLine("\t\tDelete with SqlException");
    inventoryDal.DeleteCar(5);
    ViewListOfCar(inventoryDal.GetAllInventory());
}
Run();

static void ViewListOfCar(List<CarViewModel> cars)
{
    Console.WriteLine("Id\tMake\tColor\tPet Name");
    foreach (var item in cars)
    {
        Console.WriteLine($"{item.Id}\t{item.Make}\t{item.Color}\t{item.PetName}");
    }
}
```
Додадкова функція вивлвдить весь список таблиці. 


```console
                All list
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Lapik



                Finding first car by Make
Id      Make    Color   Pet Name
5       BMW     Black   Bimmer



                Insert
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Lapik
17      BMW     Red     Cher



                Delete
Last ID 17
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Lapik



                Update
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Shmapik



                Delete with SqlException
Exception in DB:The DELETE statement conflicted with the REFERENCE constraint "FK_Orders_Inventory". The conflict occurred in database "AutoLot", table "dbo.Orders", column 'CarId'.
The statement has been terminated.
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Shmapik
```

## Транзакції бази даних

Транзакція - це набір операцій з базою даних, які успішно або невдало виконуються як сукупна одиниця. Якщо одна з операцій виявляється невдалою, усі інші операції відкочуються, ніби нічого й не було. Транзакції дуже важливі для забезпечення безпеки, дійсності та узгодженості даних таблиці.
Транзакції важливі, коли операція з базою даних передбачає взаємодію з декількома таблицями або кількома збереженими процедурами (або комбінацією атомів бази даних). Класичний приклад транзакції включає процес переказу грошових коштів між двома банківськими рахунками. Наприклад, якщо ви мали переказати $500 зі свого ощадного рахунку на свій поточний рахунок, наступні кроки мають відбутися під час транзакції:

1. Банк повинен зняти $500 з вашого ощадного рахунку.
2. Банк повинен додати $500 на ваш поточний рахунок.

Було б дуже погано, якби гроші були зняті з ощадного рахунку, але не переведені на поточний (через якусь помилку з боку банку), тому що тоді ви втратите $500. Однак, якщо ці кроки загорнуті в транзакцію бази даних, СУБД гарантує, що всі пов’язані кроки відбуваються як єдине ціле. Якщо будь-яка частина транзакції не вдається, вся операція повертається до початкового стану (rolled back). З іншого боку, якщо всі кроки виконані успішно, транзакція зафіксована (committed).

Ви можете бути знайомі з акронімом ACID, переглядаючи літературу про транзакції. Це відображає чотири ключові властивості первинної транзакції: атомарність (усе або нічого), послідовність(consistent, дані залишаються стабільними протягом усієї транзакції), ізольована (транзакції не заважають іншим операціям) і довговічність (durable транзакції зберігаються та зареєстровано).

Платформа .NET підтримує транзакції різними способами. Тут буде розглянуто об’єкт транзакції вашого постачальника даних ADO.NET (SqlTransaction, у випадку Microsoft.Data.SqlClient). 
На додаток до вбудованої підтримки транзакцій у бібліотеках базових класів .NET, можна використовувати мову SQL вашої системи керування базами даних. Наприклад, ви можете створити збережену процедуру, яка використовує оператори BEGIN TRANSACTION, ROLLBACK і COMMIT.

### Ключові члени об’єкта транзакції ADO.NET

Усі транзакції, які ми будемо використовувати, реалізують інтерфейс IDbTransaction. Він визначає декілька членів наступним чином

```cs
public interface IDbTransaction : IDisposable
{
  IDbConnection Connection { get; }
  IsolationLevel IsolationLevel { get; }
  void Commit();
  void Rollback();
}
```
Зверніть увагу на властивість Connection, яка повертає посилання на об’єкт підключення, який ініціював поточну транзакцію (як ви побачите, ви отримуєте об’єкт транзакції з певного об’єкта підключення). Ви викликаєте метод Commit(), коли кожна з ваших операцій з базою даних завершується успішно. Це призведе до того, що кожна зміна, що очікує на розгляд, збережеться в сховищі даних. І навпаки, ви можете викликати метод Rollback() у разі виняткової ситуації під час виконання, яка інформує СУБД ігнорувати будь-які очікувані зміни, залишаючи вихідні дані недоторканими.
Властивість IsolationLevel об’єкта транзакції дозволяє вказати, наскільки агресивно транзакція повинна бути захищена від дій інших паралельних транзакцій. За замовчуванням транзакції повністю ізольовані, доки не будуть зафіксовані.
Крім членів, визначених інтерфейсом IDbTransaction, тип SqlTransaction визначає додатковий член під назвою Save(), який дозволяє визначати точки збереження. Ця концепція дозволяє відкочувати невдалу транзакцію до названої точки, а не відкочувати всю транзакцію. По суті, коли ви викликаєте Save() за допомогою об’єкта SqlTransaction, ви можете вказати зрозумілий монікер рядка. Коли ви викликаєте Rollback(), ви можете вказати той самий псевдонім як аргумент для ефективного часткового відкоту. Виклик Rollback() без аргументів призводить до відкоту всіх незавершених змін.

### Додавання методу транзакції 

Повернемось до класу InventoryDal та добавимо можливість для вирішення передбачуваних кредитних ризиків. Додамо метод який шукає клієнта додає його до таблиці CreditRisks.


```cs
public void ProcessCreditRisk(bool throwEx, int customerId)
{
    OpenConnection();

    // Look up customer by id

    string firstName, lastName;

    SqlParameter parameterId = new()
    {
        ParameterName = "@CustomerId",
        SqlDbType = SqlDbType.Int,
        Value = customerId,
        Direction = ParameterDirection.Input
    };

    string sql = "Select * from Customers where Id = @CustomerId";
    var cmdSelect = new SqlCommand(sql, _sqlConnection);

    cmdSelect.Parameters.Add(parameterId);

    using var dataReader = cmdSelect.ExecuteReader();

    if (dataReader.HasRows)
    {
        dataReader.Read();
        firstName = dataReader.GetString("FirstName");
        lastName = dataReader.GetString("LastName");
        dataReader.Close();
    }
    else
    {
        CloseConnection();
        return;
    }

    // Insert command
    SqlParameter parameterId1 = new()
    {
        ParameterName = "@CustomerId",
        SqlDbType = SqlDbType.Int,
        Value = customerId,
        Direction = ParameterDirection.Input
    };
    SqlParameter parameterFirstName = new()
    {
        ParameterName ="@FirstName",
        SqlDbType = SqlDbType.NVarChar,
        Size = 50,
        Value = firstName
    };
    SqlParameter parameterLastName = new()
    {
        ParameterName = "@LastName",
        SqlDbType = SqlDbType.NVarChar,
        Size = 50,
        Value = lastName
    };

    sql = "Insert Into CreditRisks (CustomerId, FirstName, LastName) Values(@CustomerId,@FirstName, @LastName)";
    var cmdInsert = new SqlCommand(sql, _sqlConnection);
    cmdInsert.Parameters.Add(parameterId1);    
    cmdInsert.Parameters.Add(parameterFirstName);
    cmdInsert.Parameters.Add(parameterLastName);

    // Update command
    SqlParameter parameterId2 = new()
    {
        ParameterName = "@CustomerId",
        SqlDbType = SqlDbType.Int,
        Value = customerId,
        Direction = ParameterDirection.Input
    };
    sql = "Update Customers set LastName = LastName + ' (CreditRisk) ' where Id = @CustomerId";
    var cmdUpdate = new SqlCommand(sql, _sqlConnection);
    cmdUpdate.Parameters.Add(parameterId2);

    // Use transaction object. We will get this from the connection object
    SqlTransaction? transaction = null;
    try
    {
        transaction = _sqlConnection?.BeginTransaction();
        cmdInsert.Transaction = transaction;
        cmdUpdate.Transaction = transaction;

        cmdInsert.ExecuteNonQuery();
        cmdUpdate.ExecuteNonQuery();

        // Simulate error
        if (throwEx)
        {
            throw new Exception('Sorry!  Database error! Tx failed...');
        }
        transaction?.Commit();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        transaction?.Rollback();                     
    }
    finally
    {
        CloseConnection();
    };
}
```
Тут ви використовуєте вхідний параметр bool, щоб ініціювати ситуацію коли виник виняток.
Це дозволяє імітувати непередбачену обставину, яка призведе до збою транзакції бази даних. Очевидно, ви робите це тут лише для ілюстрації;справжній метод транзакцій бази даних не хотів би дозволити абоненту змусити логіку відмовити через примху! 
Зверніть увагу, що ви використовуєте два об’єкти SqlCommand для представлення кожного кроку в транзакції, яку ви розпочинаєте.
Спочатку отримуєте дані клієнта. Далі підготовлюєте дві команди. Ви можете отримати дійсний об’єкт SqlTransaction з об’єкта підключення за допомогою BeginTransaction(). Далі, і це найважливіше, ви повинні залучити кожен об’єкт команди, призначивши властивість Transaction об’єкту транзакції, який ви щойно отримали. Якщо ви цього не зробите, логіка Insert/Update не буде в контексті транзакції.
Після виклику ExecuteNonQuery() для кожної команди ви створюєте виняток, якщо (і тільки якщо) значення параметра bool є істинним. У цьому випадку всі незавершені операції бази даних відкочуються. Якщо ви не створите виняток, обидва кроки будуть зафіксовані в таблицях бази даних після виклику Commit().

Додамо методи шо повертає дані таблиць в яких виконуються команди.

```cs
    public void GetAllCustomer()
    {

        OpenConnection();
        string sql = "Select * from Customers";
        var cmdSelect = new SqlCommand(sql, _sqlConnection);

        using var dataReader = cmdSelect.ExecuteReader();

        while(dataReader.Read())
        {
            Console.WriteLine(dataReader.GetInt32("Id") + "\t" +
                dataReader.GetString("FirstName") + "\t" +
                dataReader.GetString("LastName")
                );
        }
       
        CloseConnection();
    }
    public void GetAllCreditRisks()
    {

        OpenConnection();
        string sql = "Select * from CreditRisks";
        var cmdSelect = new SqlCommand(sql, _sqlConnection);

        using var dataReader = cmdSelect.ExecuteReader();

        while (dataReader.Read())
        {
            Console.WriteLine(dataReader.GetInt32("Id") + "\t" +
                dataReader.GetString("FirstName") + "\t" +
                dataReader.GetString("LastName") + "\t" +
                dataReader.GetInt32("CustomerId")
                );
        }
        CloseConnection();
    }
```

Протестуєм процес з транзауцією.

```cs
static void Test_ProcessCreditRisk()
{
    InventoryDal inventoryDal = new InventoryDal();

    inventoryDal.GetAllCustomer(); Console.WriteLine();
    inventoryDal.GetAllCreditRisks(); Console.WriteLine();

    Console.WriteLine("Run process with transaction.");

    inventoryDal.ProcessCreditRisk(false, 1);
    inventoryDal.ProcessCreditRisk(true, 3);

    inventoryDal.GetAllCustomer(); Console.WriteLine();
    inventoryDal.GetAllCreditRisks(); Console.WriteLine();

}
Test_ProcessCreditRisk();
```
```
1       Dave    Brenner
2       Matt    Walton
3       Steve   Hagen
4       Pat     Walton
5       Bad     Customer

1       Bad     Customer        5

Run process with transaction.
Sorry!  Database error! Transaction failed...
1       Dave    Brenner (CreditRisk)
2       Matt    Walton
3       Steve   Hagen
4       Pat     Walton
5       Bad     Customer

1       Bad     Customer        5
3       Dave    Brenner 1
```

Як бачимо в першому виклику всі зміни в БД зафіксувались а другий виклик нічого не змінив.

## Виконання масових(bulk) копій за допомогою ADO.NET

У випадках, коли вам потрібно завантажити багато записів у базу даних, методи, показані досі, будуть досить неефективними. SQL Server має функцію під назвою масове копіювання, розроблену спеціально для цього сценарію, і її загорнуто в ADO.NET за допомогою класу SqlBulkCopy.

### Ознайомлення з класом SqlBulkCopy

Клас SqlBulkCopy має метод WriteToServer() і асинхронну версію WriteToServerAsync(), який обробляє список записів і записує дані в базу даних більш ефективно, ніж написання серії операторів вставки та їх виконання з об’єктом Command. Перевантаження WriteToServer приймають DataTable, DataReader або масив DataRows. Щоб дотримуватися теми цього розділу, ви збираєтеся використовувати версію DataReader. Для цього вам потрібно створити спеціальний зчитувач даних.

### Створення спеціального зчитувача даних

Спецфальний зчитувач даних краще зробити узагальненим і містив список моделей, які ви хочете імпортувати. Почніть із створення нової папки в проекті AutoLot.DataAccessLayer під назвою BulkImport. У папці створіть новий клас інтерфейсу під назвою IMyDataReader.cs, який реалізує IDataReader.

```cs

namespace AutoLot.DataAccessLayer.BulkImport;

internal interface IMyDataReader<T> : IDataReader
{
    public List<T> Records { get; set; }
}
```
Як ви вже бачили, зчитувачі даних мають багато методів. Хороша новина полягає в тому, що для SqlBulkCopy ви повинні реалізувати лише кілька з них. Створіть новий клас під назвою MyDataReader.cs, оновіть клас до public і sealed і запровадьте IMyDataReader.Додайте конструктор для записів і встановлення властивості.

```cs
public sealed class MyDataReader<T> : IMyDataReader<T>
{
    public List<T> Records { get; set; }
    public MyDataReader(List<T> records)
    {
        Records = records;
    }
}

```
Попросіть Visual Studio або Visual Studio Code реалізувати всі методи для вас, і ви матимете початкову точку для користувацького зчитувача даних. 

Далі описано методи, які необхідно реалізувати для нашого сценарію.

Ключові методи IDataReader для SqlBulkCopy

    Read : Отримує наступний запис; повертає true, якщо наступний запис є, або повертає false, якщо в кінці списку

    FieldCount : Отримує загальну кількість полів у джерелі даних

    GetValue : Отримує значення поля на основі порядкової позиції

    GetSchemaTable : Отримує інформацію про схему для цільової таблиці

Додайте змінну рівня класу для зберігання поточного індексу List<T>.

```cs
    private int _index = -1;
```
Почнемо з методу Read.

```cs
    public bool Read()
    {
        if (_index + 1 >= Records.Count)
        {
            return false;
        }
        _index++;
        return true;
    }

```
По суті метод змінює индекс поточного єлемента на наступний якшо такий є. Якшо індекс вказує на кінець повертає false інакше true.

Для кожного з методів get і метод FieldCount потрібне глибоке знання конкретної моделі, яку потрібно завантажити. Приклад методу GetValue() з використанням класу Car.

```cs
public object GetValue(int i)
{
  Car currentRecord = Records[_currentIndex] as Car;
  return i switch
  {
    0 => currentRecord.Id,
    1 => currentRecord.MakeId,
    2 => currentRecord.Color,
    3 => currentRecord.PetName,
    4 => currentRecord.TimeStamp,
    _ => string.Empty,
  };
}
```

База даних містить лише чотири таблиці, але це означає, що у вас все ще є чотири варіанти читача даних. Уявіть, якби у вас була справжня виробнича база даних з набагато більшою кількістю таблиць. Можна зробити краще використовуючи рефлексію (reflection) та LINQ to Objects.

Для ціх цілей додамо змінні, щоб зберігати значення PropertyInfo для моделі, а також словник, який використовуватиметься для зберігання позиції поля та імені для таблиці в SQL Server.

```cs
    private readonly PropertyInfo[] _propertyInfos;
    private readonly Dictionary<int,string> _nameDictionary;
```
Оновіть конструктор, щоб отримати властивості узагального типу та ініціалізувати словник.

```cs
    public MyDataReader(List<T> records)
    {
        Records = records;
        _propertyInfos = typeof(T).GetProperties();
        _nameDictionary = new();
    }
```

Додайте змінні які будуть зберігати SQLConnection, схему та назву таблиці в яку додаються записи. 

```cs
    private readonly SqlConnection _connection;
    private readonly string _schema;
    private readonly string _tableName;
```
Оновіть конструктор щоб приймати ці змінні.

```cs
    public MyDataReader(List<T> records, SqlConnection connection, string schema, string tableName)
    {
        Records = records;
        _propertyInfos = typeof(T).GetProperties();
        _nameDictionary = new();
        _connection = connection;
        _schema = schema;
        _tableName = tableName;
    }
```
Створимо допоміжний метод який отримує інформацію SQL Server щодо цільової таблиці.

```cs
    public DataTable GetSchaemaTable()
    {
        string sql = $"SELECT * FROM {_schema}.{_tableName}";
        using var schemaCommand = new SqlCommand(sql, _connection);
        using var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly);
        return reader.GetSchemaTable();
    }
```
Змінемо конструктор заповнивши словник назвами стовпців.

```cs
    public MyDataReader(List<T> records, SqlConnection connection, string schema, string tableName)
    {
        Records = records;
        _propertyInfos = typeof(T).GetProperties();
        _nameDictionary = new();
        _connection = connection;
        _schema = schema;
        _tableName = tableName;

        DataTable schemaTable = GetSchaemaTable();
        for (int i = 0; i < schemaTable?.Rows.Count; i++)
        {
            DataRow dataRow = schemaTable.Rows[i];
            var columnName = dataRow.Field<string>("ColumnName");
            _nameDictonary.Add(i, columnName);
        }
    }
```
Додамо реалізацію необхідних для копіювання методів.

```cs

    public int FieldCount => _propertyInfos.Length;

    public object GetValue(int i) =>
        _propertyInfos
        .First(x => x.Name.Equals(_nameDictionary[i], StringComparison.OrdinalIgnoreCase))
        .GetValue(Records[_index])!;

```
Решта методів які не використовуються для рішеня нашого завдання залишимо нереалізованими.

Перевіримо спеціальний клас зчитувача. 

AutoLot.DataAccessLayer\Program.cs
```cs
// Data for Test_MyDataReader
List<Car> cars = new()
{
    new Car() {Color = "Blue", MakeId = 2, PetName = "Snuppy1"},
    new Car() {Color = "White", MakeId = 1, PetName = "Snuppy2"},
    new Car() {Color = "Red", MakeId = 4, PetName = "Snuppy3"},
    new Car() {Color = "Yellow", MakeId = 1, PetName = "Snuppy4"},
}; 

void Test_MyDataReader()
{
    string connectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";

    var connection = new SqlConnection { ConnectionString = connectionString };

    connection.Open();

    MyDataReader<Car> myDataReader = new(cars, connection, "dbo", "Inventory");

    Console.WriteLine("FildCount:"+myDataReader.FieldCount);

    Console.WriteLine("Id\tMakeId\tColor\tPetName\tTimeStep");

    while (myDataReader.Read())
    {
        Console.WriteLine(
            myDataReader.GetValue(0) + "\t" +
            myDataReader.GetValue(1) + "\t" +
            myDataReader.GetValue(2) + "\t" +
            myDataReader.GetValue(3) + "\t" +
            myDataReader.GetValue(4));
    }
    connection.Close();
}
Test_MyDataReader();
```
```
FildCount:5
Id      MakeId  Color   PetName TimeStep
0       2       Blue    Snuppy1
0       1       White   Snuppy2
0       4       Red     Snuppy3
0       1       Yellow  Snuppy4
```

### Статичний клас для виконання масового копіювання за допомогою SqlBulkCopy

Додайте новий публічний статичний клас під назвою ProcessBulkImport.cs до папки BulkImport. Додайте код для обробки відкриття та закриття з’єднань.

```cs

namespace AutoLot.DataAccessLayer.BulkImport;

public static class ProcessBulkImport
{
    private const string ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    private static SqlConnection? _sqlConnection = null;

    private static void OpenConnection()
    {
        _sqlConnection = new()
        {
            ConnectionString = ConnectionString,
        };
        _sqlConnection.Open();
    }

    private static void CloseConnection() 
    {
        if(_sqlConnection?.State != ConnectionState.Closed)
        {
            _sqlConnection?.Close();
        }
    }
}
```
Додамо метод шо виконує копіювання.

```cs
    public static void ExecuteBulkImport<T>(IEnumerable<T> records, string tableName)
    {
        OpenConnection();
        
        using SqlConnection connection = _sqlConnection!;
        SqlBulkCopy bulkCopy = new(connection)
        {
            DestinationTableName = tableName,
        };

        var dataReader = new MyDataReader<T>(records.ToList(), _sqlConnection, "dbo", "Inventory");

        try
        {
            bulkCopy.WriteToServer(dataReader);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            CloseConnection();
        } 
    }

```
При створені екземпляра класу SqlBulkCopy передеється відкрите підключення та далі встановлюється назва таблиці в яку буде виконуватися копіювання. Потім створіть новий екземпляр користувацького зчитувача даних, що містить список для масового копіювання і викличте WriteToServer().

### Тестування класу масового копіювання

Повернувшись до проекту AutoLot.Client, додайте новий метод у файл Program.cs під назвою DoBulkCopy().


```cs
void DoBulkCopy()
{
    InventoryDal inventoryDal = new();
    Console.WriteLine("\t\tBefore bulk copy");
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");

    var cars = new List<Car>
    {
        new Car() {Color = "Blue", MakeId = 4, PetName = "MyCar1"},
        new Car() {Color = "Red", MakeId = 3, PetName = "MyCar2"},
        new Car() {Color = "White", MakeId = 1, PetName = "MyCar3"},
        new Car() {Color = "Yellow", MakeId = 2, PetName = "MyCar4"}
    };

    ProcessBulkImport.ExecuteBulkImport(cars, "Inventory");

    Console.WriteLine("\t\tAfter bulk copy");
    ViewListOfCar(inventoryDal.GetAllInventory());
}
DoBulkCopy();
```
```
                Before bulk copy
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Shmapik



                After bulk copy
Id      Make    Color   Pet Name
1       VW      Black   Zippy
2       Ford    Rust    Rusty
3       Saab    Black   Mel
4       Yugo    Yellow  Clunker
5       BMW     Black   Bimmer
6       BMW     Green   Hank
7       BMW     Pink    Pinky
8       Pinto   Black   Pete
9       Yugo    Brown   Brownie
10      VW      Green   Electra
13      Ford    White   Shmapik
18      Yugo    Blue    MyCar1
19      Saab    Red     MyCar2
20      VW      White   MyCar3
21      Ford    Yellow  MyCar4
```

Хоча додавання чотирьох нових записів не демонструє переваг роботи, пов’язаної з використанням класу SqlBulkCopy, уявіть спробу завантажити тисячі записів.
Як і все в .NET, це ще один інструмент, який потрібно мати у своєму наборі інструментів, щоб використовувати його, коли це буде найбільш доцільним.
