# Практика "Игра" | Дизайн-документ
## Основное
- Платформа: `ПК`
- Технологии: `C#` `MonoGame`
- Язык: `Русский`
- Жанры: `2D` `Top-Down` `BulletHell` `Fantasy` `Roguelike`
- Настроение: `Мрачное`
- Сеттинг: `Средневековье`
- Длительность игры: `5 минут`
- Главная игровая механика: `Необратимая смерть (PermaDeath)`

## Сюжет
Главный герой - Воин, которому необходимо сбежать из какой-либо локации, попутно расправляясь с врагами.

## Игровой мир
В игре присутствует несколько различных локаций, каждая из которых имеент своих уникальных противников.

Возможные варианты локаций и противников:
- Темный лес. Гоблины
- Заброшенная деревня. Бандиты и Волки
- Река с бурлящими порогами. Крокодилы и Водные элементали

## Геймплей
Локация выбирается случайно.

Главный герой появляется в комнате, со случайным оружием, с помощью которого будет сражаться первое время.

Со всех сторон будут появляться враги, которые будут идти в сторону Воина. С некоторым шансом из поверженных врагов можно получить ресурсы и новое оружие. Новое оружие имеет случайные характеристики.

После прохождения комнаты будет открываться проход дальше.

### Комнаты
Виды
- `Обычные`
  - Необходимо победить N количество врагов
- `Специальные`
  - `Магазин` в котором можно приобрести снаряжение за ресурсы собранные с врагов
  - `Кузница` в которой можно перековать оружие для того чтобы повысить его характеристики
  - `Святилище` в котором можно восполнить здоровье.
- `Арена`
  - Комната, в которой нужно сражаться с `Боссом`

Всего необходимо преодолеть 5 комнат:
- 1-3 комнаты - `Обычные`
- 4 комната - `Специальная`
- 5 комната - `Арена` Босс


### Оружие
Виды оружия: `Меч` `Копьё` `Топор`

Характеристики оружия:
- `Урон`
- `Скорость атаки`
- `Дальность атаки`
- `Скорость передвижения`
- `Дальность отталкивания`

Скриншоты, демонстрирующие понимание поведение пользователя в игре:

[Imgur](https://imgur.com/a/JBPxtBr)
