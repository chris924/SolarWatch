import React, { useEffect, useState } from 'react';

export default function TestText() {
  const [baseText, setBaseText] = useState([]);

  useEffect(() => {
    fetch("http://localhost:80/SunriseSunset/get-sunrise-sunset?city=London")
      .then(res => res.json())
      .then(data => setBaseText(data))
      .catch(err => console.error(err));
  }, []);

useEffect(() =>  {
    console.log(baseText);
}, [baseText])

  return (
    <>
    <p>{baseText.city}</p>
    <p>{baseText.sunrise}</p>
    <p>{baseText.sunset}</p>
    </>
  );
}