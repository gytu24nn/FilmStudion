# Filmstudion inlämning reflektion 

Jag påbörjade detta projekt med en planering, men jag upplevde det som utmanande på grund av de många krav som var beroende av varandra. Varje krav innehöll dessutom flera delar som behövde uppfyllas, vilket gjorde det svårt att få en helhetsbild. Även om jag gjorde en planering som verkade fungera i början, upptäckte jag under kodens gång att planen inte höll. Istället för att stanna upp och justera planen fortsatte jag att skriva kod, vilket ledde till att jag blev förvirrad över vad som var klart och vad som återstod.

En konsekvens av detta var att jag skapade flera nya DTO:er där jag i vissa fall hade kunnat återanvända de som redan fanns. Till nästa liknande projekt planerar jag att justera min planering under arbetets gång om den inte fungerar, för att undvika samma misstag.

En sak jag ska tänka på till nästa gång är att lägga in lite data i databasen från början. För nu när man ska testa frontend så måste man skapa filmer via till exempel Postman eller något liknande för att kunna se filmerna när man loggat in som filmstudio. Så det är något jag ska tänka på till nästa gång. 

Däremot är jag väldigt nöjd med hur jag fick till frontend att jag fetchade authenticate så man kan logga in både som admin och filmstudio. Det kommer också upp en alert vad man loggat in som samt att om du uppdaterar sidan så är du fortfarande inloggad. Eftersom jag sparade token i localstorage. 

## REST

Under detta projekt har jag använt interfaces, klasser och framförallt DTO:er (Data Transfer Objects) för att strukturera resurserna i mitt API. Syftet var att säkerställa att endast relevant data hanteras och skickas tillbaka, vilket är en viktig del av att följa REST-principerna. REST innebär att datamodeller separeras från de externa representationerna i API:et för att göra det flexibelt och säkert.

De viktigaste interfaces jag har använt är ICreateFilm, IFilm, IFilmStudio, IRegisterFilmStudio, IUser, IUserAuthenticate och IUserRegister. Dessa interfaces definierar vilka fält som krävs för olika operationer, som att skapa en film eller registrera en filmstudio.

Mina DTO:er ärver från dessa interfaces och används vid både in- och utgående anrop. Till exempel hanterar IUserRegister registreringsdata och IUserAuthenticate används för inloggningsuppgifter. Detta säkerställer att varje endpoint hanterar rätt data och att känslig information som lösenord hanteras korrekt.

Eftersom jag inte hade en fungerande planering från början, skapade jag i början en ny DTO för nästan varje krav utan att tänka på att jag kunde återanvända de jag redan hade skapat. Detta resulterade i många filer och gjorde projektet svårare att överblicka. När jag upptäckte detta gick jag tillbaka och började återanvända DTO:er där det var möjligt. Genom att minska antalet filer blev projektet mer överskådligt och lättare att förstå. Detta var en viktig insikt som jag kommer ta med mig till framtida projekt.

En annan sak jag ska tänka på till nästa projekt är att vara mer konsekvent och systematisk i namngivningen av mina DTO:er. Jag ska även dokumentera vad varje DTO representerar för att göra koden mer lättförståelig för både mig själv och andra utvecklare. Ett konkret exempel på detta är att jag skulle kunna skapa en struktur där alla DTO:er relaterade till filmer finns i en mapp med ett gemensamt namnprefix, som 'FilmDTO', för att underlätta navigeringen och göra koden mer överskådlig."

Till nästa projekt planerar jag också att lägga mer tid på att göra en bättre planering så att jag redan från början kan tänka på hur DTO:er ska återanvändas och därmed undvika att skapa onödigt många. Jag inser också vikten av att dokumentera mina DTO:er ordentligt för att göra det enklare för både mig och andra att förstå deras syfte och användning.

