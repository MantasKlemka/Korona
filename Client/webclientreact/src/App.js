import { Route, Routes } from "react-router-dom";
import MainPage from "./MainPage";
import Login from "./Login"
import Register from "./Register";
import Doctors from "./Doctors";
import Pacients from "./Pacients";
import Isolations from "./Isolations";
import React from 'react';

function App() {
  return (
    <>
    <Routes>
      <Route path='' element={<Login/>}/>
      <Route path='/Register' element={<Register/>}/>
      <Route path='/MainPage' element={<MainPage/>}/>
      <Route path='/Doctors' element={<Doctors/>}/>
      <Route path='/Pacients' element={<Pacients/>}/>
      <Route path='/Isolations' element={<Isolations/>}/>
    </Routes>
    </>

  )
}

export default App;
