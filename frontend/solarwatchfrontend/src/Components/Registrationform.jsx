import { useState } from "react"


export default function RegistrationFrom()
{
   const [email, setEmail] = useState("");
   const [username, setUsername] = useState("");
   const [password, setPassword] = useState("");
    



    return(
        <>
        <form>
        <label for="email">E-mail:</label><br />
        <input type="text" id="email" name="email" /><br />
        <label for="username">Username:</label><br />
        <input type="text" id="username" name="username" /><br />
        <label for="password">Password:</label><br />
        <input type="password" id="password" name="password" /><br />
        <input type="submit" value="Register"></input>
        </form>
        </>
    )


}