En sak jag ska tänka på nästa projekt är att använda verktyg som Automapper och bas-DTO:er. Det är något jag ska läsa på mer till nästa projekt så att jag kan använda. Det hade underlätta både för mig och de andra som läser koden för det skulle göra koden mer strukturerad, överskådlig och effektivisera kod struktur. 

## Implementation 
Under implementationen av API:et fanns det flera olika valmöjligheter när det gäller design och struktur. Jag valde i mitt projekt att skapa interfaces som inne håller de objekt som behövs och sen skapa en DTO eller klass som ärver från interfacet. Detta tyckte jag var viktigt då de skulle innehålla liknande information. Till exempel när jag skapar en film som admin så har jag ett interface och en dto för de och de innehåller samma objekt. Men vissa DTO:er jag skapat innehåller allt som interfacet gör plus något mer som behövs för de kravet. 

Jag valde att använda de för att separera interna datamodeller från de externa resurser som exponeras via API:et. Detta var en del av arbetet med följa REST-principerna där syftet är att säkerställa att endast relevant data skickas till användaren samtidigt som den interna logiken förblir osynlig. 

En utmaning jag stötte på under implementationen var att säkerställa att den data som skickades till användaren inte innehöll känslig information som till exempel lösenord. Därför valde jag använda DTO:er för att hantera och filtrera bort onödig data innan den skickades vidare. Detta förbättrade säkerheten och enkelheten i API:et.

I projektet använde jag HTTP-metoderna GET, POST och PATCH för att hantera de olika kraven. Vilket följer REST-konventionerna för att skapa, läsa, uppdatera och ta bort data. 

Till nästa projekt där jag ska använda API så ska jag tänka mer vilka objekt jag ska ha i varje interface och DTO. Då jag nu i efterhand skulle till exempel vilja ha mer information om en film till exempel regissör. Så det är något jag ska tänka på till nästa projekt är att planera mer vilka objekt jag ska ha i varje interface och DTO jag skapar. 

Jag hade i projektet väldigt svårt att få till relationen mellan filmcopies listan och avaliablecopies variablen. Det löste sig tillslut men hade ganska stora problem med att får databasen att hänga med hur många filmcopies det fanns efter jag uppdaterat filmen med en patch men de löste sig tillslut. Det är jag väldigt nöjd över att jag gjorde. 

## Säkerhet
Under projektets gång har jag använt mig av DTO:er för att kontrollera vilken data som behövs vid in- och ut-skick. Till exempel har jag endast använt lösenord vid in-skick, eftersom det aldrig ska skickas tillbaka till användaren för säkerhetens skull. Vid registrering har jag även använt hashade lösenord för att öka säkerheten ytterligare.

När användaren loggar in på frontend har jag använt en autentisering (authenticate) POST som genererar en token, vilken lagras i localStorage. Denna POST kontrollerar också att både användarnamn, lösenord och e-postadress stämmer överens med de konton som är registrerade. Jag tycker detta är väldigt viktigt för säkerheten, eftersom enbart kontroll av ett av fälten skulle kunna skapa en säkerhetsrisk.

I autentisering POST på både backend och frontend har jag implementerat noggrann felhantering så att endast ett registrerat konto kan få en token. Detta är en viktig säkerhetsåtgärd, eftersom det innebär att man kan begränsa åtkomsten till vissa funktioner. Till exempel kan endast en admin skapa en film, vilket hindrar filmstudior från att skapa sina egna filmer.

För att begränsa åtkomsten har jag skapat två variabler: en admin-token och en filmstudio-token. Jag kontrollerar att token är korrekt och att den inte är tom. Jag hade dock velat använda unika, slumpmässiga tokens för varje konto för att öka säkerheten ytterligare. Jag testade att använda detta men lyckades inte riktigt få det att fungera, så jag använde istället en variabel för token. Trots detta uppstod ibland problem med denna lösning. Jag planerar att läsa mer om JWT-token och hoppas kunna implementera det i nästa projekt för att förbättra säkerheten ännu mer.

 