import React from "react";
import { Outlet, Link } from "react-router-dom";
import TestPage from "./SolarPage";
import NavigationBar from "../Components/NavigationBar.jsx"
import VideoBackground from "./VideoBackground";

const Layout = () => {
  return (
    
    <div className="Layout">
        <NavigationBar/>
        
        <Outlet />
      
    </div>
  );
};

export default Layout;