import NavLink from "react-bootstrap/NavLink";
import {Link} from 'react-router-dom';
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";
import "bootstrap/dist/css/bootstrap.css";
import { ReactDOM } from "react";
import NavItem from "react-bootstrap/NavItem";

export default function NavigationBar()
{
    return(

    
    <Navbar
      bg="dark"
      variant="dark"
      expand="lg"
      className="me-auto my-2 my-lg-0"
    >
      <Container style={{ float: "right" }} fluid>
        <Nav>
          <NavLink><Link to="/register">Register</Link></NavLink>
          <NavLink> <Link to="/login">Login</Link></NavLink>
        </Nav>
      </Container>
    </Navbar>

    )

}