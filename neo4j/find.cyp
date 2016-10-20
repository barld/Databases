// Q1
match (a:Airport)-[:Is]->(:AirportSize{name:"Large"}) return a.name, a.Capicity

// Q2
match (a:Airport)-[:LocatedIn]->(c:City) return c.name, sum(a.Capicity)

// Q3
match (a:Airport)
with a order by a.Capicity DESC LIMIT 1 return a.name, a.Capicity;

// Q4
MATCH (A:Airport { name:"Schiphol"})-[:Includes]->(T:Terminal) WHERE T.open = true return A, T

// Q5
match (A:Airport)-[:LocatedIn]->(C:City {name:"London"})
MATCH (A)-[:Includes]->(T:Terminal)
return T, A, C;

// Q6
match (A {name:"Venezia Marco Polo"})-[:Includes]->(T:Terminal { code:"B" })
match (G:Gate {state:"Boarding"})-[:BelongsTo]-(T)
return A, T, G;

// Q7
match (A:Airport)-[:LocatedIn]->(C:City {name:"Rome"})
Match (K:Company)-[:Sells]->(F:Flight)
match (F)-[:Travels]->(A)
WHERE K.name = "Lufthansa" OR K.name = "KLM"
return F.code, F.plane;

// Q8
match (A:Airport)-[:LocatedIn]->(C:City {name:"Rome"})
Match (K:Company)-[:Sells]->(F:Flight)
match (F)-[T:Travels]->(A)
WHERE T.timeH < 15
return K.name, count(F)