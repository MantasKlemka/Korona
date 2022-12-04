import React, { useEffect, useRef, useState} from "react";
import { Navigate, useNavigate } from "react-router-dom";
import {ReactComponent as Logo} from './medication.svg';

export default function MainPage() {
    const navigate = useNavigate();

    useEffect(() => {
        if(sessionStorage.getItem("token") === null) {
            navigate("/");
        }
    }, []);

    return (
        <>
            <nav className="navbar fixed-top navbar-expand-md navBar">
                <a className="navbar-brand" href="/mainpage">
                    <Logo className='logoMedic'/>
                </a>
                <button className="navbar-toggler toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon">
                        <img src="/menu.svg"></img>
                    </span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                    <div className="navbar-nav">
                        <a className="nav-item nav-link" href="/Doctors">Doctors</a>
                        <a className="nav-item nav-link" href="/Pacients">Pacients</a>
                        <a className="nav-item nav-link" href="/Isolations">Isolations</a>
                        <a className="nav-item nav-link" href="/Tests">Tests</a>
                    </div>
                    <div className="navbar-nav">
                        <a className="nav-item nav-link" href="/">Logout</a>
                    </div>
                </div>
            </nav>
        </>
    )
}




