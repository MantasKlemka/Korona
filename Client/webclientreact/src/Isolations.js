import React, { useEffect, useRef, useState} from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { Button } from 'react-bootstrap';
import {ReactComponent as Logo} from './medication.svg';
import ProgressBar from 'react-bootstrap/ProgressBar';

export default function Isolations() {
    const navigate = useNavigate();
    const [tableRows, setTableRows] = useState([]);

    const [cause, setCause] = useState([]);
    const [start, setStart] = useState([]);
    const [amount, setAmount] = useState([]);
    const [pacient, setPacient] = useState([]);
    const [code, setCode] = useState([]);
    const [visibilityStatus, setVisibilityStatus] = useState([]);
    const [isolationID, setIsolationID] = useState([]);
    const [pacientID, setPacientID] = useState([]);
    const [idEdit, setIdEdit] = useState([]);
    const [causeEdit, setCauseEdit] = useState([]);
    const [startEdit, setStartEdit] = useState([]);
    const [amountEdit, setAmountEdit] = useState([]);
    const [pacientEdit, setPacientEdit] = useState([]);
    const [idToDelete, setIdToDelete] = useState([]);
    const [loading, setLoading] = useState([]);
    var amountLeft = useRef(); 
    var percentAmount = useRef(); 
    var totalAmount = useRef(); 
    const [progress, setProgress] = useState([]);


    useEffect(() => {
        setLoading(false);
        setVisibilityStatus("hidden");
        if(sessionStorage.getItem("token") === null) {
            navigate("/");
        }
    }, []);

    function saveEdit(){
        setLoading(true);
        let json = {};
        if(causeEdit != cause){
            json["cause"] = cause;
        }
        if(startEdit != start){
            json["startDate"] = start;
        }
        if(pacientEdit != pacient){
            json["pacient"] = pacient;
        }
        if(amountEdit != amount){
            json["amountOfDays"] = amount;
        }
        if(JSON.stringify(json) != '{}')
        {
            json = JSON.stringify(json)
            fetchEdit(json);
        }
        else{
            setLoading(false);
        }
    }

    function fetchCreate(json){
        const requestOptions = {
            method: 'POST',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'},
            body : json
        };
        fetch('https://korona.azurewebsites.net/api/Isolation/', requestOptions)
        .then(res => afterFetchCreate(res))
    }

    function fetchEdit(json){
        const requestOptions = {
            method: 'PUT',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'},
            body : json
        };
        fetch('https://korona.azurewebsites.net/api/Isolation/' + idEdit, requestOptions)
        .then(res => afterFetchEdit(res))
    }

    function afterFetchCreate(res){
        if(res.status != 200)
        {
            let error = document.getElementById('errorCreate');
            res.text().then(result => error.textContent = result);
        }
        else
        {
            window.location.reload(true);
        }
        setLoading(false);
    }
    
    function afterFetchEdit(res){
        if(res.status != 200)
        {
            let error = document.getElementById('errorEdit');
            res.text().then(result => error.textContent = result);
        }
        else
        {
            window.location.reload(true);
        }
        setLoading(false);
    }

    async function fetchGetAllByPacient(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Pacient/' + pacientID + '/Isolations', requestOptions)
        const data = await res.text();
        fillTable(res, data, "Pacient " + pacientID + " isolations", true);
    }

    async function fetchGetAll(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Isolation/All/', requestOptions)
        const data = await res.text();
        fillTable(res, data, "All isolations");
    }

    async function fetchDelete(){
        const requestOptions = {
            method: 'DELETE',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Isolation/' + idToDelete, requestOptions);
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

    async function fetchGetById(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Isolation/' + isolationID, requestOptions);
        const data = await res.text();
        fillTable(res, data, "Isolation " + isolationID);
    }

    function fillTable(res, answer, title, byPacient){
        if(res.status != 200)
        {
            let error = document.getElementById('errorLoad');
            error.textContent = answer;
        }
        else{
            document.getElementById('tableTitleText').textContent = title;
            var isolations = JSON.parse(answer);
            let i = 0;
            if(isolations.constructor == Array){
                isolations.map(isolation=>
                (
                    byPacient ? createRow(isolation, i, true) : createRow(isolation, i),
                    i++
                ));
            }
            else{
                byPacient ? createRow(isolations, i, true) : createRow(isolations, i);
            }
            setVisibilityStatus("visible");
        }
        if(byPacient){
            if(amountLeft > 0){
                document.getElementById('isolationTermID').textContent = "Pacient isolation: " + Math.floor(amountLeft) + " days left";
                percentAmount = (100 - ((amountLeft * 100) / totalAmount));
                console.log(percentAmount)
                var element = React.createElement(ProgressBar, {"now": percentAmount, "id": "progress"});
                setProgress(element);
                console.log(element);
            }
            else{

                document.getElementById('isolationTermID').textContent = "Pacient has no active isolation";
            }

        }
        setLoading(false);

    }

    function createRow(isolation, i, byPacient){

        if(byPacient){
            var dateNow = Date.now();
            var amountDays = parseInt(isolation.amountOfDays);
            var startDate = Date.parse(isolation.startDate)
            var daysLeft = ((startDate + (86400000 * amountDays)) - dateNow) / 60000 / 60 / 24;
            if(daysLeft > 0){
                totalAmount = parseInt(totalAmount) + amountDays;
                amountLeft = parseFloat(amountLeft) + daysLeft;
            }
        }
        
        let causeText = isolation.cause;
        let amountText = isolation.amountOfDays;
        let startText = isolation.startDate.split("/");
        startText = startText[2] + "-" + (startText[0] > 9 ? "" + startText[0] : "0" + startText[0]) + "-" + (startText[1]> 9 ? "" + startText[1] : "0" + startText[1])
        let pacientText = isolation.pacient;
        let codeText = isolation.code;

        let id = React.createElement('td',{key: "id" + i}, isolation.id);
        let cause = React.createElement('td',{key: "cause" + i}, causeText);
        let amount = React.createElement('td',{key: "amount" + i}, amountText);
        let start = React.createElement('td',{key: "startDate" + i}, startText);
        let pacient = React.createElement('td',{key: "pacient" + i},pacientText);
        let code = React.createElement('td',{key: "code" + i},codeText);
        let deleteIsolation = React.createElement('td', {key: "delete" + i},"", React.createElement('button',{key: "deleteModal" + i, 'data-bs-toggle': "modal", 'data-bs-target': "#deleteModal", 'className': "btn floatButton", onClick: () => setIdToDelete(isolation.id) },"Delete"));
        let edit = React.createElement('td', {key: "edit" + i},"", React.createElement('button',{key: "editModal" + i, 'data-bs-toggle': "modal", 'data-bs-target': "#editModal", 'className': "btn floatButton", onClick: () => fillEdit(isolation.id, causeText, startText, amountText, pacientText, codeText) },"Edit"));
        let childs = [id, cause, amount, start, pacient, code, edit, deleteIsolation]
        let element = React.createElement('tr', {key: "row" + i}, childs);
        setTableRows(oldArray => [oldArray, element]);
    }

    function DeleteIsolation(){
        fetchDelete();
    }

    function fillEdit(id, cause, start, amount, pacient, code){
        setCauseEdit(cause);
        setAmountEdit(amount);
        setStartEdit(start);
        setPacientEdit(pacient);
        setIdEdit(id);
        setCause(cause);
        setAmount(amount);
        setStart(start);
        setPacient(pacient);
        setCode(code);
    }

    function loadAll(){
        document.getElementById('isolationTermID').textContent = "";
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        setTableRows();
        fetchGetAll();
    }

    function loadByPacient(){
        document.getElementById('isolationTermID').textContent = "";
        amountLeft = 0;
        totalAmount = 0;
        percentAmount = 0;
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        if(pacientID.length === 0){
            setLoading(false);
            return;
        }
        setTableRows();
        fetchGetAllByPacient();
        document.getElementById('isolationTermID').textContent = "Pacient isolation:";
    }

    function loadById(){
        document.getElementById('isolationTermID').textContent = "";
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        if(isolationID.length === 0){
            setLoading(false);
            return;
        }
        setTableRows();
        fetchGetById();
    }

    function create(){
        setLoading(true);
        if(cause.length === 0 || start.length === 0 || amount.length === 0 || pacient.length === 0){
            let error = document.getElementById('errorCreate');
            error.textContent = "All fields should be filled!"
            setLoading(false);
        }
        else{
            let json = {};
            json["cause"] = cause;
            json["startDate"] = start;
            json["amountOfDays"] = amount;
            json["pacient"] = pacient;
    
            if(JSON.stringify(json) != '{}')
            {
                json = JSON.stringify(json)
                fetchCreate(json);
            }
            else{
                setLoading(false);
            }
        }
    }

    return (
        <>
            {loading ? (<div className="loaderDiv">
                <div className="spinner"></div>
            </div>) : ("")}
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
                        <a className="nav-link nav-link" active>Isolations</a>
                        <a className="nav-item nav-link" href="/Tests">Tests</a>
                    </div>
                    <div className="navbar-nav">
                        <a className="nav-item nav-link" href="/">Logout</a>
                    </div>
                </div>
            </nav>
            <div className="infoTable">
                <div className="actionElements">
                <p id="actionsText"><b>Actions</b></p>
                    <p id="errorLoad" className="errorTextTitle"></p>
                    <input className="form-control inputs inputLoad" type="text" size="20" placeholder="Isolation ID" onChange = {(e) => {setIsolationID(e.target.value); }}/>
                    <Button className="btn-secondary loadButton" onClick={() => loadById()}>Load Isolations by Id</Button><br></br><br></br>           
                    <input className="form-control inputs inputLoad" type="text" size="20" placeholder="Pacient ID" onChange = {(e) => {setPacientID(e.target.value); }}/>
                    <Button className="btn-secondary loadButton" onClick={() => loadByPacient()}>Load Pacient' Isolations</Button><br></br><br></br>                     
                    <Button className="btn-secondary loadButton" onClick={() => loadAll()}>Load All Isolations</Button><br></br>
                    <Button className="btn-secondary loadButton" data-bs-toggle="modal" data-bs-target = "#createModal">Create new Isolation</Button>
                </div>
                <br></br><br></br><br></br><div className="isolationTerm" id="isolationTermID"><b></b></div><br></br>
                <b id="tableTitleText"></b>
                <table id="pacientsTable" className="table" style={{visibility: visibilityStatus}}>
                    <thead className="titleRow">
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Cause</th>
                            <th scope="col">Amount of Days</th>
                            <th scope="col">Start Date</th>
                            <th scope="col">Pacient</th>
                            <th scope="col">Code</th>
                            <th scope="col"></th>
                            <th scope="col"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}                       
                    </tbody>
                </table>
            </div>
            <div className="modal fade" id="editModal" tabIndex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="editModalLabel">Edit</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <p id="errorEdit" className="errorTextTitle"></p><br></br>
                            <a>Cause</a><br></br>
                            <input className="form-control" value={cause} onChange = {(e) => {setCause(e.target.value); }}/><br></br>
                            <a>Start Date</a><br></br>
                            <input className="form-control" type="date" value={start} onChange = {(e) => {setStart(e.target.value); }}/><br></br>
                            <a>Amount of Days</a><br></br>
                            <input className="form-control" value={amount} onChange = {(e) => {setAmount(e.target.value); }}/><br></br>
                            <a>Pacient</a><br></br>
                            <input className="form-control" value={pacient} onChange = {(e) => {setPacient(e.target.value); }}/><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => saveEdit()}>Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
            <div className="modal fade" id="createModal" tabIndex="-1" role="dialog" aria-labelledby="createModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="createModalLabel">Create new Isolation</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                        <p id="errorCreate" className="errorTextTitle"></p><br></br>
                            <a>Cause</a><br></br>
                            <input className="form-control" value={cause} onChange = {(e) => {setCause(e.target.value); }}/><br></br>
                            <a>Start Date</a><br></br>
                            <input className="form-control" type="date" value={start} onChange = {(e) => {setStart(e.target.value); }}/><br></br>
                            <a>Amount of Days</a><br></br>
                            <input className="form-control" value={amount} onChange = {(e) => {setAmount(e.target.value); }}/><br></br>
                            <a>Pacient</a><br></br>
                            <input className="form-control" value={pacient} onChange = {(e) => {setPacient(e.target.value); }}/><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => create()}>Create</button>
                        </div>
                    </div>
                </div>
            </div>
            <div className="modal fade" id="deleteModal" tabIndex="-1" role="dialog" aria-labelledby="deleteModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="deleteModalLabel">Delete the Isolation</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <p id="errorDelete" className="errorTextTitle"></p><br></br>
                            <a>Are you sure you want to delete the isolation?</a><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => DeleteIsolation()}>Delete</button>
                        </div>
                    </div>
                </div>
            </div>
            <footer className="footerLogin">
                <p>Copyright © 2022</p>
            </footer>
        </>
    )
}




