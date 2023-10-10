import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; 
import Cookies from "js-cookie";

export default function LoginForm() {
  const navigate = useNavigate(); 
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loginResponse, setLoginResponse] = useState(null);
  const [jwtToken, setJwtToken] = useState(""); 

  async function SendLogin(email, password) {
    var LoginData = {
      Email: email,
      Password: password,
    };

    try {
      const response = await fetch("http://localhost:80/Auth/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(LoginData),
      });

      if (response.ok) {
        response.json().then((data) => {
          const token = data.token;
          Cookies.set("jwtToken", token);
          setJwtToken(token);
          console.log("Successfully logged in!");
          navigate("/solar-watch"); 
        });
      } else {
        console.log("Error during login");
        setLoginResponse("Wrong username or password"); 
      }
    } catch (err) {
      console.error(err);
    }
  }

  const handleSubmit = (e) => {
    e.preventDefault();
    SendLogin(email, password);
  };

  return (
    <>
      <form onSubmit={handleSubmit}>
        <label htmlFor="email">Email:</label> <br />
        <input
          type="text"
          id="email"
          name="email"
          onChange={(e) => setEmail(e.target.value)}
        />{" "}
        <br />
        <label htmlFor="password">Password:</label> <br />
        <input
          type="password"
          id="password"
          name="password"
          onChange={(e) => setPassword(e.target.value)}
        />{" "}
        <br />
        <input type="submit" value="Login" />
      </form>

      
      {loginResponse && (
        <div className="loginerror">
          <p>{loginResponse}</p>
        </div>
      )}

       

    </>
  );
}