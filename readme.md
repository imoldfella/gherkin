A proof of concept where Gherkin files are generated from C# attributes.

The advantage of this approach is combining the strong typing of C# with the documentation of Gherkin. 
It would probably be better to use a more markdown-ized version of gherkin.

Example from specflow

Feature: Calculator

Simple calculator for adding two numbers

@mytag
Scenario: Add two numbers
Given I have entered 50 into the calculator
And I have entered 70 into the calculator
When I press add
Then the result should be 120 on the screen

@mytag
Scenario Outline: Add two numbers
Given I have entered <First> in the calculator
And I have entered <Second> into the calculator
When I press add
Then the result should be <Result> on the screen

Examples:
  | First | Second | Result |
  | 50    | 70     | 120    |
  | 30    | 40     | 70     |
  | 60    | 30     | 90     |

  

