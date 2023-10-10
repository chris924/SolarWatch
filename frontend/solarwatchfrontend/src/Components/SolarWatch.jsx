import React, { useEffect, useState } from 'react';
import Cookies from 'js-cookie';
import { useNavigate } from "react-router-dom"; 
import "../Styles/solarwatch.css";


export default function SolarWatch() {
  const navigate = useNavigate();
  const [city, setCity] = useState("");
  const [bearer, setBearer] = useState("");
  const [cityData, setCityData] = useState("");

  useEffect(() => {
    const jwtToken = Cookies.get("jwtToken");
    if (!jwtToken) {
      console.error("JWT token not found.");
      navigate("/login");
    }
    setBearer("Bearer " + jwtToken);
  }, []);

  async function fetchData() {
    try {
      const response = await fetch(`http://localhost:80/SunriseSunset/get-sunrise-sunset?city=${city}`, {
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
      setCityData(data);
    } catch (error) {
      console.error(error);
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    await fetchData();
  }

  return (
    <>
      <form onSubmit={handleSubmit}>
        <label htmlFor="city">City:</label> <br />
        <input
          type="text"
          id="city"
          name="city"
          onChange={(e) => setCity(e.target.value)}
        /><br />
        <input type="submit" value="Search City" />
      </form>
      <div className='citydata'>
      <p>{cityData.city}</p>
      <p>Sunrise {cityData.sunrise}</p>
      <p>Sunset {cityData.sunset}</p>
      </div>
    </>
  );
}