import React, { useRef } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from 'react-bootstrap';

export default function Register() {
    const emailInput = useRef()
    const identificationInput = useRef()
    const nameInput = useRef()
    const surnameInput = useRef()
    const passwordInput = useRef()
    const passwordInputTwo = useRef()
    const navigate = useNavigate();

    async function processRegister(e){
        const emailText = emailInput.current.value
        const nameText = nameInput.current.value
        const surnameText = surnameInput.current.value
        const passwordText = passwordInput.current.value
        const passwordTextTwo = passwordInputTwo.current.value
        let error = document.getElementById('errorLogin');

        if(emailText.length !== 0 && nameText.length !== 0 && surnameText.length !== 0 && passwordText.length !== 0 && passwordTextTwo.length !== 0){
            if(!emailText.includes("@")){
                error.textContent = "Email has to have @!";
            }
            else if(passwordText === passwordTextTwo && passwordText.length >= 8){
                const requestOptions = {
                    method: 'POST',
                    headers: { 
                        'Content-Type': 'application/json'},
                    body:JSON.stringify({ "email": emailText, "password": passwordText, "name": nameText, "surname": surnameText })
                };
                const res = await fetch('https://korona.azurewebsites.net/api/Doctor/', requestOptions)
                const data = await res.text();
                if(res.status != 200){
                    error.textContent = data;
                }
                else{
                    directToLogin();
                }
            }
            else{
                if(passwordText != passwordTextTwo){
                    error.textContent = "Passwords do not match!";
                }
                else{
                    error.textContent = "Password can not be shorter than 8 characters!";
                }
            }
        }
        else{
            error.textContent = "All fields need to be filled!";
        }

    }

    function directToLogin(e){
        navigate("/")
    }

    return (
        <>
            <div className="centerMiddle">
                <div className="logo"><b>KORONA WEB</b></div>
                <div className="formBack">
                    <div className="header"><b>Employee Register</b></div>
                    <br></br>
                    <p id="errorLogin" className="errorTextTitle"></p>
                    <input className="form-control inputs" ref={emailInput} type="text" placeholder="Email" maxLength="22" required/>
                    <br></br>
                    <input className="form-control inputs" ref={nameInput} type="text" placeholder="Name" maxLength="22" required/>
                    <br></br>
                    <input className="form-control inputs" ref={surnameInput} type="text" placeholder="Surname" maxLength="22" required/>
                    <br></br>
                    <input className="form-control inputs" ref={passwordInput} type="password" placeholder="Password" maxLength="27" required/>
                    <br></br>
                    <input className="form-control inputs" ref={passwordInputTwo} type="password" placeholder="Re-type Password" maxLength="27" required/>
                    <br></br>
                    <Button className="loginButton btn-secondary" onClick={processRegister}>Register</Button>
                    <Button className="registerButton btn-secondary" onClick={directToLogin}>Login</Button>       
                </div>       
            </div>
            <footer className="footerLogin">
                <p>Copyright © 2022</p>
            </footer>
        </>
    )
}
