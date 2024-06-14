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

Кожен із сумісних із .NET постачальників даних містить тип класу, який походить від System.Data.Common.DbProviderFactory. Цей базовий клас визначає кілька методів отримують провайдер-спеціфічні об'єкти даних. Ось члени DbProviderFactory:

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

