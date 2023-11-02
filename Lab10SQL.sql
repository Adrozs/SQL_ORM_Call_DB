--Hämta alla produkter med deras namn, pris och kategori namn. Sortera på kategori namn och sen produkt namn
SELECT ProductName, UnitPrice, CategoryName FROM Products
JOIN Categories ON  Products.CategoryID = Categories.CategoryID
ORDER BY CategoryName, ProductName

--Hämta alla kunder och antal ordrar de gjort. Sortera fallande på antal ordrar.
SELECT CompanyName, COUNT(OrderID) AS Orders FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY CompanyName
ORDER BY Orders DESC

--Hämta alla anställda tillsammans med territorie de har hand om (EmployeeTerritories och Territories tabellerna)
SELECT FirstName + ' ' + LastName AS Employees, TerritoryDescription AS Territory FROM Employees
JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID

--[Extra] Istället för att skriva antal ordrar, skriv ut summan för deras totala ordervärde
SELECT c.CompanyName AS Company, SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS [Total Order Value] FROM Customers c
JOIN Orders o ON c.CustomerID = o.CustomerID
JOIN [Order Details] od ON o.OrderID = od.OrderID
GROUP BY c.CompanyName
ORDER BY [Total Order Value] DESC;
