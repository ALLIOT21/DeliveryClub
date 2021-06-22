1. Установить Microsoft SQL Server 2017 или версии выше.

2. Перейти в исходный код приложения и открыть файл appsettings.json

3. В файле найти запись по ключу "ConnectionStrings" и в данном массиве найти запись по ключу "LocalConnection" и вписать строку подключения к установленной ранее Microsoft SQL Server.

4. Найти в исходном коде в библиотеке DeliveryClub.Data файл DataExtensions.cs и проверить, какая строка подключения указана в коде. Указать "LocalConnection".