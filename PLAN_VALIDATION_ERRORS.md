# План: понятные сообщения об ошибках валидации (Cygnus.Studio)

Проект: `/srv/GitHub/Cygnus.Studio`
Режим: English-only, без локализации (файлы `.po` не трогаем).

## Цель

Заменить общее сообщение FluentValidation `'Name' is not in the correct format.`
на конкретное, объясняющее требуемый формат, и сделать понятными сообщения
кросс-поля Field (Type / Entity / Enum).

## Диагностика

- Формат проверяется через `Matches(RegexHelper.CSharpName)`, где
  `CSharpName = "^[A-Z][a-zA-Z0-9]{1,}$"` — начинается с заглавной буквы,
  дальше только буквы/цифры, минимум 2 символа.
- Сообщение по умолчанию из FluentValidation: `'{PropertyName}' is not in the correct format.`
- У `Field` есть 6 условных правил для `Type`/`EntityId`/`EnumId`
  (`NotEmpty`/`Empty` с `.When`) с общими сообщениями «must not be empty» / «must be empty».
- На фронтенде `model-error.pipe.ts` показывает только первую ошибку поля (`errors[0]`).

## Изменения

### 1. Общее правило C#-имени (единый источник правды)

Файл: `Cynosura.Studio.Core/Requests/RegexHelper.cs` — добавить extension
в том же namespace `Cynosura.Studio.Core.Requests` (автоподхватится во всех
`...Requests.*` валидаторах):

```csharp
using FluentValidation;

public static class RegexHelper
{
    public const string CSharpName = "^[A-Z][a-zA-Z0-9]{1,}$";

    public static IRuleBuilderOptions<T, string?> MustBeCSharpName<T>(this IRuleBuilder<T, string?> ruleBuilder)
        => ruleBuilder
            .Matches(CSharpName)
            .WithMessage("'{PropertyName}' must start with an uppercase letter and contain only letters and digits (minimum 2 characters).");
}
```

### 2. Заменить `.Matches(RegexHelper.CSharpName)` → `.MustBeCSharpName()`

- `Requests/Entities/CreateEntityValidator.cs` — `Name`, `PluralName`
- `Requests/Entities/UpdateEntityValidator.cs` — `Name`, `PluralName`
- `Requests/Fields/CreateFieldValidator.cs` — `Name`
- `Requests/Fields/UpdateFieldValidator.cs` — `Name`
- `Requests/Enums/CreateEnumValidator.cs` — `Name`
- `Requests/Enums/UpdateEnumValidator.cs` — `Name`
- `Requests/EnumValues/CreateEnumValueValidator.cs` — `Name`
- `Requests/EnumValues/UpdateEnumValueValidator.cs` — `Name`

### 3. Field: одно понятное правило вместо 6 условных

В `CreateFieldValidator` и `UpdateFieldValidator` удалить 6 правил
`Type`/`EntityId`/`EnumId` (NotEmpty/Empty с `.When`) и добавить одно:

```csharp
RuleFor(x => x)
    .Must(f => new[] { f.Type != null, f.EntityId != null, f.EnumId != null }.Count(v => v) == 1)
    .WithName(x => localizer["Field"])
    .WithMessage("A field must have exactly one of Type, Entity, or Enum specified.");
```

Семантика полностью совпадает с текущей (ровно одно из трёх заполнено).

### 4. Фронтенд: показывать все ошибки поля

Файл: `Cynosura.Studio.Web.Ng/src/app/core/pipes/model-error.pipe.ts`

```ts
transform(value, args: string[]): any {
  if (value && value.errors && value.errors.length > 0) {
    return value.errors.map(e => e.errorMessage).join(' ');
  }
  return null;
}
```

### 5. Тесты

В `Tests/Cynosura.Studio.Core.UnitTests/Requests/` добавить юнит-тесты
валидаторов (без новых пакетов — через `validator.Validate(instance)` и
проверку `.Errors`):

- Entity/Field/Enum/EnumValue: некорректные имена (`foo`, `Foo-bar`, `F`,
  `Foo_bar`, `Foo`) дают понятное сообщение; корректные (`Foo`, `Foo1`) проходят.
- Field: «ровно одно из Type/Entity/Enum» — ветки: ничего не заполнено → ошибка;
  заполнено два → ошибка; заполнено одно → ок.

## Порядок работ

1. `RegexHelper.cs` (extension).
2. Обновить 8 валидаторов (п.2).
3. Переписать Field-валидаторы (п.3).
4. `model-error.pipe.ts` (п.4).
5. Написать тесты (п.5).
6. `dotnet build` + `dotnet test` (проекты: `Cynosura.Studio.Core`,
   `Cynosura.Studio.Core.UnitTests`).

## Файлы (сводка)

| Где | Что |
| --- | --- |
| `Cynosura.Studio.Core/Requests/RegexHelper.cs` | extension `MustBeCSharpName` |
| `Cynosura.Studio.Core/Requests/Entities/CreateEntityValidator.cs` | Name, PluralName |
| `Cynosura.Studio.Core/Requests/Entities/UpdateEntityValidator.cs` | Name, PluralName |
| `Cynosura.Studio.Core/Requests/Fields/CreateFieldValidator.cs` | Name + правило «ровно одно» |
| `Cynosura.Studio.Core/Requests/Fields/UpdateFieldValidator.cs` | Name + правило «ровно одно» |
| `Cynosura.Studio.Core/Requests/Enums/CreateEnumValidator.cs` | Name |
| `Cynosura.Studio.Core/Requests/Enums/UpdateEnumValidator.cs` | Name |
| `Cynosura.Studio.Core/Requests/EnumValues/CreateEnumValueValidator.cs` | Name |
| `Cynosura.Studio.Core/Requests/EnumValues/UpdateEnumValueValidator.cs` | Name |
| `Cynosura.Studio.Web.Ng/src/app/core/pipes/model-error.pipe.ts` | показывать все ошибки |
| `Tests/Cynosura.Studio.Core.UnitTests/Requests/...` | новые тесты |

## Вне scope

`ClassNameValidator` / `JsonValidator` (Worker-сущности) — зашитые общие
сообщения; можно отдельным шагом.

## Решения (подтверждены пользователем)

- Охват CSharpName: Entity, Field, Enum, EnumValue.
- Кросс-поле Field: одно понятное правило (рекомендованный вариант).
- Фронтенд: включить правку `model-error.pipe`.
