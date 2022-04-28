# Interval
In mathematics, a interval is a set of real numbers that contains all real numbers lying between any two numbers of the set. We brought interval into ASP.Net so that you simply can manage overlaps, unions, intersections and subtractions of intervals. Unlike mathematics, an interval can contains a range of any type which its values are comparable.
## Installation
Simply add ```Ako.Interval``` nuget package using ```dotnet CLI```
```
dotnet add package Ako.Interval --version 1.0.0
```
## Documentation
#### 1- Constructing Interval
An standard interval contains two edges (endpoints). Each edge can be included withing the range or excluded. Also start edge can be minus infinity and end edge can be infinity. Every value (including edges) withing the range of interval must be comparable. Therefore you can simply construct an interval like below:
```lang-cs
using Ako.IntervalCore;
using System;

namespace Program
{
    public class IntervalApp
    {
        public void Main(){
            var interval1 = new Interval<int>(1, 10); 
            // An interval of type int from 1 to 10 
            // Including both start and end edge. 
            // Standard notation: [1, 10]

            var interval2 = new Interval<int>(1, 5, false);
            // Start edge is excluded
            // Standard notation: (1, 5]

            
            var interval3 = new Interval<int>(1, 5, false, false);
            // Both edges are excluded
            // Standard notation: (1, 5)

            
            var interval3 = new Interval<int>(null, 5);
            // Start edge is infinity
            // Standard notation: (-∞, 5]

            var interval4 = new Interval<int>(0, null, false);
            // End edge is infinity
            // Standard notation: (0, ∞)

            var interval5 = new Interval<int>(null, null);
            // The infinite interval of integers
            // Standard notation: (-∞, ∞)

            /* Intervals can also take different types; Unless the given type is not comparable. */
            var interval6 = new Interval<DateTime>(DateTime.Today, DateTime.Now);
            // Standard notation: [2022/28/4-00:00:00, 2022/28/4-16:35:00]
        }
    }
}
```
Interval can also be constructed using standard notation string. 
An standard interval notation is an string having two values separated by a comma. It should be starting with [ <open-bracket> as if start edge is included or ( <open-parenthesis> as if start edge is excluded. It also should be ending with ] <closed-bracket> or ) <closed-parenthesis>. As of the matter of infinity, ∞ symbol or the word infinity can be used as value.
Note that the values of interval is casted into the given type using ```System.Convert```
```lang-cs
var interval1 = Interval.Parse<int>("[1, 10]");

var interval1 = Interval.Parse<int>("(1, 10]");

var interval1 = Interval.Parse<int>("(0, ∞)");
var interval1 = Interval.Parse<int>("(0, infinity)");
var interval1 = Interval.Parse<int>("(infinity, infinity)");
var interval1 = Interval.Parse<int>("(-∞, infinity)");
```
