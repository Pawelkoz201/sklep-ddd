import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import routes from "./utils/router";  
import TopNavbar from "./components/topnav";
import UserProvider from "./services/userContext";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export default function App() {
  return (
    <Router>
      <UserProvider>
      <ToastContainer position="bottom-right" autoClose={2000} />
      <TopNavbar />
      <Routes>
        {routes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Routes>
      </UserProvider>
    </Router>
  );
}
