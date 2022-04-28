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

namespace Program
{
    public class IntervalApp
    {
        public void Main(){
            var interval = new Interval<int>(1, 10); // An interval of type int from 1 to 10 including both start and end edge. Standard notation: [1, 10]

        }
    }
}
```

