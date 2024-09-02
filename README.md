[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/62lhu51b)
# ProductsControllersTest

Det här är en samling tester för att säkerställa att `ProductController`-klassen i MVC-applikationen fungerar korrekt. Testerna För CategoryContrtoller är skrivna på samma sätt som productController.

## Testbeskrivningar för productController/CategoryContrtoller

1. **Index_ReturnsViewResult_WithListOfProducts**
   - Kontrollerar om `Index`-metoden returnerar en webbsidevy (`ViewResult`) som innehåller en lista med produkter.

2. **Create_ReturnsViewResult**
   - Testar om `Create`-metoden ger tillbaka en webbsidevy.

3. **Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid**
   - Testar att när en ny produkt skapas och produkten är "valid" så omdirigerar metoden oss till indexsidan.

4. **Create_Post_ReturnsViewResult_WhenModelStateIsInvalid**
   - Kontrollerar om `Create`-metoden returnerar en vy om det finns fel i den skickade datan.

5. **Edit_Get_ReturnsNotFoundResult_WhenIdIsNull**
   - Ser om `Edit`-metoden returnerar ett "not found"-resultat om inget ID ges.

6. **Edit_Get_ReturnsNotFoundResult_WhenProductDoesNotExist**
   - Kollar om `Edit`-metoden ger tillbaka "not found" om produkten med det angivna ID inte existerar.

7. **Edit_Get_ReturnsViewResult_WithProduct**
   - Testar att när vi försöker redigera en befintlig produkt, så får vi tillbaka en vy med information om den produkten.

8. **Edit_Post_ReturnsNotFoundResult_WhenIdDoesNotMatchProductId**
   - Kollar om `Edit`-metoden ger tillbaka "not found" om det skickade ID inte matchar produkten som försöker redigeras.

9. **Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid**
   - Testar om `Edit`-metoden omdirigerar till indexsidan när modellen är korrekt.

10. **Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid**
    - Kontrollerar om `Edit`-metoden ger tillbaka en vy om det finns fel i den skickade datan.

11. **Edit_Post_ReturnsNotFoundResult_WhenProductDoesNotExist**
    - Kollar om `Edit`-metoden ger tillbaka "not found" om produkten som ska redigeras inte existerar.

12. **Delete_Get_ReturnsNotFoundResult_WhenIdIsNull**
    - Kollar om `Delete`-metoden ger "not found" när ingen ID ges.

13. **Delete_Get_ReturnsNotFoundResult_WhenProductDoesNotExist**
    - Testar om `Delete`-metoden ger "not found" om produkten med det angivna ID inte existerar.

14. **Delete_Get_ReturnsViewResult_WithProduct**
    - Kollar om `Delete`-metoden ger tillbaka en vy med information om den valda produkten.

15. **DeleteConfirmed_ReturnsRedirectToActionResult**
    - Testar om `DeleteConfirmed`-metoden omdirigerar till indexsidan efter att en produkt har bekräftats som raderad.

16. **Index_ThrowsException**
    - Kontrollerar om ett undantag (`ObjectDisposedException`) kastas om kontexten för databasen har stängts innan `Index`-metoden anropas.

17. **Create_ThrowsException**
    - Kollar om ett undantag (`ObjectDisposedException`) kastas om kontexten för databasen har stängts innan `Create`-metoden anropas.

18. **Controller_ThrowsException**
    - Testar om undantag (ObjectDisposedException eller TargetInvocationException) kastas när databasens kontext har stängts för olika åtgärder på controllermetoder.

19. **Get_HasAllowAnonymousAttribute**
    - Kollar om metoderna `Details` och `Index` på `ProductController` är öppna för alla, vilket innebär att de inte kräver inloggning.

20. **Get_Has_authorize_attribute**
    - Kollar om metoderna `Create`, `Delete` och `Edit` på `ProductController` kräver att användaren är auktoriserad innan de kan användas.

21. **Controller_has_authorize_attribute**
    - Kollar om hela `ProductController` kräver att användaren är auktoriserad för att komma åt någon av dess åtgärder.

22. **TearDown**
    - Städar upp efter varje test genom att stänga av databaskontexten och sätta kontrollern till `null`.

# Tester för autentisering

1. **Get_SecurePage_RedirectToLogin("/Product/Create")**
    Försöker få tillgång till sidan för att skapa en ny produkt (/Product/Create) utan att vara inloggad. Då omdirigeras användaren till inloggningssidan.

2. **Get_SecurePage_RedirectToLogin("/Category/Create")**
    Försöker få tillgång till sidan för att skapa en ny kategori (/Category/Create) utan att vara inloggad. Då omdirigeras användaren till inloggningssidan.
   
![Testcov](https://github.com/Distansakademin/inl-mningsuppgift-3-mvc-autentisering-Wetterbro/assets/47597724/0dc85540-fb12-4cd8-b0fd-7527fede91df)
![test](https://github.com/Distansakademin/inl-mningsuppgift-3-mvc-autentisering-Wetterbro/assets/47597724/02506b8f-7083-410c-a74b-278b479e38e6)

Rapport Inlämningsuppgift 3: Dynamisk webbsida med MVC & autentisering
I denna uppgift har jag byggt en mvc applikation som hanterar produkter och kategorier samt har ett system för att kontrollera vilka användare som har tillgång till känsliga funktioner. För mig har det varit väldigt nyttigt att få jobba mer med MVC tillsammans med entity framework. Även om det känns som man förstår konceptet och grunderna i det så fastnar man ändå på vissa problem och jag tror att det är väldigt viktigt att hamna där för att lära sig, Speciellt när man jobbar med stora ramverk där det både finns mycket färdigskriven kod samt mycket som händer ”bakom ytan” som man inte har helt koll på. 
I denna uppgift så stötte jag på en hel del varierande problem: Det första problemet var på grund av att det hade blivit fel när jag skapade mitt projekt vilket jag inte märkte förens jag hade börjat skriva egen kod. Jag satt fast många timmar med detta innan jag till slut testade att skapa ett nytt projekt och testade det och sedan kopierade över min kod till det nya projektet, då märkte jag att det inte var min egen kod det var fel på. Så jag ska ta med mig att alltid testa koden innan jag börjar skriva in egna ändringar i mina MVC projekt. Jag stötte på ett annat problem när jag skulle göra testerna för autentiseringen, först så skrev jag testerna för de anonyma metoderna och det gick utan problem men när jag kom till det som kräver autentisering så fick jag inte tillbaka det jag förväntade mig. Jag har gjort så att hela min controller kräver autentisering men sedan så ”låser jag upp” några specifika metoder.  Då trodde jag att jag kunde ändra inherit parametern så den känner av det ärvda attributet men fick inte det att fungera. Så nu fick jag lägga en ”[Authorize]” på alla metoder för att testerna skulle gå igenom. Om det finns någon lösning på detta skulle jag gärna vilja veta hur.
 
Jag tycker att det har varit intressant att jobba med detta för det har lärt mig att se på websidor på ett lite annorlunda sätt och nu har jag fått lite mer förståelse för hur stora websidor kan vara uppbyggda. 

Udemy feedback
Jag tycker de första delarna av udemy kursen var bra men tyvärr så tycker jag att kvaliteten sjönk ganska snabbt efter det. Det blev mest en korvstoppning av material utan någon direkt förklaring. Mest ”gör så här” och sen gick det vidare, vissa delar fanns inte med utan det hade han gjort mellan klippningarna. Så det blev en hel del googlande för att förstå. Så det har varit svårt att hinna göra övningarna som fanns i Canvas och det känns lite synd för att för mig så är det nog det bästa sättet att lära mig.  




