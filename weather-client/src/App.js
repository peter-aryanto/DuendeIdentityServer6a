/*
import logo from './logo.svg';
import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}
*/

import { BrowserRouter, Routes, Route } from "react-router-dom";
import { /*React, */useState, useEffect } from "react";
import { UserManager } from "oidc-client";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<HomePage />} />
        <Route path='/signin-oidc' element={<Callback />} />
      </Routes>
    </BrowserRouter>
  );
}

function HomePage() {
  const [state, setState] = useState(null);

  const mgr = new UserManager({
    authority: "https://localhost:5001",
    client_id: "weather-client",
    response_type: "code",
    scope: "openid profile weatherapi.read",
    redirect_uri: "http://localhost:3000/signin-oidc",
    post_logout_redirect_uri: "http://localhost:3000"
  });

  useEffect(() => {
    mgr.getUser().then(user => {
      if (user) {
        // console.log(user);
        // console.log(user.profile);
        // setState({ user });
        fetch('https://localhost:5003/WeatherForecast', {
          headers: {
            Authorization: `Bearer ${user.access_token}`,
          },
        })
          .then((res) => res.json())
          .then((data) => setState({ user, data }));
      }
    });
  }, []);

  return (
    <div>
      { state ?
        <>
          <h3>Welcome {state?.user?.profile?.name}</h3>
          <pre>{JSON.stringify(state?.data, null, 2)}</pre>
          <button onClick={() => mgr.signoutRedirect()}>Logout</button>
        </> :
        <>
          <h3>React Weather App</h3>
          <button onClick={() => mgr.signinRedirect()}>Login</button>
        </>
      }
    </div>
  );
}

function Callback() {
  useEffect(() => {
    const mgr = new UserManager({ response_mode: "query" });
    mgr.signinRedirectCallback()
      .then(() => window.location.href = '/')
      .catch((/*error*/) => {});
  }, []);

  return <p>Loading...</p>;
}

export default App;
