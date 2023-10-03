import React from "react";
import { Outlet, Link } from "react-router-dom";
import TestPage from "./TestPage";

const Layout = () => {
  return (
    
    <div className="Layout">
        <TestPage/>
        <Outlet />
    </div>
  );
};

export default Layout;