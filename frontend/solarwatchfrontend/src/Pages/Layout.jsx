import React from "react";
import { Outlet, Link } from "react-router-dom";
import NavigationBar from "../Components/NavigationBar.jsx"


const Layout = () => {
  return (

    <div className="Layout">
        <NavigationBar/>
        
        <Outlet />
      
    </div>
  );
};

export default Layout;