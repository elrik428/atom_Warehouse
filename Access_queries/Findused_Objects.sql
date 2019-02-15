SELECT DISTINCT 
MSysObjects.Name, MSysQueries.Name1, MSysQueries.Name2, MSysQueries.Expression
FROM 
MSysQueries 
INNER JOIN 
MSysObjects ON MSysQueries.ObjectId = MSysObjects.Id
WHERE 
(   (((MSysQueries.Name1) Like "*" & [String to search for] & "*")) 
 OR (((MSysQueries.Name2) Like "*" & [String to search for] & "*"))
 OR (((MSysQueries.Expression) Like "*" & [String to search for] & "*"))  )