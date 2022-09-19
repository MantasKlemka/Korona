# Projektas „Korona“

## Sistemos paskirtis 
Projekto tikslas – sukurti sistemą, kuri lengvintu Koronos viruso užsikrėtusių asmenų registraciją bei stebėjimą.
Veikimo principas – kuriamą platformą sudaro dvi dalys: internetinė aplikacija, kuria naudosis gydytojai, pacientai ir administratorius bei aplikacijų programavimo sąsaja (angl. trump. API). Gydytojas, norėdamas naudotis šia platforma, prie jos prisiregistruos ir po sėkmingo paskyros sukūrimo, prisijungs. Prisijungęs gydytojas gali užregistruoti naujus pacientus, pacientam paskirti izoliaciją, parinkti izoliacijos tipą bei sunkiai sergančiam pacientui rezervuoti vietą atitinkamoje ligoninėje. Gydytojas, gali matyti visų esamų pacientų informaciją įvedus jų asmens kodą, bei matyti visus savo užregistruotus pacientus. Neprisijungęs vartotojas gali peržiūrėti asmens izoliacijos informaciją, pateikus asmens kodą pagrindiniame puslapyje. Administratorius turės galimybę patvirtinti gydytojų registracijas, įvesti naujas ligonines į sistemą.

## Funkciniai reikalavimai 
Neregistruotas sistemos naudotojas galės: 
1.	Peržiūrėti platformos pagrindinį puslapį; 
2.	Peržiūrėti detalią izoliacijos pabaigą pagal asmens kodą;

Gydytojas galės: 
1.	Atsijungti nuo internetinės aplikacijos; 
2.	Prisijungti prie platformos; 
3.	Užregistruoti pacientą:
    -	Pridėti paciento asmeninę informaciją;
    - Gyvenamą vietą;
4.	Užregistruoti paciento izoliaciją:
    - Įtraukti izoliacijos priežastį;
    - Pateikti izoliacijos laikotarpį;
5.	Užregistruoti paciento testą:
    - Įtraukti testo rezultatą
5.	Rezervuoti pacientui vietą ligoninėje;
6.	Peržiūrėti kitus esamus pacientus, bei jų izoliacijas, įvedus jų asmens kodą;
7.	Peržiūrėti savo užregistruotų pacientų informaciją bei izoliacijas;
8.	Peržiūrėti kitų esamų gydytojų informaciją;

Administratorius galės: 
1.	Patvirtinti gydytojo registraciją;
2.	Registruoti naujas ligonines;
3.	Ištrinti gydytojo arba paciento profilius
 
## Sistemos architektūra 
Sistemos sudedamosios dalys: 
-   Kliento pusė – naudojant React.js; 
-	Serverio pusė – naudojant ASP.NET.
-	Duomenų bazė – MySQL. 

1 pav. pavaizduota kuriamos sistemos diegimo diagrama. Sistemos talpinimui yra
naudojamas Azure serveris. Kiekviena sistemos dalis yra diegiama tame pačiame serveryje.
Internetinė aplikacija yra pasiekiama per HTTP protokolą. Šios sistemos veikimui yra
reikalingas Korona API, kuris pasiekiamas per aplikacijų programavimo sąsają. Pats Korona
API vykdo duomenų mainus su duomenų baze - tam naudojama ORM sąsaja. 
