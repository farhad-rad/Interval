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
An standard interval notation is an string having two values separated by a comma. It should be starting with ```[``` (open-bracket) as if start edge is included or ```(``` (open-parenthesis) as if start edge is excluded. It also should be ending with ```]``` (closed-bracket) or ```)``` (closed-parenthesis). As of the matter of infinity, ```∞``` symbol or the word ```infinity``` can be used as value.
Note that the values of interval is casted into the given type using ```System.Convert```
```lang-cs
var interval1 = Interval.Parse<int>("[1, 10]");

var interval2 = Interval.Parse<int>("(1, 10]");

var interval3 = Interval.Parse<int>("(0, ∞)");
var interval4 = Interval.Parse<int>("(0, infinity)");
var interval5 = Interval.Parse<int>("(infinity, infinity)");
var interval6 = Interval.Parse<int>("(-∞, infinity)");
```
Intervals can also be constructed using extension methods of namespace ```Ako.IntervalCore.Extensions```
```lang-cs
var interval1 = 1.IntervalUntil(5);
// Standard notation: [1, 5]

var interval2 = 10.IntervalFrom(0, false);
// Standard notation: (0, 10]
```
#### 2- Containing of a value
You can simply check if a value is inside an intervals range or not.
```lang-cs
var interval1 = new Interval<int>(0, 100, false); // (0, 100]
interval1.Contains(10); // True
interval1.Contains(100); // True; End edge is included.
interval1.Contains(110); // False
interval1.Contains(0); // False; Start edge is excluded.

var interval2 = new Interval<int>(null, null); // (-∞, ∞)
interval2.Contains(10); // True
interval2.Contains(100); // True
interval2.Contains(1100); // True
```
#### 3- Overlaps check
```lang-cs
var interval1 = new Interval<int>(0, 10, false); // (0, 10]

var interval2 = new Interval<int>(10, 20); // [10, 20]

var interval3 = new Interval<int>(10, 20, false, false); // (10, 20)

var interval4 = new Interval<int>(null, null); // (-∞, ∞)

Interval.HasOverlap(interval1, interval2); // True
Interval.HasOverlap(interval1, interval3); // False
Interval.HasOverlap(interval2, interval3); // True
Interval.HasOverlap(interval2, interval4); // True
```
#### 4- Union
![Interval Union](https://upload.wikimedia.org/wikipedia/commons/thumb/3/30/Venn0111.svg/300px-Venn0111.svg.png)
```lang-cs
var interval1 = new Interval<int>(0, 10, false, false); // (0, 10)

var interval2 = new Interval<int>(5, 15); // [5, 15]

var interval3 = new Interval<int>(null, null); // (-∞, ∞)

Interval.Union(interval1, interval2); // (0, 15]
Interval.Union(interval1, interval3); // (-∞, ∞)
```
#### 5- Intersection
![Interval Intersection](https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Venn0001.svg/330px-Venn0001.svg.png)
```lang-cs
var interval1 = new Interval<int>(0, 10, false, false); // (0, 10)

var interval2 = new Interval<int>(5, 15); // [5, 15]

var interval3 = new Interval<int>(null, null); // (-∞, ∞)

Interval.Intersection(interval1, interval2); // [5, 10)
Interval.Intersection(interval1, interval3); // (0, 10)
```
#### 6- Subtraction
The picture below indicates ```intervalB - intervalA```
![Interval Subtraction](https://upload.wikimedia.org/wikipedia/commons/thumb/2/23/Relative_compliment.svg/345px-Relative_compliment.svg.png)
```lang-cs
var interval1 = new Interval<int>(0, 10, false, false); // (0, 10)

var interval2 = new Interval<int>(5, 15); // [5, 15]

Interval.Subtraction(interval1, interval2); // (0, 5)
Interval.Subtraction(interval2, interval1); // [10, 15]
```
## Author
[Farhad Rad](https://github.com/farhad-rad/); Ako Team ©
## License
[MIT](https://licenses.nuget.org/MIT)
## Repository
[git](https://github.com/farhad-rad/Interval)
