import React, { useEffect, useRef, useState} from "react";
import { Navigate, useNavigate } from "react-router-dom";
import {ReactComponent as Logo} from './medication.svg';

export default function Doctors() {
    const navigate = useNavigate();
    const [tableRows, setTableRows] = useState([]);
    const [idToDelete, setIdToDelete] = useState([]);

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

    async function fetchDelete(){
        const requestOptions = {
            method: 'DELETE',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Doctor/' + idToDelete, requestOptions);
        const data = await res.text();
        afterFetchDelete(res, data);
    }

    function afterFetchDelete(res, data){
        if(res.status != 200)
        {
            let error = document.getElementById('errorDelete');
            error.textContent = data;
        }
        else
        {
            window.location.reload(true);
        }
    }

    function activateDoctor(id){
        const requestOptions = {
            method: 'PUT',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        fetch('https://korona.azurewebsites.net/api/Doctor/Activate/' + id, requestOptions)
        .then(res => afterFetchEdit(res))
    }

    function afterFetchEdit(res){
        if(res.status != 200)
        {
        }
        else
        {
            window.location.reload(true);
        }
    }

    function DeleteDoctor(){
        fetchDelete();
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
        let activate = React.createElement('td', {key: "activate" + i},"", React.createElement('button',{key: "activate" + i, 'className': "btn floatButton", onClick: () => activateDoctor(doctor.id) },"Activate"));
        let deleteIsolation = React.createElement('td', {key: "delete" + i},"", React.createElement('button',{key: "deleteModal" + i, 'data-bs-toggle': "modal", 'data-bs-target': "#deleteModal", 'className': "btn floatButton", onClick: () => setIdToDelete(doctor.id) },"Delete"));
        let childs = [id, email, name, surname];
        if(sessionStorage.getItem("admin") != null){
            childs = [childs, deleteIsolation]
            if(doctor.activated === false){
                childs = [childs, activate]
            }
            let element = React.createElement('tr', {key: "row" + i}, childs);
            setTableRows(oldArray => [oldArray, element]);
        }
        else{
            if(doctor.activated === false){
                return;
            }

            let element = React.createElement('tr', {key: "row" + i}, childs);
            setTableRows(oldArray => [oldArray, element]);
        }
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
                            <th scope="col"></th>
                            <th scope="col"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}                      
                    </tbody>
                </table>
            </div>
            <div className="modal fade" id="deleteModal" tabIndex="-1" role="dialog" aria-labelledby="deleteModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="deleteModalLabel">Delete the Doctor</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <p id="errorDelete" className="errorTextTitle"></p><br></br>
                            <a>Are you sure you want to delete the doctor?</a><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => DeleteDoctor()}>Delete</button>
                        </div>
                    </div>
                </div>
            </div>
            <br></br><br></br>
            <footer className="footerLogin">
                <p>Copyright © 2022</p>
            </footer>
        </>
    )
}




