import React, { useEffect, useState } from "react";


export default function LoginForm()
{
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

   async function SendLogin(email, password)
    {
        var LoginData = {
            "Email": email,
            "Password": password
        }

        try{
            const response = await fetch("http://localhost:80/Auth/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(LoginData)
            });

            if(response.ok)
            {
                console.log("Successfully logged in!");
            }
            else
            {
            console.log("Error during login");
            }


        }catch(err)
        {
            console.error(err);
        }


    }


    const handleSubmit = (e) =>
    {
        e.preventDefault();
        SendLogin(email, password);

    }



return(
        <>
        <form onSubmit={handleSubmit}>
        <label for="email">Email:</label> <br/>
        <input type="text" id="email" name="email" onChange={(e) => setEmail(e.target.value)}  /> <br/>
        <label for="password">Password:</label> <br/>
        <input type="password" id="password" name="password" onChange={(e) => setPassword(e.target.value)} /> <br/>
        <input type="submit" value="Login" />
        </form>
        </>
    )


}