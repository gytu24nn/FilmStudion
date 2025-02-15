console.log("hej");

const loginForm = document.getElementById("loginForm");
const filmListDiv = document.getElementById("movie-List");
const filmsUl = document.getElementById("films");
const logoutBtn = document.getElementById("logoutBtn");

loginForm.addEventListener("submit", async (event) => {
  event.preventDefault();

  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;
  const username = document.getElementById("username").value;

  try {
    const response = await fetch(
      "http://localhost:5033/api/users/authenticate",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          password: password,
          username: username,
        }),
      }
    );

    if (response.ok) {
      const data = await response.json();
      console.log(data);
      localStorage.setItem("token", data.Token);
      alert(`Inloggning lyckades som ${data.role}`);
      showFilmList();
      console.log("inloggad!");
    } else {
      alert("Felaktig e-postadress eller lösenord");
    }
  } catch (error) {
    console.error("Ett fel inträffade vid inloggning:", error);
  }
});

async function showFilmList() {
    loginForm.style.display = "none"; // Döljer loginformuläret
    filmListDiv.style.display = "block"; // Visar filmsektionen
  
    const token = localStorage.getItem("token");
    let headers = {};
  
    if (token) {
      headers["Authorization"] = `Bearer ${token}`; // Lägg till token i headern
    }
  
    try {
      const response = await fetch("http://localhost:5033/api/films", {
        headers, // Skickar med headers med token
      });
  
      if (response.ok) {
        const films = await response.json(); // Hämtar filmer från API
        console.log(films); // Kontrollera att filmerna kommer rätt här
        console.error("API-fel: Statuskod", response.status);
  
        filmsUl.innerHTML = ""; // Tömmer listan på gamla filmer
  
        films.forEach((film) => {
            console.log("Film object:", film);  // Logga hela filmobjektet
        
            // Om filmCopies inte finns, kolla om movieAvailableCopies finns
            const availableCopies = film.movieAvailableCopies || 0;
        
            const li = document.createElement("li");
            li.textContent = `${film.movieName} - ${film.movieDescription} (${film.movieGenre})`;
        
            // Kolla om vi har kopior att visa
            if (availableCopies > 0) {
                li.textContent += ` - ${availableCopies} kopior tillgängliga`;
            } else {
                li.textContent += ` - Inga kopior tillgängliga`;
            }
        
            filmsUl.appendChild(li);
        });       
      } else {
        console.error("Fel vid hämtning av filmer");
      }
    } catch (error) {
      console.error("Ett fel inträffade vid hämtning av filmer:", error);
    }
  }
  

// Logga ut
logoutBtn.addEventListener("click", () => {
  localStorage.removeItem("token"); // Ta bort token från localStorage
  alert("Du har loggats ut");
  loginForm.style.display = "block"; // Visa inloggningsformuläret igen
  filmListDiv.style.display = "none"; // Dölj filmsektionen
});

// Om användaren redan är inloggad, visa filmer direkt
if (localStorage.getItem("token")) {
  showFilmList();
}
