import React, { useEffect, useState } from "react";

export default function RegistrationFrom()
{
   const [email, setEmail] = useState("");
   const [username, setUsername] = useState("");
   const [password, setPassword] = useState("");
    

    function SendRegister(email, username, password)
    {
        var regData = {
            "Email": email,
            "UserName": username,
            "Password": password
        }

        try {
          
            const response = fetch("http://localhost:80/Auth/Register", {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify(regData),
            });
          
            response
              .then((result) => {
                if (result.ok) {
                  console.log("Registered user");
                } else {
                  console.log("Error during register");
                }
              })
              .catch((error) => {
                console.error(error);
              });
          } catch (error) {
            console.error(error);
          }
        
        
        
    }

    const handleSubmit = (e) => {

        e.preventDefault();
        SendRegister(email, username, password);

    }



    return(
        <>
        <form onSubmit={handleSubmit}>
        <label for="email">E-mail:</label><br />
        <input type="text" id="email" name="email" onChange={(e) => setEmail(e.target.value)}/><br />
        <label for="username">Username:</label><br />
        <input type="text" id="username" name="username" onChange={(e) => setUsername(e.target.value)}/><br />
        <label for="password">Password:</label><br />
        <input type="password" id="password" name="password" onChange={(e) => setPassword(e.target.value)}/><br />
        <input type="submit" value="Register"></input>
        </form>
        </>
    )


}