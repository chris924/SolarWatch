import React, { useEffect, useState } from 'react';
import Cookies from 'js-cookie';


export default function TestText() {
  const [baseText, setBaseText] = useState([]);

  useEffect(() => {
    const jwtToken = Cookies.get("jwtToken");
    if (!jwtToken) {
      console.error("JWT token not found.");
      return;
    }
    console.log("asd")
    var bearer = "Bearer "+jwtToken

    async function fetchData() {
      try {
        const response = await fetch("http://localhost:80/SunriseSunset/get-sunrise-sunset?city=London", {
          method: "GET",
          withCredentials: true,
          credentials: 'include',
          headers: {
            "Authorization": bearer,
            "Content-Type": "application/json"
          }
        });
    
        if (!response.ok) {
          throw new Error('Request failed with status ' + response.status);
        }
    
        const data = await response.json();
        setBaseText(data);
      } catch (error) {
        console.error(error);
      }
    }
    
    fetchData();
}, [])


  return (
    <>
    <p>{baseText.city}</p>
    <p>{baseText.sunrise}</p>
    <p>{baseText.sunset}</p>
    </>
  );
}