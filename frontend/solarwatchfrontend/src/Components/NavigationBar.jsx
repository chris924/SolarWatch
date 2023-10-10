import NavLink from "react-bootstrap/NavLink";
import {Link} from 'react-router-dom';
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";
import "bootstrap/dist/css/bootstrap.css";
import { ReactDOM } from "react";
import NavItem from "react-bootstrap/NavItem";
import Cookies from "js-cookie";
import "../Styles/navigationbar.css";

export default function NavigationBar()
{
    const handleLogout = () => {

      Cookies.remove("jwtToken");

    }


    return (
      <Navbar
        bg="transparent"
        variant="dark"
        expand="lg"
        style={{ color: "white" }}
      >
        <Container>
          <Navbar.Toggle
            aria-controls="basic-navbar-nav"
            className="ml-auto"
          />
          <Navbar.Collapse id="basic-navbar-nav" className="justify-content-center">
            <Nav>
              <Nav.Item>
                <Link to="/register" className="navbartext">
                  Register&nbsp;
                </Link>
              </Nav.Item>
              <Nav.Item>
                <Link to="/login" className="navbartext">
                  Login&nbsp;
                </Link>
              </Nav.Item>
              <Nav.Item>
                <Link to="/login" onClick={handleLogout} className="navbartext">
                  Logout&nbsp;
                </Link>
              </Nav.Item>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    );
  }
  

