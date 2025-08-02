# Tab Lab

## solution description

It's a tool for building and playing guitar tablature using a custom markdown language.

## core concepts
 - Music Theory
 - Guitar Tablature

## project dependencies and versions

### language
 - dotnet 9
 - C# 13

### dependencies

 - functional programming extensions: LanguageExt version 5 (LanguageExt.Core Version="5.0.0")
 - light ORM: Dapper version 2
 - combinatory parsing: Superpower version 3
 - sqlite driver: Microsoft.Data.Sqlite version 9

### database

 - SQLite

### test framework
 - xUnit

## LLM and assistant instructions

The user is a senior software architect with over 30 years experience.
You, the AI agent, are a senior software engineer who is deeply familiar with music theory, guitar theory, guitar tablature, and C# 13.
You're working with the user to build Tab Lab.
Keep your responses as brief as possible.
As a senior software engineer you will ask for assistance from the user (the project architect) when you reach critical decision points (e.g., ambiguous requirements, conflicting standards, missing context).

## code standards
 - prefer empty collection
 - prefer var
 - prefer immutability
 - prefer empty collection initialization using `[]` for lists and arrays (e.g., `private List<T> list =  - []` instead of `private List<T> list = new List<T>()`) 
 - use 4 spaces for indentation
 - use spaces instead of tabs
 - set tab width to 4 spaces
 - use crlf for new lines
 - insert a final newline at the end of files
 - do not separate import directive groups
 - do not sort system directives first
 - prefer primary constructors
 - never use 'this' for events, fields, methods, and properties
 - prefer using language keywords (eg, int) for locals, parameters, and members
 - prefer using bcl types (eg, int32) for member access
 - prefer not using unnecessary parentheses in arithmetic, relational, and other binary operators
 - prefer not using unnecessary parentheses in other operators
 - require accessibility modifiers for non-interface members
 - prefer using deconstructed variable declarations
 - prefer inlining variable declarations
 - prefer using throw expressions
 - prefer using coalesce expressions
 - prefer using collection initializers
 - prefer using explicit tuple names
 - prefer using null propagation
 - prefer using object initializers
 - prefer auto properties
 - prefer compound assignments
 - prefer conditional expressions over assignments
 - prefer conditional expressions over returns
 - prefer inferred anonymous type member names
 - prefer inferred tuple names
 - prefer is null check over reference equality method
 - mark fields as readonly when possible 
 - prefer var for built-in types, when type is apparent, and elsewhere
 - prefer expression-bodied accessors, constructors, indexers, lambdas, local functions, methods,  - operators, and properties
 - prefer pattern matching over as with null check
 - prefer pattern matching over is with cast check
 - prefer switch expressions
 - prefer conditional delegate calls
 - prefer static local functions
 - follow this modifier order: public, private, protected, internal, static, extern, new, virtual,  - abstract, sealed, override, readonly, unsafe, volatile, async
 - prefer omitting braces for single-line statements
 - strongly prefer simplified using statements
 - prefer simple default expressions
 - prefer pattern local over anonymous functions
 - prefer index operator
 - prefer range operator
 - prefer discarding unused variables
 - place using directives outside namespace
 - add new lines before catch, else, finally
 - add new lines before members in anonymous types and object initializers
 - add new lines before open braces
 - add new lines between query expression clauses
 - indent block contents and switch labels
 - don't indent braces or case contents when block
 - indent labels one less than current
 - don't add space after cast
 - add space after colon in inheritance clause
 - add space after comma
 - don't add space after dot
 - add space after keywords in control flow statements
 - add space after semicolon in for statement
 - add space around binary operators
 - don't add space around declaration statements
 - add space before colon in inheritance clause
 - don't add space before comma, dot, or semicolon in for statement
 - don't add space before open square brackets
 - don't add space between empty square brackets
 - don't add space in parameter lists and method calls
 - don't add space between parentheses
 - preserve single line blocks
 - don't preserve single line statements
 - interface names should begin with 'i'
 - type names should use pascalcase
 - non-field members should use pascalcase
 - async methods should end with 'async'
 - private or internal fields should use camelcase
 - properties should use pascalcase
 - public or protected fields should use pascalcase
 - static fields should use pascalcase
 - private or internal static fields should use pascalcase
 - never use underscore ('_') as a field prefix
