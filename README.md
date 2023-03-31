# LeoECSLite Labeled Systems - Контроль системы через порядок ярлыков
Контролируй порядок выполнения систем через проектирования последовательности ярлыков, а не последовательности самих систем

# Содержание
* [Социальные ресурсы](#Социальные-ресурсы)
* [Установка](#Установка)
* [Особенности Labeled Systems](#Особенности-Labeled-Systems)
* [Подключение](#Подключение)
* [Создание последовательности](#Создание-последовательности)
  * [Добавление систем](#Добавление-систем)
* [От автора](#От-автора)
* [Контакты](#Контакты)

# Социальные ресурсы
> #### Discord [Группа по LeoEcsLite](https://discord.gg/5GZVde6)
> #### Telegram [Группа по Ecs](https://t.me/ecschat)

# Установка
> **ВАЖНО!** Зависит от [LeoECS Lite](https://github.com/Leopotam/ecslite) - фреймворк должен быть установлен до этого расширения.

## В виде unity модуля
Поддерживается установка в виде unity-модуля через git-ссылку в PackageManager:
```
https://github.com/skelitheprogrammer/LeoECSLite-Events.git
```
или прямое редактирование `Packages/manifest.json`:
```
"com.skillitronic.leoecsevents": "https://github.com/skelitheprogrammer/LeoECSLite-Events.git",
```
# Особенности Labeled Systems
Последовательность обработки систем соблюдается во всем:
* При инициализации систем
* При итерации
  * Run/PostRun
  * Destroy/PostDestroy 

# Подключение
```c#
  EcsWorld world = new();
  IEcsLabeledSystems systems = new EcsLabeledSystems(world);
```

# Создание последовательности
> Последовательность может быть произвольной, все зависит от архитектуры вашего приложения
```c#
  systems
    .AddMarker("Start")
    .AddMarker("Events")
    .AddMarker("Event_Recievers")
    .AddMarker("MainLoop")
    .AddMarker("Cleanup")
    
```
## добавление систем
Порадок добавления не важен, важно правильно указать ярлыки
```c#
  ...
    .AddAt(system: system, "Event_Recievers")
    .AddAt(system: someSystem, "Events")
    .Add(system: otherSystem) // добавит к самому последнему маркеру
```

## От автора
- При создании данной реализации меня сподвигла идея Leopotam с его модульными системами.
- В будущем я собираюсь улучшить способ создания маркеров, уйдя от прописывания string параметров. На данный момент могу только посоветовать создать enum с требуемыми маркерами, чтобы минимально защитить себя от ошибок (но система вас все равно предупредит о не верных аргументах)

# Контакты
E-mail: dosynkirill@gmail.com </br>
Discord: @skilli на [сервере дискорд](#Социальные-ресурсы)
