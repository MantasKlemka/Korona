import React, { useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from 'react-bootstrap';


export default function Main() {
    const isolationInput = useRef()
    const navigate = useNavigate();

    async function processIsolation(e){

        const isolationText = isolationInput.current.value
        let error = document.getElementById('errorLogin');
        error.textContent = "";
        document.getElementById("IsolationResponse").textContent = "";
        document.getElementById("IsolationCause").textContent = "";
        document.getElementById("IsolationAmount").textContent = "";
        document.getElementById("IsolationStart").textContent = "";

        if(isolationText.length !== 0){
            const requestOptions = {
                method: 'GET',
                headers: { 
                    'Content-Type': 'application/json'}
            };
            const res = await fetch('https://korona.azurewebsites.net/api/Isolation/Check/' + isolationText, requestOptions)
            const data = await res.text();
            if(res.status != 200){
                error.textContent = data;
            }
            else{
                FillResponse(data);
            }
        }

                
    }

    useEffect(() => {
        sessionStorage.clear();
    });

    function FillResponse(data){
        var json = JSON.parse(data);
        document.getElementById("IsolationResponse").textContent = "Isolation info:";
        document.getElementById("IsolationCause").textContent = "Cause: " + json.Cause;
        document.getElementById("IsolationAmount").textContent ="Amount of days: " + json.AmountOfDays;
        document.getElementById("IsolationStart").textContent = "Start date: " + json.StartDate;

    }

    function directToLogin(e){
        navigate("/Login")
    }
    
    return (
        <>
            <div className="centerMiddle">
                <div className="logo"><b>KORONA WEB</b></div>
                <div className="formBack">
                    <div className="header"><b>Check Isolation</b></div>
                    <br></br>
                    <p id="errorLogin" className="errorTextTitle"></p>
                    <input  className="form-control inputs" ref={isolationInput} type="text" placeholder="Isolation code" maxLength="22" required />

                    <Button className="registerButton btn-secondary" onClick={processIsolation}>Check</Button>
                    <Button className="employeeButton btn-secondary" onClick={directToLogin}>Employee Login</Button>   
                    <br></br>
                    <br></br>
                    <br></br>
                    <b><p id="IsolationResponse" className="isolationResponse"></p></b>
                    <p id="IsolationCause" className="isolationResponse"></p>
                    <p id="IsolationAmount" className="isolationResponse"></p>
                    <p id="IsolationStart" className="isolationResponse"></p>
                    <br></br>
                </div>       
            </div>
            <footer className="footerLogin">
                <p>Copyright © 2022</p>
            </footer>
        </>
    )
}
