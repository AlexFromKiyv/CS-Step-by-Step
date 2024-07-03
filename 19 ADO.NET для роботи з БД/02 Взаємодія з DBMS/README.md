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

    ShowConnectionStatus(connection);

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


