# Contributing

## Bug reports
A bug is a _demonstrable problem_ that is caused by the code in the repository.
Good bug reports are extremely helpful - thank you!

Guidelines for bug reports:

1. **Use the GitHub issue search** &mdash; check if the issue has already been
   reported.

2. **Check if the issue has been fixed** &mdash; try to reproduce it using the
   latest `master` or `development` branch in the repository.

Example of a bug issue:

> # Exception or Error
> <ErrorType, ErrorMessage>
> <StackTrace>
> # Description
> What have you tried? Any specific information like platform, custom template and other
> # Steps to Reproduce Bug
> 1. ...
> 

## Feature requests
Please, create an Issue before your [Pull Request](https://help.github.com/articles/using-pull-requests/). 
Provide as much detail and context as possible.

Example of a feature request issue:

> # The problem description
> Describe problem and context for new feature. We need to understand why this is necessary
> # Implementation
> If you have plan for implementation describe it here
> 

**IMPORTANT**: By submitting a patch, you agree to allow the project owner to license your work under the same license as that used by the project.

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

### `Task`, `Task<>`

For Task, Task<> methods name must have `Async` postfix.


## Language optimization

* Prefer to use Implicit typing instead Explicit
* Use lazy conditions
* Use object and collection initializers
* Prefer to use System.Linq extensions instead of loops, etc.
* Prefer to use autoproperties
* Use language special type names instead of build-in classes
* Prefer to use new language features
* Prefer to use `.?`, `??`, etc.

## Other

* Not recomended to use recursions. If possible it is better to use loops

## Restrictions
* Do not use `goto`
* Do not use LINQ syntax

# ES/TS Code Styling

* Use 4 spaces instead of tab
* Use single quotes (`'`) for literals
* We use semicolon;
* Prefer to use `const` instead of `let`
* Do not use `var`
* Prefer to use triple equal sign
* Use JSDoc notation for documentation
* Prefer to use Array methods instead of loops and etc.
* Prefer to use newest language features
