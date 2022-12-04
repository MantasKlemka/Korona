import React, { useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from 'react-bootstrap';


export default function Login() {
    const usernameInput = useRef()
    const passwordInput = useRef()
    const navigate = useNavigate();

    async function processLogin(e){
        const usernameText = usernameInput.current.value
        const passwordText = passwordInput.current.value
        var requestOptions;
        var login = "";

        if(usernameText.includes("@")){
            login = "Doctor";
            requestOptions = {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/json'},
                body:JSON.stringify({ "email": usernameText, "password": passwordText })
            };
        }
        else{
            login = "Admin";
            requestOptions= {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/json'},
                body:JSON.stringify({ "username": usernameText, "password": passwordText })
            };
        }

        if(usernameText.length !== 0 && passwordText.length !== 0){
           const res = await fetch('https://korona.azurewebsites.net/api/Main/Login/' + login, requestOptions)
           const data = await res.text();
           if(res.status === 200){
                if(login === "Doctor"){
                    var split = data.split(",");
                    directToMainPage(split[1], split[0]);
                }
                else{
                    directToMainPage(data, -1)
                }

           }
           else{
             let error = document.getElementById('errorLogin');
             error.textContent = data;
           }
        }
        
    }

    useEffect(() => {
        sessionStorage.clear();
    });

    function directToRegister(e){
        navigate("/Register")
    }

    function directToMainPage(token, id){
        sessionStorage.setItem('token', token)
        if(id != -1){
            sessionStorage.setItem('doctorID', id)
        }
        navigate("/MainPage")
    }

    
    return (
        <>
            <div className="centerMiddle">
                <div className="logo"><b>KORONA WEB</b></div>
                <div className="formBack">
                    <div className="header"><b>Employee Login</b></div>
                    <br></br>
                    <p id="errorLogin" className="errorTextTitle"></p>
                    <input  className="form-control inputs" ref={usernameInput} type="text" placeholder="Username/Email" maxLength="22" required />
                    <input className="form-control inputs" ref={passwordInput} type="password" placeholder="Password" maxLength="27" required/>
                    <br></br>
                    <Button className="loginButton btn-secondary" onClick={processLogin}>Login</Button>
                    <Button className="registerButton btn-secondary" onClick={directToRegister}>Register</Button>       
                </div>       
            </div>
            <footer className="footerLogin">
                <p>Copyright © 2022</p>
            </footer>
        </>
    )
}
