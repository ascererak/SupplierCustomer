import React from 'react';
import { Route } from "react-router-dom";
import Header from "./shared/Header";
import Form from "./authorization/Form";
import { Form as RegistrationForm } from "./registration/Registration";
import config from "../config";
import HomePage from "./homepage/HomePage";
import './App.css';

class App extends React.Component {
  state = {
    sideDrawerOpen: false,
    user: { email: "", role: "manager", token: null }
  };
  drawerToggleClickHandler = () => {
    this.setState(prevState => {
      return {
        sideDrawerOpen: !prevState.sideDrawerOpen
      };
    });
  };
  backdropClickHandler = () => {
    this.setState({ sideDrawerOpen: false });
  };
  loginSuccess = token => {
    localStorage.setItem("jwt", token);
    fetch(`${config.apiDomain}/api/account/get/${localStorage.getItem("jwt")}`)
      .then(res => res.json())
      .then(res => {
        if (res && res.email && res.role) {
          localStorage.setItem("name", res.name);
          localStorage.setItem("role", res.role);
          localStorage.setItem("photoPath", "/" + res.photoPath);
          this.forceUpdate();
        }
      });
  };
  logoutCallback = () => {
    fetch(`${config.apiDomain}/api/account/logout`, {
      method: "post",
      headers: {
        "Content-type": "application/json"
      },
      body: JSON.stringify({ token: localStorage.getItem("jwt") })
    });
    localStorage.removeItem("jwt");
    localStorage.removeItem("name");
    localStorage.removeItem("role");
    localStorage.removeItem("photoPath");
    this.forceUpdate();
  };

  orderSuccess = () => {
    this.setState({ type: "success" });
  };

  componentDidMount() {
    localStorage.setItem("nameFilter", "");
    localStorage.setItem("cityFilter", "");
    localStorage.setItem("priceFromFilter", "");
    localStorage.setItem("priceToFilter", "");
    localStorage.setItem("checkInFilter", "");
    localStorage.setItem("checkOutFilter", "");
    localStorage.setItem("adultsFilter", "");
  }

  render() {
    const { sideDrawerOpen, user } = this.state;
  return (
    <div className="container p-0 border" style={{ minHeight: "100%" }}>
    <Header
      userProfile={user}
      logoutCallback={this.logoutCallback}
    />
    <div className="container pt-5">
      <Route exact path="/" component={HomePage} />

      <Route
        path="/login"
        render={props => (
          <Form {...props} successCallback={this.loginSuccess} />
        )}
      />
      
      <Route
        path="/registration"
        render={props => (
          <RegistrationForm
            type={"registration"}
            submitText={"Register"}
            successCallback={this.loginSuccess}
          />
        )}
      />     
    </div>
  </div>
  );
}
}

export default App;
