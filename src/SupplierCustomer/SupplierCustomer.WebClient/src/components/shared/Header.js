import React from "react";
import { NavLink } from "react-router-dom";
import "./style.css";
import { Link } from "react-router-dom";
import ProfileIcon from "./ProfileIcon";

const Header = props => {
  const activeStyle = { color: "#004c5f" };
  const picture = require("../../media/picture.jpg");

  let authenticationButtons;
  let profile;

  if (localStorage.getItem("jwt") !== null) {
    profile = (
      <ProfileIcon
        email={localStorage.getItem("name")}
        role={localStorage.getItem("role")}
        logoutCallback={props.logoutCallback}
      />
    );
  } else {
    authenticationButtons = (
      <div className="container" id="links">
        <Link to="/login" className="link">
          Log in
        </Link>
        <Link to="/registration" className="link">
          Register
        </Link>
      </div>
    );
  }
  return (
    <div>
      <div className="container p-0 header-image-buttons">
        {authenticationButtons}
        {profile}
        <div className="blue"></div>
      </div>
      <div className="toolbar">
        <nav className="bg-light border toolbar-navigation">
          <div className="toolbar-items">
            <div className="toolbar-item">
              <NavLink to="/" activeStyle={activeStyle} exact>
                Home
              </NavLink>
            </div>
            <div className="toolbar-item">
                <NavLink to="/orders" activeStyle={activeStyle} >
                  Orders
                </NavLink>
              </div>
              <div className="toolbar-item">
                <NavLink to="/deliveries" activeStyle={activeStyle} >
                  Deliveries
                </NavLink>
              </div>
              <div className="toolbar-item">
                <NavLink to="/customer" activeStyle={activeStyle} >
                  Customers
                </NavLink>
              </div>
          </div>
        </nav>
      </div>
    </div>
  );
};

export default Header;
