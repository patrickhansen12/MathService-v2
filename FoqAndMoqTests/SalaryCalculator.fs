module SalaryCalculator

open System

let rec fact x =
   if x < 1 then 1
   else x * fact (x - 1)

Console.WriteLine(fact 6)