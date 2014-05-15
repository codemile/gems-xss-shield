# Gems XSS Shield

This C# library is focused on providing .NET developers with a tool to guard against Cross Site Scripting attacks.

The library was designed to prevent XSS attacks using the following approaches.

- A white list approach to filtering HTML tags and attributes.
- URL validation and rewriting.
- reporting of possible XSS attempts or suspicious content.
- detection of HTML filter evasion techniques.

## How It Works

XSS Shield uses the [HTML Agility Pack](http://htmlagilitypack.codeplex.com/) to validate HTML, and then it walks the HTML node tree filtering it to produce a new clean version of the HTML.

XSS Shield employs a technique where single-purpose objects each perform a specific sanitizing job. By selecting what objects XSS Shield uses you can configure the type of sanitizing and filtering to be performed on the HTML.

## Examples

Examples of how to use XSS Shield have been implemented as unit tests. Each test demonstrating a different feature or use case. Please see the Examples project for more information.