# Contributing

Please, create an Issue before your PR. Describe in maximum detail what you want to do.

# C# Code styling

## General information

Names should be clear, relatively short (1-3 words). The use of single-letter variables is prohibited, except for special cases (short lambda, for, etc.).

Use `PascalCase` for

* Names of classes and structures
* Method Names
* Names of public fields, public and private properties

Use `camelCase` for

* Variable names
* Parameter names

use `_camelCase` for

* Private Field Names

### Interfaces

The interface name must have the prefix "I"


### Abbreviations

Whenever possible, avoid the use of abbreviations. When using, apply the policy, perceiving the abbreviation as one whole word.

### `Task`, `Task <>`

For Task, Task <> methods name must have `Async` postfix.


## Language optimization

* Prefer to use Implicit typeing insted Explicit
* Use lazy conditions
* Use object and collection initializers
* Prefer to use System.Linq extensions instead of loops, etc.
* Prefer to use autoproperties
* Use language special type names instead of build-in classes
* Prefer to use new language features
* Prefer to use `.?`, `??`, and et. operators

## Other

* Not recomended to use recursions. If possible it is better to use loops

## Restrictions
* Do not use `goto`
* Do not use LINQ syntax

# ES/TS Code Styling

* Use 4 spaces instead of tab
* Use double qout(`"`) for literals
* We use semicolon;
* Prefer to use `const` instead of `let`
* Do no use `var`
* Prefer to use triple equal sign
* Use JSDoc notation for documentation
* Prefer to use Array methods instead of loops and etc.
* Prefer to use newest language features