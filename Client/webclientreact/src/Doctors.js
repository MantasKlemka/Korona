import React, { useEffect, useRef, useState} from "react";
import { Navigate, useNavigate } from "react-router-dom";
import {ReactComponent as Logo} from './medication.svg';

export default function Doctors() {
    const navigate = useNavigate();
    const [tableRows, setTableRows] = useState([]);

    useEffect(() => {
        if(sessionStorage.getItem("token") === null) {
            navigate("/");
        }
        fetchGetAll();
    }, []);
    
    function fetchGetAll(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        fetch('https://korona.azurewebsites.net/api/Doctor/All/', requestOptions)
        .then(res => res.text())
        .then(res => fillTable(res))
    }

    function fillTable(res){
        var doctors = JSON.parse(res);
        let i = 0;
        doctors.map(doctor=>
        (
            createRow(doctor, i),
            i++
        ));
    }

    function createRow(doctor, i){
        let id = React.createElement('td',{key: "id" + i}, doctor.id);
        let email = React.createElement('td',{key: "identification" + i}, doctor.email);
        let name = React.createElement('td',{key: "name" + i}, doctor.name);
        let surname = React.createElement('td',{key: "surname" + i},doctor.surname);
        let childs = [id, email, name, surname];
        let element = React.createElement('tr', {key: "row" + i}, childs);
        setTableRows(oldArray => [oldArray, element]);
    }

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
                        <a className="nav-link active">Doctors</a>
                        <a className="nav-item nav-link" href="/Pacients">Pacients</a>
                        <a className="nav-item nav-link" href="/Isolations">Isolations</a>
                        <a className="nav-item nav-link" href="/Tests">Tests</a>
                    </div>
                    <div className="navbar-nav">
                        <a className="nav-item nav-link" href="/">Logout</a>
                    </div>
                </div>
            </nav>
            <div className="infoTable">
                <b id="tableTitleText">All doctors</b>
                <table className="table">
                    <thead className="titleRow">
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Email</th>
                            <th scope="col">Name</th>
                            <th scope="col">Surname</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}                      
                    </tbody>
                </table>
            </div>
        </>
    )
}




