// Q1
match (a:Airport {Size:"Large"}) return a.name, a.Capicity;

// Q2
match (a:Airtport) return a.City, sum(a.Capicity) as totalCapicity;

// Q3
MATCH (a:Airport)
with a order by a.Capicity DESC LIMIT 1 return a.name, a.Capicity

// Q4
MATCH (a:Airport {name:'Schiphol'})-[:Includes]->(t:Terminal {open:true}) return t

// Q5
match(a:Airport {City:'London'})-[:Includes]->(t:Terminal) return t

// Q6
match(a:Airport {name:'Venezia Marco Polo'})-[:Includes]->(t:Terminal {code:'B'})
match(g:Gate {state:"Boarding"})-[:BelongsTo]->(t) return g.state, g.number

// Q7
Match (K:Company)-[:Sells]->(F:Flight)-[:Travels]->(a:Airport {City:'Rome'})
WHERE K.name = "Lufthansa" OR K.name = "KLM"
return F.code, F.plane;

// Q8
Match (K:Company)-[:Sells]->(F:Flight)-[T:Travels]->(a:Airport {City:'Rome'}) WHERE T.timeH < 15 return K.name, count(F)

