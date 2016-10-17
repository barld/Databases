// Q1
match (a:Airport)-[:Is]->(:AirportSize{name:"Large"}) return a.name, a.Capicity